#pragma warning disable 1591

namespace Braintree
{
    public class FundingRequest : Request
    {
        public FundingDestination? Destination { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Descriptor { get; set; }

        public override string ToXml()
        {
            return ToXml("funding");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("funding");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("destination", Destination.GetDescription()).
                AddElement("email", Email).
                AddElement("mobile-phone", MobilePhone).
                AddElement("routing-number", RoutingNumber).
                AddElement("account-number", AccountNumber).
                AddElement("descriptor", Descriptor);
        }
    }
}
