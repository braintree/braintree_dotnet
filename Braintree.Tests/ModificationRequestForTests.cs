using System;
namespace Braintree.Tests
{
    public class ModificationRequestForTests : Request
    {
        public Decimal? Amount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public String Description { get; set; }
        public String Id { get; set; }
        new public String Kind { get; set; }
        public String MerchantId { get; set; }
        public String Name { get; set; }
        public Boolean? NeverExpires { get; set; }
        public Int32? NumberOfBillingCycles { get; set; }
        public String PlanId { get; set; }
        public Int32? Quantity { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public override String ToXml()
        {
            return ToXml("modification");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        public RequestBuilder BuildRequest(String root)
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

