#pragma warning disable 1591

using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Text;
using Braintree.Exceptions;

namespace Braintree
{
    public class BraintreeService
    {
        public string ApiVersion = "4";

#if netcoreapp10 == false
        static BraintreeService()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }
#endif

        protected Configuration Configuration;

        public Environment Environment
        {
            get { return Configuration.Environment; }
        }

        public string MerchantId
        {
            get { return Configuration.MerchantId; }
        }

        public string PublicKey
        {
            get { return Configuration.PublicKey; }
        }

        public string PrivateKey
        {
            get { return Configuration.PrivateKey; }
        }

        public string ClientId
        {
            get { return Configuration.ClientId; }
        }

        public string ClientSecret
        {
            get { return Configuration.ClientSecret; }
        }

        public BraintreeService(Configuration configuration)
        {
            this.Configuration = configuration;
        }

        public XmlNode Get(string URL)
        {
            return GetXmlResponse(URL, "GET", null);
        }

        internal XmlNode Delete(string URL)
        {
            return GetXmlResponse(URL, "DELETE", null);
        }

        public XmlNode Post(string URL, Request requestBody)
        {
            return GetXmlResponse(URL, "POST", requestBody);
        }

        internal XmlNode Post(string URL)
        {
            return GetXmlResponse(URL, "POST", null);
        }

        public XmlNode Put(string URL)
        {
            return Put(URL, null);
        }

        internal XmlNode Put(string URL, Request requestBody)
        {
            return GetXmlResponse(URL, "PUT", requestBody);
        }

        private XmlNode GetXmlResponse(string URL, string method, Request requestBody)
        {
            try
            {
                var request = new HttpRequestMessage(new HttpMethod(method), Environment.GatewayURL + URL);

                request.Headers.Add("Authorization", GetAuthorizationHeader());
                request.Headers.Add("X-ApiVersion", ApiVersion);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.Headers.Accept.ParseAdd("application/xml");
                request.Headers.UserAgent.ParseAdd("Braintree .NET " + typeof(BraintreeService).GetTypeInfo().Assembly.GetName().Version);

                

                if (requestBody != null)
                {
                    var xmlPrefix = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                 
                    request.Content = new StringContent(xmlPrefix + requestBody.ToXml());
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
                }

                var proxy = GetProxy();
                var decompressionHandler = new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                    Proxy = !string.IsNullOrEmpty(proxy) ? new BasicProxy(proxy) : null,
                    UseProxy = !string.IsNullOrEmpty(proxy)
                };
#if netcoreapp10
                WinHttpHandler handler = new WinHttpHandler();
                SetServerCertificateValidationCallback(handler);
#endif
                var client = new HttpClient(decompressionHandler)
                {
                    Timeout = TimeSpan.FromMilliseconds(Configuration.Timeout),
                    
                };

                var response = client.SendAsync(request).GetAwaiter().GetResult();
                

                XmlNode doc = ParseResponseStream(response.Content.ReadAsStreamAsync().GetAwaiter().GetResult());
                
                ThrowExceptionIfErrorStatusCode(response.StatusCode, null);
                response.EnsureSuccessStatusCode();
                return doc;
            }
            catch (HttpRequestException)
            {
                throw new ServerException();
            }
        }

#if netcoreapp10
        static void SetServerCertificateValidationCallback(WinHttpHandler handler)
        {
            handler.ServerCertificateValidationCallback = ChainValidator(handler.ServerCertificateValidationCallback);
        }

        static Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> ChainValidator(Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> previousValidator)
        {
            if (previousValidator == null)
            {
                return OnValidateServerCertificate;
            }

            Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> chained =
                (request, certificate, chain, sslPolicyErrors) =>
                {
                    bool valid = OnValidateServerCertificate(request, certificate, chain, sslPolicyErrors);
                    if (valid)
                    {
                        return previousValidator(request, certificate, chain, sslPolicyErrors);
                    }
                    return false;
                };

            return chained;
        }

        static bool OnValidateServerCertificate(HttpRequestMessage request, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            bool valid = true; // Do validation
            if (valid)
            {
                return (sslPolicyErrors == SslPolicyErrors.None);
            }

            return false;
        }


#endif

        private XmlNode ParseResponseStream(Stream stream)
        {
            var body = new StreamReader(stream).ReadToEnd();
            return StringToXmlNode(body);
        }

        internal XmlNode StringToXmlNode(string xml)
        {
            if (xml.Trim() == "")
            {
                return new XmlDocument();
            }
            else
            {
                var doc = new XmlDocument();
                doc.LoadXml(xml);
                if (doc.ChildNodes.Count == 1) return doc.ChildNodes[0];
                return doc.ChildNodes[1];
            }
        }

        public string BaseMerchantURL()
        {
            return Environment.GatewayURL + MerchantPath();
        }

        public string MerchantPath()
        {
            return "/merchants/" + MerchantId;
        }

        public string GetProxy()
        {
            return Configuration.Proxy;
        }

        public string GetAuthorizationHeader()
        {
            string credentials;
            if (Configuration.IsAccessToken)
            {
                return "Bearer " + Configuration.AccessToken;
            } else {
                if (Configuration.IsClientCredentials)
                {
                    credentials = ClientId + ":" + ClientSecret;
                }
                else
                {
                    credentials = PublicKey + ":" + PrivateKey;
                }
                return "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)).Trim();
            }
        }

        public static void ThrowExceptionIfErrorStatusCode(HttpStatusCode httpStatusCode, string message)
        {
            if (httpStatusCode != HttpStatusCode.OK && httpStatusCode != HttpStatusCode.Created)
            {
                switch (httpStatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new AuthenticationException();
                    case HttpStatusCode.Forbidden:
                        throw new AuthorizationException(message);
                    case HttpStatusCode.NotFound:
                        throw new NotFoundException();
                    case HttpStatusCode.InternalServerError:
                        throw new ServerException();
                    case HttpStatusCode.ServiceUnavailable:
                        throw new DownForMaintenanceException();
                    case (HttpStatusCode) 429:
                        throw new TooManyRequestsException();
                    case (HttpStatusCode) 426:
                        throw new UpgradeRequiredException();
                    default:
						var exception = new UnexpectedException();
						exception.Source = "Unexpected HTTP_RESPONSE " + httpStatusCode;
                        throw exception;
                }
            }
        }
    }
}
