#pragma warning disable 1591

using System.ComponentModel;

namespace Braintree
{

    public enum IndustryDataAdditionalChargeKind
    {
        [Description("mini_bar")] MINI_BAR,
        [Description("restaurant")] RESTAURANT,
        [Description("gift_shop")] GIFT_SHOP,
        [Description("telephone")] TELEPHONE,
        [Description("laundry")] LAUNDRY,
        [Description("other")] OTHER
    }

    public class IndustryDataAdditionalChargeRequest : Request
    {
        public virtual IndustryDataAdditionalChargeKind? AdditionalChargeKind { get; set; }
        public virtual decimal Amount { get; set; }

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
            var builder = new RequestBuilder(root).
                AddElement("kind", AdditionalChargeKind.GetDescription()).
                AddElement("amount", Amount);

            return builder;
        }
    }
}
