#pragma warning disable 1591

using System.Threading.Tasks;

namespace Braintree
{
    /// <summary>
    /// Provides operations for sales, credits, refunds, voids, submitting for settlement, and searching for transactions in the vault
    /// </summary>
    public interface ITransactionGateway
    {
        Result<Transaction> AdjustAuthorization(string id, decimal amount);
        Task<Result<Transaction>> AdjustAuthorizationAsync(string id, decimal amount);
        Result<Transaction> CancelRelease(string id);
        Task<Result<Transaction>> CancelReleaseAsync(string id);
        Result<Transaction> CloneTransaction(string id, TransactionCloneRequest cloneRequest);
        Task<Result<Transaction>> CloneTransactionAsync(string id, TransactionCloneRequest cloneRequest);
        Result<Transaction> Credit(TransactionRequest request);
        Task<Result<Transaction>> CreditAsync(TransactionRequest request);
        Transaction Find(string id);
        Task<Transaction> FindAsync(string id);
        Result<Transaction> Refund(string id);
        Task<Result<Transaction>> RefundAsync(string id);
        Result<Transaction> Refund(string id, decimal amount);
        Task<Result<Transaction>> RefundAsync(string id, decimal amount);
        Result<Transaction> Refund(string id, TransactionRefundRequest refundRequest);
        Task<Result<Transaction>> RefundAsync(string id, TransactionRefundRequest refundRequest);
        Result<Transaction> Sale(TransactionRequest request);
        Task<Result<Transaction>> SaleAsync(TransactionRequest request);
        ResourceCollection<Transaction> Search(TransactionSearchRequest query);
        Task<ResourceCollection<Transaction>> SearchAsync(TransactionSearchRequest query);
        Result<Transaction> PackageTracking(string id, PackageTrackingRequest request);
        Task<Result<Transaction>> PackageTrackingAsync(string id, PackageTrackingRequest request);
        Result<Transaction> SubmitForPartialSettlement(string id, TransactionRequest request);
        Result<Transaction> SubmitForPartialSettlement(string id, decimal amount);
        Result<Transaction> SubmitForSettlement(string id);
        Task<Result<Transaction>> SubmitForSettlementAsync(string id);
        Result<Transaction> SubmitForSettlement(string id, decimal amount);
        Task<Result<Transaction>> SubmitForSettlementAsync(string id, decimal amount);
        Task<Result<Transaction>> SubmitForSettlementAsync(string id, TransactionRequest request);
        Result<Transaction> SubmitForSettlement(string id, TransactionRequest request);
        Result<Transaction> UpdateDetails(string id, TransactionRequest request);
        Result<Transaction> Void(string id);
        Task<Result<Transaction>> VoidAsync(string id);
    }
}
