#pragma warning disable 1591

namespace Braintree
{
    public class PaymentMethodOptionsRequest : Request
    {
        public bool? MakeDefault { get; set; }
        public bool? VerifyCard { get; set; }
        public bool? FailOnDuplicatePaymentMethod { get; set; }
        public string VerificationMerchantAccountId { get; set; }
        public string VerificationAmount { get; set; }
        public string VerificationAccountType { get; set; }
        public PaymentMethodOptionsPayPalRequest OptionsPayPal { get; set; }
        public UsBankAccountVerificationMethod UsBankAccountVerificationMethod { get; set; }

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
                AddElement("make-default", MakeDefault).
                AddElement("verification-merchant-account-id", VerificationMerchantAccountId).
                AddElement("verification-account-type", VerificationAccountType).
                AddElement("verify-card", VerifyCard).
                AddElement("us-bank-account-verification-method", UsBankAccountVerificationMethod).
                AddElement("verification-amount", VerificationAmount).
                AddElement("fail-on-duplicate-payment-method", FailOnDuplicatePaymentMethod).
                AddElement("paypal", OptionsPayPal);
        }
    }
}
