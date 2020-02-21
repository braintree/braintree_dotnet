using System;

namespace Braintree
{
    public class ThreeDSecureInfo
    {
        public virtual string Status { get; protected set; }
        public virtual string Enrolled { get; protected set; }
        public virtual bool? LiabilityShifted { get; protected set; }
        public virtual bool? LiabilityShiftPossible { get; protected set; }
        public virtual string Cavv { get; protected set; }
        public virtual string Xid { get; protected set; }
        public virtual string DsTransactionId { get; protected set; }
        public virtual string EciFlag { get; protected set; }
        public virtual string ThreeDSecureVersion { get; protected set; }
        public virtual string ThreeDSecureAuthenticationId { get; protected set; }

        public ThreeDSecureInfo(NodeWrapper node)
        {
            if (node == null)
                return;

            Enrolled = node.GetString("enrolled");
            Status = node.GetString("status");
            LiabilityShifted = node.GetBoolean("liability-shifted");
            LiabilityShiftPossible = node.GetBoolean("liability-shift-possible");
            Cavv = node.GetString("cavv");
            Xid = node.GetString("xid");
            DsTransactionId = node.GetString("ds-transaction-id");
            EciFlag = node.GetString("eci-flag");
            ThreeDSecureVersion = node.GetString("three-d-secure-version");
            ThreeDSecureAuthenticationId = node.GetString("three-d-secure-authentication-id");
        }

        public ThreeDSecureInfo(dynamic info)
        {
            if (info == null)
                return;

            Enrolled = info.enrolled;
            Status = info.status;
            LiabilityShifted = info.liabilityShifted;
            LiabilityShiftPossible = info.liabilityShiftPossible;
            Cavv = info.cavv;
            Xid = info.xid;
            DsTransactionId = info.dsTransactionId;
            EciFlag = info.eciFlag;
            ThreeDSecureVersion = info.threeDSecureVersion;
            ThreeDSecureAuthenticationId = info.threeDSecureAuthenticationId;
        }

        [Obsolete("Mock Use Only")]
        protected internal ThreeDSecureInfo() { }
    }
}

