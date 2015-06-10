#pragma warning disable 1591

using System;

namespace Braintree
{
    public class MerchantAccountBusinessDetails
    {
        public string DbaName { get; protected set; }
        public string LegalName { get; protected set; }
        public string TaxId { get; protected set; }
        public Address Address { get; protected set; }

        protected internal MerchantAccountBusinessDetails(NodeWrapper node)
        {
            DbaName = node.GetString("dba-name");
            LegalName = node.GetString("legal-name");
            TaxId = node.GetString("tax-id");
            Address = new Address(node.GetNode("address"));
        }
    }
}
