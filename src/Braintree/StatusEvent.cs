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
            if (node == null) return;

            Amount = node.GetDecimal("amount");
            Status = (TransactionStatus)CollectionUtil.Find(TransactionStatus.ALL, node.GetString("status"), TransactionStatus.UNRECOGNIZED);
            Timestamp = node.GetDateTime("timestamp");
            Source = (TransactionSource)CollectionUtil.Find(TransactionSource.ALL, node.GetString("transaction-source"), TransactionSource.UNRECOGNIZED);
            User = node.GetString("user");
        }

        [Obsolete("Mock Use Only")]
        protected internal StatusEvent() { }
    }
}
