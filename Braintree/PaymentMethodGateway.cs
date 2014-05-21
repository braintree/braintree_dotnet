using System;

namespace Braintree
{
    public class PaymentMethodGateway
    {
        private BraintreeService service;

        public PaymentMethodGateway(BraintreeService service)
        {
            this.service = service;
        }

        public Result<PaymentMethod> Create(PaymentMethodRequest request)
        {
            NodeWrapper response = new NodeWrapper(service.Post("/payment_methods", request));

            if (response.GetName() == "paypal-account")
            {
                return new ResultImpl<PayPalAccount>(response, service);
            }
            else if (response.GetName() == "credit-card")
            {
                return new ResultImpl<CreditCard>(response, service);
            }
            else
            {
                return new ResultImpl<UnknownPaymentMethod>(response, service);
            }
        }

    }
}
