using Braintree.Exceptions;
using Braintree.Test;
using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Collections.Generic;
#if net452
using System.Threading;
#endif
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
            string planToken = $"plan{new Random().Next(1000000).ToString()}";
           
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
            string planToken = $"plan{new Random().Next(1000000).ToString()}";

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
            string planToken = $"plan{new Random().Next(1000000).ToString()}";

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

        [Test]
        public void Create_Plan()
        {
            PlanRequest request = new PlanRequest
            {
                BillingDayOfMonth = 12,
                BillingFrequency = 1,
                CurrencyIsoCode = "USD",
                Description = "My Plan Description",
                Name = "My Plan Name",
                NumberOfBillingCycles = 1,
                Price = 9.99M,
                TrialPeriod = false,
            };

            Result<Plan> result = gateway.Plan.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Plan plan = result.Target;

            Assert.AreEqual(plan.BillingDayOfMonth, 12);
            Assert.AreEqual(plan.BillingFrequency, 1);
            Assert.AreEqual(plan.CurrencyIsoCode, "USD");
            Assert.AreEqual(plan.Description, "My Plan Description");
            Assert.AreEqual(plan.Name, "My Plan Name");
            Assert.AreEqual(plan.NumberOfBillingCycles, 1);
            Assert.AreEqual(plan.Price, 9.99M);
        }
        
        [Test]
#if netcore
        public async Task CreateAsync_Plan()
#else
        public void CreateAsync_Plan()
        {
            Task.Run(async () =>
#endif
        {
             PlanRequest request = new PlanRequest
            {
                BillingDayOfMonth = 12,
                BillingFrequency = 1,
                CurrencyIsoCode = "USD",
                Description = "My Plan Description",
                Name = "My Plan Name",
                NumberOfBillingCycles = 1,
                Price = 9.99M,
                TrialPeriod = false,
            };

            Result<Plan> result = await gateway.Plan.CreateAsync(request);
            Assert.IsTrue(result.IsSuccess());
            Plan plan = result.Target;

            Assert.AreEqual(plan.BillingDayOfMonth, 12);
            Assert.AreEqual(plan.BillingFrequency, 1);
            Assert.AreEqual(plan.CurrencyIsoCode, "USD");
            Assert.AreEqual(plan.Description, "My Plan Description");
            Assert.AreEqual(plan.Name, "My Plan Name");
            Assert.AreEqual(plan.NumberOfBillingCycles, 1);
            Assert.AreEqual(plan.Price, 9.99M);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Find()
        {
            PlanRequest request = new PlanRequest
            {
                BillingDayOfMonth = 12,
                BillingFrequency = 1,
                CurrencyIsoCode = "USD",
                Description = "My Plan Description",
                Name = "My Plan Name",
                NumberOfBillingCycles = 1,
                Price = 9.99M,
                TrialPeriod = false,
            };

            Result<Plan> result = gateway.Plan.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Plan plan = result.Target;
            
            Plan foundPlan = gateway.Plan.Find(plan.Id);
            Assert.AreEqual(plan.Id, foundPlan.Id);
            Assert.AreEqual(plan.Price, foundPlan.Price);
        }

        [Test]
#if netcore
        public async Task FindAsync()
#else
        public void FindAsync()
        {
        Task.Run(async () =>
#endif
        {
             PlanRequest request = new PlanRequest
            {
                BillingDayOfMonth = 12,
                BillingFrequency = 1,
                CurrencyIsoCode = "USD",
                Description = "My Plan Description",
                Name = "My Plan Name",
                NumberOfBillingCycles = 1,
                Price = 9.99M,
                TrialPeriod = false,
            };

            Result<Plan> result = await gateway.Plan.CreateAsync(request);
            Plan plan = result.Target;
            
            Plan foundPlan = await gateway.Plan.FindAsync(plan.Id);
            Assert.AreEqual(plan.Id, foundPlan.Id);
            Assert.AreEqual(plan.Price, foundPlan.Price);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Find_FindsErrorsOutOnWhitespaceIds()
        {
            Assert.Throws<NotFoundException>(() => gateway.Plan.Find(" "));
        }
        
        [Test]
        public void Update_Plan()
        {
            PlanRequest request = new PlanRequest
            {
                BillingDayOfMonth = 12,
                BillingFrequency = 1,
                CurrencyIsoCode = "USD",
                Description = "My Plan Description",
                Name = "My Plan Name",
                NumberOfBillingCycles = 1,
                Price = 9.99M,
                TrialPeriod = false,
            };

            Result<Plan> createResult = gateway.Plan.Create(request);
            Assert.IsTrue(createResult.IsSuccess());
            Plan plan = createResult.Target;
            
            PlanRequest updateRequest = new PlanRequest
            {
                Name = "My Updated Plan Name",
                Price = 99.99M
            };

            Result<Plan> result = gateway.Plan.Update(plan.Id, updateRequest);
            Assert.IsTrue(result.IsSuccess());
            Plan updatedPlan = result.Target;

            Assert.AreEqual(updatedPlan.Name, "My Updated Plan Name");
            Assert.AreEqual(updatedPlan.Price, 99.99M);
        }
        
        [Test]
#if netcore
        public async Task UpdateAsync_Plan()
#else
        public void UpdateAsync_Plan()
        {
            Task.Run(async () =>
#endif
        {
             PlanRequest request = new PlanRequest
            {
                BillingDayOfMonth = 12,
                BillingFrequency = 1,
                CurrencyIsoCode = "USD",
                Description = "My Plan Description",
                Name = "My Plan Name",
                NumberOfBillingCycles = 1,
                Price = 9.99M,
                TrialPeriod = false,
            };

            Result<Plan> createResult = await gateway.Plan.CreateAsync(request);
            Assert.IsTrue(createResult.IsSuccess());
            Plan plan = createResult.Target;
                        
            PlanRequest updateRequest = new PlanRequest
            {
                Name = "My Updated Plan Name",
                Price = 99.99M
            };

            Result<Plan> result = await gateway.Plan.UpdateAsync(plan.Id, updateRequest);
            Assert.IsTrue(result.IsSuccess());
            Plan updatedPlan = result.Target;

            Assert.AreEqual(updatedPlan.Name, "My Updated Plan Name");
            Assert.AreEqual(updatedPlan.Price, 99.99M);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif



    }
}

