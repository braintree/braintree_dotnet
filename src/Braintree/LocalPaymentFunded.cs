using System;

namespace Braintree
{
    public class LocalPaymentFunded
    {
        public virtual string PaymentId { get; protected set; }
        public virtual string PaymentContextId { get; protected set; }
        public virtual Transaction Transaction { get; protected set; }

        protected internal LocalPaymentFunded(NodeWrapper node, IBraintreeGateway gateway)
        {
            PaymentId = node.GetString("payment-id");
            PaymentContextId = node.GetString("payment-context-id");

            var transactionNode = node.GetNode("transaction");
            if (transactionNode != null)
            {
                Transaction = new Transaction(transactionNode, gateway);
            }
        }

        [Obsolete("Mock Use Only")]
        protected internal LocalPaymentFunded() { }
    }
}
