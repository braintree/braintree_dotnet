using System;

namespace Braintree
{
    public class TransactionReview
    {
        public virtual string TransactionId { get; protected set; }
        public virtual string Decision { get; protected set; }
        public virtual string ReviewerEmail { get; protected set; }
        public virtual string ReviewerNote { get; protected set; }
        public virtual DateTime? ReviewedTime { get; protected set; }

        public TransactionReview(NodeWrapper node)
        {
            TransactionId = node.GetString("transaction-id");
            Decision = node.GetString("decision");
            ReviewerEmail = node.GetString("reviewer-email");
            ReviewerNote = node.GetString("reviewer-note");
            ReviewedTime = node.GetDateTime("reviewed-time");
        }
    }
}
