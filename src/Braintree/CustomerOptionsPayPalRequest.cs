#pragma warning disable 1591

namespace Braintree
{
    public class CustomerOptionsPayPalRequest : Request
    {
        public string PayeeEmail { get; set; }
        public string OrderId { get; set; }
        public string CustomField { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("payee-email", PayeeEmail).
                AddElement("order-id", OrderId).
                AddElement("custom-field", CustomField).
                AddElement("description", Description).
                AddElement("amount", Amount);
        }
    }
}
