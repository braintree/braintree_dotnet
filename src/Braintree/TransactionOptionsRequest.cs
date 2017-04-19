#pragma warning disable 1591

namespace Braintree
{
    public class TransactionOptionsRequest : Request
    {
        public bool? HoldInEscrow { get; set; }
        public bool? StoreInVault { get; set; }
        public bool? StoreInVaultOnSuccess { get; set; }
        public bool? AddBillingAddressToPaymentMethod { get; set; }
        public bool? StoreShippingAddressInVault { get; set; }
        public bool? SubmitForSettlement { get; set; }
        public string VenmoSdkSession { get; set; }
        public string PayeeEmail { get; set; }
        public bool? SkipAdvancedFraudChecking { get; set; }
        public bool? SkipAvs { get; set; }
        public bool? SkipCvv { get; set; }
        public TransactionOptionsPayPalRequest PayPal { get; set; }
        public TransactionOptionsThreeDSecureRequest ThreeDSecure { get; set; }
        public TransactionOptionsAmexRewardsRequest AmexRewards { get; set; }

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
                AddElement("hold-in-escrow", HoldInEscrow).
                AddElement("store-in-vault", StoreInVault).
                AddElement("store-in-vault-on-success", StoreInVaultOnSuccess).
                AddElement("add-billing-address-to-payment-method", AddBillingAddressToPaymentMethod).
                AddElement("store-shipping-address-in-vault", StoreShippingAddressInVault).
                AddElement("submit-for-settlement", SubmitForSettlement).
                AddElement("venmo-sdk-session", VenmoSdkSession).
                AddElement("payee-email", PayeeEmail).
                AddElement("skip-advanced-fraud-checking", SkipAdvancedFraudChecking).
                AddElement("skip-avs", SkipAvs).
                AddElement("skip-cvv", SkipCvv).
                AddElement("three-d-secure", ThreeDSecure).
                AddElement("paypal", PayPal).
                AddElement("amex-rewards", AmexRewards);
        }
    }
}
