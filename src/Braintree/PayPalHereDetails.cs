using System;

namespace Braintree
{
    public class PayPalHereDetails
    {
        public virtual string AuthorizationId { get; protected set; }
        public virtual string CaptureId { get; protected set; }
        public virtual string InvoiceId { get; protected set; }
        public virtual string Last4 { get; protected set; }
        public virtual string PaymentId { get; protected set; }
        public virtual string PaymentType { get; protected set; }
        public virtual string RefundId { get; protected set; }
        public virtual string TransactionFeeAmount { get; protected set; }
        public virtual string TransactionFeeCurrencyIsoCode { get; protected set; }
        public virtual string TransactionInitiationDate { get; protected set; }
        public virtual string TransactionUpdatedDate { get; protected set; }

        protected internal PayPalHereDetails(NodeWrapper node)
        {
            AuthorizationId = node.GetString("authorization-id");
            CaptureId = node.GetString("capture-id");
            InvoiceId = node.GetString("invoice-id");
            Last4 = node.GetString("last-4");
            PaymentId = node.GetString("payment-id");
            PaymentType = node.GetString("payment-type");
            RefundId = node.GetString("refund-id");
            TransactionFeeAmount = node.GetString("transaction-fee-amount");
            TransactionFeeCurrencyIsoCode = node.GetString("transaction-fee-currency-iso-code");
            TransactionInitiationDate = node.GetString("transaction-initiation-date");
            TransactionUpdatedDate = node.GetString("transaction-updated-date");
        }

        [Obsolete("Mock Use Only")]
        protected internal PayPalHereDetails() { }
    }
}

