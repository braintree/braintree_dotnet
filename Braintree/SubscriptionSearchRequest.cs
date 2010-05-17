#pragma warning disable 1591

using System;

namespace Braintree
{
    public class SubscriptionSearchRequest : SearchRequest
    {
        public MultipleValueNode<SubscriptionSearchRequest> Ids
        {
            get
            {
                return new MultipleValueNode<SubscriptionSearchRequest>("ids", this);
            }
        }
        public TextNode<SubscriptionSearchRequest> PlanId
        {
            get
            {
                return new TextNode<SubscriptionSearchRequest>("plan-id", this);
            }
        }
        public TextNode<SubscriptionSearchRequest> DaysPastDue
        {
            get
            {
                return new TextNode<SubscriptionSearchRequest>("days-past-due", this);
            }
        }
        public MultipleValueNode<SubscriptionSearchRequest> Status
        {
            get
            {
                return new MultipleValueNode<SubscriptionSearchRequest>("status", this);
            }
        }

        public SubscriptionSearchRequest() : base()
        {
        }
    }
}
