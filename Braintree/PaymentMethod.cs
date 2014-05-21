using System;

namespace Braintree
{
    public interface PaymentMethod
    {
        String Token { get; }
    }
}
