namespace Braintree.Exceptions
{
    public class UnprocessableEntityException : BraintreeException
    {
        public UnprocessableEntityException(string message) : base(message) { }
    }
}
