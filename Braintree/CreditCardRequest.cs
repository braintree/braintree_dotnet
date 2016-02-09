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
        public string CustomerId { get; set; }
        public CreditCardAddressRequest BillingAddress { get; set; }
        public string BillingAddressId { get; set; }
        public string DeviceData { get; set; }
        public string DeviceSessionId { get; set; }
        public string FraudMerchantId { get; set; }
        public CreditCardOptionsRequest Options { get; set; }
        public string PaymentMethodToken { get; set; }
        public string PaymentMethodNonce { get; set; }
        public string VenmoSdkPaymentMethodCode { get; set; }
        public string Token { get; set; }

        public override string Kind()
        {
            if (PaymentMethodToken == null)
            {
                return TransparentRedirectGateway.CREATE_PAYMENT_METHOD;
            }

            return TransparentRedirectGateway.UPDATE_PAYMENT_METHOD;
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).
                AddTopLevelElement("payment-method-token", PaymentMethodToken).
                ToQueryString();
        }

        protected override RequestBuilder BuildRequest(string root)
        {
            return base.BuildRequest(root).
                AddElement("billing-address", BillingAddress).
                AddElement("billing-address-id", BillingAddressId).
                AddElement("device-data", DeviceData).
                AddElement("customer-id", CustomerId).
                AddElement("device-session-id", DeviceSessionId).
                AddElement("fraud-merchant-id", FraudMerchantId).
                AddElement("options", Options).
                AddElement("payment-method-nonce", PaymentMethodNonce).
                AddElement("venmo-sdk-payment-method-code", VenmoSdkPaymentMethodCode).
                AddElement("token", Token);
        }
    }
}
