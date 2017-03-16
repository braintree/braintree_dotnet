using System.Threading.Tasks;

namespace Braintree
{
    public class PaymentMethodNonceGateway : IPaymentMethodNonceGateway
    {
        private readonly BraintreeService service;
        private readonly BraintreeGateway gateway;

        public PaymentMethodNonceGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            this.gateway = gateway;
            this.service = new BraintreeService(gateway.Configuration);
        }

        public Result<PaymentMethodNonce> Create(string token)
        {
            var response = new NodeWrapper(service.Post(service.MerchantPath() + "/payment_methods/" + token + "/nonces"));

            return new ResultImpl<PaymentMethodNonce>(response, gateway);
        }

        public async Task<Result<PaymentMethodNonce>> CreateAsync(string token)
        {
            var response = new NodeWrapper(await service.PostAsync(service.MerchantPath() + "/payment_methods/" + token + "/nonces"));

            return new ResultImpl<PaymentMethodNonce>(response, gateway);
        }

        public virtual PaymentMethodNonce Find(string nonce)
        {
            var response = new NodeWrapper(service.Get(service.MerchantPath() + "/payment_method_nonces/" + nonce));

            return new PaymentMethodNonce(response, gateway);
        }

        public virtual async Task<PaymentMethodNonce> FindAsync(string nonce)
        {
            var response = new NodeWrapper(await service.GetAsync(service.MerchantPath() + "/payment_method_nonces/" + nonce));

            return new PaymentMethodNonce(response, gateway);
        }
    }
}
