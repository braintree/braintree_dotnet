using System;

namespace Braintree
{
    public class PaymentMethodRequest : Request
    {
        public String CustomerId { get; set; }
        public String Token { get; set; }
        public String PaymentMethodNonce { get; set; }
        public String DeviceData { get; set; }
        public PaymentMethodOptionsRequest Options { get; set; }
        public PaymentMethodAddressRequest BillingAddress { get; set; }
        public String BillingAddressId { get; set; }
        public String CardholderName { get; set; }
        public String CVV { get; set; }
        public String ExpirationMonth { get; set; }
        public String ExpirationYear { get; set; }
        public String ExpirationDate { get; set; }
        public String Number { get; set; }

        public override String ToXml()
        {
            return ToXml("payment-method");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            return new RequestBuilder(root).
                AddElement("customer-id", CustomerId).
                AddElement("payment-method-nonce", PaymentMethodNonce).
                AddElement("device-data", DeviceData).
                AddElement("token", Token).
                AddElement("options", Options).
                AddElement("billing-address", BillingAddress).
                AddElement("billing-address-id", BillingAddressId).
                AddElement("cardholder-name", CardholderName).
                AddElement("cvv", CVV).
                AddElement("expiration-date", ExpirationDate).
                AddElement("expiration-month", ExpirationMonth).
                AddElement("expiration-year", ExpirationYear).
                AddElement("number", Number);
        }
    }
}
