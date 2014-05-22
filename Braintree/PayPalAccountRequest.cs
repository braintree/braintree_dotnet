using System;

namespace Braintree
{
    public class PayPalAccountRequest : Request
    {
        public String Token { get; set; }

        public override String ToXml()
        {
            return ToXml("paypal-account");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            return new RequestBuilder(root).
                AddElement("token", Token);
        }
    }
}
