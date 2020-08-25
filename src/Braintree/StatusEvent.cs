#pragma warning disable 1591

using System;

namespace Braintree
{
    public class StatusEvent
    {
        public virtual decimal? Amount { get; protected set; }
        public virtual TransactionStatus Status { get; protected set; }
        public virtual DateTime? Timestamp { get; protected set; }
        public virtual TransactionSource Source { get; protected set; }
        public virtual string User { get; protected set; }

        public StatusEvent(NodeWrapper node)
        {
            if (node == null)
                return;

            Amount = node.GetDecimal("amount");
            Status = node.GetEnum("status", TransactionStatus.UNRECOGNIZED);
            Timestamp = node.GetDateTime("timestamp");
            Source = node.GetEnum("transaction-source", TransactionSource.UNRECOGNIZED);
            User = node.GetString("user");
        }

        [Obsolete("Mock Use Only")]
        protected internal StatusEvent() { }
    }
}
