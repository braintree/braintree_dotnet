using System;

namespace Braintree
{
    public class DisputeStatusHistory
    {
        public virtual DateTime? DisbursementDate { get; protected set; }
        public virtual DateTime? EffectiveDate { get; protected set; }
        public virtual DateTime? Timestamp { get; protected set; }
        public virtual DisputeStatus Status { get; protected set; }

        public DisputeStatusHistory(NodeWrapper node)
        {
            EffectiveDate = node.GetDateTime("effective-date");
            DisbursementDate = node.GetDateTime("disbursement-date");
            Timestamp = node.GetDateTime("timestamp");
            Status = node.GetEnum("status", DisputeStatus.UNRECOGNIZED);
        }

        [Obsolete("Mock Use Only")]
        protected internal DisputeStatusHistory() { }
    }
}
