#pragma warning disable 1591

using System.Threading.Tasks;
using System.Collections.Generic;

namespace Braintree
{
    /// <summary>
    /// Provides operations for finding verifications
    /// </summary>
    public interface IUsBankAccountVerificationGateway
    {
        Result<UsBankAccountVerification> ConfirmMicroTransferAmounts(string Id, UsBankAccountVerificationConfirmRequest request);
        UsBankAccountVerification Find(string Id);
        ResourceCollection<UsBankAccountVerification> Search(UsBankAccountVerificationSearchRequest query);
        Task<ResourceCollection<UsBankAccountVerification>> SearchAsync(UsBankAccountVerificationSearchRequest query);
    }
}
