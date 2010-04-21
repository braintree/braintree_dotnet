#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Braintree
{
    /// <summary>
    /// Provides operations for creating, finding, updating, searching, and deleting subscriptions in the vault
    /// </summary>
    public class SubscriptionGateway
    {
        public delegate void SearchDelegate(SubscriptionSearchRequest search);

        public virtual Result<Subscription> Create(SubscriptionRequest request)
        {
            XmlNode subscriptionXML = WebServiceGateway.Post("/subscriptions", request);

            return new Result<Subscription>(new NodeWrapper(subscriptionXML));
        }

        public virtual Subscription Find(String id)
        {
            XmlNode subscriptionXML = WebServiceGateway.Get("/subscriptions/" + id);

            return new Subscription(new NodeWrapper(subscriptionXML));
        }

        public virtual Result<Subscription> Update(String id, SubscriptionRequest request)
        {
            XmlNode subscriptionXML = WebServiceGateway.Put("/subscriptions/" + id, request);

            return new Result<Subscription>(new NodeWrapper(subscriptionXML));
        }

        public virtual Result<Subscription> Cancel(String id)
        {
            XmlNode subscriptionXML = WebServiceGateway.Put("/subscriptions/" + id + "/cancel");

            return new Result<Subscription>(new NodeWrapper(subscriptionXML));
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
        public virtual PagedCollection<Subscription> Search(SubscriptionSearchRequest query)
        {
            return Search(query, 1);
        }

        public virtual PagedCollection<Subscription> Search(SubscriptionSearchRequest query, int pageNumber)
        {
            NodeWrapper response = new NodeWrapper(WebServiceGateway.Post("/subscriptions/advanced_search?page=" + pageNumber, query));

            int totalItems = response.GetInteger("total-items").Value;

            List<Subscription> subscriptions = new List<Subscription>();
            foreach (NodeWrapper subscriptionNode in response.GetList("subscription"))
            {
                subscriptions.Add(new Subscription(subscriptionNode));
            }

            return new PagedCollection<Subscription>(subscriptions, totalItems, delegate() {
                return Search(query, pageNumber + 1);
            });
        }

        public virtual PagedCollection<Subscription> Search(SearchDelegate searchDelegate)
        {
            return Search(searchDelegate, 1);
        }

        public virtual PagedCollection<Subscription> Search(SearchDelegate searchDelegate, int pageNumber)
        {
            var search = new SubscriptionSearchRequest();
            searchDelegate(search);
            return Search(search, pageNumber);
        }
    }
}
