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
        public string Token { get; set; }
        public string CustomerId { get; set; }
        public string Number { get; set; }
        public string CardholderName { get; set; }
        public string CVV { get; set; }
        public CreditCardAddressRequest BillingAddress { get; set; }
        public CreditCardOptionsRequest Options { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string ExpirationDate { get; set; }
        public string PaymentMethodToken { get; set; }

        protected override RequestBuilder Build(RequestBuilder builder)
        {
            return base.Build(builder).Override("c_v_v", "cvv", CVV);
        }

        public override string ToQueryString()
        {
            return ToQueryString("credit_card");
        }

        public override string ToQueryString(string root)
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
