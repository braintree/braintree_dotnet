#pragma warning disable 1591

namespace Braintree
{
    // NEXT_MAJOR_VERSION Rename Android Pay to Google Pay
    public class TransactionAndroidPayCardRequest : Request
    {
        public string Cryptogram { get; set; }
        public string EciIndicator { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string GoogleTransactionId { get; set; }
        public string Number { get; set; }
        public string SourceCardLastFour { get; set; }
        public string SourceCardType { get; set; }

        public override string ToXml()
        {
            return ToXml("android-pay-card");
        }

        public override string ToXml(string rootElement)
        {
            return BuildRequest(rootElement).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("cryptogram", Cryptogram).
                AddElement("eci-indicator", EciIndicator).
                AddElement("expiration-month", ExpirationMonth).
                AddElement("expiration-year", ExpirationYear).
                AddElement("number", Number).
                AddElement("google-transaction-id", GoogleTransactionId).
                AddElement("source-card-last-four", SourceCardLastFour).
                AddElement("source-card-type", SourceCardType);
        }
    }
}
