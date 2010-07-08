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

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        public override String ToQueryString()
        {
            return ToQueryString("credit-card");
        }

        public override String ToQueryString(String root)
        {
            return BuildRequest(root).
                AddTopLevelElement("payment-method-token", PaymentMethodToken).
                ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            return new RequestBuilder(root).
                AddElement("billing-address", BillingAddress).
                AddElement("cardholder-name", CardholderName).
                AddElement("customer-id", CustomerId).
                AddElement("cvv", CVV).
                AddElement("expiration-date", ExpirationDate).
                AddElement("expiration-month", ExpirationMonth).
                AddElement("expiration-year", ExpirationYear).
                AddElement("number", Number).
                AddElement("options", Options).
                AddElement("token", Token);
        }
    }
}
