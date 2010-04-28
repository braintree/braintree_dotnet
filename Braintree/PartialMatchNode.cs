#pragma warning disable 1591

using System;

namespace Braintree
{
    public class PartialMatchNode<T> : EqualityNode<T> where T : SearchRequest
    {
        public PartialMatchNode(String name, T parent) : base(name, parent)
        {
        }

        public T EndsWith(String value) {
            Parent.AddCriteria(Name, new SearchCriteria("ends-with", value));
            return Parent;
        }

        public T StartsWith(String value) {
            Parent.AddCriteria(Name, new SearchCriteria("starts-with", value));
            return Parent;
        }
    }
}
