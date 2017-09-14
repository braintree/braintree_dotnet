using System;
using System.Collections.Generic;

namespace Braintree
{
    public class DocumentUploadKind : Enumeration
    {
        public static readonly DocumentUploadKind EVIDENCE_DOCUMENT = new DocumentUploadKind("evidence_document");

        public static readonly DocumentUploadKind[] ALL = {
            EVIDENCE_DOCUMENT
        };

        protected DocumentUploadKind(string name) : base(name) {}
    }

    public class DocumentUpload
    {
        public virtual decimal? Size { get; protected set; }
        public virtual DocumentUploadKind Kind { get; protected set; }
        public virtual string ContentType { get; protected set; }
        public virtual string Id { get; protected set; }
        public virtual string Name { get; protected set; }

        public DocumentUpload(NodeWrapper node)
        {
            Size = node.GetDecimal("size");
            Kind = (DocumentUploadKind)CollectionUtil.Find(DocumentUploadKind.ALL, node.GetString("kind"), null);
            ContentType = node.GetString("content-type");
            Id = node.GetString("id");
            Name = node.GetString("name");
        }

        [Obsolete("Mock Use Only")]
        protected internal DocumentUpload() { }
    }
}
