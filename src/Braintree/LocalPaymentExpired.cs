using System;

namespace Braintree
{
    public class LocalPaymentExpired
    {
        public virtual string PaymentId { get; protected set; }
        public virtual string PaymentContextId { get; protected set; }

        protected internal LocalPaymentExpired(NodeWrapper node, IBraintreeGateway gateway)
        {
            PaymentId = node.GetString("payment-id");
            PaymentContextId = node.GetString("payment-context-id");
        }

        [Obsolete("Mock Use Only")]
        protected internal LocalPaymentExpired() { }
    }
}
