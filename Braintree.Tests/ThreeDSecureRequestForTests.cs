using System;
namespace Braintree.Tests
{
    public class ThreeDSecureRequestForTests : Request
    {
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string Number { get; set; }

        public override string ToXml()
        {
            return ToXml("three-d-secure-verification");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("expiration-month", ExpirationMonth).
                AddElement("expiration-year", ExpirationYear).
                AddElement("number", Number);
        }
    }
}

