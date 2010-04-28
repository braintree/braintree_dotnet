#pragma warning disable 1591

using System;

namespace Braintree
{
    public class TextNode<T> : PartialMatchNode<T> where T : SearchRequest
    {
        public TextNode(String name, T parent) : base(name, parent)
        {
        }

        public T Contains(String value) {
            Parent.AddCriteria(Name, new SearchCriteria("contains", value));
            return Parent;
        }
    }
}
