#pragma warning disable 1591

using Braintree.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace Braintree
{
    /// <summary>
    /// Provides operations for creating, finding, updating, searching, and deleting subscriptions in the vault
    /// </summary>
    public class SubscriptionGateway : ISubscriptionGateway
    {
        private readonly BraintreeService service;
        private readonly BraintreeGateway gateway;

        protected internal SubscriptionGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            this.gateway = gateway;
            service = new BraintreeService(gateway.Configuration);
        }

        public delegate void SearchDelegate(SubscriptionSearchRequest search);

        public virtual Result<Subscription> Create(SubscriptionRequest request)
        {
            XmlNode subscriptionXML = service.Post(service.MerchantPath() + "/subscriptions", request);

            return new ResultImpl<Subscription>(new NodeWrapper(subscriptionXML), gateway);
        }

        public virtual async Task<Result<Subscription>> CreateAsync(SubscriptionRequest request)
        {
            XmlNode subscriptionXML = await service.PostAsync(service.MerchantPath() + "/subscriptions", request);

            return new ResultImpl<Subscription>(new NodeWrapper(subscriptionXML), gateway);
        }

        public virtual Subscription Find(string id)
        {
            if(id == null || id.Trim().Equals(""))
                throw new NotFoundException();

            XmlNode subscriptionXML = service.Get(service.MerchantPath() + "/subscriptions/" + id);

            return new Subscription(new NodeWrapper(subscriptionXML), gateway);
        }

        public virtual async Task<Subscription> FindAsync(string id)
        {
            if(id == null || id.Trim().Equals(""))
                throw new NotFoundException();

            XmlNode subscriptionXML = await service.GetAsync(service.MerchantPath() + "/subscriptions/" + id);

            return new Subscription(new NodeWrapper(subscriptionXML), gateway);
        }

        public virtual Result<Subscription> Update(string id, SubscriptionRequest request)
        {
            XmlNode subscriptionXML = service.Put(service.MerchantPath() + "/subscriptions/" + id, request);

            return new ResultImpl<Subscription>(new NodeWrapper(subscriptionXML), gateway);
        }

        public virtual async Task<Result<Subscription>> UpdateAsync(string id, SubscriptionRequest request)
        {
            XmlNode subscriptionXML = await service.PutAsync(service.MerchantPath() + "/subscriptions/" + id, request);

            return new ResultImpl<Subscription>(new NodeWrapper(subscriptionXML), gateway);
        }

        public virtual Result<Subscription> Cancel(string id)
        {
            XmlNode subscriptionXML = service.Put(service.MerchantPath() + "/subscriptions/" + id + "/cancel");

            return new ResultImpl<Subscription>(new NodeWrapper(subscriptionXML), gateway);
        }

        public virtual async Task<Result<Subscription>> CancelAsync(string id)
        {
            XmlNode subscriptionXML = await service.PutAsync(service.MerchantPath() + "/subscriptions/" + id + "/cancel");

            return new ResultImpl<Subscription>(new NodeWrapper(subscriptionXML), gateway);
        }

        /// <summary>
        /// Search for subscriptions based on PlanId, DaysPastDue and Status
        /// </summary>
        /// <example>
        /// Quick Start Example:
        /// </example>
        /// <code>
        /// BraintreeGateway gateway = new BraintreeGateway(...);
        /// gateway.Subscription.Search(delegate(SubscriptionSearchRequest search) {
        ///     search.PlanId().StartsWith("abc");
        ///     search.DaysPastDue().Is("30");
        ///     search.Status().IncludedIn(Subscription.Status.ACTIVE, Subscription.Status.CANCELED);
        /// });
        /// </code>
        public virtual ResourceCollection<Subscription> Search(SubscriptionSearchRequest query)
        {
            var response = new NodeWrapper(service.Post(service.MerchantPath() + "/subscriptions/advanced_search_ids", query));

            return new ResourceCollection<Subscription>(response, delegate(string[] ids) {
                return FetchSubscriptions(query, ids);
            });
        }

        public virtual async Task<ResourceCollection<Subscription>> SearchAsync(SubscriptionSearchRequest query)
        {
            var response = new NodeWrapper(await service.PostAsync(service.MerchantPath() + "/subscriptions/advanced_search_ids", query));

            return new ResourceCollection<Subscription>(response, delegate(string[] ids) {
                return FetchSubscriptions(query, ids);
            });
        }

        private List<Subscription> FetchSubscriptions(SubscriptionSearchRequest query, string[] ids)
        {
            query.Ids.IncludedIn(ids);

            var response = new NodeWrapper(service.Post(service.MerchantPath() + "/subscriptions/advanced_search", query));

            var subscriptions = new List<Subscription>();
            foreach (var node in response.GetList("subscription"))
            {
                subscriptions.Add(new Subscription(node, gateway));
            }
            return subscriptions;
        }

        public virtual ResourceCollection<Subscription> Search(SearchDelegate searchDelegate)
        {
            var query = new SubscriptionSearchRequest();
            searchDelegate(query);
            return Search(query);
        }

        private Result<Transaction> RetryCharge(SubscriptionTransactionRequest txnRequest) {
            XmlNode response = service.Post(service.MerchantPath() + "/transactions", txnRequest);
            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        private async Task<Result<Transaction>> RetryChargeAsync(SubscriptionTransactionRequest txnRequest) {
            XmlNode response = await service.PostAsync(service.MerchantPath() + "/transactions", txnRequest);
            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public Result<Transaction> RetryCharge(string subscriptionId) {
            return RetryCharge(new SubscriptionTransactionRequest
            {
                SubscriptionId = subscriptionId
            });
        }

        public async Task<Result<Transaction>> RetryChargeAsync(string subscriptionId) {
            return await RetryChargeAsync(new SubscriptionTransactionRequest
            {
                SubscriptionId = subscriptionId
            });
        }

        public Result<Transaction> RetryCharge(string subscriptionId, decimal amount) {
            return RetryCharge(new SubscriptionTransactionRequest
            {
                SubscriptionId = subscriptionId,
                Amount = amount
            });
        }

        public async Task<Result<Transaction>> RetryChargeAsync(string subscriptionId, decimal amount) {
            return await RetryChargeAsync(new SubscriptionTransactionRequest
            {
                SubscriptionId = subscriptionId,
                Amount = amount
            });
        }
    }
}
