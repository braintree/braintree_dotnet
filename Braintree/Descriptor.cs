#pragma warning disable 1591

using System;

namespace Braintree
{
    [Serializable]
    public class Descriptor
    {
        public string Name { get; protected set; }
        public string Phone { get; protected set; }

        protected internal Descriptor(NodeWrapper node)
        {
            if (node != null) {
                Name = node.GetString("name");
                Phone = node.GetString("phone");
            }
        }
    }
}
