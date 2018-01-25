#pragma warning disable 1591

namespace Braintree
{
    /// <summary>
    /// A class for building requests to manipulate <see cref="DisputeEvidence"/> on a Dispute.
    /// </summary>
    /// <example>
    /// An text evidence request can be constructed as follows:
    /// <code>
    ///  var textEvidenceRequest = new TextEvidenceRequest
    ///  {
    ///      Content = "UPS-45676",
    ///      Tag = "CARRIER_NAME",
    ///      SequenceNumber = "0",
    /// };
    /// </code>
    /// </example>
    public class TextEvidenceRequest : Request
    {
        public string Content { get; set; }
        public string Tag { get; set; }
        public string SequenceNumber { get; set; }

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
                AddElement("comments", Content).
                AddElement("category", Tag).
                AddElement("sequence-number", SequenceNumber);
        }
    }
}
