#pragma warning disable 1591

using System;

namespace Braintree
{
    public class TransactionOptionsRequest : Request
    {
        public bool? StoreInVault { get; set; }
        public bool? StoreInVaultOnSuccess { get; set; }
        public bool? AddBillingAddressToPaymentMethod { get; set; }
        public bool? StoreShippingAddressInVault { get; set; }
        public bool? SubmitForSettlement { get; set; }
        // NEXT_MAJOR_VERSION Remove VenmoSdkSession
        // The old venmo SDK class has been deprecated
        [ObsoleteAttribute("the Venmo SDK integration is deprecated. Use Pay with Venmo instead https://developer.paypal.com/braintree/docs/guides/venmo/overview", false)]
        public string VenmoSdkSession { get; set; }
        public string PayeeId { get; set; }
        public string PayeeEmail { get; set; }
        public bool? SkipAdvancedFraudChecking { get; set; }
        public bool? SkipAvs { get; set; }
        public bool? SkipCvv { get; set; }
        public TransactionOptionsPayPalRequest PayPal { get; set; }
        public TransactionOptionsThreeDSecureRequest ThreeDSecure { get; set; }
        public TransactionOptionsAmexRewardsRequest AmexRewards { get; set; }
        public TransactionOptionsVenmoRequest Venmo { get; set; }
        public TransactionOptionsCreditCardRequest CreditCard { get; set; }
        public TransactionOptionsUsBankAccountRequest UsBankAccount { get; set; }
        public TransactionOptionsProcessingOverridesRequest ProcessingOverrides { get; set; }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("store-in-vault", StoreInVault).
                AddElement("store-in-vault-on-success", StoreInVaultOnSuccess).
                AddElement("add-billing-address-to-payment-method", AddBillingAddressToPaymentMethod).
                AddElement("store-shipping-address-in-vault", StoreShippingAddressInVault).
                AddElement("submit-for-settlement", SubmitForSettlement).
                // NEXT_MAJOR_VERSION Remove this pragma warning when we remove VenmoSdkSession
                // We have this so we can build the SDK without obsolete error messages
                #pragma warning disable 618
                AddElement("venmo-sdk-session", VenmoSdkSession).
                #pragma warning restore 618
                AddElement("payee-id", PayeeId).
                AddElement("payee-email", PayeeEmail).
                AddElement("skip-advanced-fraud-checking", SkipAdvancedFraudChecking).
                AddElement("skip-avs", SkipAvs).
                AddElement("skip-cvv", SkipCvv).
                AddElement("three-d-secure", ThreeDSecure).
                AddElement("paypal", PayPal).
                AddElement("credit-card", CreditCard).
                AddElement("amex-rewards", AmexRewards).
                AddElement("venmo", Venmo).
                AddElement("us-bank-account", UsBankAccount).
                AddElement("processing-overrides", ProcessingOverrides);
        }
    }
}
