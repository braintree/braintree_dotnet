using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class TransactionRequest : Request
    {
        public CreditCardRequest CreditCardRequest { get; set; }
        public Decimal Amount { get; set; }
        public String OrderId { get; set; }
        public CustomerRequest CustomerRequest { get; set; }
        public AddressRequest BillingAddressRequest { get; set; }
        public AddressRequest ShippingAddressRequest { get; set; }
        public TransactionType Type { get; set; }
        public Dictionary<String, String> CustomFields { get; set; }
        public TransactionOptionsRequest TransactionOptionsRequest { get; set; }
        public String PaymentMethodToken { get; set; }
        public String CustomerID { get; set; }
        public String ShippingAddressId { get; set; }

        public TransactionRequest()
        {
            CustomFields = new Dictionary<String, String>();
        }

        internal override String ToXml()
        {
            return ToXml("transaction");
        }

        internal override String ToXml(String rootElement)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("<{0}>", rootElement));
            if (Amount != 0) builder.Append(BuildXMLElement("amount", Amount.ToString()));
            builder.Append(BuildXMLElement("customer-id", CustomerID));
            builder.Append(BuildXMLElement("order-id", OrderId));
            builder.Append(BuildXMLElement("payment-method-token", PaymentMethodToken));
            builder.Append(BuildXMLElement("shipping-address-id", ShippingAddressId));
            builder.Append(BuildXMLElement("type", Type.ToString().ToLower()));
            builder.Append(BuildXMLElement("custom-fields", CustomFields));
            builder.Append(BuildXMLElement(CreditCardRequest));
            builder.Append(BuildXMLElement(CustomerRequest));
            builder.Append(BuildXMLElement("billing", BillingAddressRequest));
            builder.Append(BuildXMLElement("shipping", ShippingAddressRequest));
            builder.Append(BuildXMLElement(TransactionOptionsRequest));
            builder.Append(String.Format("</{0}>", rootElement));

            return builder.ToString();
        }

        public override String ToQueryString()
        {
            return ToQueryString("transaction");
        }

        public override String ToQueryString(String root)
        {
            QueryString qs = new QueryString().
                Append(ParentBracketChildString(root, "type"), Type.ToString().ToLower()).
                Append(ParentBracketChildString(root, "customer_id"), CustomerID).
                Append(ParentBracketChildString(root, "order_id"), OrderId).
                Append(ParentBracketChildString(root, "payment_method_token"), PaymentMethodToken).
                Append(ParentBracketChildString(root, "shipping_address_id"), ShippingAddressId).
                Append(ParentBracketChildString(root, "custom_fields"), CustomFields).
                Append(ParentBracketChildString(root, "credit_card"), CreditCardRequest).
                Append(ParentBracketChildString(root, "customer"), CustomerRequest).
                Append(ParentBracketChildString(root, "billing"), BillingAddressRequest).
                Append(ParentBracketChildString(root, "shipping"), ShippingAddressRequest).
                Append(ParentBracketChildString(root, "options"), TransactionOptionsRequest).
                Append("payment_method_token", PaymentMethodToken);

            if (Amount != 0) qs.Append(ParentBracketChildString(root, "amount"), Amount.ToString());

            return qs.ToString();
        }
    }
}
