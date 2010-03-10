#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Braintree
{
    /// <summary>
    /// Provides operations for creating, finding, updating, and deleting subscriptions in the vault
    /// </summary>
    public class SubscriptionGateway
    {
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
    }
}
