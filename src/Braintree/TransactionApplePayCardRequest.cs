using System;
namespace Braintree
{
    public class TransactionApplePayCardRequest : Request
    {
        public string Number { get; set; }
        public string CardholderName { get; set; }
        public string Cryptogram { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string EciIndicator { get; set; }

        public override string ToXml()
        {
            return ToXml("apple-pay-card");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("number", Number).
                AddElement("cardholder-name", CardholderName).
                AddElement("cryptogram", Cryptogram).
                AddElement("expiration-month", ExpirationMonth).
                AddElement("expiration-year", ExpirationYear).
                AddElement("eci-indicator", EciIndicator);
        }
    }
}