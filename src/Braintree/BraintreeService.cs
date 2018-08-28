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
using System.Threading.Tasks;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace Braintree
{
    public class BraintreeService
    {
        private static readonly Encoding encoding = Encoding.UTF8;

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
            return GetXmlResponse(URL, "GET", null, null);
        }

        public Task<XmlNode> GetAsync(string URL)
        {
            return GetXmlResponseAsync(URL, "GET", null, null);
        }

        internal XmlNode Delete(string URL)
        {
            return GetXmlResponse(URL, "DELETE", null, null);
        }

        internal Task<XmlNode> DeleteAsync(string URL)
        {
            return GetXmlResponseAsync(URL, "DELETE", null, null);
        }

        public XmlNode Post(string URL, Request requestBody)
        {
            return GetXmlResponse(URL, "POST", requestBody, null);
        }

        public Task<XmlNode> PostAsync(string URL, Request requestBody)
        {
            return GetXmlResponseAsync(URL, "POST", requestBody, null);
        }

        internal XmlNode Post(string URL)
        {
            return Post(URL, null);
        }

        internal Task<XmlNode> PostAsync(string URL)
        {
            return PostAsync(URL, null);
        }

        public XmlNode PostMultipart(string URL, Request requestBody, FileStream file)
        {
            return GetXmlResponse(URL, "POST", requestBody, file);
        }

        public Task<XmlNode> PostMultipartAsync(string URL, Request requestBody, FileStream file)
        {
            return GetXmlResponseAsync(URL, "POST", requestBody, file);
        }

        public XmlNode Put(string URL)
        {
            return Put(URL, null);
        }

        public Task<XmlNode> PutAsync(string URL)
        {
            return PutAsync(URL, null);
        }

        internal XmlNode Put(string URL, Request requestBody)
        {
            return GetXmlResponse(URL, "PUT", requestBody, null);
        }

        internal Task<XmlNode> PutAsync(string URL, Request requestBody)
        {
            return GetXmlResponseAsync(URL, "PUT", requestBody, null);
        }

#if netcore
        internal void SetRequestHeaders(HttpRequestMessage request)
        {
                request.Headers.Add("X-ApiVersion", ApiVersion);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.Headers.Add("Accept", "application/xml");
                request.Headers.Add("User-Agent", "Braintree .NET " + typeof(BraintreeService).GetTypeInfo().Assembly.GetName().Version.ToString());
                request.Headers.Add("Keep-Alive", "false");
        }
#else
        internal void SetRequestHeaders(HttpWebRequest request)
        {
            request.Headers.Add("Authorization", GetAuthorizationHeader());
            request.Headers.Add("X-ApiVersion", ApiVersion);
            request.Headers.Add("Accept-Encoding", "gzip");
            request.Accept = "application/xml";
            request.UserAgent = "Braintree .NET " + typeof(BraintreeService).Assembly.GetName().Version.ToString();
        }
#endif

        private XmlNode GetXmlResponse(string URL, string method, Request requestBody, FileStream file)
        {
#if netcore
            try
            {
                var request = Configuration.HttpRequestMessageFactory(new HttpMethod(method), Environment.GatewayURL + URL);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(GetAuthorizationSchema(), GetAuthorizationHeader());

                SetRequestHeaders(request);

                if (requestBody != null && file == null)
                {
                    var xmlPrefix = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                    var content = xmlPrefix + requestBody.ToXml();
                    var utf8_string = encoding.GetString(encoding.GetBytes(content));
                    request.Content = new StringContent(utf8_string, encoding, "application/xml");
                    request.Content.Headers.ContentLength = System.Text.UTF8Encoding.UTF8.GetByteCount(utf8_string);
                }

                if (file != null)
                {
                    string formDataBoundary = GenerateMultipartFormBoundary();

                    Dictionary<string, object> postParameters = requestBody.ToDictionary();
                    postParameters.Add("file", file);

                    byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);
                    var ascii_string = Encoding.ASCII.GetString(formData);
                    request.Content = new StringContent(ascii_string);
                    request.Content.Headers.Remove("Content-Type");
                    request.Content.Headers.TryAddWithoutValidation("Content-Type", MultipartFormContentType(formDataBoundary));
                    request.Content.Headers.ContentLength = System.Text.UTF8Encoding.UTF8.GetByteCount(ascii_string);
                }

                var httpClientHandler = new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                };

                SetWebProxy(httpClientHandler, Environment.GatewayURL + URL);

                using (var client = new HttpClient(httpClientHandler))
                {
                    client.Timeout = TimeSpan.FromMilliseconds(Configuration.Timeout);
                    var response = client.SendAsync(request).GetAwaiter().GetResult();

                    if (response.StatusCode != (HttpStatusCode)422)
                    {
                        ThrowExceptionIfErrorStatusCode(response.StatusCode, null);
                    }

                    return ParseResponseStream(response.Content.ReadAsStreamAsync().GetAwaiter().GetResult());
                }
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
                SetRequestHeaders(request);
                setRequestProxy(request);
                request.Method = method;
                request.KeepAlive = false;
                request.Timeout = Configuration.Timeout;
                request.ReadWriteTimeout = Configuration.Timeout;

                if (requestBody != null && file == null)
                {
                    var xmlPrefix = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                    byte[] buffer = encoding.GetBytes(xmlPrefix + requestBody.ToXml());
                    request.ContentType = "application/xml";
                    request.ContentLength = buffer.Length;
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(buffer, 0, buffer.Length);
                    }
                }

                if (file != null)
                {
                    string formDataBoundary = GenerateMultipartFormBoundary();
                    request.ContentType = MultipartFormContentType(formDataBoundary);

                    Dictionary<string, object> postParameters = requestBody.ToDictionary();
                    postParameters.Add("file", file);

                    byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);
                    request.ContentLength = formData.Length;
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(formData, 0, formData.Length);
                        requestStream.Close();
                    }
                }

                using (var response = (HttpWebResponse) request.GetResponse())
                {
                    return ParseResponseStream(GetResponseStream(response));
                }
            }
            catch (WebException e)
            {
                using (var response = (HttpWebResponse) e.Response)
                {
                    if (response == null) throw e;

                    if (response.StatusCode == (HttpStatusCode)422) // UnprocessableEntity
                    {
                        return ParseResponseStream(GetResponseStream((HttpWebResponse)e.Response));
                    }

                    ThrowExceptionIfErrorStatusCode(response.StatusCode, null);
                }

                throw e;
            }
