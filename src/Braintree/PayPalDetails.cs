using System;

namespace Braintree
{
    public class PayPalDetails
    {   
        public virtual string AuthorizationId { get; protected set; }
        public virtual string BillingAgreementId { get; protected set; }
        public virtual string CaptureId { get; protected set; }
        public virtual string CustomField { get; protected set; }
        public virtual string DebugId { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual string ImageUrl { get; protected set; }
        public virtual string ImplicitlyVaultedPaymentMethodGlobalId { get; protected set; }
        public virtual string ImplicitlyVaultedPaymentMethodToken { get; protected set; }
        public virtual string PayeeEmail { get; protected set; }
        public virtual string PayeeId { get; protected set; }
        public virtual string PayerEmail { get; protected set; }
        public virtual string PayerFirstName { get; protected set; }
        public virtual string PayerId { get; protected set; }
        public virtual string PayerLastName { get; protected set; }
        public virtual string PayerStatus { get; protected set; }
        public virtual string PaymentId { get; protected set; }
        public virtual string RecipientEmail {get; protected set; }
        public virtual RecipientPhoneDetails RecipientPhone {get; protected set;}
        public virtual string RefundFromTransactionFeeAmount { get; protected set; }
        public virtual string RefundFromTransactionFeeCurrencyIsoCode { get; protected set; }
        public virtual string RefundId { get; protected set; }
        public virtual string SellerProtectionStatus { get; protected set; }
        public virtual string TaxId { get; protected set; }
        public virtual string TaxIdType { get; protected set; }
        public virtual string Token { get; protected set; }
        public virtual string TransactionFeeAmount { get; protected set; }
        public virtual string TransactionFeeCurrencyIsoCode { get; protected set; }

        protected internal PayPalDetails(NodeWrapper node)
        {
            AuthorizationId = node.GetString("authorization-id");
            BillingAgreementId = node.GetString("billing-agreement-id");
            CaptureId = node.GetString("capture-id");
            CustomField = node.GetString("custom-field");
            DebugId = node.GetString("debug-id");
            Description = node.GetString("description");
            ImageUrl = node.GetString("image-url");
            ImplicitlyVaultedPaymentMethodGlobalId = node.GetString("implicitly-vaulted-payment-method-global-id");
            ImplicitlyVaultedPaymentMethodToken = node.GetString("implicitly-vaulted-payment-method-token");
            PayeeEmail = node.GetString("payee-email");
            PayeeId = node.GetString("payee-id");
            PayerEmail = node.GetString("payer-email");
            PayerFirstName = node.GetString("payer-first-name");
            PayerId = node.GetString("payer-id");
            PayerLastName = node.GetString("payer-last-name");
            PayerStatus = node.GetString("payer-status");
            PaymentId = node.GetString("payment-id"); 
            RecipientEmail = node.GetString("recipient-email");
            var recipientPhoneNode = node.GetNode("recipient-phone"); 
            if (recipientPhoneNode != null)
            {
                RecipientPhone = new RecipientPhoneDetails(recipientPhoneNode);
            }
            RefundFromTransactionFeeAmount = node.GetString("refund-from-transaction-fee-amount");
            RefundFromTransactionFeeCurrencyIsoCode = node.GetString("refund-from-transaction-fee-currency-iso-code");
            RefundId = node.GetString("refund-id");
            SellerProtectionStatus = node.GetString("seller-protection-status");
            TaxId = node.GetString("tax-id");
            TaxIdType = node.GetString("tax-id-type");
            Token = node.GetString("token");
            TransactionFeeAmount = node.GetString("transaction-fee-amount");
            TransactionFeeCurrencyIsoCode = node.GetString("transaction-fee-currency-iso-code");

        }

        [Obsolete("Mock Use Only")]
        protected internal PayPalDetails() { }
    }
}
