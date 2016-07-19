using System;
using System.Collections.Generic;

namespace Braintree
{
    public class TransactionOptionsPayPalRequest : Request
    {
        public string Description { get; set; }
        public string CustomField { get; set; }
        public string PayeeEmail { get; set; }
        public Dictionary<string, string> SupplementaryData { get; set; }

        public TransactionOptionsPayPalRequest()
        {
            SupplementaryData = new Dictionary<string, string>();
        }

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
            var builder = new RequestBuilder(root).
                AddElement("description", Description).
                AddElement("custom-field", CustomField).
                AddElement("payee-email", PayeeEmail);

            if(SupplementaryData.Count != 0) builder.AddElement("supplementary-data", SupplementaryData);

            return builder;
        }
    }
}
