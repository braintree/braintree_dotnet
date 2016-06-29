using System;

namespace Braintree
{
    public class TransactionPayPalRequest : Request
    {
        public string PayeeEmail { get; set; }

        public override string ToXml()
        {
            return ToXml("paypal-account");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("paypal-account");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).AddElement("payee-email", PayeeEmail);
        }
    }
}
