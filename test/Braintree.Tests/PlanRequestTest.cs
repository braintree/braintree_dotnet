using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class PlanRequestTest
    {
        [Test]
        public void ToXml_IncludesAllData()
        {
            var request = new PlanRequest()
            {
                BillingDayOfMonth = 12,
                BillingFrequency = 1,
                CurrencyIsoCode = "USD",
                Description = "My Plan Description",
                Name = "My Plan Name",
                NumberOfBillingCycles = 1,
                Price = 9.99M,
                TrialPeriod = true,
                TrialDuration = 2,
                TrialDurationUnit = PlanDurationUnit.DAY,
                AddOns = new PlanAddOnsRequest[]
                {
                    new PlanAddOnsRequest
                    {
                        Add = new AddPlanModificationRequest[] 
                        {
                            new AddPlanModificationRequest 
                            {
                                InheritedFromId = "increase_30",
                                Amount = 35M,
                                Description = "this describes the modification",
                                Name = "30 dollar increase",
                                NumberOfBillingCycles = 3,
                            }
                        },
                    }
                },
                Discounts = new PlanDiscountsRequest[]
                {
                    new PlanDiscountsRequest
                    {
                        Add = new AddPlanModificationRequest[]
                        {
                            new AddPlanModificationRequest
                            {
                                InheritedFromId = "discount_15",
                                Amount = 17M,
                                Description = "this describes the modification",
                                Name = "15 dollar discount",
                                NumberOfBillingCycles = 2,
                            }
                        }
                    }
                }
            };

            string xmlString = request.ToXml();

            Assert.IsTrue(xmlString.Contains("billing-day-of-month"));
            Assert.IsTrue(xmlString.Contains("billing-frequency"));
            Assert.IsTrue(xmlString.Contains("currency-iso-code"));
            Assert.IsTrue(xmlString.Contains("description"));
            Assert.IsTrue(xmlString.Contains("id"));
            Assert.IsTrue(xmlString.Contains("name"));
            Assert.IsTrue(xmlString.Contains("number-of-billing-cycles"));
            Assert.IsTrue(xmlString.Contains("price"));
            Assert.IsTrue(xmlString.Contains("trial-period"));
            Assert.IsTrue(xmlString.Contains("trial-duration"));
            Assert.IsTrue(xmlString.Contains("trial-duration-unit"));
            Assert.IsTrue(xmlString.Contains("add-ons"));
            Assert.IsTrue(xmlString.Contains("discounts"));
        }
    }
}