using System;

namespace Braintree
{
    public class LocalPaymentReversed
    {
        public virtual string PaymentId { get; protected set; }

        protected internal LocalPaymentReversed(NodeWrapper node, IBraintreeGateway gateway)
        {
            PaymentId = node.GetString("payment-id");
        }

        [Obsolete("Mock Use Only")]
        protected internal LocalPaymentReversed() { }
    }
}
