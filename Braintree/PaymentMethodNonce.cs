using System;

namespace Braintree
{
    public class PaymentMethodNonce
    {
        public String Nonce { get; protected set; }

        protected internal PaymentMethodNonce(NodeWrapper node, BraintreeService service)
        {
            Nonce = node.GetString("nonce");
        }
    }
}
