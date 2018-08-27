#pragma warning disable 1591

using System;

namespace Braintree
{
    public class SubscriptionDetails
    {
        public virtual DateTime? BillingPeriodEndDate { get; protected set; }
        public virtual DateTime? BillingPeriodStartDate { get; protected set; }

        public SubscriptionDetails(NodeWrapper node)
        {
            BillingPeriodEndDate = node.GetDateTime("billing-period-end-date");
            BillingPeriodStartDate = node.GetDateTime("billing-period-start-date");
        }

        protected internal SubscriptionDetails() { }
    }
}
