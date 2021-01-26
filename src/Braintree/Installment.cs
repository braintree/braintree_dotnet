using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace Braintree
{

    public class Installment
    {
        public virtual string Id { get; protected set; }
        public virtual decimal? Amount { get; protected set; }
        public virtual DateTime? ProjectedDisbursementDate { get; protected set; }
        public virtual DateTime? ActualDisbursementDate { get; protected set; }
        public virtual List<Adjustment> Adjustments { get; protected set; }

        protected internal Installment(NodeWrapper node)
        {
            Amount = node.GetDecimal("amount");
            Id = node.GetString("id");
            ProjectedDisbursementDate = node.GetDateTime("projected_disbursement_date");
            ActualDisbursementDate = node.GetDateTime("actual_disbursement_date");
            Adjustments = new List<Adjustment>();
            foreach (var adjustmentNode in node.GetList("adjustments/adjustment")) {
               Adjustments.Add(new Adjustment(adjustmentNode));
            }
        }

        [Obsolete("Mock Use Only")]
        protected internal Installment() { }
    }
}
