using System;
using System.ComponentModel;

namespace Braintree
{

    public enum Kind
    {
        [Description("refund")] REFUND,
        [Description("dispute")] DISPUTE,        
        [Description("unrecognized")] UNRECOGNIZED
    }

    public class Adjustment 
    {
        public virtual decimal? Amount { get; protected set; }
        public virtual DateTime? ProjectedDisbursementDate { get; protected set; }
        public virtual DateTime? ActualDisbursementDate { get; protected set; }
        public virtual Kind Kind { get; protected set; }

        protected internal Adjustment(NodeWrapper node)
        {
            Amount = node.GetDecimal("amount");
            ProjectedDisbursementDate = node.GetDateTime("projected_disbursement_date");
            ActualDisbursementDate = node.GetDateTime("actual_disbursement_date");
            Kind = node.GetEnum("kind", Kind.UNRECOGNIZED);
        }

        [Obsolete("Mock Use Only")]
        protected internal Adjustment() { }
    }
}
