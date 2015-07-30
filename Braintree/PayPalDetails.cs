using System;

namespace Braintree
{
    public class PayPalDetails
    {
        public string PayerEmail { get; protected set; }
        public string PaymentId { get; protected set; }
        public string AuthorizationId { get; protected set; }
        public string Token { get; protected set; }
        public string ImageUrl { get; protected set; }
        public string DebugId { get; protected set; }
        public string PayeeEmail { get; protected set; }
        public string CustomField { get; protected set; }
        public string PayerId { get; protected set; }
        public string PayerFirstName { get; protected set; }
        public string PayerLastName { get; protected set; }
        public string SellerProtectionStatus { get; protected set; }
        public string CaptureId { get; protected set; }
        public string RefundId { get; protected set; }
        public string TransactionFeeAmount { get; protected set; }
        public string TransactionFeeCurrencyIsoCode { get; protected set; }
        public string Description { get; protected set; }

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
    }
}
