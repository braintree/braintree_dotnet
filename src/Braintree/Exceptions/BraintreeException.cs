#pragma warning disable 1591

using System;

namespace Braintree.Exceptions
{
    public class BraintreeException : Exception
    {
        public BraintreeException(string message) : base(message) {}
        public BraintreeException() : base() {}
    }
}
