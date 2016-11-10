#pragma warning disable 1591

using System;

namespace Braintree
{
    public class SubscriptionStatusEvent
    {
        public virtual decimal? Price { get; protected set; }
        public virtual decimal? Balance { get; protected set; }
        public virtual SubscriptionStatus Status { get; protected set; }
        public virtual DateTime? Timestamp { get; protected set; }
        public virtual SubscriptionSource Source { get; protected set; }
        public virtual string User { get; protected set; }
        public virtual string CurrencyIsoCode { get; protected set; }
        public virtual string PlanId { get; protected set; }

        public SubscriptionStatusEvent(NodeWrapper node)
        {
            if (node == null) return;

            Price = node.GetDecimal("price");
            Balance = node.GetDecimal("balance");
            Status = (SubscriptionStatus)CollectionUtil.Find(SubscriptionStatus.STATUSES, node.GetString("status"), SubscriptionStatus.UNRECOGNIZED);
            Timestamp = node.GetDateTime("timestamp");
            Source = (SubscriptionSource)CollectionUtil.Find(SubscriptionSource.ALL, node.GetString("subscription-source"), SubscriptionSource.UNRECOGNIZED);
            User = node.GetString("user");
            CurrencyIsoCode = node.GetString("currency-iso-code");
            PlanId = node.GetString("plan-id");
        }

        [Obsolete("Mock Use Only")]
        protected internal SubscriptionStatusEvent() { }
    }
}
