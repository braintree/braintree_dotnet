#pragma warning disable 1591

using System;

namespace Braintree
{
    public class SubscriptionSearchRequest : SearchRequest
    {
        public RangeNode<SubscriptionSearchRequest> BillingCyclesRemaining
        {
            get
            {
                return new RangeNode<SubscriptionSearchRequest>("billing-cycles-remaining", this);
            }
        }
        public TextNode<SubscriptionSearchRequest> Id
        {
            get
            {
                return new TextNode<SubscriptionSearchRequest>("id", this);
            }
        }
        public MultipleValueNode<SubscriptionSearchRequest, string> Ids
        {
            get
            {
                return new MultipleValueNode<SubscriptionSearchRequest, string>("ids", this);
            }
        }
        public BooleanMultipleValueNode<SubscriptionSearchRequest> InTrialPeriod
        {
            get
            {
                return new BooleanMultipleValueNode<SubscriptionSearchRequest>("in_trial_period", this);
            }
        }
       public MultipleValueNode<SubscriptionSearchRequest, string> MerchantAccountId
        {
            get
            {
                return new MultipleValueNode<SubscriptionSearchRequest, string>("merchant-account-id", this);
            }
        }
        public MultipleValueOrTextNode<SubscriptionSearchRequest, string> PlanId
        {
            get
            {
                return new MultipleValueOrTextNode<SubscriptionSearchRequest, string>("plan-id", this);
            }
        }
        public RangeNode<SubscriptionSearchRequest> Price
        {
            get
            {
                return new RangeNode<SubscriptionSearchRequest>("price", this);
            }
        }
        public RangeNode<SubscriptionSearchRequest> DaysPastDue
        {
            get
            {
                return new RangeNode<SubscriptionSearchRequest>("days-past-due", this);
            }
        }
        public MultipleValueNode<SubscriptionSearchRequest, SubscriptionStatus> Status
        {
            get
            {
                return new MultipleValueNode<SubscriptionSearchRequest, SubscriptionStatus>("status", this);
            }
        }

        public SubscriptionSearchRequest() : base()
        {
        }
    }
}
