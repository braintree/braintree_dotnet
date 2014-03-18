#pragma warning disable 1591

using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Xml;
using System.Text;
using Braintree.Exceptions;
using System.Threading.Tasks;

namespace Braintree
{
    public class BraintreeService
    {
        public String ApiVersion = "3";

        protected Configuration Configuration;

        public Environment Environment
        {
            get { return Configuration.Environment; }
        }

        public String MerchantId
        {
            get { return Configuration.MerchantId; }
        }

        public String PublicKey
        {
            get { return Configuration.PublicKey; }
        }

        public String PrivateKey
        {
            get { return Configuration.PrivateKey; }
        }

        public BraintreeService(Configuration configuration)
        {
            this.Configuration = configuration;
        }

        public XmlNode Get(string URL)
        {
            return GetXmlResponse(URL, "GET", null);
        }

        public async Task<XmlNode> GetAsync(string URL)
        {
            return await GetXmlResponseAsync(URL, "GET", null);
        }

        internal bool Delete(string URL)
        {
            GetXmlResponse(URL, "DELETE", null);
            return true;
        }

        internal async Task<bool> DeleteAsync(string URL)
        {
            await GetXmlResponseAsync(URL, "DELETE", null);
            return true;
        }

        public XmlNode Post(string URL, Request requestBody)
        {
            return GetXmlResponse(URL, "POST", requestBody);
        }

        public async Task<XmlNode> PostAsync(string URL, Request requestBody)
        {
            return await GetXmlResponseAsync(URL, "POST", requestBody);
        }

        internal XmlNode Post(string URL)
        {
            return GetXmlResponse(URL, "POST", null);
        }

        internal async Task<XmlNode> PostAsync(string URL)
        {
            return await GetXmlResponseAsync(URL, "POST", null);
        }

        public XmlNode Put(String URL)
        {
            return Put(URL, null);
        }

        public async Task<XmlNode> PutAsync(String URL)
        {
            return await PutAsync(URL, null);
        }

        internal XmlNode Put(String URL, Request requestBody)
        {
            return GetXmlResponse(URL, "PUT", requestBody);
        }

        internal async Task<XmlNode> PutAsync(String URL, Request requestBody)
        {
            return await GetXmlResponseAsync(URL, "PUT", requestBody);
        }

        private XmlNode GetXmlResponse(String URL, String method, Request requestBody)
        {
            try
            {
                var request = WebRequest.Create(BaseMerchantURL() + URL) as HttpWebRequest;
                request.Headers.Add("Authorization", GetAuthorizationHeader());
                request.Headers.Add("X-ApiVersion", ApiVersion);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.Accept = "application/xml";
                request.UserAgent = "Braintree .NET " + typeof(BraintreeService).Assembly.GetName().Version.ToString();
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
                    XmlNode doc = ParseResponseStream(GetResponseStream((HttpWebResponse)e.Response));
                    e.Response.Close();
                    return doc;
                }

                ThrowExceptionIfErrorStatusCode(response.StatusCode, null);

                throw e;
            }
        }

        private async Task<XmlNode> GetXmlResponseAsync(String URL, String method, Request requestBody)
        {
            try
            {
                var request = WebRequest.Create(BaseMerchantURL() + URL) as HttpWebRequest;
                request.Headers.Add("Authorization", GetAuthorizationHeader());
                request.Headers.Add("X-ApiVersion", ApiVersion);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.Accept = "application/xml";
                request.UserAgent = "Braintree .NET " + typeof(BraintreeService).Assembly.GetName().Version.ToString();
                request.Method = method;
                request.KeepAlive = false;
                request.Timeout = 60000;
                request.ReadWriteTimeout = 60000;

                if (requestBody != null)
                {
                    string xmlPrefix = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                    byte[] buffer = Encoding.UTF8.GetBytes(xmlPrefix + requestBody.ToXml());
                    request.ContentType = "application/xml";
                    request.ContentLength = buffer.Length;
                    Stream requestStream = await request.GetRequestStreamAsync();
                    await requestStream.WriteAsync(buffer, 0, buffer.Length);
                    requestStream.Close();
                }

                using (WebResponse response = await request.GetResponseAsync())
                {
                    XmlNode doc = await ParseResponseStreamAsync(GetResponseStream(response));
                    return doc;
                }
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

        private Stream GetResponseStream(WebResponse response)
        {
            HttpWebResponse httpResponse = response as HttpWebResponse;
            Stream stream = response.GetResponseStream();
            if (httpResponse.ContentEncoding.Equals("gzip", StringComparison.CurrentCultureIgnoreCase))
            {
                stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
            }
            return stream;
        }

        private XmlNode ParseResponseStream(Stream stream)
        {
            string body = new StreamReader(stream).ReadToEnd();
            return StringToXmlNode(body);
        }

        private async Task<XmlNode> ParseResponseStreamAsync(Stream stream)
        {
            string body = await new StreamReader(stream).ReadToEndAsync();
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
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                if (doc.ChildNodes.Count == 1) return doc.ChildNodes[0];
                return doc.ChildNodes[1];
            }
        }

        public String BaseMerchantURL()
        {
            return Environment.GatewayURL + "/merchants/" + MerchantId;
        }

        public String GetAuthorizationHeader()
        {
            return "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(PublicKey + ":" + PrivateKey)).Trim();

        }

        public static void ThrowExceptionIfErrorStatusCode(HttpStatusCode httpStatusCode, String message)
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
