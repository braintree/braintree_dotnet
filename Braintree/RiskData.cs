using System;
using System.Collections.Generic;

namespace Braintree
{
    public class RiskData
    {
        public String id { get; protected set; }
        public String decision { get; protected set; }

        public RiskData(NodeWrapper node)
        {
            if (node == null) return;
            id = node.GetString("id");
            decision = node.GetString("decision");
        }
    }
}

