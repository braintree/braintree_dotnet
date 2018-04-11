namespace Braintree
{
    /// <summary>
    /// A class for building requests to manipulate <see cref="DisputeEvidence"/> on a Dispute.
    /// </summary>
    /// <example>
    /// An file evidence request can be constructed as follows:
    /// <code>
    ///  var fileEvidenceRequest = new FileEvidenceRequest
    ///  {
    ///      DocumentId = "document_id",
    ///      Category = "GENERAL",
    ///  };
    /// </code>
    /// </example>
    public class FileEvidenceRequest : Request
    {
        public string DocumentId { get; set; }
        public string Category { get; set; }

        public override string ToXml()
        {
            return ToXml("evidence");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("evidence");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("document-upload-id", DocumentId).
                AddElement("category", Category);
        }

    }
}
