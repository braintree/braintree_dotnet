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
        protected HttpClient httpClient;
#endif

        protected Configuration Configuration;

        public Environment Environment => Configuration.Environment;

        public string MerchantId => Configuration.MerchantId;

        public string PublicKey => Configuration.PublicKey;

        public string PrivateKey => Configuration.PrivateKey;

        public string ClientId => Configuration.ClientId;

        public string ClientSecret => Configuration.ClientSecret;

        public IWebProxy WebProxy => Configuration.WebProxy;

        public HttpService(Configuration configuration)
        {
            Configuration = configuration;
            // NEXT_MAJOR_VERSION setting Configuration in an existing gateway instance does NOT update
            // the underlying httpClient in this service. We should pull Proxy and Timeout settings out
            // of the Configuration class for easier setting an existing gateway
#if netcore
            var httpClientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            };

            if (Configuration.WebProxy != null)
            {
                httpClientHandler.UseProxy = true;
                httpClientHandler.Proxy = Configuration.WebProxy;
            }

            httpClient = new HttpClient(httpClientHandler, false);
            httpClient.Timeout = TimeSpan.FromMilliseconds(Configuration.Timeout);
#endif
        }

#if netcore
        public virtual void SetRequestHeaders(HttpRequestMessage request)
        {
            request.Headers.Add("Accept-Encoding", "gzip");
            request.Headers.Add("User-Agent", "Braintree .NET " + typeof(BraintreeService).GetTypeInfo().Assembly.GetName().Version.ToString());
            request.Headers.Add("Keep-Alive", "false");
            // NET Core sends Expect:100-Continue by default which causes sporadic API errors. This setting turns off this default behavior.
            request.Headers.ExpectContinue = false;
        }
#else
        public virtual void SetRequestHeaders(HttpWebRequest request)
        {
            request.Headers.Add("Authorization", GetAuthorizationHeader());
            request.Headers.Add("Accept-Encoding", "gzip");
            request.UserAgent = "Braintree .NET " + typeof(BraintreeService).Assembly.GetName().Version.ToString();
            // NET Framework sends Expect:100-Continue by default which causes sporadic API errors. This setting turns off this default behavior.
            request.ServicePoint.Expect100Continue = false;
        }
#endif

#if netcore
        public HttpRequestMessage GetHttpRequest(string URL, string method)
        {
            var request = Configuration.HttpRequestMessageFactory(new HttpMethod(method), URL);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(GetAuthorizationSchema(), GetAuthorizationHeader());
            SetRequestHeaders(request);
            return request;
        }

        public string GetHttpResponse(HttpRequestMessage request)
        {
            using (var response = httpClient.SendAsync(request).GetAwaiter().GetResult())
            {
                using (var responseStream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult())
                {
                    string responseContent = ParseResponseStream(responseStream);
                    ThrowExceptionIfErrorStatusCode(response.StatusCode, responseContent);
                    return responseContent;
                }
            }
        }

        public async Task<string> GetHttpResponseAsync(HttpRequestMessage request)
        {
            using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    string responseContent = await ParseResponseStreamAsync(responseStream);
                    ThrowExceptionIfErrorStatusCode(response.StatusCode, responseContent);
                    return responseContent;
                }
            }
        }
#else
        public HttpWebRequest GetHttpRequest(string URL, string method)
        {
            const int SecurityProtocolTypeTls12 = 3072;
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | ((SecurityProtocolType)SecurityProtocolTypeTls12);

            var request = Configuration.HttpWebRequestFactory(URL);
            SetRequestProxy(request);
            SetRequestHeaders(request);
            request.Method = method;
            request.KeepAlive = false;
            request.Timeout = Configuration.Timeout;
            request.ReadWriteTimeout = Configuration.Timeout;

            return request;
        }

        public string GetHttpResponse(HttpWebRequest request)
        {
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var responseStream = GetResponseStream(response))
                    {
                        return ParseResponseStream(responseStream);
                    }
                }
            }
            catch (WebException e)
            {
                using (var response = (HttpWebResponse)e.Response)
                {
                    if (response == null) throw;
                    using (var responseStream = GetResponseStream((HttpWebResponse)e.Response))
                    {
                        string responseContent = ParseResponseStream(responseStream);
                        ThrowExceptionIfErrorStatusCode(response.StatusCode, responseContent);
                        return responseContent;
                    }
                }
                throw;
            }
        }

        public async Task<string> GetHttpResponseAsync(HttpWebRequest request)
        {
            try
            {
                using (var response = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (var responseStream = GetResponseStream(response))
                    {
                        return await ParseResponseStreamAsync(responseStream).ConfigureAwait(false);
                    }
                }
            }
            catch (WebException e)
            {
                using (var response = (HttpWebResponse)e.Response)
                {
                    if (response == null) throw;
                    using (var responseStream = GetResponseStream((HttpWebResponse)e.Response))
                    {
                        string responseContent = await ParseResponseStreamAsync(responseStream).ConfigureAwait(false);
                        ThrowExceptionIfErrorStatusCode(response.StatusCode, responseContent);
                        return responseContent;
                    }
                }
                throw;
            }
        }
#endif

#if net452
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
                switch ((int)httpStatusCode)
                {
                    case 401:
                        throw new AuthenticationException();
                    case 403:
                        throw new AuthorizationException(message);
                    case 404:
                        throw new NotFoundException();
                    case 408:
                        throw new RequestTimeoutException();
                    case 422:
                        throw new UnprocessableEntityException(message);
                    case 426:
                        throw new UpgradeRequiredException();
                    case 429:
                        throw new TooManyRequestsException();
                    case 500:
                        throw new ServerException();
                    case 503:
                        throw new ServiceUnavailableException();
                    case 504:
                        throw new GatewayTimeoutException();
                    default:
                        throw new UnexpectedException("Unexpected HTTP_RESPONSE " + httpStatusCode + ". Response: " + message);
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
                    string header =
                        $"--{boundary}\r\nContent-Disposition: form-data; name=\"{param.Key}\"; filename=\"{filename ?? param.Key}\"\r\nContent-Type: {mimeType ?? "application/octet-stream"}\r\n\r\n";

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
                    string postData =
                        $"--{boundary}\r\nContent-Disposition: form-data; name=\"{param.Key}\"\r\n\r\n{param.Value}";
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
            return $"---------------------{Guid.NewGuid():N}";
        }

        public static string MultipartFormContentType(string boundary)
        {
            return "multipart/form-data; boundary=" + boundary;
        }
    }
}

#pragma warning restore 1591
