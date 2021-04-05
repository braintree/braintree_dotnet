using System;

namespace Braintree
{
    public class ThreeDSecureInfo
    {
        public virtual ThreeDSecureAuthenticationInfo Authentication { get; protected set; }
        public virtual ThreeDSecureLookupInfo Lookup { get; protected set; }
        public virtual bool? LiabilityShiftPossible { get; protected set; }
        public virtual bool? LiabilityShifted { get; protected set; }
        public virtual string AcsTransactionId { get; protected set; }
        public virtual string Cavv { get; protected set; }
        public virtual string DsTransactionId { get; protected set; }
        public virtual string EciFlag { get; protected set; }
        public virtual string Enrolled { get; protected set; }
        public virtual string ParesStatus { get; protected set; }
        public virtual string Status { get; protected set; }
        public virtual string ThreeDSecureAuthenticationId { get; protected set; }
        public virtual string ThreeDSecureServerTransactionId { get; protected set; }
        public virtual string ThreeDSecureVersion { get; protected set; }
        public virtual string Xid { get; protected set; }

        public ThreeDSecureInfo(NodeWrapper node)
        {
            if (node == null)
                return;

            AcsTransactionId = node.GetString("acs-transaction-id");
            Cavv = node.GetString("cavv");
            DsTransactionId = node.GetString("ds-transaction-id");
            EciFlag = node.GetString("eci-flag");
            Enrolled = node.GetString("enrolled");
            LiabilityShiftPossible = node.GetBoolean("liability-shift-possible");
            LiabilityShifted = node.GetBoolean("liability-shifted");
            ParesStatus = node.GetString("pares-status");
            Status = node.GetString("status");
            ThreeDSecureAuthenticationId = node.GetString("three-d-secure-authentication-id");
            ThreeDSecureServerTransactionId = node.GetString("three-d-secure-server-transaction-id");
            ThreeDSecureVersion = node.GetString("three-d-secure-version");
            Xid = node.GetString("xid");

            var AuthenticationNode = node.GetNode("authentication");
            if (AuthenticationNode != null)
            {
                Authentication = new ThreeDSecureAuthenticationInfo(AuthenticationNode);
            }

            var LookupNode = node.GetNode("lookup");
            if (LookupNode != null)
            {
                Lookup = new ThreeDSecureLookupInfo(LookupNode);
            }
        }

        // this is used when constructing ThreeDSecureInfo within another response
        // (like a PaymentMethodNonce class)
        public ThreeDSecureInfo(dynamic info)
        {
            if (info == null)
                return;

            AcsTransactionId = info.acsTransactionId;
            Enrolled = info.enrolled;
            Status = info.status;
            LiabilityShifted = info.liabilityShifted;
            LiabilityShiftPossible = info.liabilityShiftPossible;
            ParesStatus = info.paresStatus;
            Cavv = info.cavv;
            Xid = info.xid;
            DsTransactionId = info.dsTransactionId;
            EciFlag = info.eciFlag;
            ThreeDSecureVersion = info.threeDSecureVersion;
            ThreeDSecureAuthenticationId = info.threeDSecureAuthenticationId;
            ThreeDSecureServerTransactionId = info.threeDSecureServerTransactionId;
            Authentication = new ThreeDSecureAuthenticationInfo(info.authentication);
            Lookup = new ThreeDSecureLookupInfo(info.lookup);
        }

        [Obsolete("Mock Use Only")]
        protected internal ThreeDSecureInfo() { }
    }
}