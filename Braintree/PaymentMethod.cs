using System;

namespace Braintree
{
    public interface PaymentMethod
    {
        string Token { get; }
        Boolean? IsDefault { get; }
        string ImageUrl { get; }
    }
}
