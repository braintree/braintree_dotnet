#pragma warning disable 1591

namespace Braintree
{
    /// <summary>
    /// Provides operations for creating, finding, updating, searching, and deleting subscriptions in the vault
    /// </summary>
    public interface ISubscriptionGateway
    {
        Result<Subscription> Cancel(string id);
        Result<Subscription> Create(SubscriptionRequest request);
        Subscription Find(string id);
        Result<Transaction> RetryCharge(string subscriptionId);
        Result<Transaction> RetryCharge(string subscriptionId, decimal amount);
        ResourceCollection<Subscription> Search(SubscriptionSearchRequest query);
        ResourceCollection<Subscription> Search(SubscriptionGateway.SearchDelegate searchDelegate);
        Result<Subscription> Update(string id, SubscriptionRequest request);
    }
}
