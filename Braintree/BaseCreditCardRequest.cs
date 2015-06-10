#pragma warning disable 1591

using System;

namespace Braintree
{
    public abstract class BaseCreditCardRequest : Request
    {
        public string CardholderName { get; set; }
        public string CVV { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string ExpirationDate { get; set; }
        public string Number { get; set; }
        public string Token { get; set; }

        public override string ToXml()
        {
            return ToXml("credit-card");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("credit-card");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("cardholder-name", CardholderName).
                AddElement("cvv", CVV).
                AddElement("expiration-date", ExpirationDate).
                AddElement("expiration-month", ExpirationMonth).
                AddElement("expiration-year", ExpirationYear).
                AddElement("number", Number).
                AddElement("token", Token);
        }
    }
}
