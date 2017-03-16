#pragma warning disable 1591

using System.Threading.Tasks;

namespace Braintree
{
    /// <summary>
    /// Provides operations for finding verifications
    /// </summary>
    public interface ICreditCardVerificationGateway
    {
        CreditCardVerification Find(string Id);
        ResourceCollection<CreditCardVerification> Search(CreditCardVerificationSearchRequest query);
        Task<ResourceCollection<CreditCardVerification>> SearchAsync(CreditCardVerificationSearchRequest query);
        Result<CreditCardVerification> Create(CreditCardVerificationRequest request);
    }
}
