#pragma warning disable 1591

using System;

namespace Braintree
{
    public class TextNode
    {
        private String Name;
        private SubscriptionSearchRequest Parent;

        public TextNode(String name, SubscriptionSearchRequest parent)
        {
            Name = name;
            Parent = parent;
        }

        public SubscriptionSearchRequest Is(String value) {
            Parent.AddCriteria(Name, new SearchCriteria("is", value));
            return Parent;
        }
    }
}
