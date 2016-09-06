#pragma warning disable 1591

namespace Braintree
{
    public class CreditCardVerificationCreditCardRequest : BaseCreditCardRequest {
        public CreditCardAddressRequest BillingAddress { get; set; }
    }

    public class CreditCardVerificationRequest : Request {
        public CreditCardVerificationCreditCardRequest CreditCard { get; set; }
        public CreditCardVerificationOptionsRequest Options { get; set; }

        public override string ToXml()
        {
            return ToXml("verification");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("credit-card", CreditCard).
                AddElement("options", Options);
        }
    }
}
