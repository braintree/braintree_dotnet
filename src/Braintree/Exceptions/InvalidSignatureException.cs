#pragma warning disable 1591

namespace Braintree.Exceptions
{
    public class InvalidSignatureException : BraintreeException
    {
        public InvalidSignatureException(string message) : base(message) {}
        public InvalidSignatureException() : base() {}
    }
}
