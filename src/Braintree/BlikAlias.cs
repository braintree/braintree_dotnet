using System;
using System.ComponentModel;

namespace Braintree
{

    public class BlikAlias 
    {
        public virtual string Key { get; protected set; }
        public virtual string Label { get; protected set; }

        protected internal BlikAlias(NodeWrapper node)
        {
            Key = node.GetString("key");
            Label = node.GetString("label");
        }

        [Obsolete("Mock Use Only")]
        protected internal BlikAlias() { }
    }
}
