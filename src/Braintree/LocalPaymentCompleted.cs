using System;

namespace Braintree
{
    public class LocalPaymentCompleted
    {
        public virtual string PaymentId { get; protected set; }
        public virtual string PayerId { get; protected set; }
        public virtual string PaymentMethodNonce { get; protected set; }
        public virtual Transaction Transaction { get; protected set; }

        protected internal LocalPaymentCompleted(NodeWrapper node, IBraintreeGateway gateway)
        {
            PaymentId = node.GetString("payment-id");
            PayerId = node.GetString("payer-id");
            PaymentMethodNonce = node.GetString("payment-method-nonce");

            var transactionNode = node.GetNode("transaction");
            if(transactionNode != null)
            {
                Transaction = new Transaction(transactionNode, gateway);
            }
        }

        [Obsolete("Mock Use Only")]
        protected internal LocalPaymentCompleted() { }
    }
}
