using System;

namespace Braintree
{
    public class DisputeStatusHistory
    {
        public virtual DateTime? EffectiveDate { get; protected set; }
        public virtual DateTime? Timestamp { get; protected set; }
        public virtual DisputeStatus Status { get; protected set; }

        public DisputeStatusHistory(NodeWrapper node)
        {
            EffectiveDate = node.GetDateTime("effective-date");
            Timestamp = node.GetDateTime("timestamp");
            Status = (DisputeStatus)CollectionUtil.Find(DisputeStatus.ALL, node.GetString("status"), DisputeStatus.UNRECOGNIZED);
        }

        [Obsolete("Mock Use Only")]
        protected internal DisputeStatusHistory() { }
    }
}
