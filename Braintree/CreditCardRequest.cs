#pragma warning disable 1591

using System;
using System.Text;

namespace Braintree
{
    /// <summary>
    /// A class for building requests to manipulate <see cref="CreditCard"/> records in the vault.
    /// </summary>
    /// <example>
    /// A credit card request can be constructed as follows:
    /// <code>
    /// CreditCardRequest createRequest = new CreditCardRequest
    /// {
    ///     CustomerId = customer.Id,
    ///     CardholderName = "John Doe",
    ///     Number = "5105105105105100",
    ///     ExpirationDate = "05/12",
    ///     BillingAddress = new AddressRequest
    ///     {
    ///         PostalCode = "44444"
    ///     },
    ///     Options = new CreditCardOptionsRequest
    ///     {
    ///         VerifyCard = true
    ///     }
    /// };
    /// </code>
    /// </example>
    public class CreditCardRequest : Request
    {
        public String Token { get; set; }
        public String CustomerId { get; set; }
        public String Number { get; set; }
        public String CardholderName { get; set; }
        public String CVV { get; set; }
        public CreditCardAddressRequest BillingAddress { get; set; }
        public CreditCardOptionsRequest Options { get; set; }
        public String ExpirationMonth { get; set; }
        public String ExpirationYear { get; set; }
        public String ExpirationDate { get; set; }
        public String PaymentMethodToken { get; set; }

        public override String Kind()
        {
            if (PaymentMethodToken == null)
            {
                return TransparentRedirectGateway.CREATE_PAYMENT_METHOD;
            }
            else
            {
                return TransparentRedirectGateway.UPDATE_PAYMENT_METHOD;
            }
        }

        public override String ToXml()
        {
            return ToXml("credit-card");
        }

        public override String ToXml(String rootElement)
        {
            var builder = new StringBuilder();
            builder.Append(String.Format("<{0}>", rootElement));
            builder.Append(BuildXMLElement("billing-address", BillingAddress));
            builder.Append(BuildXMLElement("cardholder-name", CardholderName));
            builder.Append(BuildXMLElement("customer-id", CustomerId));
            builder.Append(BuildXMLElement("cvv", CVV));
            builder.Append(BuildXMLElement("expiration-date", ExpirationDate));
            builder.Append(BuildXMLElement("expiration-month", ExpirationMonth));
            builder.Append(BuildXMLElement("expiration-year", ExpirationYear));
            builder.Append(BuildXMLElement("number", Number));
            builder.Append(BuildXMLElement("options", Options));
            builder.Append(BuildXMLElement("token", Token));
            builder.Append(String.Format("</{0}>", rootElement));

            return builder.ToString();
        }

        public override String ToQueryString()
        {
            return ToQueryString("credit_card");
        }

        public override String ToQueryString(String root)
        {
            return new QueryString().
                Append(ParentBracketChildString(root, "billing_address"), BillingAddress).
                Append(ParentBracketChildString(root, "customer_id"), CustomerId).
                Append(ParentBracketChildString(root, "cardholder_name"), CardholderName).
                Append(ParentBracketChildString(root, "cvv"), CVV).
                Append(ParentBracketChildString(root, "number"), Number).
                Append(ParentBracketChildString(root, "options"), Options).
                Append(ParentBracketChildString(root, "expiration_date"), ExpirationDate).
                Append(ParentBracketChildString(root, "token"), Token).
                Append("payment_method_token", PaymentMethodToken).
                ToString();
        }

    }
}
