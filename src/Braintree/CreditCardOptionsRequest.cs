#pragma warning disable 1591

namespace Braintree
{
    public class CreditCardOptionsRequest : Request
    {
        public bool? FailOnDuplicatePaymentMethod { get; set; }
        public bool? MakeDefault { get; set; }
        public bool? SkipAdvancedFraudChecking { get; set; }
        public bool? VerifyCard { get; set; }
        public string UpdateExistingToken { get; set; }
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
                AddElement("fail-on-duplicate-payment-method", FailOnDuplicatePaymentMethod).
                AddElement("make-default", MakeDefault).
                AddElement("skip-advanced-fraud-checking", SkipAdvancedFraudChecking).
                AddElement("update-existing-token", UpdateExistingToken).
                AddElement("venmo-sdk-session", VenmoSdkSession).
                AddElement("verification-account-type", VerificationAccountType).
                AddElement("verification-amount", VerificationAmount).
                AddElement("verification-currency-iso-code", VerificationCurrencyIsoCode).
                AddElement("verification-merchant-account-id", VerificationMerchantAccountId).
                AddElement("verify-card", VerifyCard);
        }
    }
}
