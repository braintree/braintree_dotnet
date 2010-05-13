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
        public bool? HasTrialPeriod { get; set; }
        public string Id { get; set; }
        public string PaymentMethodToken { get; set; }
        public string PlanId { get; set; }
        public decimal? Price { get; set; }
        public int? TrialDuration { get; set; }
        public SubscriptionDurationUnit TrialDurationUnit { get; set; }
        public string MerchantAccountId { get; set; }

        public override String ToXml()
        {
            return Build(new XmlRequestBuilder("subscription"));
        }

        protected String Build(RequestBuilder builder)
        {
            builder.
                Append("id", Id).
                Append("merchant_account_id", MerchantAccountId).
                Append("payment_method_token", PaymentMethodToken).
                Append("plan_id", PlanId).
                Append("price", Price).
                Append("trial_period", HasTrialPeriod).
                Append("trial_duration", TrialDuration).
                Append("trial_duration_unit", TrialDurationUnit);

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
