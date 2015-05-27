using System;
using System.Xml;
using Braintree.Exceptions;

namespace Braintree
{
    public class PayPalAccountGateway
    {
        private BraintreeService Service;

        public PayPalAccountGateway(BraintreeService service)
        {
            Service = service;
        }

        public PayPalAccount Find(String token)
        {
            XmlNode xml = Service.Get(Service.MerchantPath() + "/payment_methods/paypal_account/" + token);

            return new PayPalAccount(new NodeWrapper(xml), Service);
        }

        public void Delete(String token)
        {
            Service.Delete(Service.MerchantPath() + "/payment_methods/paypal_account/" + token);
        }

        public Result<PayPalAccount> Update(String token, PayPalAccountRequest request)
        {
            XmlNode xml = Service.Put(Service.MerchantPath() + "/payment_methods/paypal_account/" + token, request);
            return new ResultImpl<PayPalAccount>(new NodeWrapper(xml), Service);
        }
    }
}

