#pragma warning disable 1591

using System;

namespace Braintree
{
    [Serializable]
    public abstract class BaseCreditCardRequest : Request
    {
        public String CardholderName { get; set; }
        public String CVV { get; set; }
        public String ExpirationMonth { get; set; }
        public String ExpirationYear { get; set; }
        public String ExpirationDate { get; set; }
        public String Number { get; set; }
        public String Token { get; set; }

        public override String ToXml()
        {
            return ToXml("credit-card");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        public override String ToQueryString()
        {
            return ToQueryString("credit-card");
        }

        public override String ToQueryString(String root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(String root)
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
