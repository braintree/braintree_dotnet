using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Braintree
{
    public class BasicProxy : IWebProxy
    {
        private readonly Uri _uri;
        public BasicProxy(string uri)
        {
            _uri = new Uri(uri);
        }
        public Uri GetProxy(Uri destination)
        {
            return _uri;
        }

        public bool IsBypassed(Uri host)
        {
            return false;
        }

        public ICredentials Credentials { get; set; }
    }
}
