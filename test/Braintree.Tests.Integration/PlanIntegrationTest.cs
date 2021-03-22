using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class PlanIntegrationTest
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
            service = new BraintreeService(gateway.Configuration); }

        [Test]
        public void All_ReturnsAllPlans()
        {
            string planToken = string.Format("plan{0}", new Random().Next(1000000).ToString());
           
            service.Post(service.MerchantPath() + "/plans/create_plan_for_tests", new PlanRequestForTests {
                BillingDayOfMonth = 1,
                BillingFrequency = 1,
                CurrencyIsoCode = "USD",
                Description = "a_test_plan",
                Id = planToken,
                Name = "dotnet_plan",
                NumberOfBillingCycles = 1,
                Price = 100.00M,
                TrialPeriod = false,
            });

            List<Plan> collection = gateway.Plan.All();
            Assert.IsNotEmpty(collection);

            Plan plan = collection.Find
            (
                p => p.Id == planToken
            );

            Assert.AreEqual("dotnet_plan", plan.Name);
            Assert.AreEqual(1, plan.BillingDayOfMonth);
            Assert.AreEqual(1, plan.BillingFrequency);
            Assert.AreEqual("USD", plan.CurrencyIsoCode);
            Assert.AreEqual("a_test_plan", plan.Description);
            Assert.AreEqual(1, plan.NumberOfBillingCycles);
            Assert.AreEqual(100.00M, plan.Price);
            Assert.AreEqual(false, plan.TrialPeriod);
        }

        [Test]
#if netcore
        public async Task AllAsync_ReturnsAllPlans()
#else
        public void AllAsync_ReturnsAllPlans()
        {
            Task.Run(async () =>
#endif
        {
            string planToken = string.Format("plan{0}", new Random().Next(1000000).ToString());

            await service.PostAsync(service.MerchantPath() + "/plans/create_plan_for_tests", new PlanRequestForTests {
                BillingDayOfMonth = 1,
                BillingFrequency = 1,
                CurrencyIsoCode = "USD",
                Description = "a_test_plan",
                Id = planToken,
                Name = "dotnet_plan",
                NumberOfBillingCycles = 1,
                Price = 100.00M,
                TrialPeriod = false,
            });

            List<Plan> collection = await gateway.Plan.AllAsync();
            Assert.IsNotEmpty(collection);

            Plan plan = collection.Find
            (
                p => p.Id == planToken
            );

            Assert.AreEqual("dotnet_plan", plan.Name);
            Assert.AreEqual(1, plan.BillingDayOfMonth);
            Assert.AreEqual(1, plan.BillingFrequency);
            Assert.AreEqual("USD", plan.CurrencyIsoCode);
            Assert.AreEqual("a_test_plan", plan.Description);
            Assert.AreEqual(1, plan.NumberOfBillingCycles);
            Assert.AreEqual(100.00M, plan.Price);
            Assert.AreEqual(false, plan.TrialPeriod);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void All_ReturnPlansWithAddOnsAndDiscounts()
        {
            string planToken = string.Format("plan{0}", new Random().Next(1000000).ToString());

            service.Post(service.MerchantPath() + "/plans/create_plan_for_tests", new PlanRequestForTests {
                BillingDayOfMonth = 1,
                BillingFrequency = 1,
                CurrencyIsoCode = "USD",
                Description = "a_test_plan",
                Id = planToken,
                Name = "dotnet_plan",
                NumberOfBillingCycles = 1,
                Price = 100.00M,
                TrialPeriod = false,
            });

            service.Post(service.MerchantPath() + "/modifications/create_modification_for_tests", new ModificationRequestForTests {
                Amount = 1,
                Kind = "add_on",
                Name = "dotnet_test_modification_add_on",
                PlanId = planToken
            });

            service.Post(service.MerchantPath() + "/modifications/create_modification_for_tests", new ModificationRequestForTests {
                Amount = 1,
                Kind = "discount",
                Name = "dotnet_test_modification_discount",
                PlanId = planToken
            });

            List<Plan> collection = gateway.Plan.All();
            Assert.IsNotEmpty(collection);

            Plan plan = collection.Find
            (
                p => p.Id == planToken
            );

            Assert.AreEqual("dotnet_plan", plan.Name);
            Assert.AreEqual("dotnet_test_modification_add_on", plan.AddOns[0].Name);
            Assert.AreEqual("dotnet_test_modification_discount", plan.Discounts[0].Name);
        }
    }
}

