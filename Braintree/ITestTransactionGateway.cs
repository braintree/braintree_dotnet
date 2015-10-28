#pragma warning disable 1591

using System;

namespace Braintree
{
    /// <summary>
    /// Provides test operations for settling, settlement confirming, settlement pending, and settlement declining for transactions in the sandbox vault
    /// </summary>
    public interface ITestTransactionGateway
    {
        Transaction Settle(string id);
        Transaction SettlementConfirm(string id);
        Transaction SettlementDecline(string id);
        Transaction SettlementPending(string id);
    }
}
