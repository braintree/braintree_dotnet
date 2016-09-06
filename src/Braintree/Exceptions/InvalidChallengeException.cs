#pragma warning disable 1591

namespace Braintree.Exceptions
{
    public class InvalidChallengeException : BraintreeException
    {
        public InvalidChallengeException(string message) : base(message) {}
        public InvalidChallengeException() : base() {}
    }
}
