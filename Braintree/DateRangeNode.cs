#pragma warning disable 1591

using System;

namespace Braintree
{
    public class DateRangeNode<T> : SearchNode<T> where T : SearchRequest
    {
        public DateRangeNode(String name, T parent) : base(name, parent)
        {
        }

        public T Between(DateTime min, DateTime max) {
            GreaterThanOrEqualTo(min);
            LessThanOrEqualTo(max);
            return Parent;
        }

        public T GreaterThanOrEqualTo(DateTime min) {
            Parent.AddRangeCriteria(Name, new SearchCriteria("min", min));
            return Parent;
        }

        public T LessThanOrEqualTo(DateTime max) {
            Parent.AddRangeCriteria(Name, new SearchCriteria("max", max));
            return Parent;
        }
    }
}
