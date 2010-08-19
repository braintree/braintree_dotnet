#pragma warning disable 1591

using System;
using System.Collections.Generic;

namespace Braintree
{
    public class MultipleValueNode<T, S> : SearchNode<T> where T : SearchRequest where S : class
    {
        public MultipleValueNode(String name, T parent) : base(name, parent)
        {
        }

        public T IncludedIn(params S[] values)
        {
            Parent.AddMultipleValueCriteria(Name, new SearchCriteria(values));
            return Parent;
        }

        public T Is(S value)
        {
            return IncludedIn(value);
        }
    }
}
