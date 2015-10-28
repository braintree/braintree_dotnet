#pragma warning disable 1591

using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Xml;
using System.Text;
using Braintree.Exceptions;

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
                var request = WebRequest.Create(Environment.GatewayURL + URL) as HttpWebRequest;
                request.Headers.Add("Authorization", GetAuthorizationHeader());
                request.Headers.Add("X-ApiVersion", ApiVersion);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.Accept = "application/xml";
                request.UserAgent = "Braintree .NET " + typeof(BraintreeService).Assembly.GetName().Version.ToString();
                request.Proxy = new WebProxy(GetProxy());
                request.Method = method;
                request.KeepAlive = false;
                request.Timeout = 60000;
                request.ReadWriteTimeout = 60000;

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
                    XmlNode doc = ParseResponseStream(GetResponseStream((HttpWebResponse) e.Response));
                    e.Response.Close();
                    return doc;
                }

                ThrowExceptionIfErrorStatusCode(response.StatusCode, null);

                throw e;
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
                return "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(credentials)).Trim();
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
