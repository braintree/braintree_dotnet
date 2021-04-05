using System;
namespace Braintree
{
    public class ThreeDSecureAuthenticationInfo
    {
        public virtual string TransStatus { get; protected set; }
        public virtual string TransStatusReason { get; protected set; }

        public ThreeDSecureAuthenticationInfo(NodeWrapper node)
        {
            if (node == null)
                return;

            TransStatus = node.GetString("trans-status");
            TransStatusReason = node.GetString("trans-status-reason");
        }

        public ThreeDSecureAuthenticationInfo(dynamic info)
        {
            if (info == null)
                return;

            TransStatus = info.transStatus;
            TransStatusReason = info.transStatusReason;
        }

        [Obsolete("Mock Use Only")]
        protected internal ThreeDSecureAuthenticationInfo() { }
    }
}
