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

        public override String ToXml(String rootElement)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("<{0}>", rootElement));
            if (Amount != 0) builder.Append(BuildXMLElement("amount", Amount.ToString()));
            builder.Append(BuildXMLElement("subscriptionId", SubscriptionId));
            builder.Append(BuildXMLElement("type", TransactionType.SALE.ToString().ToLower()));
            builder.Append(String.Format("</{0}>", rootElement));

            return builder.ToString ();
        }
    }
}
