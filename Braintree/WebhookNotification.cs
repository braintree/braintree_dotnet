#pragma warning disable 1591

using System;

namespace Braintree
{
    public class WebhookKind : Enumeration
    {
        public static readonly WebhookKind SUBSCRIPTION_PAST_DUE = new WebhookKind("subscription_past_due");
        public static readonly WebhookKind UNRECOGNIZED = new WebhookKind("unrecognized");

        public static readonly WebhookKind[] ALL = { SUBSCRIPTION_PAST_DUE };

        protected WebhookKind(String name) : base(name) {}
    }

    public class WebhookNotification
    {
        public WebhookKind Kind { get; protected set; }
        public Subscription Subscription { get; protected set; }

        public WebhookNotification(NodeWrapper node, BraintreeService service)
        {
            Kind = (WebhookKind)CollectionUtil.Find(WebhookKind.ALL, node.GetString("kind"), WebhookKind.UNRECOGNIZED);
            Subscription = new Subscription(node.GetNode("subject/subscription"), service);
        }
    }
}
