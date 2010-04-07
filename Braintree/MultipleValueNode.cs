#pragma warning disable 1591

using System;

namespace Braintree
{
    public class MultipleValueNode : SearchNode
    {
        public MultipleValueNode(String name, SubscriptionSearchRequest parent) : base(name, parent)
        {
        }

        public SubscriptionSearchRequest IncludedIn(params object[] values) {
            Parent.AddMultipleValueCriteria(Name, new SearchCriteria(values));
            return Parent;
        }
    }
}
