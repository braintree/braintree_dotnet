#pragma warning disable 1591

namespace Braintree
{
    public class SubscriptionSearchRequest : SearchRequest
    {
        public RangeNode<SubscriptionSearchRequest> BillingCyclesRemaining => new RangeNode<SubscriptionSearchRequest>("billing-cycles-remaining", this);

        public TextNode<SubscriptionSearchRequest> Id => new TextNode<SubscriptionSearchRequest>("id", this);

        public MultipleValueNode<SubscriptionSearchRequest, string> Ids => new MultipleValueNode<SubscriptionSearchRequest, string>("ids", this);

        public BooleanMultipleValueNode<SubscriptionSearchRequest> InTrialPeriod => new BooleanMultipleValueNode<SubscriptionSearchRequest>("in_trial_period", this);

        public MultipleValueNode<SubscriptionSearchRequest, string> MerchantAccountId => new MultipleValueNode<SubscriptionSearchRequest, string>("merchant-account-id", this);

        public DateRangeNode<SubscriptionSearchRequest> NextBillingDate => new DateRangeNode<SubscriptionSearchRequest>("next-billing-date", this);

        public MultipleValueOrTextNode<SubscriptionSearchRequest, string> PlanId => new MultipleValueOrTextNode<SubscriptionSearchRequest, string>("plan-id", this);

        public RangeNode<SubscriptionSearchRequest> Price => new RangeNode<SubscriptionSearchRequest>("price", this);

        public RangeNode<SubscriptionSearchRequest> DaysPastDue => new RangeNode<SubscriptionSearchRequest>("days-past-due", this);

        public RangeNode<SubscriptionSearchRequest> CreatedAt => new RangeNode<SubscriptionSearchRequest>("created-at", this);

        public EnumMultipleValueNode<SubscriptionSearchRequest, SubscriptionStatus> Status => new EnumMultipleValueNode<SubscriptionSearchRequest, SubscriptionStatus>("status", this);

        public TextNode<SubscriptionSearchRequest> TransactionId => new TextNode<SubscriptionSearchRequest>("transaction-id", this);

        public SubscriptionSearchRequest() : base()
        {
        }
    }
}
