#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    /// <summary>
    /// A class for building requests to manipulate <see cref="Subscription"/> records in the vault.
    /// </summary>
    /// <example>
    /// A subscription request can be constructed as follows:
    /// <code>
    /// SubscriptionRequest request = new SubscriptionRequest
    /// {
    ///     PaymentMethodToken = "token",
    ///     PlanId = "planId",
    ///     HasTrialPeriod = true,
    ///     TrialDuration = 2,
    ///     TrialDurationUnit = SubscriptionDurationUnit.MONTH
    /// };
    /// </code>
    /// </example>
    public class SubscriptionRequest : Request
    {
        public Boolean? HasTrialPeriod { get; set; }
        public String Id { get; set; }
        public String PaymentMethodToken { get; set; }
        public String PlanId { get; set; }
        public Decimal Price { get; set; }
        public Int32 TrialDuration { get; set; }
        public SubscriptionDurationUnit TrialDurationUnit { get; set; }
        public String MerchantAccountId { get; set; }

        public override String ToXml()
        {
            return Build(new XmlRequestBuilder("subscription"));
        }

        protected String Build(XmlRequestBuilder builder)
        {
            builder.
                Append("id", Id).
                Append("merchant_account_id", MerchantAccountId).
                Append("payment_method_token", PaymentMethodToken).
                Append("plan_id", PlanId);

            if (Price != 0) builder.Append("price", Price);

            if (HasTrialPeriod.HasValue)
            {
                builder.Append("trial_period", HasTrialPeriod);

                if (HasTrialPeriod.Value)
                {
                    if (TrialDuration != 0) builder.Append("trial_duration", TrialDuration);
                    builder.Append("trial_duration_unit", TrialDurationUnit.ToString());
                }
            }

            Console.WriteLine(builder.ToString());
            return builder.ToString();
        }

        // -------------------------------

        public override String ToXml(String rootElement)
        {
            return Build(new XmlRequestBuilder(rootElement));
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
