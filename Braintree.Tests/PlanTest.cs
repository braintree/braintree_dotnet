using System;
using System.Collections.Generic;
using NUnit.Framework;
using Braintree;

namespace Braintree.Tests
{
    [TestFixture]
    public class PlanTest
    {
        private BraintreeGateway gateway;
        private BraintreeService service;

        [SetUp]
        public void Setup()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
            service = new BraintreeService(gateway.Configuration);
        }

        [Test]
        public void All_ReturnsAllPlans()
        {
            String planToken = String.Format("plan{0}", new Random().Next(1000000).ToString());

            service.Post("/plans/create_plan_for_tests", new PlanRequestForTests {
                BillingDayOfMonth = 1,
                BillingFrequency = 1,
                CurrencyIsoCode = "USD",
                Description = "a_test_plan",
                Id = planToken,
                Name = "dotnet_plan",
                NumberOfBillingCycles = 1,
                Price = 100.00M,
                TrialPeriod = false,
                TrialDuration = 1,
                TrialDurationUnit = PlanDurationUnit.DAY
            });

            List<Plan> collection = gateway.Plan.All();
            Assert.IsNotEmpty(collection);

            Plan plan = collection.Find
            (
                delegate(Plan p)
                {
                    return p.Id == planToken;
                }
            );

            Assert.AreEqual("dotnet_plan", plan.Name);
            Assert.AreEqual(1, plan.BillingDayOfMonth);
            Assert.AreEqual(1, plan.BillingFrequency);
            Assert.AreEqual("USD", plan.CurrencyIsoCode);
            Assert.AreEqual("a_test_plan", plan.Description);
            Assert.AreEqual(1, plan.NumberOfBillingCycles);
            Assert.AreEqual(100.00M, plan.Price);
            Assert.AreEqual(false, plan.TrialPeriod);
            Assert.AreEqual(1, plan.TrialDuration);
            Assert.AreEqual(PlanDurationUnit.DAY, plan.TrialDurationUnit);
        }

        [Test]
        public void All_ReturnPlansWithAddOnsAndDiscounts()
        {
            String planToken = String.Format("plan{0}", new Random().Next(1000000).ToString());

            service.Post("/plans/create_plan_for_tests", new PlanRequestForTests {
                BillingDayOfMonth = 1,
                BillingFrequency = 1,
                CurrencyIsoCode = "USD",
                Description = "a_test_plan",
                Id = planToken,
                Name = "dotnet_plan",
                NumberOfBillingCycles = 1,
                Price = 100.00M,
                TrialPeriod = false,
                TrialDuration = 1,
                TrialDurationUnit = PlanDurationUnit.DAY
            });

            service.Post("/modifications/create_modification_for_tests", new ModificationRequestForTests {
                Amount = 1,
                Kind = "add_on",
                Name = "dotnet_test_modification_add_on",
                PlanId = planToken
            });

            service.Post("/modifications/create_modification_for_tests", new ModificationRequestForTests {
                Amount = 1,
                Kind = "discount",
                Name = "dotnet_test_modification_discount",
                PlanId = planToken
            });

            List<Plan> collection = gateway.Plan.All();
            Assert.IsNotEmpty(collection);

            Plan plan = collection.Find
            (
                delegate(Plan p)
                {
                    return p.Id == planToken;
                }
            );

            Assert.AreEqual("dotnet_plan", plan.Name);
            Assert.AreEqual("dotnet_test_modification_add_on", plan.AddOns[0].Name);
            Assert.AreEqual("dotnet_test_modification_discount", plan.Discounts[0].Name);
        }
    }
}

