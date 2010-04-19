#pragma warning disable 1591

using System;

namespace Braintree
{
    public class TextNode : SearchNode
    {
        public TextNode(String name, SubscriptionSearchRequest parent) : base(name, parent)
        {
        }

        public SubscriptionSearchRequest Contains(String value) {
            Parent.AddCriteria(Name, new SearchCriteria("contains", value));
            return Parent;
        }

        public SubscriptionSearchRequest EndsWith(String value) {
            Parent.AddCriteria(Name, new SearchCriteria("ends-with", value));
            return Parent;
        }

        public SubscriptionSearchRequest Is(String value) {
            Parent.AddCriteria(Name, new SearchCriteria("is", value));
            return Parent;
        }

        public SubscriptionSearchRequest IsNot(String value) {
            Parent.AddCriteria(Name, new SearchCriteria("is-not", value));
            return Parent;
        }

        public SubscriptionSearchRequest StartsWith(String value) {
            Parent.AddCriteria(Name, new SearchCriteria("starts-with", value));
            return Parent;
        }
    }
}
