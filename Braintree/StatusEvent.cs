#pragma warning disable 1591

using System;

namespace Braintree
{
    public class StatusEvent
    {
        public Decimal? Amount { get; protected set; }
        public TransactionStatus Status { get; protected set; }
        public DateTime? Timestamp { get; protected set; }
        public TransactionSource Source { get; protected set; }
        public string User { get; protected set; }

        public StatusEvent(NodeWrapper node)
        {
            if (node == null) return;

            Amount = node.GetDecimal("amount");
            Status = (TransactionStatus)CollectionUtil.Find(TransactionStatus.ALL, node.GetString("status"), TransactionStatus.UNRECOGNIZED);
            Timestamp = node.GetDateTime("timestamp");
            Source = (TransactionSource)CollectionUtil.Find(TransactionSource.ALL, node.GetString("transaction-source"), TransactionSource.UNRECOGNIZED);
            User = node.GetString("user");
        }
    }
}
