#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    /// <summary>
    /// A class for building requests to generate client tokens, which are used
    ///   to authenticate requests clients make directly on behalf of merchants
    /// </summary>
    public class ClientTokenRequest : Request
    {
        public String CustomerId { get; set; }
        public ClientTokenOptionsRequest Options { get; set; }

        public override String ToXml()
        {
            return ToXml("client-token");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            RequestBuilder builder = new RequestBuilder(root);

            if (CustomerId != null) builder.AddElement("customer-id", CustomerId);
            if (Options != null) builder.AddElement("options", Options);

            return builder;
        }
    }
}
