#pragma warning disable 1591

using System;

namespace Braintree
{
    public class MultipleValueNode
    {
        private String Name;
        private SubscriptionSearchRequest Parent;

        public MultipleValueNode(String name, SubscriptionSearchRequest parent)
        {
            Name = name;
            Parent = parent;
        }

        public SubscriptionSearchRequest IncludedIn(params object[] values) {
            Parent.AddMultipleValueCriteria(Name, new SearchCriteria(values));
            return Parent;
        }
    }
}
