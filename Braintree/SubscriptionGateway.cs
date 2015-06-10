#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Braintree.Exceptions;

namespace Braintree
{
    /// <summary>
    /// Provides operations for creating, finding, updating, searching, and deleting subscriptions in the vault
    /// </summary>
    public class SubscriptionGateway
    {
        private BraintreeService Service;
        private BraintreeGateway Gateway;

        protected internal SubscriptionGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            Gateway = gateway;
            Service = new BraintreeService(gateway.Configuration);
        }

        public delegate void SearchDelegate(SubscriptionSearchRequest search);

        public virtual Result<Subscription> Create(SubscriptionRequest request)
        {
            XmlNode subscriptionXML = Service.Post(Service.MerchantPath() + "/subscriptions", request);

            return new ResultImpl<Subscription>(new NodeWrapper(subscriptionXML), Gateway);
        }

        public virtual Subscription Find(string id)
        {
            if(id == null || id.Trim().Equals(""))
                throw new NotFoundException();

            XmlNode subscriptionXML = Service.Get(Service.MerchantPath() + "/subscriptions/" + id);

            return new Subscription(new NodeWrapper(subscriptionXML), Gateway);
        }

        public virtual Result<Subscription> Update(string id, SubscriptionRequest request)
        {
            XmlNode subscriptionXML = Service.Put(Service.MerchantPath() + "/subscriptions/" + id, request);

            return new ResultImpl<Subscription>(new NodeWrapper(subscriptionXML), Gateway);
        }

        public virtual Result<Subscription> Cancel(string id)
        {
            XmlNode subscriptionXML = Service.Put(Service.MerchantPath() + "/subscriptions/" + id + "/cancel");

            return new ResultImpl<Subscription>(new NodeWrapper(subscriptionXML), Gateway);
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
            NodeWrapper response = new NodeWrapper(Service.Post(Service.MerchantPath() + "/subscriptions/advanced_search_ids", query));

            return new ResourceCollection<Subscription>(response, delegate(string[] ids) {
                return FetchSubscriptions(query, ids);
            });
        }

        private List<Subscription> FetchSubscriptions(SubscriptionSearchRequest query, string[] ids)
        {
            query.Ids.IncludedIn(ids);

            NodeWrapper response = new NodeWrapper(Service.Post(Service.MerchantPath() + "/subscriptions/advanced_search", query));

            List<Subscription> subscriptions = new List<Subscription>();
            foreach (NodeWrapper node in response.GetList("subscription"))
            {
                subscriptions.Add(new Subscription(node, Gateway));
            }
            return subscriptions;
        }

        public virtual ResourceCollection<Subscription> Search(SearchDelegate searchDelegate)
        {
            SubscriptionSearchRequest query = new SubscriptionSearchRequest();
            searchDelegate(query);
            return Search(query);
        }

        private Result<Transaction> RetryCharge(SubscriptionTransactionRequest txnRequest) {
           XmlNode response = Service.Post(Service.MerchantPath() + "/transactions", txnRequest);
           return new ResultImpl<Transaction>(new NodeWrapper(response), Gateway);
       }

       public Result<Transaction> RetryCharge(string subscriptionId) {
          return RetryCharge(new SubscriptionTransactionRequest
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
    }
}
