using System;

namespace Braintree
{
    public class TransactionPayPalRequest : Request
    {
        public String PayeeEmail { get; set; }

        public override String ToXml()
        {
            return ToXml("paypal-account");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        public override String ToQueryString()
        {
            return ToQueryString("paypal-account");
        }

        public override String ToQueryString(String root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            return new RequestBuilder(root).AddElement("payee-email", PayeeEmail);
        }
    }
}
