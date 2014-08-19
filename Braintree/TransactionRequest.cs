#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class TransactionCreditCardRequest : BaseCreditCardRequest {}

    /// <summary>
    /// A class for building requests to manipulate <see cref="Transaction"/> records in the vault.
    /// </summary>
    /// <example>
    /// A transaction request can be constructed as follows:
    /// <code>
    /// var request = new TransactionRequest
    /// {
    ///     Amount = SandboxValues.TransactionAmount.AUTHORIZE,
    ///     CreditCard = new CreditCardRequest
    ///     {
    ///         Number = SandboxValues.CreditCardNumber.VISA,
    ///         ExpirationDate = "05/2009",
    ///     }
    /// };
    /// </code>
    /// </example>
    public class TransactionRequest : Request
    {
        public TransactionCreditCardRequest CreditCard { get; set; }
        public Decimal Amount { get; set; }
        public String DeviceData { get; set; }
        public String DeviceSessionId { get; set; }
        public String FraudMerchantId { get; set; }
        public String Channel { get; set; }
        public String OrderId { get; set; }
        public Boolean? Recurring { get; set; }
        public String MerchantAccountId { get; set; }
        public String PurchaseOrderNumber { get; set; }
        public CustomerRequest Customer { get; set; }
        public DescriptorRequest Descriptor { get; set; }
        public AddressRequest BillingAddress { get; set; }
        public AddressRequest ShippingAddress { get; set; }
        public TransactionPayPalRequest PayPalAccount { get; set; }
        public Decimal TaxAmount { get; set; }
        public Boolean? TaxExempt { get; set; }
        public TransactionType Type { get; set; }
        public Dictionary<String, String> CustomFields { get; set; }
        public TransactionOptionsRequest Options { get; set; }
        public String PaymentMethodToken { get; set; }
        public String CustomerId { get; set; }
        public String ShippingAddressId { get; set; }
        public String BillingAddressId { get; set; }
        public String VenmoSdkPaymentMethodCode { get; set; }
        public String PaymentMethodNonce { get; set; }
        public Decimal? ServiceFeeAmount { get; set; }
        private bool _threeDSecureTransaction;
        private String _threeDSecureToken;
        public String ThreeDSecureToken {
            get { return _threeDSecureToken; }
            set
            {
                _threeDSecureTransaction = true;
                _threeDSecureToken = value;
            }
        }

        public TransactionRequest()
        {
            CustomFields = new Dictionary<String, String>();
        }

        public override String Kind()
        {
            return TransparentRedirectGateway.CREATE_TRANSACTION;
        }

        public override String ToXml()
        {
            return ToXml("transaction");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        public override String ToQueryString()
        {
            return ToQueryString("transaction");
        }

        public override String ToQueryString(String root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            RequestBuilder builder = new RequestBuilder(root);

            if (Amount != 0) builder.AddElement("amount", Amount);
            builder.AddElement("device-data", DeviceData);
            builder.AddElement("customer-id", CustomerId);
            builder.AddElement("order-id", OrderId);
            builder.AddElement("channel", Channel);
            builder.AddElement("device-session-id", DeviceSessionId);
            builder.AddElement("fraud-merchant-id", FraudMerchantId);
            if (Recurring.HasValue) builder.AddElement("recurring", Recurring);
            builder.AddElement("payment-method-token", PaymentMethodToken);
            builder.AddElement("payment-method-nonce", PaymentMethodNonce);
            builder.AddElement("purchase-order-number", PurchaseOrderNumber);
            builder.AddElement("shipping-address-id", ShippingAddressId);
            builder.AddElement("billing-address-id", BillingAddressId);
            if (TaxAmount != 0) builder.AddElement("tax-amount", TaxAmount);
            if (TaxExempt.HasValue) {
                builder.AddElement("tax-exempt", TaxExempt);
            }
            builder.AddElement("merchant-account-id", MerchantAccountId);
            if (ServiceFeeAmount.HasValue) builder.AddElement("service-fee-amount", ServiceFeeAmount);

            if (Type != null) builder.AddElement("type", Type.ToString().ToLower());

            if (CustomFields.Count != 0) builder.AddElement("custom-fields", CustomFields);

            builder.AddElement("credit-card", CreditCard);
            builder.AddElement("customer", Customer);
            builder.AddElement("descriptor", Descriptor);
            builder.AddElement("billing", BillingAddress);
            builder.AddElement("shipping", ShippingAddress);
            builder.AddElement("paypal-account", PayPalAccount);
            builder.AddElement("options", Options);
            builder.AddElement("venmo-sdk-payment-method-code", VenmoSdkPaymentMethodCode);
            if (_threeDSecureTransaction) {
                builder.AddElement("three-d-secure-token", ThreeDSecureToken ?? "");
            }
            return builder;
        }
    }
}
