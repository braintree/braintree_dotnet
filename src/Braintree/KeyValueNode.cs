#pragma warning disable 1591

namespace Braintree
{

    public class KeyValueNode<T> : SearchNode<T> where T : SearchRequest
    {
        public KeyValueNode(string name, T parent) : base(name, parent)
        {
        }

        public T Is(object value) {
            Parent.AddCriteria(Name, value.ToString());
            return Parent;
        }
    }
}
