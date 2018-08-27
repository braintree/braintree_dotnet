#pragma warning disable 1591

namespace Braintree
{
    public class EndsWithNode<T> : SearchNode<T> where T : SearchRequest
    {
        public EndsWithNode(string name, T parent) : base(name, parent)
        {
        }

        public T EndsWith(string value) {
            Parent.AddCriteria(Name, new SearchCriteria("ends-with", value));
            return Parent;
        }
    }
}
