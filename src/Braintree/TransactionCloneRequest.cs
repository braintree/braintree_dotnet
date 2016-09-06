#pragma warning disable 1591

namespace Braintree
{
    public class TransactionCloneRequest : Request
    {
        public decimal Amount { get; set; }
        public string Channel { get; set; }
        public TransactionOptionsCloneRequest Options { get; set; }

        public override string ToXml()
        {
            return ToXml("transaction-clone");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("amount", Amount).
                AddElement("channel", Channel).
                AddElement("options", Options);
        }
    }
}
