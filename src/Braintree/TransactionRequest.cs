#pragma warning disable 1591

using System.Collections.Generic;
using System;

namespace Braintree
{
    public class TransactionCreditCardRequest : BaseCreditCardRequest
    {
        public NetworkTokenizationAttributesRequest NetworkTokenizationAttributes { get; set; }
        public PaymentReaderCardDetailsRequest PaymentReaderCardDetails { get; set; }
        public string Token { get; set; }

        protected override RequestBuilder BuildRequest(string root)
        {
            return base.BuildRequest(root)
                .AddElement("network-tokenization-attributes", NetworkTokenizationAttributes)
                .AddElement("payment-reader-card-details", PaymentReaderCardDetails)
                .AddElement("token", Token);
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

    // NEXT_MAJOR_VERSION Remove ObsoleteAttributes
    public class TransactionRequest : Request
    {
        public decimal Amount { get; set; }
        // NEXT_MAJOR_VERSION Rename Android Pay to Google Pay
        public TransactionAndroidPayCardRequest AndroidPayCard { get; set; }
        public TransactionApplePayCardRequest ApplePayCard { get; set; }
        public AddressRequest BillingAddress { get; set; }
        public string BillingAddressId { get; set; }
        public string Channel { get; set; }
        public TransactionCreditCardRequest CreditCard { get; set; }
        public string CurrencyIsoCode { get; set; }
        public CustomerRequest Customer { get; set; }
        public string CustomerId { get; set; }
        public Dictionary<string, string> CustomFields { get; set; }
        public DescriptorRequest Descriptor { get; set; }
        public string DeviceData { get; set; }
        [ObsoleteAttribute("use DeviceData instead", false)]
        public string DeviceSessionId { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string ExchangeRateQuoteId { get; set; }
        public ExternalVaultRequest ExternalVault { get; set; }
        public bool? FinalCapture { get; set; }
        public bool? ForeignRetailer { get; set; }
        [ObsoleteAttribute("use DeviceData instead", false)]
        public string FraudMerchantId { get; set; }
        public IndustryRequest Industry { get; set; }
        public InstallmentRequest InstallmentRequest { get; set; }
        public TransactionLineItemRequest[] LineItems { get; set; }
        public string MerchantAccountId { get; set; }
        public TransactionOptionsRequest Options { get; set; }
        public string OrderId { get; set; }
        public PaymentFacilitatorRequest PaymentFacilitator { get; set; }
        public string PaymentMethodNonce { get; set; }
        public string PaymentMethodToken { get; set; }
        public TransactionPayPalRequest PayPalAccount { get; set; }
        public string ProcessingMerchantCategoryCode { get; set; }
        public string ProductSku { get; set; }
        public string PurchaseOrderNumber { get; set; }
        [ObsoleteAttribute("use TransactionSource instead", false)]
        public bool? Recurring { get; set; }
        public RiskDataRequest RiskData { get; set; }
        public string ScaExemption { get; set; }
        public decimal? ServiceFeeAmount { get; set; }
        public string SharedBillingAddressId { get; set; }
        public string SharedCustomerId { get; set; }
        public string SharedPaymentMethodNonce { get; set; }
        public string SharedPaymentMethodToken { get; set; }
        public string SharedShippingAddressId { get; set; }
        public AddressRequest ShippingAddress { get; set; }
        public string ShippingAddressId { get; set; }
        public decimal? ShippingAmount { get; set; }
        public decimal? ShippingTaxAmount { get; set; }
        public string ShipsFromPostalCode { get; set; }
        public decimal? TaxAmount { get; set; }
        public bool? TaxExempt { get; set; }
        public string ThreeDSecureAuthenticationId { get; set; }
        public ThreeDSecurePassThruRequest ThreeDSecurePassThru { get; set; }
        [ObsoleteAttribute("use threeDSecureAuthenticationId instead", false)]
        public string ThreeDSecureToken {
            get => _threeDSecureToken;
            set
            {
                _threeDSecureTransaction = true;
                _threeDSecureToken = value;
            }
        }
        public string TransactionSource { get; set; }
        public TransactionType? Type { get; set; }
        public TransferRequest Transfer { get; set; }
        [ObsoleteAttribute("the Venmo SDK integration is deprecated. Use Pay with Venmo instead https://developer.paypal.com/braintree/docs/guides/venmo/overview", false)]
        public string VenmoSdkPaymentMethodCode { get; set; }
        
        private TransactionUsBankAccountRequest usBankAccountRequest;

        /// <summary>
        /// Creates a new TransactionUsBankAccountRequest for configuring US bank account details.
        /// </summary>
        /// <returns>a TransactionUsBankAccountRequest</returns>
        public TransactionUsBankAccountRequest UsBankAccount()
        {
            usBankAccountRequest = new TransactionUsBankAccountRequest(this);
            return usBankAccountRequest;
        }
    
        // NEXT_MAJOR_VERSION replace ThreeDSecureToken with ThreeDSecureAuthenticationId
        // threeDSecureToken has been deprecated in favor of threeDSecureAuthenticationId
        private string _threeDSecureToken;
        private bool _threeDSecureTransaction;

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
            if (AndroidPayCard != null) builder.AddElement("android-pay-card", AndroidPayCard);
            if (ApplePayCard != null) builder.AddElement("apple-pay-card", ApplePayCard);
            builder.AddElement("billing", BillingAddress);
            builder.AddElement("billing-address-id", BillingAddressId);
            builder.AddElement("channel", Channel);
            builder.AddElement("credit-card", CreditCard);
            builder.AddElement("currency-iso-code", CurrencyIsoCode);
            builder.AddElement("customer", Customer);
            builder.AddElement("customer-id", CustomerId);
            if (CustomFields.Count != 0) builder.AddElement("custom-fields", CustomFields);
            builder.AddElement("descriptor", Descriptor);
            builder.AddElement("device-data", DeviceData);
            if (DiscountAmount.HasValue) builder.AddElement("discount-amount", DiscountAmount);
            builder.AddElement("exchange-rate-quote-id",ExchangeRateQuoteId);
            if (ExternalVault != null) builder.AddElement("external-vault", ExternalVault);
            builder.AddElement("final-capture", FinalCapture);
            if (ForeignRetailer.HasValue) builder.AddElement("foreign-retailer", ForeignRetailer);
            builder.AddElement("industry", Industry);
            if (InstallmentRequest != null) builder.AddElement("installments", InstallmentRequest);
            if (LineItems != null) builder.AddElement("line-items", LineItems);
            builder.AddElement("merchant-account-id", MerchantAccountId);
            builder.AddElement("options", Options);
            builder.AddElement("order-id", OrderId); 
            if (PaymentFacilitator != null) builder.AddElement("payment-facilitator", PaymentFacilitator);
            builder.AddElement("payment-method-nonce", PaymentMethodNonce);
            builder.AddElement("payment-method-token", PaymentMethodToken);
            builder.AddElement("paypal-account", PayPalAccount);
            builder.AddElement("processing-merchant-category-code", ProcessingMerchantCategoryCode);
            builder.AddElement("product-sku", ProductSku);
            builder.AddElement("purchase-order-number", PurchaseOrderNumber);
            // Remove this pragma warning when we remove DeviceSessionId, FraudMerchantId, and Recurring.
            // We have this so we can build the SDK without obsolete error messages
            #pragma warning disable 618
            builder.AddElement("device-session-id", DeviceSessionId);
            builder.AddElement("fraud-merchant-id", FraudMerchantId);
            if (Recurring.HasValue) builder.AddElement("recurring", Recurring);
            #pragma warning restore 618
            builder.AddElement("risk-data", RiskData);
            builder.AddElement("sca-exemption", ScaExemption);
            if (ServiceFeeAmount.HasValue) builder.AddElement("service-fee-amount", ServiceFeeAmount);
            builder.AddElement("shared-billing-address-id", SharedBillingAddressId);
            builder.AddElement("shared-customer-id", SharedCustomerId);
            builder.AddElement("shared-payment-method-nonce", SharedPaymentMethodNonce);
            builder.AddElement("shared-payment-method-token", SharedPaymentMethodToken);
            builder.AddElement("shared-shipping-address-id", SharedShippingAddressId);
            builder.AddElement("shipping", ShippingAddress);
            builder.AddElement("shipping-address-id", ShippingAddressId);
            if (ShippingAmount.HasValue) builder.AddElement("shipping-amount", ShippingAmount);
            if (ShippingTaxAmount.HasValue) builder.AddElement("shipping-tax-amount", ShippingTaxAmount);
            builder.AddElement("ships-from-postal-code", ShipsFromPostalCode);
            if (TaxAmount.HasValue) builder.AddElement("tax-amount", TaxAmount);
            if (TaxExempt.HasValue) builder.AddElement("tax-exempt", TaxExempt);
            builder.AddElement("three-d-secure-authentication-id", ThreeDSecureAuthenticationId);
            builder.AddElement("three-d-secure-pass-thru", ThreeDSecurePassThru);
            // NEXT_MAJOR_VERSION Remove this pragma warning and ThreeDSecureToken
            // We have this so we can build the SDK without obsolete error messages
            #pragma warning disable 618
            if (_threeDSecureTransaction)
                builder.AddElement("three-d-secure-token", ThreeDSecureToken ?? "");
            #pragma warning restore 618
            builder.AddElement("transaction-source", TransactionSource);
            if (Transfer != null) builder.AddElement("transfer", Transfer);
            if (Type != null) builder.AddElement("type", Type.GetDescription());
            // NEXT_MAJOR_VERSION Remove this pragma warning when we remove VenmoSdkPaymentMethodCode
            // We have this so we can build the SDK without obsolete error messages
            #pragma warning disable 618
            builder.AddElement("venmo-sdk-payment-method-code", VenmoSdkPaymentMethodCode);
            #pragma warning restore 618
            builder.AddElement("usBankAccount", usBankAccountRequest);

            return builder;
        }
    }
}
