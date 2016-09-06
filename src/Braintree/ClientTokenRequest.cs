#pragma warning disable 1591

namespace Braintree
{
    /// <summary>
    /// A class for building requests to generate client tokens, which are used
    ///   to authenticate requests clients make directly on behalf of merchants
    /// </summary>
    public class ClientTokenRequest : Request
    {
        private readonly int DEFAULT_VERSION = 2;

        public string CustomerId { get; set; }
        public int Version { get; set; }
        public ClientTokenOptionsRequest Options { get; set; }
        public string MerchantAccountId { get; set; }

        public ClientTokenRequest()
        {
            Version = DEFAULT_VERSION;
        }

        public override string ToXml()
        {
            return ToXml("client-token");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            var builder = new RequestBuilder(root);

            if (CustomerId != null) builder.AddElement("customer-id", CustomerId);
            if (Version != 0) builder.AddElement("version", Version);
            if (MerchantAccountId != null) builder.AddElement("merchant-account-id", MerchantAccountId);
            if (Options != null) builder.AddElement("options", Options);

            return builder;
        }
    }
}
