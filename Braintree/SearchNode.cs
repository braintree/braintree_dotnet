#pragma warning disable 1591

using System;

namespace Braintree
{
    public class SearchNode
    {
        protected String Name;
        protected SubscriptionSearchRequest Parent;

        public SearchNode(String name, SubscriptionSearchRequest parent)
        {
            Name = name;
            Parent = parent;
        }
    }
}
