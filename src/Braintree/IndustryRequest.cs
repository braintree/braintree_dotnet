#pragma warning disable 1591

namespace Braintree
{
    public class IndustryRequest : Request
    {
        public TransactionIndustryType? IndustryType { get; set; }
        public IndustryDataRequest IndustryData { get; set; }

        public override string ToXml()
        {
            return ToXml("industry");
        }

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
                AddElement("industry-type", IndustryType.GetDescription()).
                AddElement("data", IndustryData);
        }
    }
}
