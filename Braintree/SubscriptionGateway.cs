#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        protected internal SubscriptionGateway(BraintreeService service)
        {
            Service = service;
        }

        public delegate void SearchDelegate(SubscriptionSearchRequest search);

        public virtual Result<Subscription> Create(SubscriptionRequest request)
        {
            XmlNode subscriptionXML = Service.Post("/subscriptions", request);

            return new Result<Subscription>(new NodeWrapper(subscriptionXML), Service);
        }

        public virtual async Task<Result<Subscription>> CreateAsync(SubscriptionRequest request)
        {
            XmlNode subscriptionXML = await Service.PostAsync("/subscriptions", request);

            return new Result<Subscription>(new NodeWrapper(subscriptionXML), Service);
        }

        public virtual Subscription Find(String id)
        {
            if (id == null || id.Trim().Equals(""))
                throw new NotFoundException();

            XmlNode subscriptionXML = Service.Get("/subscriptions/" + id);

            return new Subscription(new NodeWrapper(subscriptionXML), Service);
        }

        public virtual Result<Subscription> Update(String id, SubscriptionRequest request)
        {
            XmlNode subscriptionXML = Service.Put("/subscriptions/" + id, request);

            return new Result<Subscription>(new NodeWrapper(subscriptionXML), Service);
        }

        public virtual Result<Subscription> Cancel(String id)
        {
            XmlNode subscriptionXML = Service.Put("/subscriptions/" + id + "/cancel");

            return new Result<Subscription>(new NodeWrapper(subscriptionXML), Service);
        }

        public virtual async Task<Result<Subscription>> CancelAsync(String id)
        {
            XmlNode subscriptionXML = await Service.PutAsync("/subscriptions/" + id + "/cancel");

            return new Result<Subscription>(new NodeWrapper(subscriptionXML), Service);
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
            NodeWrapper response = new NodeWrapper(Service.Post("/subscriptions/advanced_search_ids", query));

            return new ResourceCollection<Subscription>(response, delegate(String[] ids)
            {
                return FetchSubscriptions(query, ids);
            });
        }

        private List<Subscription> FetchSubscriptions(SubscriptionSearchRequest query, String[] ids)
        {
            query.Ids.IncludedIn(ids);

            NodeWrapper response = new NodeWrapper(Service.Post("/subscriptions/advanced_search", query));

            List<Subscription> subscriptions = new List<Subscription>();
            foreach (NodeWrapper node in response.GetList("subscription"))
            {
                subscriptions.Add(new Subscription(node, Service));
            }
            return subscriptions;
        }

        public virtual ResourceCollection<Subscription> Search(SearchDelegate searchDelegate)
        {
            SubscriptionSearchRequest query = new SubscriptionSearchRequest();
            searchDelegate(query);
            return Search(query);
        }

        private Result<Transaction> RetryCharge(SubscriptionTransactionRequest txnRequest)
        {
            XmlNode response = Service.Post("/transactions", txnRequest);
            return new Result<Transaction>(new NodeWrapper(response), Service);
        }

        public Result<Transaction> RetryCharge(String subscriptionId)
        {
            return RetryCharge(new SubscriptionTransactionRequest
              {
                  SubscriptionId = subscriptionId
              });
        }

        public Result<Transaction> RetryCharge(String subscriptionId, Decimal amount)
        {
            return RetryCharge(new SubscriptionTransactionRequest
            {
                SubscriptionId = subscriptionId,
                Amount = amount
            });
        }
    }
}
