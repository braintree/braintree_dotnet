using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class TransactionOptionsRequest : Request
    {
        public Boolean StoreInVault { get; set; }
        public Boolean AddBillingAddressToPaymentMethod { get; set; }
        public Boolean StoreShippingAddressInVault { get; set; }
        public Boolean SubmitForSettlement { get; set; }

        internal override String ToXml()
        {
            return ToXml("options");
        }

        internal override String ToXml(String rootElement)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("<{0}>", rootElement));
            builder.Append(BuildXMLElement("store-in-vault", StoreInVault.ToString()));
            builder.Append(BuildXMLElement("add-billing-address-to-payment-method", AddBillingAddressToPaymentMethod.ToString()));
            builder.Append(BuildXMLElement("store-shipping-address-in-vault", StoreShippingAddressInVault.ToString()));
            builder.Append(BuildXMLElement("submit-for-settlement", SubmitForSettlement.ToString()));
            builder.Append(String.Format("</{0}>", rootElement));

            return builder.ToString();
        }

        public override String ToQueryString()
        {
            return ToQueryString("options");
        }

        public override String ToQueryString(String root)
        {
            return new QueryString().
                Append(ParentBracketChildString(root, "store-in-vault"), StoreInVault.ToString()).
                Append(ParentBracketChildString(root, "add-billing-address-to-payment-method"), AddBillingAddressToPaymentMethod.ToString()).
                Append(ParentBracketChildString(root, "store-shipping-address-in-vault"), StoreShippingAddressInVault.ToString()).
                Append(ParentBracketChildString(root, "submit-for-settlement"), SubmitForSettlement.ToString()).
                ToString();
        }
    }
}