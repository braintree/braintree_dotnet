using System;

namespace Braintree
{
    public interface PaymentMethod
    {
        string Token { get; }
        bool? IsDefault { get; }
        string ImageUrl { get; }
        string CustomerId { get; }
    }
}
