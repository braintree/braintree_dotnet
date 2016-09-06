#pragma warning disable 1591

namespace Braintree
{
    public class CreditCardOptionsRequest : Request
    {
        public bool? VerifyCard { get; set; }
        public string VerificationAmount { get; set; }
        public bool? MakeDefault { get; set; }
        public bool? FailOnDuplicatePaymentMethod { get; set; }
        public string VerificationMerchantAccountId { get; set; }
        public string UpdateExistingToken { get; set; }
        public string VenmoSdkSession { get; set; }

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
                AddElement("verify-card", VerifyCard).
                AddElement("verification-amount", VerificationAmount).
                AddElement("fail-on-duplicate-payment-method", FailOnDuplicatePaymentMethod).
                AddElement("update-existing-token", UpdateExistingToken).
                AddElement("venmo-sdk-session", VenmoSdkSession);
        }
    }
}
