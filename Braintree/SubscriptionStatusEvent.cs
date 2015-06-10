#pragma warning disable 1591

using System;

namespace Braintree
{
    public class SubscriptionStatusEvent
    {
        public Decimal? Price { get; protected set; }
        public Decimal? Balance { get; protected set; }
        public SubscriptionStatus Status { get; protected set; }
        public DateTime? Timestamp { get; protected set; }
        public SubscriptionSource Source { get; protected set; }
        public string User { get; protected set; }

        public SubscriptionStatusEvent(NodeWrapper node)
        {
            if (node == null) return;

            Price = node.GetDecimal("price");
            Balance = node.GetDecimal("balance");
            Status = (SubscriptionStatus)CollectionUtil.Find(SubscriptionStatus.STATUSES, node.GetString("status"), SubscriptionStatus.UNRECOGNIZED);
            Timestamp = node.GetDateTime("timestamp");
            Source = (SubscriptionSource)CollectionUtil.Find(SubscriptionSource.ALL, node.GetString("subscription-source"), SubscriptionSource.UNRECOGNIZED);
            User = node.GetString("user");
        }
    }
}