#endif
        }

        private async Task<XmlNode> GetXmlResponseAsync(string URL, string method, Request requestBody, FileStream file)
        {
#if netcore
            try
            {
                var request = Configuration.HttpRequestMessageFactory(new HttpMethod(method), Environment.GatewayURL + URL);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(GetAuthorizationSchema(), GetAuthorizationHeader());

                SetRequestHeaders(request);

                if (requestBody != null && file == null)
                {
                    var xmlPrefix = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                    var content = xmlPrefix + requestBody.ToXml();
                    var utf8_string = encoding.GetString(encoding.GetBytes(content));
                    request.Content = new StringContent(utf8_string, encoding, "application/xml");
                    request.Content.Headers.ContentLength = System.Text.UTF8Encoding.UTF8.GetByteCount(utf8_string);
                }

                if (file != null)
                {
                    string formDataBoundary = GenerateMultipartFormBoundary();

                    Dictionary<string, object> postParameters = requestBody.ToDictionary();
                    postParameters.Add("file", file);

                    byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);
                    var ascii_string = Encoding.ASCII.GetString(formData);
                    request.Content = new StringContent(ascii_string);
                    request.Content.Headers.Remove("Content-Type");
                    request.Content.Headers.TryAddWithoutValidation("Content-Type", MultipartFormContentType(formDataBoundary));
                    request.Content.Headers.ContentLength = System.Text.UTF8Encoding.UTF8.GetByteCount(ascii_string);
                }

                var httpClientHandler = new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                };

                SetWebProxy(httpClientHandler, Environment.GatewayURL + URL);

                using (var client = new HttpClient(httpClientHandler))
                {
                    client.Timeout = TimeSpan.FromMilliseconds(Configuration.Timeout);
                    var response = client.SendAsync(request).GetAwaiter().GetResult();

                    if (response.StatusCode != (HttpStatusCode)422)
                    {
                        ThrowExceptionIfErrorStatusCode(response.StatusCode, null);
                    }

                    return await ParseResponseStreamAsync(response.Content.ReadAsStreamAsync().GetAwaiter().GetResult());
                }
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
                SetRequestHeaders(request);
                setRequestProxy(request);
                request.Method = method;
                request.KeepAlive = false;
                request.Timeout = Configuration.Timeout;
                request.ReadWriteTimeout = Configuration.Timeout;

                if (requestBody != null && file == null)
                {
                    var xmlPrefix = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                    byte[] buffer = encoding.GetBytes(xmlPrefix + requestBody.ToXml());
                    request.ContentType = "application/xml";
                    request.ContentLength = buffer.Length;
                    using (Stream requestStream = await request.GetRequestStreamAsync().ConfigureAwait(false))
                    {
                        await requestStream.WriteAsync(buffer, 0, buffer.Length);
                    }
                }

                if (file != null)
                {
                    string formDataBoundary = GenerateMultipartFormBoundary();
                    request.ContentType = MultipartFormContentType(formDataBoundary);

                    Dictionary<string, object> postParameters = requestBody.ToDictionary();
                    postParameters.Add("file", file);

                    byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);
                    request.ContentLength = formData.Length;
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(formData, 0, formData.Length);
                        requestStream.Close();
                    }
                }

                using (var response = (HttpWebResponse) await request.GetResponseAsync().ConfigureAwait(false))
                {
                    return await ParseResponseStreamAsync(GetResponseStream(response));
                }
            }
            catch (WebException e)
            {
                using (var response = (HttpWebResponse) e.Response)
                {
                    if (response == null) throw e;

                    if (response.StatusCode == (HttpStatusCode)422) // UnprocessableEntity
                    {
                        return await ParseResponseStreamAsync(GetResponseStream((HttpWebResponse)e.Response));
                    }

                    ThrowExceptionIfErrorStatusCode(response.StatusCode, null);
                }

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
                stream = new GZipStream(stream, CompressionMode.Decompress);
            }
            return stream;
        }
