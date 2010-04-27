#pragma warning disable 1591

using System;

namespace Braintree
{
    public class MultipleValueNode<T> : SearchNode<T> where T : SearchRequest
    {
        public MultipleValueNode(String name, T parent) : base(name, parent)
        {
        }

        public T IncludedIn(params object[] values) {
            Parent.AddMultipleValueCriteria(Name, new SearchCriteria(values));
            return Parent;
        }
    }
}
