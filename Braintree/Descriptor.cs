#pragma warning disable 1591

using System;

namespace Braintree
{
    public class Descriptor
    {
        public string Name { get; protected set; }
        public string Phone { get; protected set; }

        internal Descriptor(NodeWrapper node)
        {
            Name = node.GetString("name");
            Phone = node.GetString("phone");
        }
    }
}
