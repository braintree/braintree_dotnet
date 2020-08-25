#pragma warning disable 1591

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
        public DateRangeNode<SubscriptionSearchRequest> NextBillingDate
        {
            get
            {
                return new DateRangeNode<SubscriptionSearchRequest>("next-billing-date", this);
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
        public RangeNode<SubscriptionSearchRequest> CreatedAt
        {
            get
            {
                return new RangeNode<SubscriptionSearchRequest>("created-at", this);
            }
        }
        public EnumMultipleValueNode<SubscriptionSearchRequest, SubscriptionStatus> Status
        {
            get
            {
                return new EnumMultipleValueNode<SubscriptionSearchRequest, SubscriptionStatus>("status", this);
            }
        }
        public TextNode<SubscriptionSearchRequest> TransactionId
        {
            get
            {
                return new TextNode<SubscriptionSearchRequest>("transaction-id", this);
            }
        }
        public SubscriptionSearchRequest() : base()
        {
        }
    }
}
