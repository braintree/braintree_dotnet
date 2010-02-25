using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class SubscriptionRequest : Request
    {
        public Boolean? HasTrialPeriod { get; set; }
        public String Id { get; set; }
        public String PaymentMethodToken { get; set; }
        public String PlanId { get; set; }
        public Decimal Price { get; set; }
        public Int32 TrialDuration { get; set; }
        public SubscriptionDurationUnit TrialDurationUnit { get; set; }

        internal override String ToXml()
        {
            return ToXml("subscription");
        }

        internal override String ToXml(String rootElement)
        {
            var builder = new StringBuilder();
            builder.Append(String.Format("<{0}>", rootElement));
            builder.Append(BuildXMLElement("payment-method-token", PaymentMethodToken));
            if (HasTrialPeriod.HasValue)
            {
                builder.Append(BuildXMLElement("trial-period", HasTrialPeriod.Value));

                if (HasTrialPeriod.Value)
                {
                    if (TrialDuration != 0) builder.Append(BuildXMLElement("trial-duration", TrialDuration.ToString()));
                    builder.Append(BuildXMLElement("trial-duration-unit", TrialDurationUnit.ToString().ToLower()));
                }
            }
            builder.Append(BuildXMLElement("id", Id));
            builder.Append(BuildXMLElement("plan-id", PlanId));
            if (Price != 0) builder.Append(BuildXMLElement("price", Price.ToString()));
            builder.Append(String.Format("</{0}>", rootElement));
            return builder.ToString();
        }

        public override String ToQueryString()
        {
            throw new NotImplementedException();
        }

        public override String ToQueryString(String root)
        {
            throw new NotImplementedException();
        }
    }
}
