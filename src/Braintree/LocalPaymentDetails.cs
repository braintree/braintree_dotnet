using System;

namespace Braintree
{
    public class LocalPaymentDetails
    {
        public virtual string CaptureId { get; protected set; }
        public virtual string CustomField { get; protected set; }
        public virtual string DebugId { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual string FundingSource { get; protected set; }
        public virtual string PayerId { get; protected set; }
        public virtual string PaymentId { get; protected set; }
        public virtual string RefundFromTransactionFeeAmount { get; protected set; }
        public virtual string RefundFromTransactionFeeCurrencyIsoCode { get; protected set; }
        public virtual string RefundId { get; protected set; }
        public virtual string TransactionFeeAmount { get; protected set; }
        public virtual string TransactionFeeCurrencyIsoCode { get; protected set; }

        protected internal LocalPaymentDetails(NodeWrapper node)
        {
            CaptureId = node.GetString("capture-id");
            CustomField = node.GetString("custom-field");
            DebugId = node.GetString("debug-id");
            Description = node.GetString("description");
            FundingSource = node.GetString("funding-source");
            PayerId = node.GetString("payer-id");
            PaymentId = node.GetString("payment-id");
            RefundFromTransactionFeeAmount = node.GetString("refund-from-transaction-fee-amount");
            RefundFromTransactionFeeCurrencyIsoCode = node.GetString("refund-from-transaction-fee-currency-iso-code");
            RefundId = node.GetString("refund-id");
            TransactionFeeAmount = node.GetString("transaction-fee-amount");
            TransactionFeeCurrencyIsoCode = node.GetString("transaction-fee-currency-iso-code");
        }

        [Obsolete("Mock Use Only")]
        protected internal LocalPaymentDetails() { }
    }
}
