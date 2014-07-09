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
        private readonly int DEFAULT_VERSION = 2;

        public String CustomerId { get; set; }
        public int Version { get; set; }
        public ClientTokenOptionsRequest Options { get; set; }
        public String MerchantAccountId { get; set; }

        public ClientTokenRequest()
        {
            Version = DEFAULT_VERSION;
        }

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
            if (Version != 0) builder.AddElement("version", Version);
            if (MerchantAccountId != null) builder.AddElement("merchant-account-id", MerchantAccountId);
            if (Options != null) builder.AddElement("options", Options);

            return builder;
        }
    }
}
