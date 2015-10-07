using System;

namespace Braintree
{
    public interface IPaymentMethodGateway
    {
        Result<PaymentMethod> Create(PaymentMethodRequest request);
        void Delete(string token);
        PaymentMethod Find(string token);
        Result<PaymentMethod> Update(string token, PaymentMethodRequest request);
    }
}