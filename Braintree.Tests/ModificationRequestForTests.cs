using System;
namespace Braintree.Tests
{
    public class ModificationRequestForTests : Request
    {
        public Decimal? Amount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        new public string Kind { get; set; }
        public string MerchantId { get; set; }
        public string Name { get; set; }
        public bool? NeverExpires { get; set; }
        public Int32? NumberOfBillingCycles { get; set; }
        public string PlanId { get; set; }
        public Int32? Quantity { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public override string ToXml()
        {
            return ToXml("modification");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("amount", Amount).
                AddElement("created-at", CreatedAt).
                AddElement("description", Description).
                AddElement("id", Id).
                AddElement("kind", Kind).
                AddElement("merchant-id", MerchantId).
                AddElement("name", Name).
                AddElement("never-expires", NeverExpires).
                AddElement("number-of-billing-cycles", NumberOfBillingCycles).
                AddElement("plan-id", PlanId).
                AddElement("quantity", Quantity).
                AddElement("updated-at", UpdatedAt);
        }
    }
}

