using System;

namespace Braintree
{
    public class LocalPaymentCompleted
    {
        public virtual string PaymentId { get; protected set; }
        public virtual string PayerId { get; protected set; }

        protected internal LocalPaymentCompleted(NodeWrapper node)
        {
            PaymentId = node.GetString("payment-id");
            PayerId = node.GetString("payer-id");
        }

        [Obsolete("Mock Use Only")]
        protected internal LocalPaymentCompleted() { }
    }
}
