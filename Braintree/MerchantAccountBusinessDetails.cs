#pragma warning disable 1591

using System;

namespace Braintree
{
    public class MerchantAccountBusinessDetails
    {
        public String DbaName { get; protected set; }
        public String LegalName { get; protected set; }
        public String TaxId { get; protected set; }
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
