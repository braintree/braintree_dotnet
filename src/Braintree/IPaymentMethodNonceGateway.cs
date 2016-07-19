#pragma warning disable 1591

using System;

namespace Braintree
{
    public interface IPaymentMethodNonceGateway
    {
        Result<PaymentMethodNonce> Create(string token);
        PaymentMethodNonce Find(string nonce);
    }
}
