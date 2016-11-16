#pragma warning disable 1591

namespace Braintree
{
    public interface IUsBankAccountGateway
    {
        UsBankAccount Find(string token);
    }
}
