#pragma warning disable 1591

using System;

namespace Braintree
{
    /// <summary>
    /// Provides operations for sales, credits, refunds, voids, submitting for settlement, and searching for transactions in the vault
    /// </summary>
    public interface ITransactionGateway
    {
        Result<Transaction> CancelRelease(string id);
        Result<Transaction> CloneTransaction(string id, TransactionCloneRequest cloneRequest);
        Result<Transaction> ConfirmTransparentRedirect(string queryString);
        Result<Transaction> Credit(TransactionRequest request);
        string CreditTrData(TransactionRequest trData, string redirectURL);
        Transaction Find(string id);
        Result<Transaction> HoldInEscrow(string id);
        Result<Transaction> Refund(string id);
        Result<Transaction> Refund(string id, decimal amount);
        Result<Transaction> ReleaseFromEscrow(string id);
        Result<Transaction> Sale(TransactionRequest request);
        string SaleTrData(TransactionRequest trData, string redirectURL);
        ResourceCollection<Transaction> Search(TransactionSearchRequest query);
        Result<Transaction> SubmitForPartialSettlement(string id, decimal amount);
        Result<Transaction> SubmitForSettlement(string id);
        Result<Transaction> SubmitForSettlement(string id, decimal amount);
        Result<Transaction> SubmitForSettlement(string id, TransactionRequest request);
        string TransparentRedirectURLForCreate();
        Result<Transaction> Void(string id);
    }
}
