#pragma warning disable 1591

using System.Threading.Tasks;

namespace Braintree
{
    public interface IBankAccountInstantVerificationGateway
    {
        Result<BankAccountInstantVerificationJwt> CreateJwt(BankAccountInstantVerificationJwtRequest request);
        Task<Result<BankAccountInstantVerificationJwt>> CreateJwtAsync(BankAccountInstantVerificationJwtRequest request);
    }
}