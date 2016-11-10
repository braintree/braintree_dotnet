#pragma warning disable 1591

using Braintree.Exceptions;
using System;
using System.IO;
using System.Net;
#if netcore
using System.Net.Http;
#else
using System.IO.Compression;
#endif
using System.Reflection;
using System.Text;
using System.Xml;

namespace Braintree
{
    public class BraintreeService
    {
        public string ApiVersion = "4";

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

        public IWebProxy WebProxy
        {
            get { return Configuration.WebProxy;  }
        }

        public BraintreeService(Configuration configuration)
        {
            Configuration = configuration;
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
#if netcore
            try
            {
                var request = Configuration.HttpRequestMessageFactory(new HttpMethod(method), Environment.GatewayURL + URL);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(GetAuthorizationSchema(), GetAuthorizationHeader());

                request.Headers.Add("X-ApiVersion", ApiVersion);
                request.Headers.Add("AcceptEncoding", "gzip");
                request.Headers.Add("Accept", "application/xml");
                request.Headers.Add("UserAgent", "Braintree .NET " + typeof(BraintreeService).GetTypeInfo().Assembly.GetName().Version.ToString());
                request.Headers.Add("Keep-Alive", "false");

                if (requestBody != null)
                {
                    var xmlPrefix = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                    var content = xmlPrefix + requestBody.ToXml();
                    var utf8_string = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(content));
                    request.Content = new StringContent(utf8_string, Encoding.UTF8, "application/xml");
                    request.Content.Headers.ContentLength = System.Text.UTF8Encoding.UTF8.GetByteCount(utf8_string);
                }

                var httpClientHandler = new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                };

                SetWebProxy(httpClientHandler, URL);

                XmlNode doc = new XmlDocument();
                using (var client = new HttpClient(httpClientHandler))
                {
                    client.Timeout = TimeSpan.FromMilliseconds(Configuration.Timeout);
                    var response = client.SendAsync(request).GetAwaiter().GetResult();

                    if (response.StatusCode != (HttpStatusCode)422)
                    {
                        ThrowExceptionIfErrorStatusCode(response.StatusCode, null);
                    }

                    doc = ParseResponseStream(response.Content.ReadAsStreamAsync().GetAwaiter().GetResult());
                }

                return doc;
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
#else
            try
            {
                const int SecurityProtocolTypeTls12 = 3072;
                ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | ((SecurityProtocolType)SecurityProtocolTypeTls12);
                var request = Configuration.HttpWebRequestFactory(Environment.GatewayURL + URL);
                request.Headers.Add("Authorization", GetAuthorizationHeader());
                request.Headers.Add("X-ApiVersion", ApiVersion);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.Accept = "application/xml";
                request.UserAgent = "Braintree .NET " + typeof(BraintreeService).Assembly.GetName().Version.ToString();
                setRequestProxy(request);
                request.Method = method;
                request.KeepAlive = false;
                request.Timeout = Configuration.Timeout;
                request.ReadWriteTimeout = Configuration.Timeout;

                if (requestBody != null)
                {
                    var xmlPrefix = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                    byte[] buffer = Encoding.UTF8.GetBytes(xmlPrefix + requestBody.ToXml());
                    request.ContentType = "application/xml";
                    request.ContentLength = buffer.Length;
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(buffer, 0, buffer.Length);
                    requestStream.Close();
                }

                var response = request.GetResponse() as HttpWebResponse;

                XmlNode doc = ParseResponseStream(GetResponseStream(response));
                response.Close();

                return doc;
            }
            catch (WebException e)
            {
                var response = (HttpWebResponse)e.Response;
                if (response == null) throw e;

                if (response.StatusCode == (HttpStatusCode)422) // UnprocessableEntity
                {
                    XmlNode doc = ParseResponseStream(GetResponseStream((HttpWebResponse)e.Response));
                    e.Response.Close();
                    return doc;
                }

                ThrowExceptionIfErrorStatusCode(response.StatusCode, null);

                throw e;
            }
#endif
        }
#if netcore
        private void SetWebProxy(HttpClientHandler httpClientHandler, string URL)
        {
            var proxy = GetWebProxy();
            bool useProxy = false;

            if (proxy != null && !proxy.IsBypassed(new Uri(URL)))
            {
                useProxy = true;
                httpClientHandler.Proxy = proxy;
            }

            httpClientHandler.UseProxy = useProxy;
        }
#else
        private void setRequestProxy(WebRequest request)
        {
            var proxy = GetWebProxy();
            if (proxy != null)
            {
                request.Proxy = proxy;
            }
        }

        private Stream GetResponseStream(HttpWebResponse response)
        {
            var stream = response.GetResponseStream();
            if (response.ContentEncoding.Equals("gzip", StringComparison.CurrentCultureIgnoreCase))
            {
                stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
            }
            return stream;
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

        public IWebProxy GetWebProxy()
        {
            return Configuration.WebProxy;
        }

        public string GetAuthorizationSchema()
        {
            
            if (Configuration.IsAccessToken)
            {
                return "Bearer";
            }
            else
            {
                return "Basic";
            }
        }

        public string GetAuthorizationHeader()
        {

            string credentials;
#if netcore
            if (Configuration.IsAccessToken)
            {
                return Configuration.AccessToken;
            }
            else
            {
                if (Configuration.IsClientCredentials)
                {
                    credentials = ClientId + ":" + ClientSecret;
                }
                else
                {
                    credentials = PublicKey + ":" + PrivateKey;
                }
                return Convert.ToBase64String(Encoding.GetEncoding(0).GetBytes(credentials)).Trim();
            }
#else
            if (Configuration.IsAccessToken)
            {
                return "Bearer " + Configuration.AccessToken;
            }
            else
            {
                if (Configuration.IsClientCredentials)
                {
                    credentials = ClientId + ":" + ClientSecret;
                }
                else
                {
                    credentials = PublicKey + ":" + PrivateKey;
                }
                return "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(credentials)).Trim();
            }
#endif
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
