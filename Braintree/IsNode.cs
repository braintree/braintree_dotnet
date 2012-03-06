#pragma warning disable 1591

using System;

namespace Braintree
{
    public class IsNode<T> : SearchNode<T> where T : SearchRequest
    {
        public IsNode(String name, T parent) : base(name, parent)
        {
        }

        public T Is(String value) {
            Parent.AddCriteria(Name, new SearchCriteria("is", value));
            return Parent;
        }

    }
}
