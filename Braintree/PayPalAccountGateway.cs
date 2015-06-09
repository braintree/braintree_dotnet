using System;
using System.Xml;
using Braintree.Exceptions;

namespace Braintree
{
    public class PayPalAccountGateway
    {
        private BraintreeService Service;
        private BraintreeGateway Gateway;

        public PayPalAccountGateway(BraintreeGateway gateway)
        {
            Gateway = gateway;
            Service = new BraintreeService(gateway.Configuration);
        }

        public PayPalAccount Find(String token)
        {
            XmlNode xml = Service.Get("/payment_methods/paypal_account/" + token);

            return new PayPalAccount(new NodeWrapper(xml), Gateway);
        }

        public void Delete(String token)
        {
            Service.Delete("/payment_methods/paypal_account/" + token);
        }

        public Result<PayPalAccount> Update(String token, PayPalAccountRequest request)
        {
            XmlNode xml = Service.Put("/payment_methods/paypal_account/" + token, request);
            return new ResultImpl<PayPalAccount>(new NodeWrapper(xml), Gateway);
        }
    }
}

