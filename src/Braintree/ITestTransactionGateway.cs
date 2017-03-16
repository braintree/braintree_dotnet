#pragma warning disable 1591

using System.Threading.Tasks;

namespace Braintree
{
    /// <summary>
    /// Provides test operations for settling, settlement confirming, settlement pending, and settlement declining for transactions in the sandbox vault
    /// </summary>
    public interface ITestTransactionGateway
    {
        Transaction Settle(string id);
        Task<Transaction> SettleAsync(string id);
        Transaction SettlementConfirm(string id);
        Task<Transaction> SettlementConfirmAsync(string id);
        Transaction SettlementDecline(string id);
        Task<Transaction> SettlementDeclineAsync(string id);
        Transaction SettlementPending(string id);
    }
}
