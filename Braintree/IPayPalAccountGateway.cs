#pragma warning disable 1591

using System;

namespace Braintree
{
    public interface IPayPalAccountGateway
    {
        void Delete(string token);
        PayPalAccount Find(string token);
        Result<PayPalAccount> Update(string token, PayPalAccountRequest request);
    }
}
