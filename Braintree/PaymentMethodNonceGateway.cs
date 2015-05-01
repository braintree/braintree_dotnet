using System;
using Braintree.Exceptions;

namespace Braintree
{
    public class PaymentMethodNonceGateway
    {
        private BraintreeService service;

        public PaymentMethodNonceGateway(BraintreeService service)
        {
            this.service = service;
        }

        public Result<PaymentMethodNonce> Create(string token)
        {
            NodeWrapper response = new NodeWrapper(service.Post("/payment_methods/" + token + "/nonces"));

            return new ResultImpl<PaymentMethodNonce>(response, service);
        }

        public virtual PaymentMethodNonce Find(String nonce)
        {
            NodeWrapper response = new NodeWrapper(service.Get("/payment_method_nonces/" + nonce));

            return new PaymentMethodNonce(response, service);
        }

    }
}
