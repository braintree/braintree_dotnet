using System.Collections.Generic;
using System.Threading.Tasks;

namespace Braintree
{
    public class PayPalPaymentResourceGateway : IPayPalPaymentResourceGateway
    {
        private readonly BraintreeService service;
        private readonly BraintreeGateway gateway;

        public PayPalPaymentResourceGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            this.gateway = gateway;
            service = gateway.Service;
        }

        public virtual Result<PaymentMethodNonce>  Update(PayPalPaymentResourceRequest request)
        {
            var response = new NodeWrapper(service.Put(service.MerchantPath() + "/paypal/payment_resource", request));

            return new ResultImpl<PaymentMethodNonce>(response, gateway);
        }

        public virtual async Task<Result<PaymentMethodNonce>> UpdateAsync(PayPalPaymentResourceRequest request)
        {
            var response = new NodeWrapper(await service.PutAsync(service.MerchantPath() + "/paypal/payment_resource", request).ConfigureAwait(false));

           return new ResultImpl<PaymentMethodNonce>(response, gateway);
        }
    }
}