using Braintree.Exceptions;
using System.Threading.Tasks;

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
            return ExtractResultFromResponse(response);
        }

        public async Task<Result<PaymentMethod>> CreateAsync(PaymentMethodRequest request)
        {
            var response = new NodeWrapper(await service.PostAsync(service.MerchantPath() + "/payment_methods", request).ConfigureAwait(false));
            return ExtractResultFromResponse(response);
        }

        public Result<PaymentMethod> Update(string token, PaymentMethodRequest request)
        {
            var response = new NodeWrapper(service.Put(service.MerchantPath() + "/payment_methods/any/" + token, request));
            return ExtractResultFromResponse(response);
        }

        public async Task<Result<PaymentMethod>> UpdateAsync(string token, PaymentMethodRequest request)
        {
            var response = new NodeWrapper(await service.PutAsync(service.MerchantPath() + "/payment_methods/any/" + token, request));
            return ExtractResultFromResponse(response);
        }

        public Result<PaymentMethod> Delete(string token)
        {
            return Delete(token, null);
        }

        public async Task<Result<PaymentMethod>> DeleteAsync(string token)
        {
            var response = new NodeWrapper(await service.DeleteAsync(service.MerchantPath() + "/payment_methods/any/" + token).ConfigureAwait(false));
            return new ResultImpl<UnknownPaymentMethod>(response, gateway);
        }

        public Result<PaymentMethod> Delete(string token, PaymentMethodDeleteRequest request)
        {
            string queryString = (request == null ? "" : "?" + request.ToQueryString());

            var response = new NodeWrapper(service.Delete(service.MerchantPath() + "/payment_methods/any/" + token + queryString));
            return new ResultImpl<UnknownPaymentMethod>(response, gateway);
        }

        public PaymentMethod Find(string token)
        {
            if(token == null || token.Trim().Equals(""))
                throw new NotFoundException();

            var response = new NodeWrapper(service.Get(service.MerchantPath() + "/payment_methods/any/" + token));
            return ExtractPaymentMethodFromResponse(response);
        }

        public async Task<PaymentMethod> FindAsync(string token)
        {
            if(token == null || token.Trim().Equals(""))
                throw new NotFoundException();

            var response = new NodeWrapper(await service.GetAsync(service.MerchantPath() + "/payment_methods/any/" + token).ConfigureAwait(false));
            return ExtractPaymentMethodFromResponse(response);
        }

        public Result<PaymentMethodNonce> Grant(string token, PaymentMethodGrantRequest request)
        {
            request.SharedPaymentMethodToken = token;
            var response = new NodeWrapper(service.Post(service.MerchantPath() + "/payment_methods/grant", request));
            return new ResultImpl<PaymentMethodNonce>(response, gateway);
        }

        public Result<PaymentMethod> Revoke(string token)
        {
            var request = new PaymentMethodGrantRevokeRequest()
            {
                SharedPaymentMethodToken = token
            };
            var response = new NodeWrapper(service.Post(service.MerchantPath() + "/payment_methods/revoke", request));
            return new ResultImpl<UnknownPaymentMethod>(response, gateway);
        }

        private Result<PaymentMethod> ExtractResultFromResponse(NodeWrapper response)
        {
            if (response.GetName() == "paypal-account")
            {
                return new ResultImpl<PayPalAccount>(response, gateway);
            }
            else if (response.GetName() == "us-bank-account")
            {
                return new ResultImpl<UsBankAccount>(response, gateway);
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

        private PaymentMethod ExtractPaymentMethodFromResponse(NodeWrapper response)
        {
            if (response.GetName() == "paypal-account")
            {
                return new PayPalAccount(response, gateway);
            }
            else if (response.GetName() == "us-bank-account")
            {
                return new UsBankAccount(response);
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
