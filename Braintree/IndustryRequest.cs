#pragma warning disable 1591

using System;

namespace Braintree
{
    public class IndustryRequest : Request
    {
        public TransactionIndustryType IndustryType { get; set; }
        public IndustryDataRequest IndustryData { get; set; }

        public override String ToXml()
        {
            return ToXml("industry");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        public override String ToQueryString(String root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            return new RequestBuilder(root).
                AddElement("industry-type", IndustryType).
                AddElement("data", IndustryData);
        }
    }
}
