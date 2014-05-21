using System;

namespace Braintree
{
    public class PaymentMethodGateway
    {
        private BraintreeService Service;

        public PaymentMethodGateway(BraintreeService service)
        {
            Service = service;
        }

        public Result<PaymentMethod> Create(PaymentMethodRequest request)
        {
            NodeWrapper response = new NodeWrapper(Service.Post("/payment_methods", request));

            return new ResultImpl<PayPalAccount>(response, Service);
        }

    }
}
