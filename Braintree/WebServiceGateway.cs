using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Text;
using Braintree.Exceptions;

namespace Braintree
{
    public abstract class WebServiceGateway
    {
        public static XmlNode Get(string URL)
        {
            return GetXmlResponse(URL, "GET", null);
        }

        internal static void Delete(string URL)
        {
            GetXmlResponse(URL, "DELETE", null);
        }

        internal static XmlNode Post(string URL, Request requestBody)
        {
            return GetXmlResponse(URL, "POST", requestBody);
        }

        internal static XmlNode Post(string URL)
        {
            return GetXmlResponse(URL, "POST", null);
        }

        public static XmlNode Put(String URL)
        {
            return Put(URL, null);
        }

        internal static XmlNode Put(String URL, Request requestBody)
        {
            return GetXmlResponse(URL, "PUT", requestBody);
        }

        private static XmlNode GetXmlResponse(String URL, String method, Request requestBody)
        {
            try
            {
                var request = WebRequest.Create(Configuration.BaseMerchantURL() + URL) as HttpWebRequest;
                request.Headers.Add("Authorization", Configuration.GetAuthorizationHeader());
                request.Headers.Add("X-ApiVersion", "1");
                request.UserAgent = "Braintree .NET " + typeof(WebServiceGateway).Assembly.GetName().Version.ToString();
                request.Method = method;
                request.KeepAlive = false;

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

                ThrowExceptionIfErrorStatusCode(response.StatusCode);

                throw e;
            }
        }

        private static XmlNode ParseResponseStream(Stream stream)
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
                return doc.ChildNodes[1];
            }
        }

        internal static void ThrowExceptionIfErrorStatusCode(HttpStatusCode httpStatusCode)
        {
            if (httpStatusCode != HttpStatusCode.OK && httpStatusCode != HttpStatusCode.Created)
            {
                switch (httpStatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new AuthenticationException();
                    case HttpStatusCode.Forbidden:
                        throw new AuthorizationException();
                    case HttpStatusCode.NotFound:
                        throw new NotFoundException();
                    case HttpStatusCode.InternalServerError:
                        throw new ServerException();
                    case HttpStatusCode.ServiceUnavailable:
                        throw new DownForMaintenanceException();
                    default:
                        throw new UnexpectedException { Source = "Unexpected HTTP_RESPONSE " + httpStatusCode };
                }
            }
        }

        private static Boolean IsErrorCode(int responseCode)
        {
            return responseCode != 200 && responseCode != 201 && responseCode != 422;
        }
    }
}
