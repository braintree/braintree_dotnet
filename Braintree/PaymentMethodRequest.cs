using System;

namespace Braintree
{
    public class PaymentMethodRequest : Request
    {
        public String CustomerId { get; set; }
        public String PaymentMethodNonce { get; set; }

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
                AddElement("payment-method-nonce", PaymentMethodNonce);
        }
    }
}
