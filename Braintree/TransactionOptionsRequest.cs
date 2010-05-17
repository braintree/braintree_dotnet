#pragma warning disable 1591

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

        public override String ToXml()
        {
            return ToXml("options");
        }

        public override String ToXml(String rootElement)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("<{0}>", rootElement));
            builder.Append(BuildXMLElement("store-in-vault", StoreInVault));
            builder.Append(BuildXMLElement("add-billing-address-to-payment-method", AddBillingAddressToPaymentMethod));
            builder.Append(BuildXMLElement("store-shipping-address-in-vault", StoreShippingAddressInVault));
            builder.Append(BuildXMLElement("submit-for-settlement", SubmitForSettlement));
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
                Append(ParentBracketChildString(root, "store_in_vault"), StoreInVault).
                Append(ParentBracketChildString(root, "add_billing_address_to_payment_method"), AddBillingAddressToPaymentMethod).
                Append(ParentBracketChildString(root, "store_shipping_address_in_vault"), StoreShippingAddressInVault).
                Append(ParentBracketChildString(root, "submit_for_settlement"), SubmitForSettlement).
                ToString();
        }
    }
}
