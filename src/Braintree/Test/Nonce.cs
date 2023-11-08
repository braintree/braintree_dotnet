namespace Braintree.Test
{
    public class Nonce
    {
        public const string AbstractTransactable = "fake-abstract-transactable-nonce";
        public const string ApplePayVisa = "fake-apple-pay-visa-nonce";
        public const string ApplePayMastercard = "fake-apple-pay-mastercard-nonce";
        public const string ApplePayAmex = "fake-apple-pay-amex-nonce";
        public const string ApplePayMpan = "fake-apple-pay-mpan-nonce";
        public const string AndroidPay = "fake-android-pay-nonce";
        public const string AndroidPayAmEx = "fake-android-pay-amex-nonce";
        public const string AndroidPayDiscover = "fake-android-pay-discover-nonce";
        public const string AndroidPayMasterCard = "fake-android-pay-mastercard-nonce";
        public const string AndroidPayVisa = "fake-android-pay-visa-nonce";
        public const string Consumed = "fake-consumed-nonce";
        public const string GatewayRejectedFraud = "fake-gateway-rejected-fraud-nonce";
        public const string GatewayRejectedTokenIssuance = "fake-token-issuance-error-venmo-account-nonce";
        public const string LocalPayment = "fake-local-payment-method-nonce";
        public const string LuhnInvalid = "fake-luhn-invalid-nonce";
        public const string MetaCheckoutCard = "fake-meta-checkout-card-nonce";
        public const string MetaCheckoutToken = "fake-meta-checkout-token-nonce";
        public const string PayPalBillingAgreement = "fake-paypal-billing-agreement-nonce";
        // NEXT_MAJOR_VERSION - no longer supported in the Gateway, remove this constant
        public const string PayPalFuturePayment = "fake-paypal-future-nonce";
        public const string PayPalFuturePaymentRefreshToken = "fake-paypal-future-refresh-token-nonce";
        public const string PayPalOneTimePayment = "fake-paypal-one-time-nonce";
        public const string ProcessorDeclinedAmEx = "fake-processor-declined-amex-nonce";
        public const string ProcessorDeclinedDiscover = "fake-processor-declined-discover-nonce";
        public const string ProcessorDeclinedMasterCard = "fake-processor-declined-mastercard-nonce";
        public const string ProcessorDeclinedVisa = "fake-processor-declined-visa-nonce";
        public const string ProcessorFailureJCB = "fake-processor-failure-jcb-nonce";
        public const string SamsungPayAmEx = "tokensam_fake_american_express";
        public const string SamsungPayDiscover = "tokensam_fake_discover";
        public const string SamsungPayMasterCard = "tokensam_fake_mastercard";
        public const string SamsungPayVisa = "tokensam_fake_visa";
        public const string SepaDirectDebitAccount = "fake-sepa-direct-debit-nonce";
        public const string ThreeDSecureTwoVisaErrorOnLookup = "fake-three-d-secure-two-visa-error-on-lookup-nonce";
        public const string ThreeDSecureTwoVisaSuccessfulFrictionlessAuthentication = "fake-three-d-secure-two-visa-successful-frictionless-authentication-nonce";
        public const string ThreeDSecureTwoVisaSuccessfulStepUpAuthentication = "fake-three-d-secure-two-visa-successful-step-up-authentication-nonce";
        public const string ThreeDSecureTwoVisaTimeoutOnLookup = "fake-three-d-secure-two-visa-timeout-on-lookup-nonce";
        public const string ThreeDSecureVisaAttemptsNonParticipating = "fake-three-d-secure-visa-attempts-non-participating-nonce";
        public const string ThreeDSecureVisaAuthenticationUnavailable = "fake-three-d-secure-visa-authentication-unavailable-nonce";
        public const string ThreeDSecureVisaBypassedAuthentication = "fake-three-d-secure-visa-bypassed-authentication-nonce";
        public const string ThreeDSecureVisaFailedAuthentication = "fake-three-d-secure-visa-failed-authentication-nonce";
        public const string ThreeDSecureVisaFailedSignature = "fake-three-d-secure-visa-failed-signature-nonce";
        public const string ThreeDSecureVisaFullAuthentication = "fake-three-d-secure-visa-full-authentication-nonce";
        public const string ThreeDSecureVisaLookupTimeout = "fake-three-d-secure-visa-lookup-timeout-nonce";
        public const string ThreeDSecureVisaMPIAuthenticateError = "fake-three-d-secure-visa-mpi-authenticate-error-nonce";
        public const string ThreeDSecureVisaMPILookupError = "fake-three-d-secure-visa-mpi-lookup-error-nonce";
        public const string ThreeDSecureVisaNoteEnrolled = "fake-three-d-secure-visa-not-enrolled-nonce";
        public const string ThreeDSecureVisaUnavailable = "fake-three-d-secure-visa-unavailable-nonce";
        public const string Transactable = "fake-valid-nonce";
        public const string TransactableAmEx = "fake-valid-amex-nonce";
        public const string TransactableCommercial = "fake-valid-commercial-nonce";
        public const string TransactableCountryOfIssuanceCAD = "fake-valid-country-of-issuance-cad-nonce";
        public const string TransactableCountryOfIssuanceUSA = "fake-valid-country-of-issuance-usa-nonce";
        public const string TransactableDebit = "fake-valid-debit-nonce";
        public const string TransactableDinersClub = "fake-valid-dinersclub-nonce";
        public const string TransactableDiscover = "fake-valid-discover-nonce";
        public const string TransactableDurbinRegulated = "fake-valid-durbin-regulated-nonce";
        public const string TransactableHealthcare = "fake-valid-healthcare-nonce";
        public const string TransactableIssuingBankNetworkOnly = "fake-valid-issuing-bank-network-only-nonce";
        public const string TransactableJCB = "fake-valid-jcb-nonce";
        public const string TransactableMaestro = "fake-valid-maestro-nonce";
        public const string TransactableMasterCard = "fake-valid-mastercard-nonce";
        public const string TransactableNoIndicators = "fake-valid-no-indicators-nonce";
        public const string TransactablePayroll = "fake-valid-payroll-nonce";
        public const string TransactablePrepaid = "fake-valid-prepaid-nonce";
        public const string TransactableUnknownIndicators = "fake-valid-unknown-indicators-nonce";
        public const string TransactableVisa = "fake-valid-visa-nonce";
        public const string UsBankAccount = "fake-us-bank-account-nonce";
        public const string VenmoAccount = "fake-venmo-account-nonce";
        public const string VisaCheckoutAmEx = "fake-visa-checkout-amex-nonce";
        public const string VisaCheckoutDiscover = "fake-visa-checkout-discover-nonce";
        public const string VisaCheckoutMasterCard = "fake-visa-checkout-mastercard-nonce";
        public const string VisaCheckoutVisa = "fake-visa-checkout-visa-nonce";
    }
}
