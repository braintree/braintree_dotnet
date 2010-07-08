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
            return ToXml("subscription");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        public override String ToQueryString()
        {
            throw new NotImplementedException();
        }

        public override String ToQueryString(String root)
        {
            throw new NotImplementedException();
        }

        public virtual RequestBuilder BuildRequest(String root)
        {
            var builder = new RequestBuilder(root);

            builder.AddElement("payment-method-token", PaymentMethodToken);
            if (HasTrialPeriod.HasValue)
            {
                builder.AddElement("trial-period", HasTrialPeriod.Value);

                if (HasTrialPeriod.Value)
                {
                    if (TrialDuration != 0) builder.AddElement("trial-duration", TrialDuration.ToString());
                    builder.AddElement("trial-duration-unit", TrialDurationUnit.ToString().ToLower());
                }
            }
            builder.AddElement("merchant-account-id", MerchantAccountId);
            builder.AddElement("id", Id);
            builder.AddElement("plan-id", PlanId);
            if (Price != 0) builder.AddElement("price", Price.ToString());

            return builder;
        }
    }
}
