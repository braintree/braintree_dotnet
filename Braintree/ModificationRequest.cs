#pragma warning disable 1591

using System;

namespace Braintree
{
    public class ModificationRequest : Request
    {
        public Decimal? Amount { get; set; }
        public bool? NeverExpires { get; set; }
        public int? NumberOfBillingCycles { get; set; }
        public int? Quantity { get; set; }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("amount", Amount).
                AddElement("never-expires", NeverExpires).
                AddElement("number-of-billing-cycles", NumberOfBillingCycles).
                AddElement("quantity", Quantity);
        }
    }
}

