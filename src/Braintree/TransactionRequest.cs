#pragma warning disable 1591

using System.Collections.Generic;
using System;

namespace Braintree
{
    public class TransactionCreditCardRequest : BaseCreditCardRequest
    {
        public string Token { get; set; }
        public PaymentReaderCardDetailsRequest PaymentReaderCardDetails { get; set; }

        protected override RequestBuilder BuildRequest(string root)
        {
            return base.BuildRequest(root)
                .AddElement("token", Token)
                .AddElement("payment-reader-card-details", PaymentReaderCardDetails);
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
        [ObsoleteAttribute("use DeviceData instead", false)]
        public string DeviceSessionId { get; set; }
        [ObsoleteAttribute("use DeviceData instead", false)]
        public string FraudMerchantId { get; set; }
        public string Channel { get; set; }
        public string ExchangeRateQuoteId { get; set; }
        public string OrderId { get; set; }
        public string ProductSku { get; set; }
        [ObsoleteAttribute("use TransactionSource instead", false)]
        public bool? Recurring { get; set; }
        public string TransactionSource { get; set; }
        public string MerchantAccountId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public CustomerRequest Customer { get; set; }
        public DescriptorRequest Descriptor { get; set; }
        public IndustryRequest Industry { get; set; }
        public AddressRequest BillingAddress { get; set; }
        public AddressRequest ShippingAddress { get; set; }
        public TransactionPayPalRequest PayPalAccount { get; set; }
        public decimal? TaxAmount { get; set; }
        public bool? TaxExempt { get; set; }
        public TransactionType? Type { get; set; }
        public Dictionary<string, string> CustomFields { get; set; }
        public TransactionOptionsRequest Options { get; set; }
        public ThreeDSecurePassThruRequest ThreeDSecurePassThru { get; set; }
        public string PaymentMethodToken { get; set; }
        public string CustomerId { get; set; }
        public string ShippingAddressId { get; set; }
        public string BillingAddressId { get; set; }
        public string VenmoSdkPaymentMethodCode { get; set; }
        public string PaymentMethodNonce { get; set; }
        public string ScaExemption { get; set; }
        public decimal? ServiceFeeAmount { get; set; }
        public string SharedPaymentMethodToken { get; set; }
        public string SharedPaymentMethodNonce { get; set; }
        public string SharedCustomerId { get; set; }
        public string SharedShippingAddressId { get; set; }
        public string SharedBillingAddressId { get; set; }
        public string ThreeDSecureAuthenticationId { get; set; }
        private bool _threeDSecureTransaction;
        private string _threeDSecureToken;
        public string ThreeDSecureToken {
            get => _threeDSecureToken;
            set
            {
                _threeDSecureTransaction = true;
                _threeDSecureToken = value;
            }
        }

        public RiskDataRequest RiskData { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? ShippingAmount { get; set; }
        public string ShipsFromPostalCode { get; set; }
        public TransactionLineItemRequest[] LineItems { get; set; }
        public ExternalVaultRequest ExternalVault { get; set; }
        // NEXT_MAJOR_VERSION Rename Android Pay to Google Pay
        public TransactionAndroidPayCardRequest AndroidPayCard { get; set; }
        public TransactionApplePayCardRequest ApplePayCard { get; set; }
        public string CurrencyIsoCode { get; set; }

        public InstallmentRequest InstallmentRequest { get; set; }

        public TransactionRequest()
        {
            CustomFields = new Dictionary<string, string>();
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
            builder.AddElement("product-sku", ProductSku);
            builder.AddElement("channel", Channel);
            builder.AddElement("exchange-rate-quote-id",ExchangeRateQuoteId);
// Remove this pragma warning when we remove DeviceSessionId, FraudMerchantId, and Recurring.
// We have this so we can build the SDK without obsolete error messages
#pragma warning disable 618
            builder.AddElement("device-session-id", DeviceSessionId);
            builder.AddElement("fraud-merchant-id", FraudMerchantId);
            if (Recurring.HasValue) builder.AddElement("recurring", Recurring);
#pragma warning restore 618
            builder.AddElement("transaction-source", TransactionSource);
            builder.AddElement("payment-method-token", PaymentMethodToken);
            builder.AddElement("payment-method-nonce", PaymentMethodNonce);
            builder.AddElement("purchase-order-number", PurchaseOrderNumber);
            builder.AddElement("shipping-address-id", ShippingAddressId);
            builder.AddElement("billing-address-id", BillingAddressId);
            if (TaxAmount.HasValue)
                builder.AddElement("tax-amount", TaxAmount);
            if (TaxExempt.HasValue)
                builder.AddElement("tax-exempt", TaxExempt);
            builder.AddElement("merchant-account-id", MerchantAccountId);
            if (ServiceFeeAmount.HasValue) builder.AddElement("service-fee-amount", ServiceFeeAmount);

            if (Type != null) builder.AddElement("type", Type.GetDescription());

            if (CustomFields.Count != 0) builder.AddElement("custom-fields", CustomFields);

            builder.AddElement("currency-iso-code", CurrencyIsoCode);
            builder.AddElement("credit-card", CreditCard);
            builder.AddElement("customer", Customer);
            builder.AddElement("descriptor", Descriptor);
            builder.AddElement("industry", Industry);
            builder.AddElement("billing", BillingAddress);
            builder.AddElement("shipping", ShippingAddress);
            builder.AddElement("paypal-account", PayPalAccount);
            builder.AddElement("options", Options);
            builder.AddElement("three-d-secure-pass-thru", ThreeDSecurePassThru);
            builder.AddElement("three-d-secure-authentication-id", ThreeDSecureAuthenticationId);
            builder.AddElement("venmo-sdk-payment-method-code", VenmoSdkPaymentMethodCode);
            builder.AddElement("sca-exemption", ScaExemption);
            builder.AddElement("shared-payment-method-token", SharedPaymentMethodToken);
            builder.AddElement("shared-payment-method-nonce", SharedPaymentMethodNonce);
            builder.AddElement("shared-customer-id", SharedCustomerId);
            builder.AddElement("shared-shipping-address-id", SharedShippingAddressId);
            builder.AddElement("shared-billing-address-id", SharedBillingAddressId);
            if (_threeDSecureTransaction)
                builder.AddElement("three-d-secure-token", ThreeDSecureToken ?? "");
            builder.AddElement("risk-data", RiskData);
            if (ShippingAmount.HasValue)
                builder.AddElement("shipping-amount", ShippingAmount);
            if (DiscountAmount.HasValue)
                builder.AddElement("discount-amount", DiscountAmount);
            builder.AddElement("ships-from-postal-code", ShipsFromPostalCode);
            if (LineItems != null)
                builder.AddElement("line-items", LineItems);
            if (ExternalVault != null)
                builder.AddElement("external-vault", ExternalVault);
            if (AndroidPayCard != null)
                builder.AddElement("android-pay-card", AndroidPayCard);
            if (ApplePayCard != null)
                builder.AddElement("apple-pay-card", ApplePayCard);
            if (InstallmentRequest != null)
                builder.AddElement("installments", InstallmentRequest);
            return builder;
        }
    }
}
