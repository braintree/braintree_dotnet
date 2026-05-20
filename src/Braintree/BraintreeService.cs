#pragma warning disable 1591

using Braintree.Exceptions;
using System;
using System.Net;
using System.IO;
#if netcore
using System.Net.Http;
#else
using System.IO.Compression;
#endif
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace Braintree
{
    public class BraintreeService : HttpService
    {
        public string ApiVersion = "6";

        public BraintreeService(Configuration configuration) : base(configuration) { }

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
            try
            {
                return GetXmlResponse(URL, "POST", requestBody, file, file?.Name);
            }
            finally
            {
                file?.Dispose();
            }
        }

        public Task<XmlNode> PostMultipartAsync(string URL, Request requestBody, FileStream file)
        {
            return PostMultipartFileStreamAsyncInternal(URL, requestBody, file);
        }

        private async Task<XmlNode> PostMultipartFileStreamAsyncInternal(string URL, Request requestBody, FileStream file)
        {
            try
            {
                return await GetXmlResponseAsync(URL, "POST", requestBody, file, file?.Name).ConfigureAwait(false);
            }
            finally
            {
                file?.Dispose();
            }
        }

        public XmlNode PostMultipart(string URL, Request requestBody, Stream fileStream, string fileName)
        {
            return GetXmlResponse(URL, "POST", requestBody, fileStream, fileName);
        }

        public Task<XmlNode> PostMultipartAsync(string URL, Request requestBody, Stream fileStream, string fileName)
        {
            return GetXmlResponseAsync(URL, "POST", requestBody, fileStream, fileName);
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
        public override void SetRequestHeaders(HttpRequestMessage request)
        {
            base.SetRequestHeaders(request);
            request.Headers.Add("X-ApiVersion", ApiVersion);
            request.Headers.Add("Accept", "application/xml");
        }
#else
        public override void SetRequestHeaders(HttpWebRequest request)
        {
            base.SetRequestHeaders(request);
            request.Headers.Add("X-ApiVersion", ApiVersion);
            request.Accept = "application/xml";
        }
#endif

        private bool IsValidURL(string URL)
        {
            var url = Environment.GatewayURL + URL;
            var uri = new Uri(url);
            return String.Equals(URL, uri.PathAndQuery);
        }

        private XmlNode GetXmlResponse(string URL, string method, Request requestBody, Stream fileStream, string fileName = null)
        {
            if (!IsValidURL(URL))
            {
                throw new ArgumentException($"Path: {URL} is malformed.");
            }
#if netcore
            var request = GetHttpRequest(Environment.GatewayURL + URL, method);

            if (requestBody != null && fileStream == null)
            {
                var xmlPrefix = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                var content = xmlPrefix + requestBody.ToXml();
                var utf8_string = encoding.GetString(encoding.GetBytes(content));
                request.Content = new StringContent(utf8_string, encoding, "application/xml");
                request.Content.Headers.ContentLength = System.Text.UTF8Encoding.UTF8.GetByteCount(utf8_string);
            }

            if (fileStream != null)
            {
                string formDataBoundary = GenerateMultipartFormBoundary();

                Dictionary<string, object> postParameters = requestBody.ToDictionary();
                postParameters.Add("file", new StreamWithName { Stream = fileStream, Name = fileName });

                byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);
                request.Content = new ByteArrayContent(formData);
                request.Content.Headers.Remove("Content-Type");
                request.Content.Headers.TryAddWithoutValidation("Content-Type", MultipartFormContentType(formDataBoundary));
                request.Content.Headers.ContentLength = formData.Length;
            }

            return StringToXmlNode(GetHttpResponse(request));
#else
            var request = GetHttpRequest(Environment.GatewayURL + URL, method);

            if (requestBody != null && fileStream == null)
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

            if (fileStream != null)
            {
                string formDataBoundary = GenerateMultipartFormBoundary();
                request.ContentType = MultipartFormContentType(formDataBoundary);

                Dictionary<string, object> postParameters = requestBody.ToDictionary();
                postParameters.Add("file", new StreamWithName { Stream = fileStream, Name = fileName });

                byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);
                request.ContentLength = formData.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(formData, 0, formData.Length);
                    requestStream.Close();
                }
            }

            var response = GetHttpResponse(request);

            return StringToXmlNode(response);

#endif
        }

        private async Task<XmlNode> GetXmlResponseAsync(string URL, string method, Request requestBody, Stream fileStream, string fileName = null)
        {
            if (!IsValidURL(URL))
            {
                throw new ArgumentException($"Path: {URL} is malformed.");
            }

#if netcore
            var request = GetHttpRequest(Environment.GatewayURL + URL, method);

            if (requestBody != null && fileStream == null)
            {
                var xmlPrefix = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                var content = xmlPrefix + requestBody.ToXml();
                var utf8_string = encoding.GetString(encoding.GetBytes(content));
                request.Content = new StringContent(utf8_string, encoding, "application/xml");
                request.Content.Headers.ContentLength = System.Text.UTF8Encoding.UTF8.GetByteCount(utf8_string);
            }

            if (fileStream != null)
            {
                string formDataBoundary = GenerateMultipartFormBoundary();

                Dictionary<string, object> postParameters = requestBody.ToDictionary();
                postParameters.Add("file", new StreamWithName { Stream = fileStream, Name = fileName });

                byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);
                request.Content = new ByteArrayContent(formData);
                request.Content.Headers.Remove("Content-Type");
                request.Content.Headers.TryAddWithoutValidation("Content-Type", MultipartFormContentType(formDataBoundary));
                request.Content.Headers.ContentLength = formData.Length;
            }

            return StringToXmlNode(await GetHttpResponseAsync(request).ConfigureAwait(false));
#else
            var request = GetHttpRequest(Environment.GatewayURL + URL, method);

            if (requestBody != null && fileStream == null)
            {
                var xmlPrefix = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                byte[] buffer = encoding.GetBytes(xmlPrefix + requestBody.ToXml());
                request.ContentType = "application/xml";
                request.ContentLength = buffer.Length;
                using (Stream requestStream = await request.GetRequestStreamAsync().ConfigureAwait(false))
                {
                    await requestStream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                }
            }

            if (fileStream != null)
            {
                string formDataBoundary = GenerateMultipartFormBoundary();
                request.ContentType = MultipartFormContentType(formDataBoundary);

                Dictionary<string, object> postParameters = requestBody.ToDictionary();
                postParameters.Add("file", new StreamWithName { Stream = fileStream, Name = fileName });

                byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);
                request.ContentLength = formData.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(formData, 0, formData.Length);
                    requestStream.Close();
                }
            }

            var response = await GetHttpResponseAsync(request).ConfigureAwait(false);

            return StringToXmlNode(response);
#endif
        }

        private XmlNode ParseResponseStreamAsXml(Stream stream)
        {
            string body = ParseResponseStream(stream);

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
                // XmlResolver property defaults to null
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

    }
}
