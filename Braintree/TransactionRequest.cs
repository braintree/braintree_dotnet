#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class TransactionCreditCardRequest : BaseCreditCardRequest
    {
        public string Token { get; set; }

        protected override RequestBuilder BuildRequest(string root)
        {
            return base.BuildRequest(root).AddElement("token", Token);
        }
    }

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
        public decimal Amount { get; set; }
        public string DeviceData { get; set; }
        public string DeviceSessionId { get; set; }
        public string FraudMerchantId { get; set; }
        public string Channel { get; set; }
        public string OrderId { get; set; }
        public bool? Recurring { get; set; }
        public string MerchantAccountId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public CustomerRequest Customer { get; set; }
        public DescriptorRequest Descriptor { get; set; }
        public IndustryRequest Industry { get; set; }
        public AddressRequest BillingAddress { get; set; }
        public AddressRequest ShippingAddress { get; set; }
        public TransactionPayPalRequest PayPalAccount { get; set; }
        public decimal TaxAmount { get; set; }
        public bool? TaxExempt { get; set; }
        public TransactionType Type { get; set; }
        public Dictionary<string, string> CustomFields { get; set; }
        public TransactionOptionsRequest Options { get; set; }
        public string PaymentMethodToken { get; set; }
        public string CustomerId { get; set; }
        public string ShippingAddressId { get; set; }
        public string BillingAddressId { get; set; }
        public string VenmoSdkPaymentMethodCode { get; set; }
        public string PaymentMethodNonce { get; set; }
        public decimal? ServiceFeeAmount { get; set; }
        private bool _threeDSecureTransaction;
        private string _threeDSecureToken;
        public string ThreeDSecureToken {
            get { return _threeDSecureToken; }
            set
            {
                _threeDSecureTransaction = true;
                _threeDSecureToken = value;
            }
        }

        public TransactionRequest()
        {
            CustomFields = new Dictionary<string, string>();
        }

        public override string Kind()
        {
            return TransparentRedirectGateway.CREATE_TRANSACTION;
        }

        public override string ToXml()
        {
            return ToXml("transaction");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("transaction");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            var builder = new RequestBuilder(root);

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
            builder.AddElement("industry", Industry);
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
