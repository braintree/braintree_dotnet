#pragma warning disable 1591

using System;

namespace Braintree
{
    public class SubscriptionSearchRequest : SearchRequest
    {
        public SubscriptionSearchRequest() : base()
        {
        }

        public virtual TextNode<SubscriptionSearchRequest> PlanId()
        {
            return new TextNode<SubscriptionSearchRequest>("plan-id", this);
        }

        public virtual TextNode<SubscriptionSearchRequest> DaysPastDue()
        {
            return new TextNode<SubscriptionSearchRequest>("days-past-due", this);
        }

        public virtual MultipleValueNode<SubscriptionSearchRequest> Status()
        {
            return new MultipleValueNode<SubscriptionSearchRequest>("status", this);
        }
    }
}
