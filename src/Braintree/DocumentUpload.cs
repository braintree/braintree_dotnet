using System;
using System.ComponentModel;

namespace Braintree
{
    public enum DocumentUploadKind
    {
        [Description("evidence_document")] EVIDENCE_DOCUMENT
    }

    public class DocumentUpload
    {
        public virtual decimal? Size { get; protected set; }
        public virtual DocumentUploadKind? Kind { get; protected set; }
        public virtual string ContentType { get; protected set; }
        public virtual string Id { get; protected set; }
        public virtual string Name { get; protected set; }

        public DocumentUpload(NodeWrapper node)
        {
            Size = node.GetDecimal("size");
            Kind = node.GetEnum<DocumentUploadKind>("kind");
            ContentType = node.GetString("content-type");
            Id = node.GetString("id");
            Name = node.GetString("name");
        }

        [Obsolete("Mock Use Only")]
        protected internal DocumentUpload() { }
    }
}
