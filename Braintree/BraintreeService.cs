#pragma warning disable 1591

using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Text;
using Braintree.Exceptions;

namespace Braintree
{
    public class BraintreeService
    {
        public String ApiVersion = "2";

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

        internal void Delete(string URL)
        {
            GetXmlResponse(URL, "DELETE", null);
        }

        internal XmlNode Post(string URL, Request requestBody)
        {
            return GetXmlResponse(URL, "POST", requestBody);
        }

        internal XmlNode Post(string URL)
        {
            return GetXmlResponse(URL, "POST", null);
        }

        public XmlNode Put(String URL)
        {
            return Put(URL, null);
        }

        internal XmlNode Put(String URL, Request requestBody)
        {
            return GetXmlResponse(URL, "PUT", requestBody);
        }

        private XmlNode GetXmlResponse(String URL, String method, Request requestBody)
        {
            try
            {
                var request = WebRequest.Create(BaseMerchantURL() + URL) as HttpWebRequest;
                request.Headers.Add("Authorization", GetAuthorizationHeader());
                request.Headers.Add("X-ApiVersion", ApiVersion);
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

                XmlNode doc = ParseResponseStream(response.GetResponseStream());
                response.Close();

                return doc;
            }
            catch (WebException e)
            {
                var response = (HttpWebResponse)e.Response;
                if (response == null) throw e;

                if (response.StatusCode == (HttpStatusCode)422) // UnprocessableEntity
                {
                    XmlNode doc = ParseResponseStream(((HttpWebResponse)e.Response).GetResponseStream());
                    e.Response.Close();
                    return doc;
                }

                ThrowExceptionIfErrorStatusCode(response.StatusCode, null);

                throw e;
            }
        }

        private XmlNode ParseResponseStream(Stream stream)
        {
            var body = new StreamReader(stream).ReadToEnd();
            if (body.Trim() == "")
            {
                return new XmlDocument();
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(body);
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
