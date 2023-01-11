#pragma warning disable 1591

namespace Braintree
{
    public interface ISepaDirectDebitAccountGateway
    {
        void Delete(string token);
        SepaDirectDebitAccount Find(string token);
    }
}
