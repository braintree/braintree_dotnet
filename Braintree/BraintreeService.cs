#pragma warning disable 1591

using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Xml;
using System.Text;
using Braintree.Exceptions;

#if NET452
using System.Threading.Tasks;
#endif

namespace Braintree
{
    public class BraintreeService
    {
        private static readonly string AssemblyVersion = typeof(BraintreeService).Assembly.GetName().Version.ToString();

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

#if NET452
        public async Task<XmlNode> GetAsync(string URL)
        {
            return await GetXmlResponseAsync(URL, "GET", null);
        }
#endif

        internal XmlNode Delete(string URL)
        {
            return GetXmlResponse(URL, "DELETE", null);
        }

#if NET452
        internal async Task<XmlNode> DeleteAsync(string URL)
        {
            return await GetXmlResponseAsync(URL, "DELETE", null);
        }
#endif

        public XmlNode Post(string URL, Request requestBody)
        {
            return GetXmlResponse(URL, "POST", requestBody);
        }

#if NET452
        public async Task<XmlNode> PostAsync(string URL, Request requestBody)
        {
            return await GetXmlResponseAsync(URL, "POST", requestBody);
        }
#endif

        internal XmlNode Post(string URL)
        {
            return GetXmlResponse(URL, "POST", null);
        }

#if NET452
        internal async Task<XmlNode> PostAsync(string URL)
        {
            return await GetXmlResponseAsync(URL, "POST", null);
        }
#endif

        public XmlNode Put(string URL)
        {
            return Put(URL, null);
        }

#if NET452
        public async Task<XmlNode> PutAsync(string URL)
        {
            return await PutAsync(URL, null);
        }
#endif

        internal XmlNode Put(string URL, Request requestBody)
        {
            return GetXmlResponse(URL, "PUT", requestBody);
        }

#if NET452
        internal async Task<XmlNode> PutAsync(string URL, Request requestBody)
        {
            return await GetXmlResponseAsync(URL, "PUT", requestBody);
        }
#endif

        private XmlNode GetXmlResponse(string URL, string method, Request requestBody)
        {
            try
            {
                var request = Configuration.HttpWebRequestFactory(Environment.GatewayURL + URL);
                request.Headers.Add("Authorization", GetAuthorizationHeader());
                request.Headers.Add("X-ApiVersion", ApiVersion);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.Accept = "application/xml";
                request.UserAgent = "Braintree .NET " + AssemblyVersion;
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
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(buffer, 0, buffer.Length);
                    }
                }

                using (var response = (HttpWebResponse) request.GetResponse())
                {
                    return ParseResponseStream(GetResponseStream(response));
                }
            }
            catch (WebException e)
            {
                using (var response = (HttpWebResponse)e.Response)
                {
                    if (response == null) throw e;

                    var statusCode = response.StatusCode;
                    if (statusCode == (HttpStatusCode)422) // UnprocessableEntity
                    {
                        XmlNode doc = ParseResponseStream(GetResponseStream(response));
                        return doc;
                    }

                    ThrowExceptionIfErrorStatusCode(statusCode, null);
                }

                throw e;
            }
        }

#if NET452
        private async Task<XmlNode> GetXmlResponseAsync(string URL, string method, Request requestBody)
        {
            try
            {
                var request = Configuration.HttpWebRequestFactory(Environment.GatewayURL + URL);
                request.Headers.Add("Authorization", GetAuthorizationHeader());
                request.Headers.Add("X-ApiVersion", ApiVersion);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.Accept = "application/xml";
                request.UserAgent = "Braintree .NET " + AssemblyVersion;
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
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(buffer, 0, buffer.Length);
                    }
                }

                using (var response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    return await ParseResponseStreamAsync(GetResponseStream(response));
                }
            }
            catch (WebException e)
            {
                using (var response = (HttpWebResponse)e.Response)
                {
                    if (response == null) throw e;

                    var statusCode = response.StatusCode;
                    if (statusCode == (HttpStatusCode) 422) // UnprocessableEntity
                    {
                        // TODO in C# 6 can await in catch block
                        XmlNode doc = ParseResponseStream(GetResponseStream(response));
                        return doc;
                    }

                    ThrowExceptionIfErrorStatusCode(statusCode, null);
                }

                throw e;
            }
        }
#endif

        private Stream GetResponseStream(HttpWebResponse response)
        {
            var stream = response.GetResponseStream();
            if (stream != null && response.ContentEncoding.Equals("gzip", StringComparison.CurrentCultureIgnoreCase))
            {
                stream = new GZipStream(stream, CompressionMode.Decompress);
            }
            return stream;
        }

        private XmlNode ParseResponseStream(Stream stream)
        {
            string body;
            using (var streamReader = new StreamReader(stream))
            {
                body = streamReader.ReadToEnd();
            }

            return StringToXmlNode(body);
        }

#if NET452
        private async Task<XmlNode> ParseResponseStreamAsync(Stream stream)
        {
            string body;
            using (var streamReader = new StreamReader(stream))
            {
                body = await streamReader.ReadToEndAsync();
            }

            return StringToXmlNode(body);
        }
#endif

        private void setRequestProxy(WebRequest request)
        {
            var proxy = GetProxy();
            if (proxy != null)
            {
                request.Proxy = new WebProxy(proxy);
            }
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
