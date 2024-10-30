#pragma warning disable 1591

namespace Braintree
{
    public class ClientTokenOptionsRequest : Request
    {
        public bool? FailOnDuplicatePaymentMethod { get; set; }
        public bool? FailOnDuplicatePaymentMethodForCustomer { get; set; }
        public bool? MakeDefault { get; set; }
        public bool? VerifyCard { get; set; }

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
                AddElement("fail-on-duplicate-payment-method-for-customer", FailOnDuplicatePaymentMethodForCustomer).
                AddElement("make-default", MakeDefault).
                AddElement("verify-card", VerifyCard);
        }
    }
}
