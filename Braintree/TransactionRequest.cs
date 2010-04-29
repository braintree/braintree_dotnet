#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    /// <summary>
    /// A class for building requests to manipulate <see cref="Transaction"/> records in the vault.
    /// </summary>
    /// <example>
    /// A transaction request can be constructed as follows:
    /// <code>
    /// var request = new TransactionRequest
    /// {
    ///     Amount = SandboxValues.TransactionAmount.AUTHORIZE,
    ///     CreditCard = new CreditCardRequest
    ///     {
    ///         Number = SandboxValues.CreditCardNumber.VISA,
    ///         ExpirationDate = "05/2009",
    ///     }
    /// };
    /// </code>
    /// </example>
    public class TransactionRequest : Request
    {
        public CreditCardRequest CreditCard { get; set; }
        public Decimal Amount { get; set; }
        public String OrderId { get; set; }
        public String MerchantAccountId { get; set; }
        public CustomerRequest Customer { get; set; }
        public AddressRequest BillingAddress { get; set; }
        public AddressRequest ShippingAddress { get; set; }
        public TransactionType Type { get; set; }
        public Dictionary<String, String> CustomFields { get; set; }
        public TransactionOptionsRequest Options { get; set; }
        public String PaymentMethodToken { get; set; }
        public String CustomerId { get; set; }
        public String ShippingAddressId { get; set; }

        public TransactionRequest()
        {
            CustomFields = new Dictionary<String, String>();
        }

        public override String ToXml()
        {
            return ToXml("transaction");
        }

        public override String ToXml(String rootElement)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("<{0}>", rootElement));
            if (Amount != 0) builder.Append(BuildXMLElement("amount", Amount.ToString()));
            builder.Append(BuildXMLElement("customer-id", CustomerId));
            builder.Append(BuildXMLElement("order-id", OrderId));
            builder.Append(BuildXMLElement("payment-method-token", PaymentMethodToken));
            builder.Append(BuildXMLElement("shipping-address-id", ShippingAddressId));
            builder.Append(BuildXMLElement("merchant-account-id", MerchantAccountId));

            if (Type != null) builder.Append(BuildXMLElement("type", Type.ToString().ToLower()));
            builder.Append(BuildXMLElement("custom-fields", CustomFields));
            builder.Append(BuildXMLElement(CreditCard));
            builder.Append(BuildXMLElement(Customer));
            builder.Append(BuildXMLElement("billing", BillingAddress));
            builder.Append(BuildXMLElement("shipping", ShippingAddress));
            builder.Append(BuildXMLElement(Options));
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
                Append(ParentBracketChildString(root, "customer_id"), CustomerId).
                Append(ParentBracketChildString(root, "order_id"), OrderId).
                Append(ParentBracketChildString(root, "payment_method_token"), PaymentMethodToken).
                Append(ParentBracketChildString(root, "shipping_address_id"), ShippingAddressId).
                Append(ParentBracketChildString(root, "custom_fields"), CustomFields).
                Append(ParentBracketChildString(root, "credit_card"), CreditCard).
                Append(ParentBracketChildString(root, "customer"), Customer).
                Append(ParentBracketChildString(root, "billing"), BillingAddress).
                Append(ParentBracketChildString(root, "shipping"), ShippingAddress).
                Append(ParentBracketChildString(root, "options"), Options).
                Append("payment_method_token", PaymentMethodToken);

            if (Type != null) qs.Append(ParentBracketChildString(root, "type"), Type.ToString().ToLower());
            if (Amount != 0) qs.Append(ParentBracketChildString(root, "amount"), Amount.ToString());

            return qs.ToString();
        }
    }
}
