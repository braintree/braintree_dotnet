using System;
using Braintree.Exceptions;

namespace Braintree
{
    public class PaymentMethodGateway : IPaymentMethodGateway
    {
        private readonly BraintreeService service;
        private readonly BraintreeGateway gateway;

        public PaymentMethodGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            this.gateway = gateway;
            this.service = new BraintreeService(gateway.Configuration);
        }

        public Result<PaymentMethod> Create(PaymentMethodRequest request)
        {
            var response = new NodeWrapper(service.Post(service.MerchantPath() + "/payment_methods", request));

            if (response.GetName() == "paypal-account")
            {
                return new ResultImpl<PayPalAccount>(response, gateway);
            }
            else if (response.GetName() == "credit-card")
            {
                return new ResultImpl<CreditCard>(response, gateway);
            }
            else if (response.GetName() == "apple-pay-card")
            {
                return new ResultImpl<ApplePayCard>(response, gateway);
            }
            else if (response.GetName() == "android-pay-card")
            {
                return new ResultImpl<AndroidPayCard>(response, gateway);
            }
            else if (response.GetName() == "amex-express-checkout-card")
            {
                return new ResultImpl<AmexExpressCheckoutCard>(response, gateway);
            }
            else if (response.GetName() == "coinbase-account")
            {
                return new ResultImpl<CoinbaseAccount>(response, gateway);
            }
            else if (response.GetName() == "venmo-account")
            {
                return new ResultImpl<VenmoAccount>(response, gateway);
            }
            else
            {
                return new ResultImpl<UnknownPaymentMethod>(response, gateway);
            }
        }

        public Result<PaymentMethod> Update(string token, PaymentMethodRequest request)
        {
            var response = new NodeWrapper(service.Put(service.MerchantPath() + "/payment_methods/any/" + token, request));

            if (response.GetName() == "paypal-account")
            {
                return new ResultImpl<PayPalAccount>(response, gateway);
            }
            else if (response.GetName() == "coinbase-account")
            {
                return new ResultImpl<CoinbaseAccount>(response, gateway);
            }
            else if (response.GetName() == "credit-card")
            {
                return new ResultImpl<CreditCard>(response, gateway);
            }
            else if (response.GetName() == "apple-pay-card")
            {
                return new ResultImpl<ApplePayCard>(response, gateway);
            }
            else if (response.GetName() == "android-pay-card")
            {
                return new ResultImpl<AndroidPayCard>(response, gateway);
            }
            else
            {
                return new ResultImpl<UnknownPaymentMethod>(response, gateway);
            }
        }

        public Result<PaymentMethod> Delete(string token)
        {
            var response = new NodeWrapper(service.Delete(service.MerchantPath() + "/payment_methods/any/" + token));
            return new ResultImpl<UnknownPaymentMethod>(response, gateway);
        }

        public PaymentMethod Find(string token)
        {
            if(token == null || token.Trim().Equals(""))
                throw new NotFoundException();

            var response = new NodeWrapper(service.Get(service.MerchantPath() + "/payment_methods/any/" + token));

            if (response.GetName() == "paypal-account")
            {
                return new PayPalAccount(response, gateway);
            }
            else if (response.GetName() == "credit-card")
            {
                return new CreditCard(response, gateway);
            }
            else if (response.GetName() == "apple-pay-card")
            {
                return new ApplePayCard(response, gateway);
            }
            else if (response.GetName() == "android-pay-card")
            {
                return new AndroidPayCard(response, gateway);
            }
            else if (response.GetName() == "coinbase-account")
            {
                return new CoinbaseAccount(response, gateway);
            }
            else if (response.GetName() == "venmo-account")
            {
                return new VenmoAccount(response, gateway);
            }
            else
            {
                return new UnknownPaymentMethod(response);
            }
        }
    }
}
