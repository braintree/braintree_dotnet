using System;
using System.Collections.Generic;

namespace Braintree
{
    public class ThreeDSecureInfo
    {
        public virtual string Status { get; protected set; }
        public virtual string Enrolled { get; protected set; }
        public virtual bool? LiabilityShifted { get; protected set; }
        public virtual bool? LiabilityShiftPossible { get; protected set; }

        public ThreeDSecureInfo(NodeWrapper node)
        {
            if (node == null) return;

            Enrolled = node.GetString("enrolled");
            Status = node.GetString("status");
            LiabilityShifted = node.GetBoolean("liability-shifted");
            LiabilityShiftPossible = node.GetBoolean("liability-shift-possible");
        }

        [Obsolete("Mock Use Only")]
        protected internal ThreeDSecureInfo() { }
    }
}

