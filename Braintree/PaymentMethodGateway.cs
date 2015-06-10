using System;
using Braintree.Exceptions;

namespace Braintree
{
    public class PaymentMethodGateway
    {
        private BraintreeService service;
        private BraintreeGateway Gateway;

        public PaymentMethodGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            Gateway = gateway;
            this.service = new BraintreeService(gateway.Configuration);
        }

        public Result<PaymentMethod> Create(PaymentMethodRequest request)
        {
            NodeWrapper response = new NodeWrapper(service.Post(service.MerchantPath() + "/payment_methods", request));

            if (response.GetName() == "paypal-account")
            {
                return new ResultImpl<PayPalAccount>(response, Gateway);
            }
            else if (response.GetName() == "credit-card")
            {
                return new ResultImpl<CreditCard>(response, Gateway);
            }
            else if (response.GetName() == "apple-pay-card")
            {
                return new ResultImpl<ApplePayCard>(response, Gateway);
            }
            else if (response.GetName() == "android-pay-card")
            {
                return new ResultImpl<AndroidPayCard>(response, Gateway);
            }
            else if (response.GetName() == "coinbase-account")
            {
                return new ResultImpl<CoinbaseAccount>(response, Gateway);
            }
            else
            {
                return new ResultImpl<UnknownPaymentMethod>(response, Gateway);
            }
        }

        public Result<PaymentMethod> Update(string token, PaymentMethodRequest request)
        {
            var response = new NodeWrapper(service.Put(service.MerchantPath() + "/payment_methods/any/" + token, request));

            if (response.GetName() == "paypal-account")
            {
                return new ResultImpl<PayPalAccount>(response, Gateway);
            }
            else if (response.GetName() == "credit-card")
            {
                return new ResultImpl<CreditCard>(response, Gateway);
            }
            else if (response.GetName() == "apple-pay-card")
            {
                return new ResultImpl<ApplePayCard>(response, Gateway);
            }
            else if (response.GetName() == "android-pay-card")
            {
                return new ResultImpl<AndroidPayCard>(response, Gateway);
            }
            else
            {
                return new ResultImpl<UnknownPaymentMethod>(response, Gateway);
            }
        }

        public void Delete(string token)
        {
            service.Delete(service.MerchantPath() + "/payment_methods/any/" + token);
        }

        public PaymentMethod Find(string token)
        {
            if(token == null || token.Trim().Equals(""))
                throw new NotFoundException();

            NodeWrapper response = new NodeWrapper(service.Get(service.MerchantPath() + "/payment_methods/any/" + token));

            if (response.GetName() == "paypal-account")
            {
                return new PayPalAccount(response, Gateway);
            }
            else if (response.GetName() == "credit-card")
            {
                return new CreditCard(response, Gateway);
            }
            else if (response.GetName() == "apple-pay-card")
            {
                return new ApplePayCard(response, Gateway);
            }
            else if (response.GetName() == "android-pay-card")
            {
                return new AndroidPayCard(response, Gateway);
            }
            else if (response.GetName() == "coinbase-account")
            {
                return new CoinbaseAccount(response, Gateway);
            }
            else
            {
                return new UnknownPaymentMethod(response);
            }
        }
    }
}
