using System;
using System.Collections.Generic;

namespace Braintree
{
    public class ThreeDSecureInfo
    {
        public string Status { get; protected set; }
        public string Enrolled { get; protected set; }
        public Boolean? LiabilityShifted { get; protected set; }
        public Boolean? LiabilityShiftPossible { get; protected set; }

        public ThreeDSecureInfo(NodeWrapper node)
        {
            if (node == null) return;

            Enrolled = node.GetString("enrolled");
            Status = node.GetString("status");
            LiabilityShifted = node.GetBoolean("liability-shifted");
            LiabilityShiftPossible = node.GetBoolean("liability-shift-possible");
        }
    }
}

