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

        private int CompareModificationsById(Modification left, Modification right)
        {
            return left.Id.CompareTo(right.Id);
        }

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
            Assert.IsTrue(Regex.IsMatch(subscription.Id, "^\\w{6}$"));
            Assert.AreEqual(SubscriptionStatus.ACTIVE, subscription.Status);
            Assert.AreEqual(0, subscription.FailureCount);
            Assert.IsFalse((Boolean)subscription.HasTrialPeriod);

            Assert.IsTrue(subscription.BillingPeriodEndDate.HasValue);
            Assert.IsTrue(subscription.BillingPeriodStartDate.HasValue);
            Assert.IsTrue(subscription.NextBillingDate.HasValue);
            Assert.IsTrue(subscription.FirstBillingDate.HasValue);
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
            addOns.Sort(CompareModificationsById);

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
            discounts.Sort(CompareModificationsById);

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
        public void Find()
        {
            Plan plan = Plan.PLAN_WITHOUT_TRIAL;
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
        public void Search_OnPlanIdIs()
        {
            Plan triallessPlan = Plan.PLAN_WITHOUT_TRIAL;
            Plan trialPlan = Plan.PLAN_WITH_TRIAL;
            SubscriptionRequest request1 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = trialPlan.Id
            };

            SubscriptionRequest request2 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = triallessPlan.Id
            };

            Subscription trialSubscription = gateway.Subscription.Create(request1).Target;
            Subscription triallessSubscription = gateway.Subscription.Create(request2).Target;

            SubscriptionSearchRequest request = new SubscriptionSearchRequest().
                PlanId.Is(trialPlan.Id);

            ResourceCollection<Subscription> collection = gateway.Subscription.Search(request);

            Assert.IsTrue(TestHelper.IncludesSubscription(collection, trialSubscription));
            Assert.IsFalse(TestHelper.IncludesSubscription(collection, triallessSubscription));
        }

        [Test]
        public void Search_OnPlanIdIsWithDelegate()
        {
            Plan triallessPlan = Plan.PLAN_WITHOUT_TRIAL;
            Plan trialPlan = Plan.PLAN_WITH_TRIAL;
            SubscriptionRequest request1 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = trialPlan.Id
            };

            SubscriptionRequest request2 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = triallessPlan.Id
            };

            Subscription trialSubscription = gateway.Subscription.Create(request1).Target;
            Subscription triallessSubscription = gateway.Subscription.Create(request2).Target;

            ResourceCollection<Subscription> collection = gateway.Subscription.Search(delegate(SubscriptionSearchRequest search) {
                search.PlanId.Is(trialPlan.Id);
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
                PlanId = trialPlan.Id
            };

            SubscriptionRequest request2 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = triallessPlan.Id
            };

            Subscription trialSubscription = gateway.Subscription.Create(request1).Target;
            Subscription triallessSubscription = gateway.Subscription.Create(request2).Target;

            SubscriptionSearchRequest request = new SubscriptionSearchRequest().
                PlanId.IsNot(triallessPlan.Id);

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
                PlanId = trialPlan.Id
            };

            SubscriptionRequest request2 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = triallessPlan.Id
            };

            Subscription trialSubscription = gateway.Subscription.Create(request1).Target;
            Subscription triallessSubscription = gateway.Subscription.Create(request2).Target;

            SubscriptionSearchRequest request = new SubscriptionSearchRequest().
                PlanId.StartsWith("integration_trial_p");

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
                PlanId = trialPlan.Id
            };

            SubscriptionRequest request2 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = triallessPlan.Id
            };

            Subscription trialSubscription = gateway.Subscription.Create(request1).Target;
            Subscription triallessSubscription = gateway.Subscription.Create(request2).Target;

            SubscriptionSearchRequest request = new SubscriptionSearchRequest().
                PlanId.EndsWith("trial_plan");

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
                PlanId = trialPlan.Id
            };

            SubscriptionRequest request2 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = triallessPlan.Id
            };

            Subscription trialSubscription = gateway.Subscription.Create(request1).Target;
            Subscription triallessSubscription = gateway.Subscription.Create(request2).Target;

            SubscriptionSearchRequest request = new SubscriptionSearchRequest().
                PlanId.Contains("ion_trial_pl");

            ResourceCollection<Subscription> collection = gateway.Subscription.Search(request);

            Assert.IsTrue(TestHelper.IncludesSubscription(collection, trialSubscription));
            Assert.IsFalse(TestHelper.IncludesSubscription(collection, triallessSubscription));
        }

        [Test]
        public void Search_OnStatusIn()
        {
            Plan triallessPlan = Plan.PLAN_WITHOUT_TRIAL;

            SubscriptionRequest request1 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = triallessPlan.Id
            };

            SubscriptionRequest request2 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = triallessPlan.Id
            };

            Subscription activeSubscription = gateway.Subscription.Create(request1).Target;
            Subscription canceledSubscription = gateway.Subscription.Create(request2).Target;
            gateway.Subscription.Cancel(canceledSubscription.Id);

            ResourceCollection<Subscription> collection = gateway.Subscription.Search(delegate(SubscriptionSearchRequest search) {
                search.Status.IncludedIn(SubscriptionStatus.ACTIVE);
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
                PlanId = triallessPlan.Id
            };

            SubscriptionRequest request2 = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = triallessPlan.Id
            };

            Subscription activeSubscription = gateway.Subscription.Create(request1).Target;
            Subscription canceledSubscription = gateway.Subscription.Create(request2).Target;
            gateway.Subscription.Cancel(canceledSubscription.Id);

            ResourceCollection<Subscription> collection = gateway.Subscription.Search(delegate(SubscriptionSearchRequest search) {
                search.Status.IncludedIn(SubscriptionStatus.ACTIVE, SubscriptionStatus.CANCELED);
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
            Assert.AreEqual(ValidationErrorCode.SUBSCRIPTION_PLAN_ID_IS_INVALID, result.Errors.ForObject("subscription").OnField("plan_id")[0].Code);
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
            Assert.AreEqual(ValidationErrorCode.SUBSCRIPTION_PAYMENT_METHOD_TOKEN_IS_INVALID, result.Errors.ForObject("subscription").OnField("payment_method_token")[0].Code);
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
            Assert.AreEqual(ValidationErrorCode.SUBSCRIPTION_TOKEN_FORMAT_IS_INVALID, errors.ForObject("subscription").OnField("id")[0].Code);
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
            Assert.AreEqual(ValidationErrorCode.SUBSCRIPTION_TOKEN_FORMAT_IS_INVALID, errors.ForObject("subscription").OnField("id")[0].Code);
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
            SubscriptionSearchRequest search = new SubscriptionSearchRequest().Status.IncludedIn(SubscriptionStatus.PAST_DUE);
            Subscription subscription = gateway.Subscription.Search(search).FirstItem;

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
            SubscriptionSearchRequest search = new SubscriptionSearchRequest().Status.IncludedIn(SubscriptionStatus.PAST_DUE);
            Subscription subscription = gateway.Subscription.Search(search).FirstItem;

            Result<Transaction> result = gateway.Subscription.RetryCharge(subscription.Id, SandboxValues.TransactionAmount.AUTHORIZE);

            Assert.IsTrue(result.IsSuccess());

            Transaction transaction = result.Target;
            Assert.AreEqual(SandboxValues.TransactionAmount.AUTHORIZE, transaction.Amount);
            Assert.IsNotNull(transaction.ProcessorAuthorizationCode);
            Assert.AreEqual(TransactionType.SALE, transaction.Type);
            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.Status);
        }
    }
}
