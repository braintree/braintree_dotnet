namespace Braintree
{
    public class TransactionOptionsAmexRewardsRequest : Request
    {
        public string RequestId { get; set; }
        public string Points { get; set; }
        public string CurrencyAmount { get; set; }
        public string CurrencyIsoCode { get; set; }

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
                AddElement("request-id", RequestId).
                AddElement("points", Points).
                AddElement("currency-amount", CurrencyAmount).
                AddElement("currency-iso-code", CurrencyIsoCode);
        }
    }
}
