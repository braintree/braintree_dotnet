#pragma warning disable 1591

using System;

namespace Braintree
{
    public class SearchNode<T>
    {
        protected String Name;
        protected T Parent;

        public SearchNode(String name, T parent)
        {
            Name = name;
            Parent = parent;
        }
    }
}
