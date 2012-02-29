#pragma warning disable 1591

using System;

namespace Braintree
{
    public class EqualityNode<T> : IsNode<T> where T : SearchRequest
    {
        public EqualityNode(String name, T parent) : base(name, parent)
        {
        }

        public T IsNot(String value) {
            Parent.AddCriteria(Name, new SearchCriteria("is-not", value));
            return Parent;
        }
    }
}