#endif

        private XmlNode ParseResponseStream(Stream stream)
        {
            string body;
            using (var streamReader = new StreamReader(stream))
            {
                body = streamReader.ReadToEnd();
            }

            return StringToXmlNode(body);
        }

        private async Task<XmlNode> ParseResponseStreamAsync(Stream stream)
        {
            string body;
            using (var streamReader = new StreamReader(stream))
            {
                body = await streamReader.ReadToEndAsync().ConfigureAwait(false);
            }

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
#if netcore
                // Do not need to set XmlResolver property for netcore
#else
                doc.XmlResolver = null;
#endif
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

        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;

            foreach (var param in postParameters)
            {
                if (needsCLRF)
                {
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));
                }
                needsCLRF = true;

                if (param.Value is FileStream)
                {
                    FileStream fileToUpload = (FileStream)param.Value;
                    string filename = fileToUpload.Name;
                    string mimeType = GetMIMEType(filename);
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                            boundary,
                            param.Key,
                            filename ?? param.Key,
                            mimeType ?? "application/octet-stream");

                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                    byte[] fileData = null;
                    using (FileStream fs = fileToUpload)
                    {
                        var binaryReader = new BinaryReader(fs, encoding);
                        fileData = binaryReader.ReadBytes((int)fs.Length);
                    }
                    formDataStream.Write(fileData, 0, fileData.Length);
                }
                else
                {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                            boundary,
                            param.Key,
                            param.Value);
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }

            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Dispose();

            return formData;
        }

        private static readonly Dictionary<string, string> MIMETypesDictionary = new Dictionary<string, string>
        {
            {"bmp", "image/bmp"},
                {"jpe", "image/jpeg"},
                {"jpeg", "image/jpeg"},
                {"jpg", "image/jpeg"},
                {"pdf", "application/pdf"},
                {"png", "image/png"}
        };

        public static string GetMIMEType(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLowerInvariant();

            if (extension.Length > 0 && MIMETypesDictionary.ContainsKey(extension.Remove(0, 1)))
            {
                return MIMETypesDictionary[extension.Remove(0, 1)];
            }
            return "application/octet-stream";
        }

        public static string GenerateMultipartFormBoundary()
        {
            return String.Format("---------------------{0:N}", Guid.NewGuid());
        }

        public static string MultipartFormContentType(string boundary)
        {
            return "multipart/form-data; boundary=" + boundary;
        }
    }
}
