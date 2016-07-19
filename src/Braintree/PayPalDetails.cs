using System;

namespace Braintree
{
    public class PayPalDetails
    {
        public virtual string PayerEmail { get; protected set; }
        public virtual string PaymentId { get; protected set; }
        public virtual string AuthorizationId { get; protected set; }
        public virtual string Token { get; protected set; }
        public virtual string ImageUrl { get; protected set; }
        public virtual string DebugId { get; protected set; }
        public virtual string PayeeEmail { get; protected set; }
        public virtual string CustomField { get; protected set; }
        public virtual string PayerId { get; protected set; }
        public virtual string PayerFirstName { get; protected set; }
        public virtual string PayerLastName { get; protected set; }
        public virtual string SellerProtectionStatus { get; protected set; }
        public virtual string CaptureId { get; protected set; }
        public virtual string RefundId { get; protected set; }
        public virtual string TransactionFeeAmount { get; protected set; }
        public virtual string TransactionFeeCurrencyIsoCode { get; protected set; }
        public virtual string Description { get; protected set; }

        protected internal PayPalDetails(NodeWrapper node)
        {
            PayerEmail = node.GetString("payer-email");
            PaymentId = node.GetString("payment-id");
            AuthorizationId = node.GetString("authorization-id");
            Token = node.GetString("token");
            ImageUrl = node.GetString("image-url");
            DebugId = node.GetString("debug-id");
            PayeeEmail = node.GetString("payee-email");
            CustomField = node.GetString("custom-field");
            PayerId = node.GetString("payer-id");
            PayerFirstName = node.GetString("payer-first-name");
            PayerLastName = node.GetString("payer-last-name");
            SellerProtectionStatus = node.GetString("seller-protection-status");
            CaptureId = node.GetString("capture-id");
            RefundId = node.GetString("refund-id");
            TransactionFeeAmount = node.GetString("transaction-fee-amount");
            TransactionFeeCurrencyIsoCode = node.GetString("transaction-fee-currency-iso-code");
            Description = node.GetString("description");
        }

        [Obsolete("Mock Use Only")]
        protected internal PayPalDetails() { }
    }
}
