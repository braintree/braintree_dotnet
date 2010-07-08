#pragma warning disable 1591

using System;
using System.Text;

namespace Braintree
{
    public class SubscriptionTransactionRequest : Request
    {
        public Decimal Amount { get; set; }
        public String SubscriptionId { get; set; }

        public SubscriptionTransactionRequest ()
        {
        }

        public override String ToQueryString()
        {
            return null;
        }

        public override String ToQueryString(String root)
        {
            return null;
        }

        public override String ToXml()
        {
            return ToXml("transaction");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            RequestBuilder builder = new RequestBuilder(root);

            if (Amount != 0) builder.AddElement("amount", Amount.ToString());
            builder.AddElement("subscription-id", SubscriptionId);
            builder.AddElement("type", TransactionType.SALE.ToString().ToLower());

            return builder;
        }
    }
}
