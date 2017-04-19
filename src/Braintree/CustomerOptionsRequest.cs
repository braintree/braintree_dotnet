#pragma warning disable 1591

namespace Braintree
{
    public class CustomerOptionsRequest : Request
    {
        public CustomerOptionsPayPalRequest OptionsPayPal { get; set; }

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
                AddElement("paypal", OptionsPayPal);
        }
    }
}
