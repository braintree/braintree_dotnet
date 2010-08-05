#pragma warning disable 1591

using System;

namespace Braintree
{
    public class RangeNode<T> : SearchNode<T> where T : SearchRequest
    {
        public RangeNode(String name, T parent) : base(name, parent)
        {
        }

        public T Between(object min, object max) {
            GreaterThanOrEqualTo(min);
            LessThanOrEqualTo(max);
            return Parent;
        }

        public T GreaterThanOrEqualTo(object min) {
            Parent.AddRangeCriteria(Name, new SearchCriteria("min", min.ToString()));
            return Parent;
        }

        public T Is(object value) {
            Parent.AddCriteria(Name, new SearchCriteria("is", value.ToString()));
            return Parent;
        }

        public T LessThanOrEqualTo(object max) {
            Parent.AddRangeCriteria(Name, new SearchCriteria("max", max.ToString()));
            return Parent;
        }
    }
}
