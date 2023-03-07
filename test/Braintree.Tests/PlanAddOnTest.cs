using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class PlanAddOnTest
    {
        [Test]
        public void PlanDiscountsRequest_ToXmlIncludesAllData()
        {
            var request = new PlanAddOnsRequest()
            {
                Add = new AddPlanModificationRequest[] 
                {
                    new AddPlanModificationRequest 
                    {
                        InheritedFromId = "an ID",
                        Amount = 3,
                        Description = "description",
                        Name = "3 dollar mod",
                        NumberOfBillingCycles = 3,
                    }
                },
                Update = new UpdatePlanModificationRequest[] 
                {
                    new UpdatePlanModificationRequest 
                    {
                        ExistingId = "an ID",
                        Amount = 3,
                        Description = "description",
                        Name = "3 dollar mod",
                        NumberOfBillingCycles = 3,
                    }
                },
            };

            string xmlString = request.ToXml("add-ons");

            Assert.IsTrue(xmlString.Contains("add"));
            Assert.IsTrue(xmlString.Contains("update"));
        }
    }
}