#pragma warning disable 1591

using System;

namespace Braintree
{
    public class EqualityNode<T> : SearchNode<T> where T : SearchRequest
    {
        public EqualityNode(String name, T parent) : base(name, parent)
        {
        }

        public T Is(String value) {
            Parent.AddCriteria(Name, new SearchCriteria("is", value));
            return Parent;
        }

        public T IsNot(String value) {
            Parent.AddCriteria(Name, new SearchCriteria("is-not", value));
            return Parent;
        }
    }
}
