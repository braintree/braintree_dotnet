#pragma warning disable 1591

namespace Braintree
{
    public class RangeNode<T> : SearchNode<T> where T : SearchRequest
    {
        public RangeNode(string name, T parent) : base(name, parent)
        {
        }

        public T Between(object min, object max) {
            GreaterThanOrEqualTo(min);
            LessThanOrEqualTo(max);
            return Parent;
        }

        public T GreaterThanOrEqualTo(object min) {
            Parent.AddRangeCriteria(Name, new SearchCriteria("min", NormalizedStringValueOfObject(min)));
            return Parent;
        }

        public T Is(object value) {
            Parent.AddCriteria(Name, new SearchCriteria("is", NormalizedStringValueOfObject(value)));
            return Parent;
        }

        public T LessThanOrEqualTo(object max) {
            Parent.AddRangeCriteria(Name, new SearchCriteria("max", NormalizedStringValueOfObject(max)));
            return Parent;
        }

        internal static string NormalizedStringValueOfObject(object value) {
            if (value is decimal) {
                return ((decimal) value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            } else {
                return value.ToString();
            }
        }
    }
}
