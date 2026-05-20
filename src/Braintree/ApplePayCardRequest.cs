#pragma warning disable 1591

namespace Braintree
{
    public class ApplePayCardRequest : Request
    {
        public CreditCardAddressRequest BillingAddress { get; set; }
        public string CardholderName { get; set; }
        public string Cryptogram { get; set; }
        public string EciIndicator { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string NetworkTransactionId { get; set; }
        public string Number { get; set; }
        public ApplePayCardOptionsRequest Options { get; set; }
        public string Token { get; set; }

        public override string ToXml()
        {
            return ToXml("apple-pay-card");
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
                AddElement("billing-address", BillingAddress).
                AddElement("cardholder-name", CardholderName).
                AddElement("cryptogram", Cryptogram).
                AddElement("eci-indicator", EciIndicator).
                AddElement("expiration-month", ExpirationMonth).
                AddElement("expiration-year", ExpirationYear).
                AddElement("network-transaction-id", NetworkTransactionId).
                AddElement("number", Number).
                AddElement("options", Options).
                AddElement("token", Token);
        }
    }
}
