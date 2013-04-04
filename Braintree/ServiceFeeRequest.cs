#pragma warning disable 1591

using System;

namespace Braintree
{
    public class ServiceFeeRequest : Request
    {
        public String MerchantAccountId { get; set; }
        public Decimal? Amount { get; set; }

        public override String ToXml()
        {
            return ToXml("service-fee");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        public override String ToQueryString()
        {
            return ToQueryString("service-fee");
        }

        public override String ToQueryString(String root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            return new RequestBuilder(root).
                AddElement("amount", Amount).
                AddElement("merchant-account-id", MerchantAccountId);
        }
    }
}
