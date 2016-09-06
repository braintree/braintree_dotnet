#pragma warning disable 1591

using System.Collections.Generic;
using System.Linq;

namespace Braintree
{
    public class BooleanMultipleValueNode<T> : MultipleValueNode<T, string> where T : SearchRequest
    {
        public BooleanMultipleValueNode (string name, T parent) : base(name, parent)
        {
        }

        public T IncludedIn(params bool[] values)
        {
            string[] stringValues = new List<bool>(values).Select(x => x.ToString()).ToArray();
            Parent.AddMultipleValueCriteria(Name, new SearchCriteria(stringValues));
            return Parent;
        }

        public T Is(bool value)
        {
            return IncludedIn(value.ToString());
        }
    }
}
