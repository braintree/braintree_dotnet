#pragma warning disable 1591

namespace Braintree
{
    public class SearchNode<T>
    {
        protected string Name;
        protected T Parent;

        public SearchNode(string name, T parent)
        {
            Name = name;
            Parent = parent;
        }
    }
}
