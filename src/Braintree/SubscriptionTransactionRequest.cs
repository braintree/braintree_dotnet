#pragma warning disable 1591

namespace Braintree
{
    public class SubscriptionTransactionRequest : Request
    {
        public decimal Amount { get; set; }
        public string SubscriptionId { get; set; }

        public override string ToXml()
        {
            return ToXml("transaction");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            var builder = new RequestBuilder(root);

            if (Amount != 0)
                builder.AddElement("amount", Amount);

            builder.AddElement("subscription-id", SubscriptionId);
            builder.AddElement("type", TransactionType.SALE.ToString().ToLower());

            return builder;
        }
    }
}
