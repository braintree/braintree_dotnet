#pragma warning disable 1591

using System.Collections.Generic;
using System;

namespace Braintree 
{
    public class TransactionSubMerchantRequest: Request 
    {
        public AddressRequest Address { get; set; }
        public string LegalName { get; set; }
        public string ReferenceNumber { get; set; }
        public string TaxId { get; set; }

        public override string ToXml()
        {
            return ToXml("sub-merchant");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("sub-merchant");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root)
                .AddElement("address", Address)
                .AddElement("legal-name", LegalName)
                .AddElement("reference-number", ReferenceNumber)
                .AddElement("tax-id", TaxId);
        }
    }
}