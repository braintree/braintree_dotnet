#pragma warning disable 1591

using System;

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
    ///     BillingAddress = new CreditCardAddressRequest
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

    // NEXT_MAJOR_VERSION Remove ObsoleteAttributes
    public class CreditCardRequest : BaseCreditCardRequest
    {
        public string CustomerId { get; set; }
        public CreditCardAddressRequest BillingAddress { get; set; }
        public ThreeDSecurePassThruRequest ThreeDSecurePassThru { get; set; }
        public string BillingAddressId { get; set; }
        public string DeviceData { get; set; }
        [ObsoleteAttribute("use DeviceData instead", false)]
        public string DeviceSessionId { get; set; }
        [ObsoleteAttribute("use DeviceData instead", false)]
        public string FraudMerchantId { get; set; }
        public CreditCardOptionsRequest Options { get; set; }
        public string PaymentMethodToken { get; set; }
        public string PaymentMethodNonce { get; set; }
        [ObsoleteAttribute("the Venmo SDK integration is deprecated. Use Pay with Venmo instead https://developer.paypal.com/braintree/docs/guides/venmo/overview", false)]
        public string VenmoSdkPaymentMethodCode { get; set; }
        public string Token { get; set; }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).
                AddTopLevelElement("payment-method-token", PaymentMethodToken).
                ToQueryString();
        }

        // NEXT_MAJOR_VERSION Remove this pragma warnings when we remove DeviceSessionId, FraudMerchantId, and VenmoSdkPaymentMethodCode
        // We have this so we can build the SDK without obsolete error messages
        protected override RequestBuilder BuildRequest(string root)
        {
            return base.BuildRequest(root).
                AddElement("billing-address", BillingAddress).
                AddElement("billing-address-id", BillingAddressId).
                AddElement("device-data", DeviceData).
                AddElement("customer-id", CustomerId).
                #pragma warning disable 618
                AddElement("device-session-id", DeviceSessionId).
                AddElement("fraud-merchant-id", FraudMerchantId).
                AddElement("venmo-sdk-payment-method-code", VenmoSdkPaymentMethodCode).
                #pragma warning restore 618
                AddElement("options", Options).
                AddElement("payment-method-nonce", PaymentMethodNonce).
                AddElement("three-d-secure-pass-thru", ThreeDSecurePassThru).
                AddElement("token", Token);
        }
    }
}
