#pragma warning disable 1591

using System;

namespace Braintree
{
    public class IndustryDataAdditionalChargeKind : Enumeration
    {
        public static readonly IndustryDataAdditionalChargeKind MINI_BAR = new IndustryDataAdditionalChargeKind("mini_bar");
        public static readonly IndustryDataAdditionalChargeKind RESTAURANT = new IndustryDataAdditionalChargeKind("restaurant");
        public static readonly IndustryDataAdditionalChargeKind GIFT_SHOP = new IndustryDataAdditionalChargeKind("gift_shop");
        public static readonly IndustryDataAdditionalChargeKind TELEPHONE = new IndustryDataAdditionalChargeKind("telephone");
        public static readonly IndustryDataAdditionalChargeKind LAUNDRY = new IndustryDataAdditionalChargeKind("laundry");
        public static readonly IndustryDataAdditionalChargeKind OTHER = new IndustryDataAdditionalChargeKind("other");

        public static readonly IndustryDataAdditionalChargeKind[] ALL = { MINI_BAR, RESTAURANT, GIFT_SHOP, TELEPHONE, LAUNDRY, OTHER };

        protected IndustryDataAdditionalChargeKind(string name) : base(name) {}
    }

    public class IndustryDataAdditionalChargeRequest : Request
    {
        public virtual IndustryDataAdditionalChargeKind AdditionalChargeKind { get; set; }
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
                AddElement("kind", AdditionalChargeKind).
                AddElement("amount", Amount);

            return builder;
        }
    }
}
