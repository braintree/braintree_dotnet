#pragma warning disable 1591

namespace Braintree
{
    public class TextNode<T> : PartialMatchNode<T> where T : SearchRequest
    {
        public TextNode(string name, T parent) : base(name, parent)
        {
        }

        public T Contains(string value) {
            Parent.AddCriteria(Name, new SearchCriteria("contains", value));
            return Parent;
        }
    }
}
