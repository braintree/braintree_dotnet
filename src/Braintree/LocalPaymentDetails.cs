using System;

namespace Braintree
{
    public class LocalPaymentDetails
    {
        public virtual string CustomField { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual string FundingSource { get; protected set; }
        public virtual string PayerId { get; protected set; }
        public virtual string PaymentId { get; protected set; }

        protected internal LocalPaymentDetails(NodeWrapper node)
        {
            CustomField = node.GetString("custom-field");
            Description = node.GetString("description");
            FundingSource = node.GetString("funding-source");
            PayerId = node.GetString("payer-id");
            PaymentId = node.GetString("payment-id");
        }

        [Obsolete("Mock Use Only")]
        protected internal LocalPaymentDetails() { }
    }
}
