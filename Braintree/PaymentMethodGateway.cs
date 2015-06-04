using System;
using Braintree.Exceptions;

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
            else if (response.GetName() == "apple-pay-card")
            {
                return new ResultImpl<ApplePayCard>(response, service);
            }
            else if (response.GetName() == "android-pay-card")
            {
                return new ResultImpl<AndroidPayCard>(response, service);
            }
            else if (response.GetName() == "coinbase-account")
            {
                return new ResultImpl<CoinbaseAccount>(response, service);
            }
            else
            {
                return new ResultImpl<UnknownPaymentMethod>(response, service);
            }
        }

        public Result<PaymentMethod> Update(string token, PaymentMethodRequest request)
        {
            var response = new NodeWrapper(service.Put("/payment_methods/any/" + token, request));

            if (response.GetName() == "paypal-account")
            {
                return new ResultImpl<PayPalAccount>(response, service);
            }
            else if (response.GetName() == "credit-card")
            {
                return new ResultImpl<CreditCard>(response, service);
            }
            else if (response.GetName() == "apple-pay-card")
            {
                return new ResultImpl<ApplePayCard>(response, service);
            }
            else if (response.GetName() == "android-pay-card")
            {
                return new ResultImpl<AndroidPayCard>(response, service);
            }
            else
            {
                return new ResultImpl<UnknownPaymentMethod>(response, service);
            }
        }

        public void Delete(String token)
        {
            service.Delete("/payment_methods/any/" + token);
        }

        public PaymentMethod Find(String token)
        {
            if(token == null || token.Trim().Equals(""))
                throw new NotFoundException();

            NodeWrapper response = new NodeWrapper(service.Get("/payment_methods/any/" + token));

            if (response.GetName() == "paypal-account")
            {
                return new PayPalAccount(response, service);
            }
            else if (response.GetName() == "credit-card")
            {
                return new CreditCard(response, service);
            }
            else if (response.GetName() == "apple-pay-card")
            {
                return new ApplePayCard(response, service);
            }
            else if (response.GetName() == "android-pay-card")
            {
                return new AndroidPayCard(response, service);
            }
            else if (response.GetName() == "coinbase-account")
            {
                return new CoinbaseAccount(response, service);
            }
            else
            {
                return new UnknownPaymentMethod(response);
            }
        }
    }
}
