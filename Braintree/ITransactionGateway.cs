#pragma warning disable 1591

#if NET452
using System.Threading.Tasks;
#endif

namespace Braintree
{
    /// <summary>
    /// Provides operations for sales, credits, refunds, voids, submitting for settlement, and searching for transactions in the vault
    /// </summary>
    public interface ITransactionGateway
    {
        Result<Transaction> CancelRelease(string id);
#if NET452
        Task<Result<Transaction>> CancelReleaseAsync(string id);
#endif
        Result<Transaction> CloneTransaction(string id, TransactionCloneRequest cloneRequest);
#if NET452
        Task<Result<Transaction>> CloneTransactionAsync(string id, TransactionCloneRequest cloneRequest);
#endif
        Result<Transaction> ConfirmTransparentRedirect(string queryString);
        Result<Transaction> Credit(TransactionRequest request);
#if NET452
        Task<Result<Transaction>> CreditAsync(TransactionRequest request);
#endif
        string CreditTrData(TransactionRequest trData, string redirectURL);
        Transaction Find(string id);
#if NET452
        Task<Transaction> FindAsync(string id);
#endif
        Result<Transaction> HoldInEscrow(string id);
#if NET452
        Task<Result<Transaction>> HoldInEscrowAsync(string id);
#endif
        Result<Transaction> Refund(string id);
#if NET452
        Task<Result<Transaction>> RefundAsync(string id);
#endif
        Result<Transaction> Refund(string id, decimal amount);
#if NET452
        Task<Result<Transaction>> RefundAsync(string id, decimal amount);
#endif
        Result<Transaction> ReleaseFromEscrow(string id);
#if NET452
        Task<Result<Transaction>> ReleaseFromEscrowAsync(string id);
#endif
        Result<Transaction> Sale(TransactionRequest request);
#if NET452
        Task<Result<Transaction>> SaleAsync(TransactionRequest request);
#endif
        string SaleTrData(TransactionRequest trData, string redirectURL);
        ResourceCollection<Transaction> Search(TransactionSearchRequest query);
#if NET452
        Task<ResourceCollection<Transaction>> SearchAsync(TransactionSearchRequest query);
#endif
        Result<Transaction> SubmitForPartialSettlement(string id, TransactionRequest request);
#if NET452
        Task<Result<Transaction>> SubmitForPartialSettlementAsync(string id, TransactionRequest request);
#endif
        Result<Transaction> SubmitForPartialSettlement(string id, decimal amount);
#if NET452
        Task<Result<Transaction>> SubmitForPartialSettlementAsync(string id, decimal amount);
#endif
        Result<Transaction> SubmitForSettlement(string id);
#if NET452
        Task<Result<Transaction>> SubmitForSettlementAsync(string id);
#endif
        Result<Transaction> SubmitForSettlement(string id, decimal amount);
#if NET452
        Task<Result<Transaction>> SubmitForSettlementAsync(string id, decimal amount);
#endif
        Result<Transaction> SubmitForSettlement(string id, TransactionRequest request);
#if NET452
        Task<Result<Transaction>> SubmitForSettlementAsync(string id, TransactionRequest request);
#endif
        Result<Transaction> UpdateDetails(string id, TransactionRequest request);
#if NET452
        Task<Result<Transaction>> UpdateDetailsAsync(string id, TransactionRequest request);
#endif
        string TransparentRedirectURLForCreate();
        Result<Transaction> Void(string id);
#if NET452
        Task<Result<Transaction>> VoidAsync(string id);
#endif
    }
}
