#pragma warning disable 1591

using System;
using System.Collections.Generic;

namespace Braintree
{
    public class BooleanMultipleValueNode<T> : MultipleValueNode<T, String> where T : SearchRequest
    {
        public BooleanMultipleValueNode (String name, T parent) : base(name, parent)
        {
        }

        public T IncludedIn(params Boolean[] values)
        {
            String[] stringValues = new List<Boolean>(values).ConvertAll(x => x.ToString()).ToArray();
            Parent.AddMultipleValueCriteria(Name, new SearchCriteria(stringValues));
            return Parent;
        }

        public T Is(Boolean value)
        {
            return IncludedIn(value.ToString());
        }
    }
}