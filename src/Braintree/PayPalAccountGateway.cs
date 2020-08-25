using System.Xml;

namespace Braintree
{
    public class PayPalAccountGateway : IPayPalAccountGateway
    {
        private readonly BraintreeService service;
        private readonly BraintreeGateway gateway;

        public PayPalAccountGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            this.gateway = gateway;
            service = gateway.Service;
        }

        public PayPalAccount Find(string token)
        {
            XmlNode xml = service.Get(service.MerchantPath() + "/payment_methods/paypal_account/" + token);

            return new PayPalAccount(new NodeWrapper(xml), gateway);
        }

        public void Delete(string token)
        {
            service.Delete(service.MerchantPath() + "/payment_methods/paypal_account/" + token);
        }

        public Result<PayPalAccount> Update(string token, PayPalAccountRequest request)
        {
            XmlNode xml = service.Put(service.MerchantPath() + "/payment_methods/paypal_account/" + token, request);
            return new ResultImpl<PayPalAccount>(new NodeWrapper(xml), gateway);
        }
    }
}

