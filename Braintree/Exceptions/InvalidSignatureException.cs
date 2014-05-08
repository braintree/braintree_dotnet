#pragma warning disable 1591

using System;

namespace Braintree.Exceptions
{
    public class InvalidSignatureException : BraintreeException
    {
        public InvalidSignatureException(String message) : base(message) {}
        public InvalidSignatureException() : base() {}
    }
}
