#pragma warning disable 1591

using System.Threading.Tasks;

namespace Braintree
{
    /// <summary>
    /// Provides operations for finding verifications
    /// </summary>
    public interface IUsBankAccountVerificationGateway
    {
        UsBankAccountVerification Find(string Id);
        ResourceCollection<UsBankAccountVerification> Search(UsBankAccountVerificationSearchRequest query);
        Task<ResourceCollection<UsBankAccountVerification>> SearchAsync(UsBankAccountVerificationSearchRequest query);
    }
}
