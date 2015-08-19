#pragma warning disable 1591

using System;

namespace Braintree.Exceptions
{
    public class TestOperationPerformedInProductionException : BraintreeException
    {
        public TestOperationPerformedInProductionException(string message) : base(message) {}
    }
}
