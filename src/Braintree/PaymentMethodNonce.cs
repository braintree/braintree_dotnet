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
        public virtual BinData BinData { get; protected set; }
        public virtual AuthenticationInsightResponse AuthenticationInsight { get; protected set; }

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

            var authenticationInsight = node.GetNode("authentication-insight");
            if (authenticationInsight != null && !authenticationInsight.IsEmpty())
            {
                AuthenticationInsight = new AuthenticationInsightResponse(authenticationInsight);
            }

            var binData = node.GetNode("bin-data");
            if (binData != null && !binData.IsEmpty())
            {
                BinData = new BinData(binData);
            }
        }

        protected internal PaymentMethodNonce(dynamic paymentMethodNonce)
        {
            IsDefault = paymentMethodNonce["default"];
            Nonce = paymentMethodNonce.nonce;
            Type = paymentMethodNonce.type;

            var details = paymentMethodNonce.details;
            if (details != null)
            {
                Details = new PaymentMethodNonceDetails(details);
            }

            var threeDSecureInfo = paymentMethodNonce.threeDSecureInfo;
            if (threeDSecureInfo != null)
            {
                ThreeDSecureInfo = new ThreeDSecureInfo(threeDSecureInfo);
            }

            var binData = paymentMethodNonce.binData;
            if (binData != null)
            {
                BinData = new BinData(binData);
            }
        }

        [Obsolete("Mock Use Only")]
        protected internal PaymentMethodNonce() { }
    }
}
