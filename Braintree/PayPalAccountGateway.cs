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

            XmlNode xml = Service.Get("/payment_methods/paypal_account/" + token);

            return new PayPalAccount(new NodeWrapper(xml));
            //throw new NotFoundException();
        }
    }
}

