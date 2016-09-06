#pragma warning disable 1591

namespace Braintree.Exceptions
{
    public class TestOperationPerformedInProductionException : BraintreeException
    {
        public TestOperationPerformedInProductionException(string message) : base(message) {}
    }
}
