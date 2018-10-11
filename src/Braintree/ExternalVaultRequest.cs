#pragma warning disable 1591

namespace Braintree
{
    public class ExternalVaultRequest : Request
    {
        public string Status { get; set; }
        public string PreviousNetworkTransactionId { get; set; }

        public override string ToXml()
        {
            return ToXml("external-vault");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("external-vault");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("status", Status).
                AddElement("previous-network-transaction-id", PreviousNetworkTransactionId);
        }
    }
}

#pragma warning restore 1591
