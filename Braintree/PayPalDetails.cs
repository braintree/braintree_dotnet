using System;

namespace Braintree
{
    public class PayPalDetails
    {
        public String PayerEmail { get; protected set; }
        public String PaymentId { get; protected set; }
        public String AuthorizationId { get; protected set; }

        protected internal PayPalDetails(NodeWrapper node)
        {
            PayerEmail = node.GetString("payer-email");
            PaymentId = node.GetString("payment-id");
            AuthorizationId = node.GetString("authorization-id");
        }
    }
}
