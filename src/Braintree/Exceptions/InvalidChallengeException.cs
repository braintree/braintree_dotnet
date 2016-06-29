#pragma warning disable 1591

using System;

namespace Braintree.Exceptions
{
    public class InvalidChallengeException : BraintreeException
    {
        public InvalidChallengeException(string message) : base(message) {}
        public InvalidChallengeException() : base() {}
    }
}
