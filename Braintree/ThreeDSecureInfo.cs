using System;
using System.Collections.Generic;

namespace Braintree
{
    public class ThreeDSecureInfo
    {
        public String XID { get; protected set; }
        public String CAVV { get; protected set; }
        public String Status { get; protected set; }
        public String Enrolled { get; protected set; }
        public Boolean? LiabilityShifted { get; protected set; }
        public Boolean? LiabilityShiftPossible { get; protected set; }

        public ThreeDSecureInfo(NodeWrapper node)
        {
            if (node == null) return;

            XID = node.GetString("xid");
            CAVV = node.GetString("cavv");
            Enrolled = node.GetString("enrolled");
            Status = node.GetString("status");
            LiabilityShifted = node.GetBoolean("liability-shifted");
            LiabilityShiftPossible = node.GetBoolean("liability-shift-possible");
        }
    }
}

