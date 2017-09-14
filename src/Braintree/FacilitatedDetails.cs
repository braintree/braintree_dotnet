using System;

namespace Braintree
{
    public class FacilitatedDetails
    {
        public virtual string MerchantId { get; protected set; }
        public virtual string MerchantName { get; protected set; }
        public virtual string PaymentMethodNonce { get; protected set; }

        public FacilitatedDetails(NodeWrapper node)
        {
            if (node == null)
                return;

            MerchantId = node.GetString("merchant-id");
            MerchantName = node.GetString("merchant-name");
            PaymentMethodNonce = node.GetString("payment-method-nonce");
        }
    }
}

