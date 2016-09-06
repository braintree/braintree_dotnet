using System;

namespace Braintree
{
    public class RiskData
    {
        public virtual string id { get; protected set; }
        public virtual string decision { get; protected set; }

        public RiskData(NodeWrapper node)
        {
            if (node == null)
                return;

            id = node.GetString("id");
            decision = node.GetString("decision");
        }

        [Obsolete("Mock Use Only")]
        protected internal RiskData() { }
    }
}

