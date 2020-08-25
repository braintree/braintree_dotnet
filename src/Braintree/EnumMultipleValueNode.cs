using System;
using System.Collections.Generic;
using System.Linq;

namespace Braintree
{
    public class EnumMultipleValueNode<T, S> : MultipleValueNode<T, string> where T : SearchRequest where S : struct, Enum
    {
        public EnumMultipleValueNode(string name, T parent) : base(name, parent)
        {
        }

        public T IncludedIn(params S[] values)
        {
            string[] stringValues = new List<S>(values).Select(x => x.GetDescription()).ToArray();
            Parent.AddMultipleValueCriteria(Name, new SearchCriteria(stringValues));
            return Parent;
        }

        public T Is(S value)
        {
            return IncludedIn(value);
        }
    }
}
