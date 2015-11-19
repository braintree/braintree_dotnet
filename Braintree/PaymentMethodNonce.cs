using System;

namespace Braintree
{
    public class PaymentMethodNonce
    {
        public virtual string Nonce { get; protected set; }
        public virtual string Type { get; protected set; }
        public virtual ThreeDSecureInfo ThreeDSecureInfo { get; protected set; }

        protected internal PaymentMethodNonce(NodeWrapper node, IBraintreeGateway gateway)
        {
            Nonce = node.GetString("nonce");
            Type = node.GetString("type");

            var threeDSecureInfoNode = node.GetNode("three-d-secure-info");
            if (threeDSecureInfoNode != null && !threeDSecureInfoNode.IsEmpty()){
                ThreeDSecureInfo = new ThreeDSecureInfo(threeDSecureInfoNode);
            }
        }

        [Obsolete("Mock Use Only")]
        protected internal PaymentMethodNonce() { }
    }
}
