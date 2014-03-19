using System;
namespace Braintree.Tests
{
    public class ThreeDSecureRequestForTests : Request
    {
        public String PublicId { get; set; }
        public String ExpirationMonth { get; set; }
        public String ExpirationYear { get; set; }
        public String Number { get; set; }
        public String Status { get; set; }

        public override String ToXml()
        {
            return ToXml("cardinal-verification");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        public RequestBuilder BuildRequest(String root)
        {
            return new RequestBuilder(root).
                AddElement("public-id", PublicId).
                AddElement("expiration-month", ExpirationMonth).
                AddElement("expiration-year", ExpirationYear).
                AddElement("number", Number).
                AddElement("status", Status);
        }
    }
}

