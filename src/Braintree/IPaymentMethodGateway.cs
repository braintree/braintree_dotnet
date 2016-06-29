#pragma warning disable 1591

using System;

namespace Braintree
{
    public interface IPaymentMethodGateway
    {
        Result<PaymentMethod> Create(PaymentMethodRequest request);
        Result<PaymentMethod> Delete(string token);
        PaymentMethod Find(string token);
        Result<PaymentMethod> Update(string token, PaymentMethodRequest request);
    }
}
