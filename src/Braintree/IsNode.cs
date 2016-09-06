#pragma warning disable 1591

namespace Braintree
{
    public class IsNode<T> : SearchNode<T> where T : SearchRequest
    {
        public IsNode(string name, T parent) : base(name, parent)
        {
        }

        public T Is(string value) {
            Parent.AddCriteria(Name, new SearchCriteria("is", value));
            return Parent;
        }

    }
}
