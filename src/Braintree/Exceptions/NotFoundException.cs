#pragma warning disable 1591

namespace Braintree.Exceptions
{
    public class NotFoundException : BraintreeException
    {
        public NotFoundException(string message) : base(message) {}
        public NotFoundException() : base() {}
    }
}
