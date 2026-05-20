#pragma warning disable 1591

namespace Braintree
{
    public class ApplePayCardOptionsRequest : Request
    {
        public bool? MakeDefault { get; set; }
        public string VerificationAccountType { get; set; }
        public string VerificationAmount { get; set; }
        public string VerificationMerchantAccountId { get; set; }
        public bool? VerifyCard { get; set; }

        public override string ToXml()
        {
            return ToXml("options");
        }

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
                AddElement("verification-account-type", VerificationAccountType).
                AddElement("verification-amount", VerificationAmount).
                AddElement("verification-merchant-account-id", VerificationMerchantAccountId).
                AddElement("verify-card", VerifyCard);
        }
    }
}
