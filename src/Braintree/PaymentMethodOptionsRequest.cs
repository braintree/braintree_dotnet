#pragma warning disable 1591

namespace Braintree
{
    public class PaymentMethodOptionsRequest : Request
    {
        public bool? FailOnDuplicatePaymentMethod { get; set; }
        public bool? MakeDefault { get; set; }
        public bool? SkipAdvancedFraudChecking { get; set; }
        public bool? VerifyCard { get; set; }
        public string VerificationAccountType { get; set; } // NEXT_MAJOR_VERSION - This should be an enum with [credit, debit]
        public string VerificationAmount { get; set; }
        public string VerificationCurrencyIsoCode { get; set; }
        public string VerificationMerchantAccountId { get; set; }
        public PaymentMethodOptionsPayPalRequest OptionsPayPal { get; set; }
        public UsBankAccountVerificationMethod? UsBankAccountVerificationMethod { get; set; }

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
                AddElement("paypal", OptionsPayPal).
                AddElement("skip-advanced-fraud-checking", SkipAdvancedFraudChecking).
                AddElement("us-bank-account-verification-method", UsBankAccountVerificationMethod.GetDescription()).
                AddElement("verification-account-type", VerificationAccountType).
                AddElement("verification-amount", VerificationAmount).
                AddElement("verification-currency-iso-code", VerificationCurrencyIsoCode).
                AddElement("verification-merchant-account-id", VerificationMerchantAccountId).
                AddElement("verify-card", VerifyCard);
        }
    }
}
