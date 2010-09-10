using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class SubscriptionTest
    {
        private BraintreeGateway gateway;
        private Customer customer;
        private CreditCard creditCard;

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

            CustomerRequest request = new CustomerRequest
            {
                CreditCard = new CreditCardRequest
                {
                    CardholderName = "Fred Jones",
                    Number = "5105105105105100",
                    ExpirationDate = "05/12"
                }
            };

            customer = gateway.Customer.Create(request).Target;
            creditCard = customer.CreditCards[0];
        }

        [Test]
        public void Create_SubscriptionWithoutTrial()
        {
            Plan plan = Plan.PLAN_WITHOUT_TRIAL;

            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Subscription subscription = result.Target;

            Assert.AreEqual(creditCard.Token, subscription.PaymentMethodToken);
            Assert.AreEqual(plan.Id, subscription.PlanId);
            Assert.AreEqual(MerchantAccount.DEFAULT_MERCHANT_ACCOUNT_ID, subscription.MerchantAccountId);
            Assert.AreEqual(plan.Price, subscription.Price);
            Assert.AreEqual(plan.Price, subscription.NextBillAmount);
            Assert.AreEqual(0.00M, subscription.Balance);
            Assert.IsTrue(Regex.IsMatch(subscription.Id, "^\\w{6}$"));
            Assert.AreEqual(SubscriptionStatus.ACTIVE, subscription.Status);
            Assert.AreEqual(0, subscription.FailureCount);
            Assert.IsFalse((Boolean)subscription.HasTrialPeriod);

            Assert.IsTrue(subscription.BillingPeriodEndDate.HasValue);
            Assert.IsTrue(subscription.BillingPeriodStartDate.HasValue);
            Assert.IsTrue(subscription.NextBillingDate.HasValue);
            Assert.IsTrue(subscription.FirstBillingDate.HasValue);
            Assert.IsTrue(subscription.PaidThroughDate.HasValue);
        }

        [Test]
        public void Create_ReturnsDeclinedTransaction()
        {
            Plan plan = Plan.PLAN_WITHOUT_TRIAL;

            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id,
                Price = SandboxValues.TransactionAmount.DECLINE
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.PROCESSOR_DECLINED, result.Transaction.Status);
        }

        [Test]
        public void Create_SubscriptionWithTrial()
        {
            Plan plan = Plan.PLAN_WITH_TRIAL;

            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Subscription subscription = result.Target;

            Assert.AreEqual(creditCard.Token, subscription.PaymentMethodToken);
            Assert.AreEqual(plan.Id, subscription.PlanId);
            Assert.AreEqual(MerchantAccount.DEFAULT_MERCHANT_ACCOUNT_ID, subscription.MerchantAccountId);
            Assert.AreEqual(plan.Price, subscription.Price);
            Assert.IsTrue(Regex.IsMatch(subscription.Id, "^\\w{6}$"));
            Assert.AreEqual(SubscriptionStatus.ACTIVE, subscription.Status);
            Assert.AreEqual(0, subscription.FailureCount);
            Assert.IsTrue(subscription.HasTrialPeriod.Value);

            Assert.IsFalse(subscription.BillingPeriodEndDate.HasValue);
            Assert.IsFalse(subscription.BillingPeriodStartDate.HasValue);
            Assert.IsTrue(subscription.NextBillingDate.HasValue);
            Assert.IsTrue(subscription.FirstBillingDate.HasValue);
        }

        [Test]
        public void Create_OverridePlanAddTrial()
        {
            Plan plan = Plan.PLAN_WITHOUT_TRIAL;

            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id,
                HasTrialPeriod = true,
                TrialDuration = 2,
                TrialDurationUnit = SubscriptionDurationUnit.MONTH
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Subscription subscription = result.Target;

            Assert.IsTrue(subscription.HasTrialPeriod.Value);
            Assert.AreEqual(2, subscription.TrialDuration);
            Assert.AreEqual(SubscriptionDurationUnit.MONTH, subscription.TrialDurationUnit);
        }

        [Test]
        public void Create_OverridePlanRemoveTrial()
        {
            Plan plan = Plan.PLAN_WITH_TRIAL;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id,
                HasTrialPeriod = false
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Subscription subscription = result.Target;

            Assert.IsFalse(subscription.HasTrialPeriod.Value);
        }

        [Test]
        public void Create_OverridePlanPrice()
        {
            Plan plan = Plan.PLAN_WITH_TRIAL;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id,
                Price = 482.48M
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Subscription subscription = result.Target;

            Assert.AreEqual(482.48M, subscription.Price);
        }

        [Test]
        public void Create_OverrideNumberOfBillingCycles()
        {
            Plan plan = Plan.PLAN_WITH_TRIAL;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id
            };

            Subscription subscription = gateway.Subscription.Create(request).Target;

            Assert.AreEqual(12, subscription.NumberOfBillingCycles);

            SubscriptionRequest overrideRequest = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id,
                NumberOfBillingCycles = 10
            };

            Subscription overridenSubscription = gateway.Subscription.Create(overrideRequest).Target;
            Assert.AreEqual(10, overridenSubscription.NumberOfBillingCycles);
            Assert.IsFalse(overridenSubscription.NeverExpires.Value);
        }

        [Test]
        public void Create_OverrideNumberOfBillingCyclesToNeverExpire()
        {
            Plan plan = Plan.PLAN_WITH_TRIAL;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id,
                NeverExpires = true
            };

            Subscription subscription = gateway.Subscription.Create(request).Target;

            Assert.IsNull(subscription.NumberOfBillingCycles);
            Assert.IsTrue(subscription.NeverExpires.Value);
        }

        [Test]
        public void Create_InheritBillingDayOfMonth()
        {
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.BILLING_DAY_OF_MONTH_PLAN.Id
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Subscription subscription = result.Target;

            Assert.AreEqual(5, subscription.BillingDayOfMonth);
        }

        [Test]
        public void Create_OverrideBillingDayOfMonth()
        {
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.BILLING_DAY_OF_MONTH_PLAN.Id,
                BillingDayOfMonth = 19
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Subscription subscription = result.Target;

            Assert.AreEqual(19, subscription.BillingDayOfMonth);
        }

        [Test]
        public void Create_OverrideBillingDayOfMonthWithStartImmediately()
        {
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.BILLING_DAY_OF_MONTH_PLAN.Id,
                Options = new SubscriptionOptionsRequest
                {
                    StartImmediately = true
                }
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Subscription subscription = result.Target;

            Assert.AreEqual(1, subscription.Transactions.Count);
        }

        [Test]
        public void Create_SetFirstBillingDate()
        {
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.BILLING_DAY_OF_MONTH_PLAN.Id,
                FirstBillingDate = DateTime.Now.AddDays(3)
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Subscription subscription = result.Target;

            Assert.AreEqual(DateTime.Now.AddDays(3).ToShortDateString(), subscription.FirstBillingDate.Value.ToShortDateString());
            Assert.AreEqual(SubscriptionStatus.PENDING, subscription.Status);
        }

        [Test]
        public void Create_SetFirstBillingDateInThePast()
        {
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.BILLING_DAY_OF_MONTH_PLAN.Id,
                FirstBillingDate = DateTime.Now.AddDays(-3)
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.SUBSCRIPTION_FIRST_BILLING_DATE_CANNOT_BE_IN_THE_PAST,
                result.Errors.ForObject("Subscription").OnField("FirstBillingDate")[0].Code);
        }

        [Test]
        public void Create_SetId()
        {
            Plan plan = Plan.PLAN_WITH_TRIAL;
            String newId = "new-id-" + new Random().Next(1000000);
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id,
                Price = 482.48M,
                Id = newId
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Subscription subscription = result.Target;

            Assert.AreEqual(newId, subscription.Id);
        }

        [Test]
        public void Create_SetMerchantAccountId()
        {
            Plan plan = Plan.PLAN_WITH_TRIAL;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id,
                MerchantAccountId = MerchantAccount.NON_DEFAULT_MERCHANT_ACCOUNT_ID
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Subscription subscription = result.Target;

            Assert.AreEqual(MerchantAccount.NON_DEFAULT_MERCHANT_ACCOUNT_ID, subscription.MerchantAccountId);
        }

        [Test]
        public void Create_HasTransactionOnCreateWithNoTrial()
        {
            Plan plan = Plan.PLAN_WITHOUT_TRIAL;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id,
                Price = 482.48M
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Subscription subscription = result.Target;
            Transaction transaction = subscription.Transactions[0];

            Assert.AreEqual(1, subscription.Transactions.Count);
            Assert.AreEqual(482.48M, transaction.Amount);
            Assert.AreEqual(TransactionType.SALE, transaction.Type);
            Assert.AreEqual(subscription.Id, transaction.SubscriptionId);
        }

        [Test]
        public void Create_HasNoTransactionOnCreateWithATrial()
        {
            Plan plan = Plan.PLAN_WITH_TRIAL;

            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Subscription subscription = result.Target;

            Assert.AreEqual(0, subscription.Transactions.Count);
        }

        [Test]
        public void Create_InheritsNoAddOnsAndDiscountsWhenOptionIsPassed()
        {
            Plan plan = Plan.ADD_ON_DISCOUNT_PLAN;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id,
                Options = new SubscriptionOptionsRequest
                {
                    DoNotInheritAddOnsOrDiscounts = true
                }
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Subscription subscription = result.Target;

            Assert.AreEqual(0, subscription.AddOns.Count);
            Assert.AreEqual(0, subscription.Discounts.Count);
        }

        [Test]
        public void Create_InheritsAddOnsAndDiscountsFromPlan() {
            Plan plan = Plan.ADD_ON_DISCOUNT_PLAN;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Subscription subscription = result.Target;

            List<AddOn> addOns = subscription.AddOns;
            addOns.Sort(TestHelper.CompareModificationsById);

            Assert.AreEqual(2, addOns.Count);

            Assert.AreEqual(10M, addOns[0].Amount);
            Assert.AreEqual(1, addOns[0].Quantity);
            Assert.IsTrue(addOns[0].NeverExpires.Value);
            Assert.IsNull(addOns[0].NumberOfBillingCycles);

            Assert.AreEqual(20.00M, addOns[1].Amount);
            Assert.AreEqual(1, addOns[1].Quantity);
            Assert.IsTrue(addOns[1].NeverExpires.Value);
            Assert.IsNull(addOns[1].NumberOfBillingCycles);

            List<Discount> discounts = subscription.Discounts;
            discounts.Sort(TestHelper.CompareModificationsById);

            Assert.AreEqual(2, discounts.Count);

            Assert.AreEqual(11M, discounts[0].Amount);
            Assert.AreEqual(1, discounts[0].Quantity);
            Assert.IsTrue(discounts[0].NeverExpires.Value);
            Assert.IsNull(discounts[0].NumberOfBillingCycles);

            Assert.AreEqual(7M, discounts[1].Amount);
            Assert.AreEqual(1, discounts[1].Quantity);
            Assert.IsTrue(discounts[1].NeverExpires.Value);
            Assert.IsNull(discounts[1].NumberOfBillingCycles);
        }

        [Test]
        public void Create_CanOverrideInheritedAddOnsAndDiscountsFromPlan()
        {
            Plan plan = Plan.ADD_ON_DISCOUNT_PLAN;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id,
                AddOns = new AddOnsRequest
                {
                    Update = new UpdateAddOnRequest[]
                    {
                        new UpdateAddOnRequest
                        {
                            ExistingId = "increase_10",
                            Amount = 30M,
                            Quantity = 9,
                            NeverExpires = true
                        },
                        new UpdateAddOnRequest
                        {
                            ExistingId = "increase_20",
                            Amount = 40M,
                            NumberOfBillingCycles = 20
                        }
                    }
                },
                Discounts = new DiscountsRequest
                {
                    Update = new UpdateDiscountRequest[]
                    {
                        new UpdateDiscountRequest
                        {
                            ExistingId = "discount_7",
                            Amount = 15M,
                            Quantity = 7,
                            NeverExpires = true
                        },
                        new UpdateDiscountRequest
                        {
                            ExistingId = "discount_11",
                            Amount = 23M,
                            NumberOfBillingCycles = 11
                        }
                    }
                }
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Subscription subscription = result.Target;

            List<AddOn> addOns = subscription.AddOns;
            addOns.Sort(TestHelper.CompareModificationsById);

            Assert.AreEqual(2, addOns.Count);

            Assert.AreEqual(30M, addOns[0].Amount);
            Assert.AreEqual(9, addOns[0].Quantity);
            Assert.IsTrue(addOns[0].NeverExpires.Value);
            Assert.IsNull(addOns[0].NumberOfBillingCycles);

            Assert.AreEqual(40.00M, addOns[1].Amount);
            Assert.AreEqual(1, addOns[1].Quantity);
            Assert.IsFalse(addOns[1].NeverExpires.Value);
            Assert.AreEqual(20, addOns[1].NumberOfBillingCycles);

            List<Discount> discounts = subscription.Discounts;
            discounts.Sort(TestHelper.CompareModificationsById);

            Assert.AreEqual(2, discounts.Count);

            Assert.AreEqual(23M, discounts[0].Amount);
            Assert.AreEqual(1, discounts[0].Quantity);
            Assert.IsFalse(discounts[0].NeverExpires.Value);
            Assert.AreEqual(11, discounts[0].NumberOfBillingCycles);

            Assert.AreEqual(15M, discounts[1].Amount);
            Assert.AreEqual(7, discounts[1].Quantity);
            Assert.IsTrue(discounts[1].NeverExpires.Value);
            Assert.IsNull(discounts[1].NumberOfBillingCycles);
        }

        [Test]
        public void Create_CanRemoveInheritedAddOnsAndDiscounts()
        {
            Plan plan = Plan.ADD_ON_DISCOUNT_PLAN;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id,
                AddOns = new AddOnsRequest
                {
                    Remove = new String[] { "increase_10", "increase_20" }
                },
                Discounts = new DiscountsRequest
                {
                    Remove = new String[] { "discount_11" }
                }
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Subscription subscription = result.Target;

            Assert.AreEqual(0, subscription.AddOns.Count);
            Assert.AreEqual(1, subscription.Discounts.Count);

            Assert.AreEqual("discount_7", subscription.Discounts[0].Id);
        }

        [Test]
        public void Create_CanAddAddOnsAndDiscounts()
        {
            Plan plan = Plan.ADD_ON_DISCOUNT_PLAN;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id,
                AddOns = new AddOnsRequest
                {
                    Add = new AddAddOnRequest[]
                    {
                        new AddAddOnRequest
                        {
                            InheritedFromId = "increase_30",
                            Amount = 35M,
                            NumberOfBillingCycles = 3,
                            Quantity = 8
                        }
                    },
                    Remove = new String[] { "increase_10", "increase_20" }
                },
                Discounts = new DiscountsRequest
                {
                    Add = new AddDiscountRequest[]
                    {
                        new AddDiscountRequest
                        {
                            InheritedFromId = "discount_15",
                            Amount = 17M,
                            NeverExpires = true,
                            Quantity = 9
                        }
                    },
                    Remove = new String[] { "discount_7", "discount_11" }
                }
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsTrue(result.IsSuccess());
            Subscription subscription = result.Target;

            Assert.AreEqual(1, subscription.AddOns.Count);

            Assert.AreEqual("increase_30", subscription.AddOns[0].Id);
            Assert.AreEqual(35M, subscription.AddOns[0].Amount);
            Assert.AreEqual(8, subscription.AddOns[0].Quantity);
            Assert.IsFalse(subscription.AddOns[0].NeverExpires.Value);
            Assert.AreEqual(3, subscription.AddOns[0].NumberOfBillingCycles);

            Assert.AreEqual(1, subscription.Discounts.Count);

            Assert.AreEqual("discount_15", subscription.Discounts[0].Id);
            Assert.AreEqual(17M, subscription.Discounts[0].Amount);
            Assert.AreEqual(9, subscription.Discounts[0].Quantity);
            Assert.IsTrue(subscription.Discounts[0].NeverExpires.Value);
            Assert.IsNull(subscription.Discounts[0].NumberOfBillingCycles);
        }

        [Test]
        public void Create_WithBadAddOnParamsCorrectlyParsesValidationErrors()
        {
            Plan plan = Plan.ADD_ON_DISCOUNT_PLAN;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id,
                AddOns = new AddOnsRequest
                {
                    Update = new UpdateAddOnRequest[]
                    {
                        new UpdateAddOnRequest
                        {
                            ExistingId = "increase_10",
                            Amount = -200M
                        },
                        new UpdateAddOnRequest
                        {
                            ExistingId = "increase_20",
                            Quantity = -9
                        }
                    }
                }
            };

            Result<Subscription> result = gateway.Subscription.Create(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.SUBSCRIPTION_MODIFICATION_AMOUNT_IS_INVALID,
                result.Errors.ForObject("Subscription").ForObject("AddOns").ForObject("Update").ForIndex(0).OnField("Amount")[0].Code);

            Assert.AreEqual(ValidationErrorCode.SUBSCRIPTION_MODIFICATION_QUANTITY_IS_INVALID,
                result.Errors.ForObject("Subscription").ForObject("AddOns").ForObject("Update").ForIndex(1).OnField("Quantity")[0].Code);
        }

        [Test]
        public void Find()
        {
            Plan plan = Plan.PLAN_WITH_TRIAL;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id
            };

            Subscription subscription = gateway.Subscription.Create(request).Target;

            Subscription foundSubscription = gateway.Subscription.Find(subscription.Id);
            Assert.AreEqual(subscription.Id, foundSubscription.Id);
            Assert.AreEqual(subscription.PaymentMethodToken, creditCard.Token);
            Assert.AreEqual(subscription.PlanId, plan.Id);
        }

        [Test]
        public void Search_OnBillingCyclesRemainingIs()
        {
            SubscriptionRequest request1 = new SubscriptionRequest
            {
                NumberOfBillingCycles = 5,
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.PLAN_WITH_TRIAL.Id,
                Price = 4M
            };

            SubscriptionRequest request2 = new SubscriptionRequest
            {
                NumberOfBillingCycles = 10,
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.PLAN_WITH_TRIAL.Id,
                Price = 4M
            };

            Subscription subscription1 = gateway.Subscription.Create(request1).Target;
            Subscription subscription2 = gateway.Subscription.Create(request2).Target;

            SubscriptionSearchRequest request = new SubscriptionSearchRequest().
                BillingCyclesRemaining.Is(5).
                Price.Is(4M);

            ResourceCollection<Subscription> collection = gateway.Subscription.Search(request);

            Assert.IsTrue(TestHelper.IncludesSubscription(collection, subscription1));
            Assert.IsFalse(TestHelper.IncludesSubscription(collection, subscription2));
        }

        [Test]
        public void Search_OnDaysPastDueBetween()
        {
            SubscriptionRequest subscriptionRequest = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.PLAN_WITH_TRIAL.Id,
            };

            Subscription subscription = gateway.Subscription.Create(subscriptionRequest).Target;
            MakePastDue(subscription, 3);

            SubscriptionSearchRequest request = new SubscriptionSearchRequest().
                DaysPastDue.Between(2, 10);

            ResourceCollection<Subscription> collection = gateway.Subscription.Search(request);
            Assert.IsTrue(collection.MaximumCount > 0);

            foreach (Subscription foundSubscription in collection) {
                Assert.IsTrue(foundSubscription.DaysPastDue >= 2 && foundSubscription.DaysPastDue <= 10);
            }
        }

        [Test]
        public void Search_OnIdIs()
        {
            SubscriptionRequest request1 = new SubscriptionRequest
            {
                Id = String.Format("find_me{0}", new Random().Next(1000000)),
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.PLAN_WITH_TRIAL.Id,
                Price = 3M
            };

            SubscriptionRequest request2 = new SubscriptionRequest
            {
                Id = String.Format("do_not_find_me{0}", new Random().Next(1000000)),
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.PLAN_WITH_TRIAL.Id,
                Price = 3M
            };

            Subscription subscription1 = gateway.Subscription.Create(request1).Target;
            Subscription subscription2 = gateway.Subscription.Create(request2).Target;

            SubscriptionSearchRequest request = new SubscriptionSearchRequest().
                Id.StartsWith("find_me").
                Price.Is(3M);

            ResourceCollection<Subscription> collection = gateway.Subscription.Search(request);

            Assert.IsTrue(TestHelper.IncludesSubscription(collection, subscription1));
            Assert.IsFalse(TestHelper.IncludesSubscription(collection, subscription2));
        }

        [Test]
        public void Search_OnMerchantAccountIdIs()
        {
            SubscriptionRequest request1 = new SubscriptionRequest
            {
                MerchantAccountId = MerchantAccount.DEFAULT_MERCHANT_ACCOUNT_ID,
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.PLAN_WITH_TRIAL.Id,
                Price = 2M
            };

            SubscriptionRequest request2 = new SubscriptionRequest
            {
                MerchantAccountId = MerchantAccount.NON_DEFAULT_MERCHANT_ACCOUNT_ID,
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.PLAN_WITH_TRIAL.Id,
                Price = 2M
            };

            Subscription defaultMerchantAccountSubscription = gateway.Subscription.Create(request1).Target;
            Subscription nonDefaultMerchantAccountSubscription = gateway.Subscription.Create(request2).Target;

            SubscriptionSearchRequest request = new SubscriptionSearchRequest().
                MerchantAccountId.Is(MerchantAccount.NON_DEFAULT_MERCHANT_ACCOUNT_ID).
                Price.Is(2M);

            ResourceCollection<Subscription> collection = gateway.Subscription.Search(request);

            Assert.IsTrue(TestHelper.IncludesSubscription(collection, nonDefaultMerchantAccountSubscription));
            Assert.IsFalse(TestHelper.IncludesSubscription(collection, defaultMerchantAccountSubscription));
        }

        [Test]
        public void Search_OnPlanIdIs()
        {
            Plan triallessPlan = Plan.PLAN_WITHOUT_TRIAL;
            Plan trialPlan = Plan.PLAN_WITH_TRIAL;
            SubscriptionRequest request1 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = trialPlan.Id,
                Price = 5M
            };

            SubscriptionRequest request2 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = triallessPlan.Id,
                Price = 5M
            };

            Subscription trialSubscription = gateway.Subscription.Create(request1).Target;
            Subscription triallessSubscription = gateway.Subscription.Create(request2).Target;

            SubscriptionSearchRequest request = new SubscriptionSearchRequest().
                PlanId.Is(trialPlan.Id).
                Price.Is(5M);

            ResourceCollection<Subscription> collection = gateway.Subscription.Search(request);

            Assert.IsTrue(TestHelper.IncludesSubscription(collection, trialSubscription));
            Assert.IsFalse(TestHelper.IncludesSubscription(collection, triallessSubscription));
        }

        [Test]
        public void Search_OnPrice()
        {
            SubscriptionRequest request10 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.PLAN_WITH_TRIAL.Id,
                Price = 10M
            };

            SubscriptionRequest request20 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.PLAN_WITH_TRIAL.Id,
                Price = 20M
            };

            SubscriptionRequest request30 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.PLAN_WITH_TRIAL.Id,
                Price = 30M
            };

            Subscription subscription10 = gateway.Subscription.Create(request10).Target;
            Subscription subscription20 = gateway.Subscription.Create(request20).Target;
            Subscription subscription30 = gateway.Subscription.Create(request30).Target;

            SubscriptionSearchRequest request = new SubscriptionSearchRequest().
                Price.Between(15M, 20M);

            ResourceCollection<Subscription> collection = gateway.Subscription.Search(request);

            Assert.IsFalse(TestHelper.IncludesSubscription(collection, subscription10));
            Assert.IsTrue(TestHelper.IncludesSubscription(collection, subscription20));
            Assert.IsFalse(TestHelper.IncludesSubscription(collection, subscription30));
        }


        [Test]
        public void Search_OnPlanIdIsWithDelegate()
        {
            Plan triallessPlan = Plan.PLAN_WITHOUT_TRIAL;
            Plan trialPlan = Plan.PLAN_WITH_TRIAL;
            SubscriptionRequest request1 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = trialPlan.Id,
                Price = 6M
            };

            SubscriptionRequest request2 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = triallessPlan.Id,
                Price = 6M
            };

            Subscription trialSubscription = gateway.Subscription.Create(request1).Target;
            Subscription triallessSubscription = gateway.Subscription.Create(request2).Target;

            ResourceCollection<Subscription> collection = gateway.Subscription.Search(delegate(SubscriptionSearchRequest search) {
                search.PlanId.Is(trialPlan.Id);
                search.Price.Is(6M);
            });

            Assert.IsTrue(TestHelper.IncludesSubscription(collection, trialSubscription));
            Assert.IsFalse(TestHelper.IncludesSubscription(collection, triallessSubscription));
        }

        [Test]
        public void Search_OnPlanIdIsNot()
        {
            Plan triallessPlan = Plan.PLAN_WITHOUT_TRIAL;
            Plan trialPlan = Plan.PLAN_WITH_TRIAL;
            SubscriptionRequest request1 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = trialPlan.Id,
                Price = 7M
            };

            SubscriptionRequest request2 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = triallessPlan.Id,
                Price = 7M
            };

            Subscription trialSubscription = gateway.Subscription.Create(request1).Target;
            Subscription triallessSubscription = gateway.Subscription.Create(request2).Target;

            SubscriptionSearchRequest request = new SubscriptionSearchRequest().
                PlanId.IsNot(triallessPlan.Id).
                Price.Is(7M);

            ResourceCollection<Subscription> collection = gateway.Subscription.Search(request);

            Assert.IsTrue(TestHelper.IncludesSubscription(collection, trialSubscription));
            Assert.IsFalse(TestHelper.IncludesSubscription(collection, triallessSubscription));
        }

        [Test]
        public void Search_OnPlanIdStartsWith()
        {
            Plan triallessPlan = Plan.PLAN_WITHOUT_TRIAL;
            Plan trialPlan = Plan.PLAN_WITH_TRIAL;
            SubscriptionRequest request1 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = trialPlan.Id,
                Price = 8M
            };

            SubscriptionRequest request2 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = triallessPlan.Id,
                Price = 8M
            };

            Subscription trialSubscription = gateway.Subscription.Create(request1).Target;
            Subscription triallessSubscription = gateway.Subscription.Create(request2).Target;

            SubscriptionSearchRequest request = new SubscriptionSearchRequest().
                PlanId.StartsWith("integration_trial_p").
                Price.Is(8M);

            ResourceCollection<Subscription> collection = gateway.Subscription.Search(request);

            Assert.IsTrue(TestHelper.IncludesSubscription(collection, trialSubscription));
            Assert.IsFalse(TestHelper.IncludesSubscription(collection, triallessSubscription));
        }

        [Test]
        public void Search_OnPlanIdEndsWith()
        {
            Plan triallessPlan = Plan.PLAN_WITHOUT_TRIAL;
            Plan trialPlan = Plan.PLAN_WITH_TRIAL;
            SubscriptionRequest request1 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = trialPlan.Id,
                Price = 9M
            };

            SubscriptionRequest request2 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = triallessPlan.Id,
                Price = 9M
            };

            Subscription trialSubscription = gateway.Subscription.Create(request1).Target;
            Subscription triallessSubscription = gateway.Subscription.Create(request2).Target;

            SubscriptionSearchRequest request = new SubscriptionSearchRequest().
                PlanId.EndsWith("trial_plan").
                Price.Is(9M);

            ResourceCollection<Subscription> collection = gateway.Subscription.Search(request);

            Assert.IsTrue(TestHelper.IncludesSubscription(collection, trialSubscription));
            Assert.IsFalse(TestHelper.IncludesSubscription(collection, triallessSubscription));
        }

        [Test]
        public void Search_OnPlanIdContains()
        {
            Plan triallessPlan = Plan.PLAN_WITHOUT_TRIAL;
            Plan trialPlan = Plan.PLAN_WITH_TRIAL;
            SubscriptionRequest request1 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = trialPlan.Id,
                Price = 10M
            };

            SubscriptionRequest request2 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = triallessPlan.Id,
                Price = 10M
            };

            Subscription trialSubscription = gateway.Subscription.Create(request1).Target;
            Subscription triallessSubscription = gateway.Subscription.Create(request2).Target;

            SubscriptionSearchRequest request = new SubscriptionSearchRequest().
                PlanId.Contains("ion_trial_pl").
                Price.Is(10M);

            ResourceCollection<Subscription> collection = gateway.Subscription.Search(request);

            Assert.IsTrue(TestHelper.IncludesSubscription(collection, trialSubscription));
            Assert.IsFalse(TestHelper.IncludesSubscription(collection, triallessSubscription));
        }

        [Test]
        public void Search_OnPlanIdIncludedIn()
        {
            SubscriptionRequest request1 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.PLAN_WITH_TRIAL.Id,
                Price = 5M
            };

            SubscriptionRequest request2 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.PLAN_WITHOUT_TRIAL.Id,
                Price = 5M
            };

            SubscriptionRequest request3 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.ADD_ON_DISCOUNT_PLAN.Id,
                Price = 5M
            };

            Subscription subscription1 = gateway.Subscription.Create(request1).Target;
            Subscription subscription2 = gateway.Subscription.Create(request2).Target;
            Subscription subscription3 = gateway.Subscription.Create(request3).Target;

            SubscriptionSearchRequest request = new SubscriptionSearchRequest().
                PlanId.IncludedIn(Plan.ADD_ON_DISCOUNT_PLAN.Id, Plan.PLAN_WITH_TRIAL.Id).
                Price.Is(5M);

            ResourceCollection<Subscription> collection = gateway.Subscription.Search(request);

            Assert.IsTrue(TestHelper.IncludesSubscription(collection, subscription1));
            Assert.IsFalse(TestHelper.IncludesSubscription(collection, subscription2));
            Assert.IsTrue(TestHelper.IncludesSubscription(collection, subscription3));
        }

        [Test]
        public void Search_OnStatusIn()
        {
            Plan triallessPlan = Plan.PLAN_WITHOUT_TRIAL;

            SubscriptionRequest request1 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = triallessPlan.Id,
                Price = 11M
            };

            SubscriptionRequest request2 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = triallessPlan.Id,
                Price = 11M
            };

            Subscription activeSubscription = gateway.Subscription.Create(request1).Target;
            Subscription canceledSubscription = gateway.Subscription.Create(request2).Target;
            gateway.Subscription.Cancel(canceledSubscription.Id);

            ResourceCollection<Subscription> collection = gateway.Subscription.Search(delegate(SubscriptionSearchRequest search) {
                search.Status.IncludedIn(SubscriptionStatus.ACTIVE);
                search.Price.Is(11M);
            });

            Assert.IsTrue(TestHelper.IncludesSubscription(collection, activeSubscription));
            Assert.IsFalse(TestHelper.IncludesSubscription(collection, canceledSubscription));
        }

        [Test]
        public void Search_OnStatusExpired()
        {
            ResourceCollection<Subscription> collection = gateway.Subscription.Search(delegate(SubscriptionSearchRequest search) {
                search.Status.IncludedIn(SubscriptionStatus.EXPIRED);
            });

            Assert.IsTrue(collection.MaximumCount > 0);
            foreach(Subscription subscription in collection) {
                Assert.AreEqual(SubscriptionStatus.EXPIRED, subscription.Status);
            }
        }

        [Test]
        public void Search_OnStatusInMultipleValues()
        {
            Plan triallessPlan = Plan.PLAN_WITHOUT_TRIAL;

            SubscriptionRequest request1 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = triallessPlan.Id,
                Price = 12M
            };

            SubscriptionRequest request2 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = triallessPlan.Id,
                Price = 12M
            };

            Subscription activeSubscription = gateway.Subscription.Create(request1).Target;
            Subscription canceledSubscription = gateway.Subscription.Create(request2).Target;
            gateway.Subscription.Cancel(canceledSubscription.Id);

            ResourceCollection<Subscription> collection = gateway.Subscription.Search(delegate(SubscriptionSearchRequest search) {
                search.Status.IncludedIn(SubscriptionStatus.ACTIVE, SubscriptionStatus.CANCELED);
                search.Price.Is(12M);
            });

            Assert.IsTrue(TestHelper.IncludesSubscription(collection, activeSubscription));
            Assert.IsTrue(TestHelper.IncludesSubscription(collection, canceledSubscription));
        }

        [Test]
        public void Update_Id()
        {
            String oldId = "old-id-" + new Random().Next(1000000);
            Plan plan = Plan.PLAN_WITHOUT_TRIAL;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id,
                Id = oldId
            };

            gateway.Subscription.Create(request);

            String newId = "new-id-" + new Random().Next(1000000);
            SubscriptionRequest updateRequest = new SubscriptionRequest
            {
                Id = newId
            };
            Result<Subscription> result = gateway.Subscription.Update(oldId, updateRequest);

            Assert.IsTrue(result.IsSuccess());
            Subscription updatedSubscription = result.Target;

            Assert.AreEqual(newId, updatedSubscription.Id);
            Assert.IsNotNull(gateway.Subscription.Find(newId));
        }

        [Test]
        public void UpdatePlan()
        {
            Plan originalPlan = Plan.PLAN_WITHOUT_TRIAL;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = originalPlan.Id,
            };

            Subscription subscription = gateway.Subscription.Create(request).Target;

            Plan newPlan = Plan.PLAN_WITH_TRIAL;
            SubscriptionRequest updateRequest = new SubscriptionRequest { PlanId = newPlan.Id };
            Result<Subscription> result = gateway.Subscription.Update(subscription.Id, updateRequest);

            Assert.IsTrue(result.IsSuccess());
            subscription = result.Target;

            Assert.AreEqual(newPlan.Id, subscription.PlanId);
        }

        [Test]
        public void Update_PaymentMethodToken()
        {
            Subscription subscription = gateway.Subscription.Create(new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.PLAN_WITHOUT_TRIAL.Id,
            }).Target;

            CreditCard newCreditCard = gateway.CreditCard.Create(new CreditCardRequest
            {
                CustomerId = creditCard.CustomerId,
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                CVV = "123",
                CardholderName = creditCard.CardholderName

            }).Target;

            SubscriptionRequest updateRequest = new SubscriptionRequest { PaymentMethodToken = newCreditCard.Token };
            Result<Subscription> result = gateway.Subscription.Update(subscription.Id, updateRequest);

            Assert.IsTrue(result.IsSuccess());
            subscription = result.Target;

            Assert.AreEqual(newCreditCard.Token, subscription.PaymentMethodToken);
        }

        [Test]
        public void UpdateMerchantAccountId()
        {
            Plan originalPlan = Plan.PLAN_WITHOUT_TRIAL;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = originalPlan.Id,
            };

            Subscription subscription = gateway.Subscription.Create(request).Target;

            SubscriptionRequest updateRequest = new SubscriptionRequest {
                MerchantAccountId = MerchantAccount.NON_DEFAULT_MERCHANT_ACCOUNT_ID
            };
            Result<Subscription> result = gateway.Subscription.Update(subscription.Id, updateRequest);

            Assert.IsTrue(result.IsSuccess());
            subscription = result.Target;

            Assert.AreEqual(MerchantAccount.NON_DEFAULT_MERCHANT_ACCOUNT_ID, subscription.MerchantAccountId);
        }

        [Test]
        public void Update_UpdatesAddOnsAndDiscounts()
        {
            Plan plan = Plan.ADD_ON_DISCOUNT_PLAN;
            SubscriptionRequest createRequest = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id
            };

            Subscription subscription = gateway.Subscription.Create(createRequest).Target;

            SubscriptionRequest request = new SubscriptionRequest
            {
                AddOns = new AddOnsRequest
                {
                    Add = new AddAddOnRequest[] {
                        new AddAddOnRequest{
                            InheritedFromId = "increase_30",
                            Amount = 31M,
                            NumberOfBillingCycles = 3,
                            Quantity = 7
                        }
                    },
                    Remove = new String[] { "increase_20" },
                    Update = new UpdateAddOnRequest[] {
                        new UpdateAddOnRequest
                        {
                            ExistingId = "increase_10",
                            Amount = 30M,
                            NeverExpires = true,
                            Quantity = 9
                        }
                    }
                },
                Discounts = new DiscountsRequest
                {
                    Add = new AddDiscountRequest[] {
                        new AddDiscountRequest{
                            InheritedFromId = "discount_15",
                            Amount = 23M,
                            NumberOfBillingCycles = 2,
                            Quantity = 9
                        }
                    },
                    Remove = new String[] { "discount_11" },
                    Update = new UpdateDiscountRequest[] {
                        new UpdateDiscountRequest
                        {
                            ExistingId = "discount_7",
                            Amount = 15M,
                        }
                    }
                }
            };

            Result<Subscription> result = gateway.Subscription.Update(subscription.Id, request);
            Assert.IsTrue(result.IsSuccess());
            subscription = result.Target;

            List<AddOn> addOns = subscription.AddOns;
            addOns.Sort(TestHelper.CompareModificationsById);

            Assert.AreEqual(2, addOns.Count);

            Assert.AreEqual("increase_10", addOns[0].Id);
            Assert.AreEqual(30M, addOns[0].Amount);
            Assert.AreEqual(9, addOns[0].Quantity);
            Assert.IsTrue(addOns[0].NeverExpires.Value);
            Assert.IsNull(addOns[0].NumberOfBillingCycles);

            Assert.AreEqual("increase_30", addOns[1].Id);
            Assert.AreEqual(31M, addOns[1].Amount);
            Assert.AreEqual(7, addOns[1].Quantity);
            Assert.IsFalse(addOns[1].NeverExpires.Value);
            Assert.AreEqual(3, addOns[1].NumberOfBillingCycles);

            List<Discount> discounts = subscription.Discounts;
            discounts.Sort(TestHelper.CompareModificationsById);

            Assert.AreEqual(2, discounts.Count);

            Assert.AreEqual("discount_15", discounts[0].Id);
            Assert.AreEqual(23M, discounts[0].Amount);
            Assert.AreEqual(9, discounts[0].Quantity);
            Assert.IsFalse(discounts[0].NeverExpires.Value);
            Assert.AreEqual(2, discounts[0].NumberOfBillingCycles);

            Assert.AreEqual("discount_7", discounts[1].Id);
            Assert.AreEqual(15M, discounts[1].Amount);
            Assert.AreEqual(1, discounts[1].Quantity);
            Assert.IsTrue(discounts[1].NeverExpires.Value);
            Assert.IsNull(discounts[1].NumberOfBillingCycles);
        }

        [Test]
        public void Update_CanReplaceAllAddOnsAndDiscounts()
        {
            Plan plan = Plan.ADD_ON_DISCOUNT_PLAN;
            SubscriptionRequest createRequest = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id
            };

            Subscription subscription = gateway.Subscription.Create(createRequest).Target;

            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id,
                AddOns = new AddOnsRequest
                {
                    Add = new AddAddOnRequest[]
                    {
                        new AddAddOnRequest
                        {
                            InheritedFromId = "increase_30"
                        }
                    },
                },
                Discounts = new DiscountsRequest
                {
                    Add = new AddDiscountRequest[]
                    {
                        new AddDiscountRequest
                        {
                            InheritedFromId = "discount_15",
                        }
                    },
                },
                Options = new SubscriptionOptionsRequest
                {
                    ReplaceAllAddOnsAndDiscounts = true
                }
            };

            Result<Subscription> result = gateway.Subscription.Update(subscription.Id, request);
            Assert.IsTrue(result.IsSuccess());
            subscription = result.Target;

            Assert.AreEqual(1, subscription.AddOns.Count);
            Assert.AreEqual(1, subscription.Discounts.Count);
        }

        [Test]
        public void IncreasePriceAndTransaction()
        {
            Plan originalPlan = Plan.PLAN_WITHOUT_TRIAL;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = originalPlan.Id,
                Price = 1.23M
            };

            Subscription subscription = gateway.Subscription.Create(request).Target;

            SubscriptionRequest updateRequest = new SubscriptionRequest { Price = 4.56M };
            Result<Subscription> result = gateway.Subscription.Update(subscription.Id, updateRequest);

            Assert.IsTrue(result.IsSuccess());
            subscription = result.Target;

            Assert.AreEqual(4.56M, subscription.Price);
            Assert.AreEqual(2, subscription.Transactions.Count);
        }

        [Test]
        public void Update_ProratesChargesWhenSpecified()
        {
            Plan originalPlan = Plan.PLAN_WITHOUT_TRIAL;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = originalPlan.Id,
                Price = 1.23M
            };

            Subscription subscription = gateway.Subscription.Create(request).Target;

            SubscriptionRequest updateRequest = new SubscriptionRequest
            {
                Price = 4.56M,
                Options = new SubscriptionOptionsRequest
                {
                    ProrateCharges = true
                }
            };

            Result<Subscription> result = gateway.Subscription.Update(subscription.Id, updateRequest);

            Assert.IsTrue(result.IsSuccess());
            subscription = result.Target;

            Assert.AreEqual(4.56M, subscription.Price);
            Assert.AreEqual(2, subscription.Transactions.Count);
        }

        [Test]
        public void Update_DoesNotProrateChargesWhenSpecifiedToNotProrate()
        {
            Plan originalPlan = Plan.PLAN_WITHOUT_TRIAL;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = originalPlan.Id,
                Price = 1.23M
            };

            Subscription subscription = gateway.Subscription.Create(request).Target;

            SubscriptionRequest updateRequest = new SubscriptionRequest
            {
                Price = 4.56M,
                Options = new SubscriptionOptionsRequest
                {
                    ProrateCharges = false
                }
            };

            Result<Subscription> result = gateway.Subscription.Update(subscription.Id, updateRequest);

            Assert.IsTrue(result.IsSuccess());
            subscription = result.Target;

            Assert.AreEqual(4.56M, subscription.Price);
            Assert.AreEqual(1, subscription.Transactions.Count);
        }

        [Test]
        public void Update_DontIncreasePriceAndDontAddTransaction()
        {
            Plan originalPlan = Plan.PLAN_WITHOUT_TRIAL;
            SubscriptionRequest createRequest = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = originalPlan.Id
            };

            Subscription subscription = gateway.Subscription.Create(createRequest).Target;

            SubscriptionRequest updateRequest = new SubscriptionRequest { Price = 4.56M };
            Result<Subscription> result = gateway.Subscription.Update(subscription.Id, updateRequest);

            Assert.IsTrue(result.IsSuccess());
            subscription = result.Target;

            Assert.AreEqual(4.56M, subscription.Price);
            Assert.AreEqual(1, subscription.Transactions.Count);
        }


        [Test]
        public void Create_WithBadPlanId()
        {
            SubscriptionRequest createRequest = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = "noSuchPlanId"
            };

            Result<Subscription> result = gateway.Subscription.Create(createRequest);

            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.SUBSCRIPTION_PLAN_ID_IS_INVALID, result.Errors.ForObject("Subscription").OnField("PlanId")[0].Code);
        }

        [Test]
        public void Create_WithBadPaymentMethod()
        {
            SubscriptionRequest createRequest = new SubscriptionRequest
            {
                PaymentMethodToken = "invalidToken",
                PlanId = Plan.PLAN_WITHOUT_TRIAL.Id
            };

            Result<Subscription> result = gateway.Subscription.Create(createRequest);

            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.SUBSCRIPTION_PAYMENT_METHOD_TOKEN_IS_INVALID, result.Errors.ForObject("Subscription").OnField("PaymentMethodToken")[0].Code);
        }

        [Test]
        public void Create_WithValidationErrors()
        {
            Plan plan = Plan.PLAN_WITHOUT_TRIAL;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id,
                Id = "invalid id"
            };

            Result<Subscription> createResult = gateway.Subscription.Create(request);
            Assert.IsFalse(createResult.IsSuccess());
            Assert.IsNull(createResult.Target);
            ValidationErrors errors = createResult.Errors;
            Assert.AreEqual(ValidationErrorCode.SUBSCRIPTION_TOKEN_FORMAT_IS_INVALID, errors.ForObject("Subscription").OnField("Id")[0].Code);
        }

        [Test]
        public void Update_WithValidationErrors() {
            Plan plan = Plan.PLAN_WITHOUT_TRIAL;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id
            };

            Result<Subscription> createResult = gateway.Subscription.Create(request);
            Assert.IsTrue(createResult.IsSuccess());
            Subscription createdSubscription = createResult.Target;

            SubscriptionRequest updateRequest = new SubscriptionRequest { Id = "invalid id" };
            Result<Subscription> result = gateway.Subscription.Update(createdSubscription.Id, updateRequest);

            Assert.IsFalse(result.IsSuccess());
            Assert.IsNull(result.Target);
            ValidationErrors errors = result.Errors;
            Assert.AreEqual(ValidationErrorCode.SUBSCRIPTION_TOKEN_FORMAT_IS_INVALID, errors.ForObject("Subscription").OnField("Id")[0].Code);
        }

        [Test]
        public void Create_GetParamsOnError()
        {
            Plan plan = Plan.PLAN_WITHOUT_TRIAL;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id,
                Id = "invalid id"
            };

            Result<Subscription> createResult = gateway.Subscription.Create(request);
            Assert.IsFalse(createResult.IsSuccess());
            Assert.IsNull(createResult.Target);

            Dictionary<String, String> parameters = createResult.Parameters;
            Assert.AreEqual(creditCard.Token, parameters["payment_method_token"]);
            Assert.AreEqual(plan.Id, parameters["plan_id"]);
            Assert.AreEqual("invalid id", parameters["id"]);
        }

        [Test]
        public void Cancel()
        {
            Plan plan = Plan.PLAN_WITHOUT_TRIAL;
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id,
            };

            Result<Subscription> createResult = gateway.Subscription.Create(request);
            Result<Subscription> cancelResult = gateway.Subscription.Cancel(createResult.Target.Id);

            Assert.IsTrue(cancelResult.IsSuccess());
            Assert.AreEqual(SubscriptionStatus.CANCELED, cancelResult.Target.Status);
            Assert.AreEqual(SubscriptionStatus.CANCELED, gateway.Subscription.Find(createResult.Target.Id).Status);
        }

        [Test]
        public void RetryCharge_WithoutAmount()
        {
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.PLAN_WITHOUT_TRIAL.Id
            };

            Subscription subscription = gateway.Subscription.Create(request).Target;
            MakePastDue(subscription, 1);

            Result<Transaction> result = gateway.Subscription.RetryCharge(subscription.Id);

            Assert.IsTrue(result.IsSuccess());

            Transaction transaction = result.Target;
            Assert.AreEqual(subscription.Price, transaction.Amount);
            Assert.IsNotNull(transaction.ProcessorAuthorizationCode);
            Assert.AreEqual(TransactionType.SALE, transaction.Type);
            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.Status);
        }

        [Test]
        public void RetryCharge_WithAmount()
        {
            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = Plan.PLAN_WITHOUT_TRIAL.Id
            };

            Subscription subscription = gateway.Subscription.Create(request).Target;
            MakePastDue(subscription, 1);

            Result<Transaction> result = gateway.Subscription.RetryCharge(subscription.Id, SandboxValues.TransactionAmount.AUTHORIZE);

            Assert.IsTrue(result.IsSuccess());

            Transaction transaction = result.Target;
            Assert.AreEqual(SandboxValues.TransactionAmount.AUTHORIZE, transaction.Amount);
            Assert.IsNotNull(transaction.ProcessorAuthorizationCode);
            Assert.AreEqual(TransactionType.SALE, transaction.Type);
            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.Status);
        }

        private void MakePastDue(Subscription subscription, int numberOfDays)
        {
            BraintreeService service = new BraintreeService(gateway.Configuration);
            NodeWrapper response = new NodeWrapper(service.Put("/subscriptions/" + subscription.Id + "/make_past_due?days_past_due=" + numberOfDays));
            Assert.IsTrue(response.IsSuccess());
        }
    }
}
