using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class PlanModificationTest
    {
        [Test]
        public void PlanModification_ToXmlIncludesAllData()
        {
            var request = new PlanModificationRequest()
            {
                Amount = 5,
                Description = "A modification",
                Name = "A name",
                NumberOfBillingCycles = 2,
            };

            string xmlString = request.ToXml("modification");

            Assert.IsTrue(xmlString.Contains("amount"));
            Assert.IsTrue(xmlString.Contains("description"));
            Assert.IsTrue(xmlString.Contains("name"));
            Assert.IsTrue(xmlString.Contains("number-of-billing-cycles"));
        }

        [Test]
        public void AddPlanModificationRequest_ToXmlIncludesAllData()
        {
            var request = new AddPlanModificationRequest()
            {
                Amount = 5,
                Description = "A modification",
                InheritedFromId = "some id",
                Name = "A name",
                NumberOfBillingCycles = 2,
            };

            string xmlString = request.ToXml("modification");

            Assert.IsTrue(xmlString.Contains("amount"));
            Assert.IsTrue(xmlString.Contains("description"));
            Assert.IsTrue(xmlString.Contains("inherited-from-id"));
            Assert.IsTrue(xmlString.Contains("name"));
            Assert.IsTrue(xmlString.Contains("number-of-billing-cycles"));
        }

        [Test]
        public void UpdatePlanModificationRequest_ToXmlIncludesAllData()
        {
            var request = new UpdatePlanModificationRequest()
            {
                Amount = 5,
                Description = "A modification",
                ExistingId = "some id",
                Name = "A name",
                NumberOfBillingCycles = 2,
            };

            string xmlString = request.ToXml("modification");

            Assert.IsTrue(xmlString.Contains("amount"));
            Assert.IsTrue(xmlString.Contains("description"));
            Assert.IsTrue(xmlString.Contains("existing-id"));
            Assert.IsTrue(xmlString.Contains("name"));
            Assert.IsTrue(xmlString.Contains("number-of-billing-cycles"));
        }
    }
}