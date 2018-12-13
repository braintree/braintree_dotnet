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
using System.Collections;
using System.Collections.Generic;

namespace Braintree
{
    public class HttpService
    {
        protected static readonly Encoding encoding = Encoding.UTF8;
#if netcore
        protected static HttpClient staticClient = new HttpClient(new HttpClientHandler {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        }, false);
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

        public IWebProxy WebProxy
        {
            get { return Configuration.WebProxy;  }
        }

        public HttpService(Configuration configuration)
        {
            Configuration = configuration;
        }

#if netcore
        public virtual void SetRequestHeaders(HttpRequestMessage request)
        {
            request.Headers.Add("Accept-Encoding", "gzip");
            request.Headers.Add("User-Agent", "Braintree .NET " + typeof(BraintreeService).GetTypeInfo().Assembly.GetName().Version.ToString());
            request.Headers.Add("Keep-Alive", "false");
        }
#else
        public virtual void SetRequestHeaders(HttpWebRequest request)
        {
            request.Headers.Add("Authorization", GetAuthorizationHeader());
            request.Headers.Add("Accept-Encoding", "gzip");
            request.UserAgent = "Braintree .NET " + typeof(BraintreeService).Assembly.GetName().Version.ToString();
        }
#endif

#if netcore
        public HttpRequestMessage GetHttpRequest(string URL, string method) {
            var request = Configuration.HttpRequestMessageFactory(new HttpMethod(method), URL);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(GetAuthorizationSchema(), GetAuthorizationHeader());
            SetRequestHeaders(request);

            return request;
        }

        public string GetHttpResponseWithClient(HttpClient client, HttpRequestMessage request) {
            var response = client.SendAsync(request).GetAwaiter().GetResult();
            if (response.StatusCode != (HttpStatusCode) 422)
            {
                ThrowExceptionIfErrorStatusCode(response.StatusCode, null);
            }

            return ParseResponseStream(response.Content.ReadAsStreamAsync().GetAwaiter().GetResult());
        }

        public async Task<string> GetHttpResponseWithClientAsync(HttpClient client, HttpRequestMessage request) {
            var response = client.SendAsync(request).GetAwaiter().GetResult();
            if (response.StatusCode != (HttpStatusCode) 422)
            {
                ThrowExceptionIfErrorStatusCode(response.StatusCode, null);
            }

            return await ParseResponseStreamAsync(response.Content.ReadAsStreamAsync().GetAwaiter().GetResult());
        }

        public string GetHttpResponse(HttpRequestMessage request) {
            if (Configuration.UseStaticHttpClient == true) {
                return GetHttpResponseWithClient(staticClient, request);
            } else {
                var httpClientHandler = new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                };

                SetWebProxy(httpClientHandler, request.RequestUri);

                using (var client = new HttpClient(httpClientHandler))
                {
                    client.Timeout = TimeSpan.FromMilliseconds(Configuration.Timeout);
                    return GetHttpResponseWithClient(client, request);
                }
            }
        }

        public async Task<string> GetHttpResponseAsync(HttpRequestMessage request) {
            if (Configuration.UseStaticHttpClient == true) {
                return await GetHttpResponseWithClientAsync(staticClient, request);
            } else {
                var httpClientHandler = new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                };

                SetWebProxy(httpClientHandler, request.RequestUri);

                using (var client = new HttpClient(httpClientHandler))
                {
                    client.Timeout = TimeSpan.FromMilliseconds(Configuration.Timeout);
                    return await GetHttpResponseWithClientAsync(client, request);
                }
            }
        }
#else
        public HttpWebRequest GetHttpRequest(string URL, string method) {
            const int SecurityProtocolTypeTls12 = 3072;
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | ((SecurityProtocolType) SecurityProtocolTypeTls12);

            var request = Configuration.HttpWebRequestFactory(URL);
            SetRequestHeaders(request);
            SetRequestProxy(request);
            request.Method = method;
            request.KeepAlive = false;
            request.Timeout = Configuration.Timeout;
            request.ReadWriteTimeout = Configuration.Timeout;

            return request;
        }

        public string GetHttpResponse(HttpWebRequest request) {
            try {
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
        }

        public async Task<string> GetHttpResponseAsync(HttpWebRequest request) {
            try {
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
                        return await ParseResponseStreamAsync(GetResponseStream((HttpWebResponse) e.Response));
                    }

                    ThrowExceptionIfErrorStatusCode(response.StatusCode, null);
                }

                throw e;
            }
        }
#endif

#if netcore
        protected void SetWebProxy(HttpClientHandler httpClientHandler, Uri URL)
        {
            var proxy = GetWebProxy();
            bool useProxy = false;

            if (proxy != null && !proxy.IsBypassed(URL))
            {
                useProxy = true;
                httpClientHandler.Proxy = proxy;
            }

            httpClientHandler.UseProxy = useProxy;
        }
#else
        protected void SetRequestProxy(WebRequest request)
        {
            var proxy = GetWebProxy();
            if (proxy != null)
            {
                request.Proxy = proxy;
            }
        }

        protected Stream GetResponseStream(HttpWebResponse response)
        {
            var stream = response.GetResponseStream();
            if (response.ContentEncoding.Equals("gzip", StringComparison.CurrentCultureIgnoreCase))
            {
                stream = new GZipStream(stream, CompressionMode.Decompress);
            }
            return stream;
        }
#endif

        protected string ParseResponseStream(Stream stream)
        {
            string body;
            using (var streamReader = new StreamReader(stream))
            {
                body = streamReader.ReadToEnd();
            }

            return body;
        }

        protected async Task<string> ParseResponseStreamAsync(Stream stream)
        {
            string body;
            using (var streamReader = new StreamReader(stream))
            {
                body = await streamReader.ReadToEndAsync().ConfigureAwait(false);
            }

            return body;
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

        protected static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
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

        protected static readonly Dictionary<string, string> MIMETypesDictionary = new Dictionary<string, string>
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

#pragma warning restore 1591
