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
    public class CreditCardRequest : BaseCreditCardRequest
    {
        public String CustomerId { get; set; }
        public CreditCardAddressRequest BillingAddress { get; set; }
        public String BillingAddressId { get; set; }
        public CreditCardOptionsRequest Options { get; set; }
        public String PaymentMethodToken { get; set; }

        public override String Kind()
        {
            if (PaymentMethodToken == null)
            {
                return TransparentRedirectGateway.CREATE_PAYMENT_METHOD;
            }

            return TransparentRedirectGateway.UPDATE_PAYMENT_METHOD;
        }

        public override String ToQueryString(String root)
        {
            return BuildRequest(root).
                AddTopLevelElement("payment-method-token", PaymentMethodToken).
                ToQueryString();
        }

        protected override RequestBuilder BuildRequest(String root)
        {
            return base.BuildRequest(root).
                AddElement("billing-address", BillingAddress).
                AddElement("billing-address-id", BillingAddressId).
                AddElement("customer-id", CustomerId).
                AddElement("options", Options);
        }
    }
}
