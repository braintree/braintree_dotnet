#pragma warning disable 1591

namespace Braintree
{
    public class MultipleValueOrTextNode<T, S> : TextNode<T> where T : SearchRequest where S : class
    {
        public MultipleValueOrTextNode(string name, T parent) : base(name, parent)
        {
        }

        public T IncludedIn(params S[] values)
        {
            Parent.AddMultipleValueCriteria(Name, new SearchCriteria(values));
            return Parent;
        }
    }
}
