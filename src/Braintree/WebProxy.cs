using System;
using System.Net;

namespace Braintree
{
    public class WebProxy : IWebProxy
    {
        public Uri ProxyUri { get; set; }
        public ICredentials Credentials { get; set; }

        public WebProxy(string proxyUri)
        {
            ProxyUri = new Uri(proxyUri);
        }

        public WebProxy(Uri proxyUri)
        {
            ProxyUri = proxyUri;
        }

        public Uri GetProxy(Uri destination)
        {
            return ProxyUri;
        }

        public bool IsBypassed(Uri host)
        {
            return false;
        }
    }
}
