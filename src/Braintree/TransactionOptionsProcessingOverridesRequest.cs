namespace Braintree
{
    public class TransactionOptionsProcessingOverridesRequest : Request
    {
        public string CustomerEmail { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerTaxIdentifier { get; set; }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        private RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("customer-email", CustomerEmail).
                AddElement("customer-first-name", CustomerFirstName).
                AddElement("customer-last-name", CustomerLastName).
                AddElement("customer-tax-identifier", CustomerTaxIdentifier);
        }
    }
}
