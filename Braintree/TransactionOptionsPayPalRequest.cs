using System;

namespace Braintree
{
    public class TransactionOptionsPayPalRequest : Request
    {
        public string Description { get; set; }
        public string CustomField { get; set; }
        public string PayeeEmail { get; set; }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        private RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("description", Description).
                AddElement("custom-field", CustomField).
                AddElement("payee-email", PayeeEmail);
        }
    }
}
