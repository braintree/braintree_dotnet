#pragma warning disable 1591

using System;

namespace Braintree
{
    public class ModificationRequest : Request
    {
        public Decimal? Amount { get; set; }
        public Boolean? NeverExpires { get; set; }
        public Int32? NumberOfBillingCycles { get; set; }
        public Int32? Quantity { get; set; }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            return new RequestBuilder(root).
                AddElement("amount", Amount).
                AddElement("never-expires", NeverExpires).
                AddElement("number-of-billing-cycles", NumberOfBillingCycles).
                AddElement("quantity", Quantity);
        }
    }
}

