#pragma warning disable 1591

using System;
namespace Braintree
{
    public class SubscriptionOptionsRequest : Request
    {
        public Boolean DoNotInheritAddOnsOrDiscounts { get; set; }
        public Boolean ProrateCharges { get; set; }
        public Boolean ReplaceAllAddOnsAndDiscounts { get; set; }
        public Boolean StartImmediately { get; set; }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            return new RequestBuilder(root).
                AddElement("replace-all-add-ons-and-discounts", ReplaceAllAddOnsAndDiscounts).
                AddElement("prorate-charges", ProrateCharges).
                AddElement("do-not-inherit-add-ons-or-discounts", DoNotInheritAddOnsOrDiscounts).
                AddElement("start-immediately", StartImmediately);
        }
    }
}
