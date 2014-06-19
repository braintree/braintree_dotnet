using System;

namespace Braintree
{
    public interface PaymentMethod
    {
        String Token { get; }
        Boolean? IsDefault { get; }
        String ImageUrl { get; }
    }
}
