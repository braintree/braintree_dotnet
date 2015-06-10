using System;
using Braintree.Exceptions;

namespace Braintree
{
    public class PaymentMethodNonceGateway
    {
        private BraintreeService service;
        private BraintreeGateway Gateway;

        public PaymentMethodNonceGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            Gateway = gateway;
            this.service = new BraintreeService(gateway.Configuration);
        }

        public Result<PaymentMethodNonce> Create(string token)
        {
            NodeWrapper response = new NodeWrapper(service.Post(service.MerchantPath() + "/payment_methods/" + token + "/nonces"));

            return new ResultImpl<PaymentMethodNonce>(response, Gateway);
        }

        public virtual PaymentMethodNonce Find(string nonce)
        {
            NodeWrapper response = new NodeWrapper(service.Get(service.MerchantPath() + "/payment_method_nonces/" + nonce));

            return new PaymentMethodNonce(response, Gateway);
        }

    }
}
