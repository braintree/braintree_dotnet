#pragma warning disable 1591

using System;

namespace Braintree
{
    public class WebhookKind : Enumeration
    {
        public static readonly WebhookKind SUBSCRIPTION_CANCELED = new WebhookKind("subscription_canceled");
        public static readonly WebhookKind SUBSCRIPTION_CHARGED_SUCCESSFULLY = new WebhookKind("subscription_charged_successfully");
        public static readonly WebhookKind SUBSCRIPTION_CHARGED_UNSUCCESSFULLY = new WebhookKind("subscription_charged_unsuccessfully");
        public static readonly WebhookKind SUBSCRIPTION_EXPIRED = new WebhookKind("subscription_expired");
        public static readonly WebhookKind SUBSCRIPTION_TRIAL_ENDED = new WebhookKind("subscription_trial_ended");
        public static readonly WebhookKind SUBSCRIPTION_WENT_ACTIVE = new WebhookKind("subscription_went_active");
        public static readonly WebhookKind SUBSCRIPTION_WENT_PAST_DUE = new WebhookKind("subscription_went_past_due");
        public static readonly WebhookKind UNRECOGNIZED = new WebhookKind("unrecognized");

        public static readonly WebhookKind[] ALL = {
          SUBSCRIPTION_CANCELED,
          SUBSCRIPTION_CHARGED_SUCCESSFULLY,
          SUBSCRIPTION_CHARGED_UNSUCCESSFULLY,
          SUBSCRIPTION_EXPIRED,
          SUBSCRIPTION_TRIAL_ENDED,
          SUBSCRIPTION_WENT_ACTIVE,
          SUBSCRIPTION_WENT_PAST_DUE
        };

        protected WebhookKind(String name) : base(name) {}
    }

    public class WebhookNotification
    {
        public WebhookKind Kind { get; protected set; }
        public Subscription Subscription { get; protected set; }
        public DateTime? Timestamp { get; protected set; }

        public WebhookNotification(NodeWrapper node, BraintreeService service)
        {
            Timestamp = node.GetDateTime("timestamp");
            Kind = (WebhookKind)CollectionUtil.Find(WebhookKind.ALL, node.GetString("kind"), WebhookKind.UNRECOGNIZED);
            Subscription = new Subscription(node.GetNode("subject/subscription"), service);
        }
    }
}
