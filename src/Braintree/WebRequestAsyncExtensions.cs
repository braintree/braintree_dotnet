#if net452

using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Braintree
{
    public static class WebRequestAsyncExtensions
    {
        public static Task<Stream> GetRequestStreamAsync(this WebRequest request)
        {
            return Task.Factory.FromAsync(request.BeginGetRequestStream, request.EndGetRequestStream, null);
        }

        public static Task<WebResponse> GetResponseAsync(this WebRequest request)
        {
            return Task.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null);
        }
    }
}

#endif