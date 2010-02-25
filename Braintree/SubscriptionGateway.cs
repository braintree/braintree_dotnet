using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Braintree
{
    public class SubscriptionGateway
    {
        public Result<Subscription> Create(SubscriptionRequest request)
        {
            XmlNode subscriptionXML = WebServiceGateway.Post("/subscriptions", request);

            return new Result<Subscription>(new NodeWrapper(subscriptionXML));
        }

        public Subscription Find(String id)
        {
            XmlNode subscriptionXML = WebServiceGateway.Get("/subscriptions/" + id);

            return new Subscription(new NodeWrapper(subscriptionXML));
        }

        public Result<Subscription> Update(String id, SubscriptionRequest request)
        {
            XmlNode subscriptionXML = WebServiceGateway.Put("/subscriptions/" + id, request);

            return new Result<Subscription>(new NodeWrapper(subscriptionXML));
        }

        public Result<Subscription> Cancel(String id)
        {
            XmlNode subscriptionXML = WebServiceGateway.Put("/subscriptions/" + id + "/cancel");

            return new Result<Subscription>(new NodeWrapper(subscriptionXML));
        }
    }
}
