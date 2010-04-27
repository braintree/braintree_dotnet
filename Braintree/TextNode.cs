#pragma warning disable 1591

using System;

namespace Braintree
{
    public class TextNode<T> : SearchNode<T> where T : SearchRequest
    {
        public TextNode(String name, T parent) : base(name, parent)
        {
        }

        public T Contains(String value) {
            Parent.AddCriteria(Name, new SearchCriteria("contains", value));
            return Parent;
        }

        public T EndsWith(String value) {
            Parent.AddCriteria(Name, new SearchCriteria("ends-with", value));
            return Parent;
        }

        public T Is(String value) {
            Parent.AddCriteria(Name, new SearchCriteria("is", value));
            return Parent;
        }

        public T IsNot(String value) {
            Parent.AddCriteria(Name, new SearchCriteria("is-not", value));
            return Parent;
        }

        public T StartsWith(String value) {
            Parent.AddCriteria(Name, new SearchCriteria("starts-with", value));
            return Parent;
        }
    }
}
