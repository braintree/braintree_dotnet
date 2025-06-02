#pragma warning disable 1591

using System;

namespace Braintree
{
    public class CreditCardOptionsRequest : Request
    {
        public bool? FailOnDuplicatePaymentMethod { get; set; }
        public bool? FailOnDuplicatePaymentMethodForCustomer { get; set; }
        public bool? MakeDefault { get; set; }
        public bool? SkipAdvancedFraudChecking { get; set; }
        public bool? VerifyCard { get; set; }
        public string AccountInformationInquiry { get; set; }
        public string UpdateExistingToken { get; set; }
        // NEXT_MAJOR_VERSION Remove VenmoSdkSession
        // The old venmo SDK class has been deprecated
        [ObsoleteAttribute("the Venmo SDK integration is deprecated. Use Pay with Venmo instead https://developer.paypal.com/braintree/docs/guides/venmo/overview", false)]
        public string VenmoSdkSession { get; set; }
        public string VerificationAccountType { get; set; } // NEXT_MAJOR_VERSION - This should be an enum with [credit, debit]
        public string VerificationAmount { get; set; }
        public string VerificationCurrencyIsoCode { get; set; }
        public string VerificationMerchantAccountId { get; set; }

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
                AddElement("account-information-inquiry", AccountInformationInquiry).
                AddElement("fail-on-duplicate-payment-method", FailOnDuplicatePaymentMethod).
                AddElement("fail-on-duplicate-payment-method-for-customer", FailOnDuplicatePaymentMethodForCustomer).
                AddElement("make-default", MakeDefault).
                AddElement("skip-advanced-fraud-checking", SkipAdvancedFraudChecking).
                AddElement("update-existing-token", UpdateExistingToken).
                // NEXT_MAJOR_VERSION Remove this pragma warning when we remove VenmoSdkSession
                // We have this so we can build the SDK without obsolete error messages
                #pragma warning disable 618
                AddElement("venmo-sdk-session", VenmoSdkSession).
                #pragma warning restore 618
                AddElement("verification-account-type", VerificationAccountType).
                AddElement("verification-amount", VerificationAmount).
                AddElement("verification-currency-iso-code", VerificationCurrencyIsoCode).
                AddElement("verification-merchant-account-id", VerificationMerchantAccountId).
                AddElement("verify-card", VerifyCard);
        }
    }
}
