using System;

namespace Braintree
{
    public class DisputeEvidence
    {
        public virtual DateTime? CreatedAt { get; protected set; }
        public virtual DateTime? SentToProcessorAt { get; protected set; }
        public virtual string Comment { get; protected set; }
        public virtual string Id { get; protected set; }
        public virtual string Url { get; protected set; }
        public virtual string Tag { get; protected set; }
        public virtual string SequenceNumber { get; protected set; }

        public DisputeEvidence(NodeWrapper node)
        {
            CreatedAt = node.GetDateTime("created-at");
            SentToProcessorAt = node.GetDateTime("sent-to-processor-at");
            Comment = node.GetString("comment");
            Id = node.GetString("id");
            Url = node.GetString("url");
            Tag = node.GetString("tag");
            SequenceNumber = node.GetString("sequence-number");
        }

        [Obsolete("Mock Use Only")]
        protected internal DisputeEvidence() { }
    }
}
