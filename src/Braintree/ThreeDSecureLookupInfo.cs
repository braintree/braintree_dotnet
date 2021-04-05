using System;
namespace Braintree
{
    public class ThreeDSecureLookupInfo
    {
        public virtual string TransStatus { get; protected set; }
        public virtual string TransStatusReason { get; protected set; }

        public ThreeDSecureLookupInfo(NodeWrapper node)
        {
            if (node == null)
                return;

            TransStatus = node.GetString("trans-status");
            TransStatusReason = node.GetString("trans-status-reason");
        }

        public ThreeDSecureLookupInfo(dynamic info)
        {
            if (info == null)
                return;

            TransStatus = info.transStatus;
            TransStatusReason = info.transStatusReason;
        }

        [Obsolete("Mock Use Only")]
        protected internal ThreeDSecureLookupInfo() { }
    }
}
