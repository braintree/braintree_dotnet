using System;
namespace Braintree
{
    public class SubscriptionOptionsRequest : Request
    {
        public Boolean DoNotInheritAddOnsOrDiscounts { get; set; }

        public override String ToXml()
        {
            return ToXml("options");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            return new RequestBuilder(root).
                AddElement("do-not-inherit-add-ons-or-discounts", DoNotInheritAddOnsOrDiscounts);
        }
    }
}

