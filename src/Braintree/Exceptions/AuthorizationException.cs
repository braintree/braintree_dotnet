#pragma warning disable 1591

namespace Braintree.Exceptions
{
    public class AuthorizationException : BraintreeException
    {
        public AuthorizationException(string message) : base(message) {}
    }
}
