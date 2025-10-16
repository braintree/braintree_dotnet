namespace Braintree
{
    public class PaymentMethodRequest : Request
    {
        public string CustomerId { get; set; }
        public string Token { get; set; }
        public string PaymentMethodNonce { get; set; }
        public string ThreeDSecureAuthenticationId { get; set; }
        public PaymentMethodOptionsRequest Options { get; set; }
        public PaymentMethodAddressRequest BillingAddress { get; set; }
        public ThreeDSecurePassThruRequest ThreeDSecurePassThru { get; set; }
        public string BillingAddressId { get; set; }
        public string CardholderName { get; set; }
        public string CVV { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string ExpirationDate { get; set; }
        public string Number { get; set; }
        public string DeviceData { get; set; }
        public string PayPalRefreshToken { get; set; }

        private PaymentMethodUsBankAccountRequest usBankAccountRequest;

        /// <summary>
        /// Creates a new PaymentMethodUsBankAccountRequest for configuring US bank account details.
        /// </summary>
        /// <returns>a PaymentMethodUsBankAccountRequest</returns>
        public PaymentMethodUsBankAccountRequest UsBankAccount()
        {
            usBankAccountRequest = new PaymentMethodUsBankAccountRequest(this);
            return usBankAccountRequest;
        }

        public override string ToXml()
        {
            return ToXml("payment-method");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("customer-id", CustomerId).
                AddElement("payment-method-nonce", PaymentMethodNonce).
                AddElement("token", Token).
                AddElement("options", Options).
                AddElement("billing-address", BillingAddress).
                AddElement("billing-address-id", BillingAddressId).
                AddElement("cardholder-name", CardholderName).
                AddElement("cvv", CVV).
                AddElement("device-data", DeviceData).
                AddElement("expiration-date", ExpirationDate).
                AddElement("expiration-month", ExpirationMonth).
                AddElement("expiration-year", ExpirationYear).
                AddElement("number", Number).
                AddElement("paypal-refresh-token", PayPalRefreshToken).
                AddElement("three-d-secure-authentication-id", ThreeDSecureAuthenticationId).
                AddElement("three-d-secure-pass-thru", ThreeDSecurePassThru).
                AddElement("us-bank-account", usBankAccountRequest);
        }
    }
}
