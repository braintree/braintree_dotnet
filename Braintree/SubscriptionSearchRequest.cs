#pragma warning disable 1591

using System;

namespace Braintree
{
    public class SubscriptionSearchRequest : SearchRequest
    {
        public MultipleValueNode<SubscriptionSearchRequest, string> Ids
        {
            get
            {
                return new MultipleValueNode<SubscriptionSearchRequest, string>("ids", this);
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
