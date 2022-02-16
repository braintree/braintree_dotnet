#pragma warning disable 1591

namespace Braintree.Exceptions
{
    public class UnexpectedException : BraintreeException
    {
        public UnexpectedException() : base() { }
        public UnexpectedException(string message) : base(message) { }
    }
}
