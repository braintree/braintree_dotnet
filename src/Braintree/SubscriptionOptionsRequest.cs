#pragma warning disable 1591

namespace Braintree
{
    public class SubscriptionOptionsRequest : Request
    {
        public bool? DoNotInheritAddOnsOrDiscounts { get; set; }
        public bool? ProrateCharges { get; set; }
        public bool? ReplaceAllAddOnsAndDiscounts { get; set; }
        public bool? RevertSubscriptionOnProrationFailure { get; set; }
        public bool? StartImmediately { get; set; }
        public SubscriptionOptionsPayPalRequest PayPal { get; set; }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("replace-all-add-ons-and-discounts", ReplaceAllAddOnsAndDiscounts).
                AddElement("prorate-charges", ProrateCharges).
                AddElement("do-not-inherit-add-ons-or-discounts", DoNotInheritAddOnsOrDiscounts).
                AddElement("revert-subscription-on-proration-failure", RevertSubscriptionOnProrationFailure).
                AddElement("start-immediately", StartImmediately).
                AddElement("paypal", PayPal);
        }
    }
}
