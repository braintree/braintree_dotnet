#pragma warning disable 1591

using System.Threading.Tasks;

namespace Braintree
{
    /// <summary>
    /// Provides operations for creating, finding, updating, searching, and deleting subscriptions in the vault
    /// </summary>
    public interface ISubscriptionGateway
    {
        Result<Subscription> Cancel(string id);
        Task<Result<Subscription>> CancelAsync(string id);
        Result<Subscription> Create(SubscriptionRequest request);
        Task<Result<Subscription>> CreateAsync(SubscriptionRequest request);
        Subscription Find(string id);
        Task<Subscription> FindAsync(string id);
        Result<Transaction> RetryCharge(string subscriptionId);
        Task<Result<Transaction>> RetryChargeAsync(string subscriptionId);
        Result<Transaction> RetryCharge(string subscriptionId, bool submitForSettlement);
        Task<Result<Transaction>> RetryChargeAsync(string subscriptionId, bool submitForSettlement);
        Result<Transaction> RetryCharge(string subscriptionId, decimal amount);
        Task<Result<Transaction>> RetryChargeAsync(string subscriptionId, decimal amount);
        Result<Transaction> RetryCharge(string subscriptionId, decimal amount, bool submitForSettlement);
        Task<Result<Transaction>> RetryChargeAsync(string subscriptionId, decimal amount, bool submitForSettlement);
        ResourceCollection<Subscription> Search(SubscriptionSearchRequest query);
        Task<ResourceCollection<Subscription>> SearchAsync(SubscriptionSearchRequest query);
        ResourceCollection<Subscription> Search(SubscriptionGateway.SearchDelegate searchDelegate);
        Result<Subscription> Update(string id, SubscriptionRequest request);
        Task<Result<Subscription>> UpdateAsync(string id, SubscriptionRequest request);
    }
}
