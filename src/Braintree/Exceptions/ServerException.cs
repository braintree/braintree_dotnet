#pragma warning disable 1591

namespace Braintree.Exceptions
{
    public class ServerException : BraintreeException
    {
        public ServerException(string message) : base(message) {}
        public ServerException() : base() {}
    }
}
