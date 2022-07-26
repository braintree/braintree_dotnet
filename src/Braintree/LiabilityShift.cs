using System;
using System.Collections.Generic;

namespace Braintree
{
    public class LiabilityShift
    {
        public virtual List<string> Conditions { get; protected set; }
        public virtual string ResponsibleParty { get; protected set; }

        public LiabilityShift(NodeWrapper node)
        {
            if (node == null)
                return;

            ResponsibleParty = node.GetString("responsible-party");
            Conditions = new List<string>();
            foreach (var stringNode in node.GetList("conditions")) 
            {
                Conditions.Add(stringNode.GetString("."));
            }
        }

        [Obsolete("Mock Use Only")]
        protected internal LiabilityShift() { }
    }
}

