#pragma warning disable 1591

namespace Braintree.Exceptions
{
    public class PaymentMethodNonceRequestInvalidException : BraintreeException
    {
        public PaymentMethodNonceRequestInvalidException(string message) : base(message) {}
    }
}
