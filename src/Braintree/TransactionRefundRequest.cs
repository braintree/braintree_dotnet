#pragma warning disable 1591

namespace Braintree
{
    public class TransactionRefundRequest : Request
    {
        public decimal Amount { get; set; }
        public string MerchantAccountId { get; set; }
        public string OrderId { get; set; }

        public override string ToXml()
        {
            return ToXml("transaction");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("transaction");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            var builder = new RequestBuilder(root);

            if (Amount != 0)
                builder.AddElement("amount", Amount);

            builder.AddElement("merchant-account-id", MerchantAccountId);
            builder.AddElement("order-id", OrderId);

            return builder;
        }
    }
}
