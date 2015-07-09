#pragma warning disable 1591

using System;

namespace Braintree.Exceptions
{
    public class AuthorizationException : BraintreeException
    {
        public AuthorizationException(string message) : base(message) {}
    }
}
