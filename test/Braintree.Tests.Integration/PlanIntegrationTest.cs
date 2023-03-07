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
        public void Create_PlanWithAddOnAndDiscount()
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

            Result<Plan> result = gateway.Plan.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Plan plan = result.Target;

            Assert.AreEqual(plan.Name, "My Plan Name");

            Assert.AreEqual(1, plan.AddOns.Count);
            Assert.AreEqual("increase_30", plan.AddOns[0].Id);
            Assert.AreEqual(35M, plan.AddOns[0].Amount);
            Assert.AreEqual("this describes the modification", plan.AddOns[0].Description);
            Assert.AreEqual("30 dollar increase", plan.AddOns[0].Name);
            Assert.AreEqual(3, plan.AddOns[0].NumberOfBillingCycles);

            Assert.AreEqual(1, plan.Discounts.Count);
            Assert.AreEqual("discount_15", plan.Discounts[0].Id);
            Assert.AreEqual(17M, plan.Discounts[0].Amount);
            Assert.AreEqual("this describes the modification", plan.Discounts[0].Description);
            Assert.AreEqual("15 dollar discount", plan.Discounts[0].Name);
            Assert.AreEqual(2, plan.Discounts[0].NumberOfBillingCycles);

        }
        
        [Test]
        public void Create_PlanWithAddOnAndDiscountIdOnly()
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
                AddOns = new PlanAddOnsRequest[]
                {
                    new PlanAddOnsRequest
                    {
                        Add = new AddPlanModificationRequest[] 
                        {
                            new AddPlanModificationRequest 
                            {
                                InheritedFromId = "increase_10",
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
                                InheritedFromId = "discount_11",
                            }
                        }
                    }
                }
            };

            Result<Plan> result = gateway.Plan.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Plan plan = result.Target;

            Assert.AreEqual(plan.Name, "My Plan Name");

            Assert.AreEqual(1, plan.AddOns.Count);
            Assert.AreEqual("increase_10", plan.AddOns[0].Id);
            Assert.AreEqual(10M, plan.AddOns[0].Amount);
            Assert.AreEqual("this describes the modification", plan.AddOns[0].Description);
            Assert.AreEqual("10 dollar increase", plan.AddOns[0].Name);
            Assert.IsNull(plan.AddOns[0].NumberOfBillingCycles);

            Assert.AreEqual(1, plan.Discounts.Count);
            Assert.AreEqual("discount_11", plan.Discounts[0].Id);
            Assert.AreEqual(11M, plan.Discounts[0].Amount);
            Assert.AreEqual("this describes the modification", plan.Discounts[0].Description);
            Assert.AreEqual("11 dollar discount", plan.Discounts[0].Name);
            Assert.IsNull(plan.Discounts[0].NumberOfBillingCycles);

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
        public void Update_PlanWithAddOnAndDiscount() 
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
                AddOns = new PlanAddOnsRequest[]
                {
                    new PlanAddOnsRequest
                    {
                        Add = new AddPlanModificationRequest[] 
                        {
                            new AddPlanModificationRequest 
                            {
                                InheritedFromId = "increase_30",
                                Amount = 30M,
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

            Result<Plan> result = gateway.Plan.Update(plan.Id, updateRequest);
            Assert.IsTrue(result.IsSuccess());
            Plan updatedPlan = result.Target;

            Assert.AreEqual(updatedPlan.Name, "My Updated Plan Name");

            Assert.AreEqual(1, updatedPlan.AddOns.Count);
            Assert.AreEqual("increase_30", updatedPlan.AddOns[0].Id);
            Assert.AreEqual(30M, updatedPlan.AddOns[0].Amount);
            Assert.AreEqual("this describes the modification", updatedPlan.AddOns[0].Description);
            Assert.AreEqual("30 dollar increase", updatedPlan.AddOns[0].Name);
            Assert.AreEqual(3, updatedPlan.AddOns[0].NumberOfBillingCycles);

            Assert.AreEqual(1, updatedPlan.Discounts.Count);
            Assert.AreEqual("discount_15", updatedPlan.Discounts[0].Id);
            Assert.AreEqual(17M, updatedPlan.Discounts[0].Amount);
            Assert.AreEqual("this describes the modification", updatedPlan.Discounts[0].Description);
            Assert.AreEqual("15 dollar discount", updatedPlan.Discounts[0].Name);
            Assert.AreEqual(2, updatedPlan.Discounts[0].NumberOfBillingCycles);
        }

        [Test]
        public void Update_WithoutModificationsRemovesThem()
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
                AddOns = new PlanAddOnsRequest[]
                {
                    new PlanAddOnsRequest
                    {
                        Add = new AddPlanModificationRequest[] 
                        {
                            new AddPlanModificationRequest 
                            {
                                InheritedFromId = "increase_20",
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
                            }
                        }
                    }
                }
            };

            Result<Plan> createResult = gateway.Plan.Create(request);
            Assert.IsTrue(createResult.IsSuccess());
            Plan plan = createResult.Target;

            PlanRequest updateRequest = new PlanRequest
            {
                Name = "My Updated Plan Name",
            };

            Result<Plan> result = gateway.Plan.Update(plan.Id, updateRequest);
            Assert.IsTrue(result.IsSuccess());
            Plan updatedPlan = result.Target;

            Assert.AreEqual(updatedPlan.Name, "My Updated Plan Name");

            Assert.AreEqual(0, updatedPlan.AddOns.Count);
            Assert.AreEqual(0, updatedPlan.Discounts.Count);
        }

        [Test]
        public void Update_WithUpdateModificationsChangesThem()
        {
            string planToken = $"plan{new Random().Next(1000000).ToString()}";
            string addOnToken = $"addon{new Random().Next(1000000).ToString()}";
            string discountToken = $"discount{new Random().Next(1000000).ToString()}";

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
                Name = "dotnet_test_modification_add_on_2",
                Id = addOnToken,
                PlanId = planToken
            });

            service.Post(service.MerchantPath() + "/modifications/create_modification_for_tests", new ModificationRequestForTests {
                Amount = 1,
                Kind = "discount",
                Name = "dotnet_test_modification_discount_2",
                Id = discountToken,
                PlanId = planToken
            });

            PlanRequest updateRequest = new PlanRequest
            {
                Name = "My Updated Plan Name",
                AddOns = new PlanAddOnsRequest[]
                {
                    new PlanAddOnsRequest
                    {
                        Update = new UpdatePlanModificationRequest[] 
                        {
                            new UpdatePlanModificationRequest 
                            {
                                ExistingId = addOnToken,
                                Amount = 30,
                                Description = "this describes the modification",
                                NumberOfBillingCycles = 3,
                            }
                        },
                    }
                },
                Discounts = new PlanDiscountsRequest[]
                {
                    new PlanDiscountsRequest
                    {
                        Update = new UpdatePlanModificationRequest[]
                        {
                            new UpdatePlanModificationRequest
                            {
                                ExistingId = discountToken,
                                Amount = 17,
                                Description = "this describes the modification",
                                NumberOfBillingCycles = 2,
                            }
                        }
                    }
                }
            };

            Result<Plan> result = gateway.Plan.Update(planToken, updateRequest);
            Assert.IsTrue(result.IsSuccess());
            Plan updatedPlan = result.Target;

            Assert.AreEqual(updatedPlan.Name, "My Updated Plan Name");

            Assert.AreEqual(1, updatedPlan.AddOns.Count);
            Assert.AreEqual(30, updatedPlan.AddOns[0].Amount);
            Assert.AreEqual("this describes the modification", updatedPlan.AddOns[0].Description);
            Assert.AreEqual(3, updatedPlan.AddOns[0].NumberOfBillingCycles);     

            Assert.AreEqual(1, updatedPlan.Discounts.Count);
            Assert.AreEqual(17, updatedPlan.Discounts[0].Amount);
            Assert.AreEqual("this describes the modification", updatedPlan.Discounts[0].Description);
            Assert.AreEqual(2, updatedPlan.Discounts[0].NumberOfBillingCycles);
        }

        [Test]
        public void Update_WithBadModificationsWontUpdate()
        {
            string planToken = $"plan{new Random().Next(1000000).ToString()}";
            string addOnToken = $"addon{new Random().Next(1000000).ToString()}";
            string discountToken = $"discount{new Random().Next(1000000).ToString()}";

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
                Name = "dotnet_test_modification_add_on_2",
                Id = addOnToken,
                PlanId = planToken
            });

            service.Post(service.MerchantPath() + "/modifications/create_modification_for_tests", new ModificationRequestForTests {
                Amount = 1,
                Kind = "discount",
                Name = "dotnet_test_modification_discount_2",
                Id = discountToken,
                PlanId = planToken
            });

            PlanRequest updateRequest = new PlanRequest
            {
                Name = "My Updated Plan Name",
                AddOns = new PlanAddOnsRequest[]
                {
                    new PlanAddOnsRequest
                    {
                        Update = new UpdatePlanModificationRequest[] 
                        {
                            new UpdatePlanModificationRequest 
                            {
                                ExistingId = addOnToken,
                                Amount = -30,
                            }
                        },
                    }
                },
                Discounts = new PlanDiscountsRequest[]
                {
                    new PlanDiscountsRequest
                    {
                        Update = new UpdatePlanModificationRequest[]
                        {
                            new UpdatePlanModificationRequest
                            {
                                ExistingId = discountToken,
                                Amount = -17,
                            }
                        }
                    }
                }
            };

            Result<Plan> result = gateway.Plan.Update(planToken, updateRequest);
            Assert.IsTrue(result.IsSuccess());
            Plan updatedPlan = result.Target;

            Assert.AreEqual(updatedPlan.Name, "My Updated Plan Name");
            
            Assert.AreEqual(1, updatedPlan.AddOns.Count);
            Assert.AreEqual(1, updatedPlan.AddOns[0].Amount);    

            Assert.AreEqual(1, updatedPlan.Discounts.Count);
            Assert.AreEqual(1, updatedPlan.Discounts[0].Amount);
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

