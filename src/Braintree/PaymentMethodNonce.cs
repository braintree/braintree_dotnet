using System;

namespace Braintree
{
    public class PaymentMethodNonce
    {
        public virtual bool? IsDefault { get; protected set; }
        public virtual string Nonce { get; protected set; }
        public virtual string Type { get; protected set; }
        public virtual PaymentMethodNonceDetails Details { get; protected set; }
        public virtual ThreeDSecureInfo ThreeDSecureInfo { get; protected set; }

        protected internal PaymentMethodNonce(NodeWrapper node, IBraintreeGateway gateway)
        {
            IsDefault = node.GetBoolean("default");
            Nonce = node.GetString("nonce");
            Type = node.GetString("type");

            var detailsNode = node.GetNode("details");
            if (detailsNode != null && !detailsNode.IsEmpty())
            {
                Details = new PaymentMethodNonceDetails(detailsNode);
            }

            var threeDSecureInfoNode = node.GetNode("three-d-secure-info");
            if (threeDSecureInfoNode != null && !threeDSecureInfoNode.IsEmpty())
            {
                ThreeDSecureInfo = new ThreeDSecureInfo(threeDSecureInfoNode);
            }
        }

        [Obsolete("Mock Use Only")]
        protected internal PaymentMethodNonce() { }
    }
}
