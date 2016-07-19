#pragma warning disable 1591

using System;

namespace Braintree
{
    public class PartialMatchNode<T> : EqualityNode<T> where T : SearchRequest
    {
        public PartialMatchNode(string name, T parent) : base(name, parent)
        {
        }

        public T EndsWith(string value) {
            Parent.AddCriteria(Name, new SearchCriteria("ends-with", value));
            return Parent;
        }

        public T StartsWith(string value) {
            Parent.AddCriteria(Name, new SearchCriteria("starts-with", value));
            return Parent;
        }
    }
}
