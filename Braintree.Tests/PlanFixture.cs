using System;
using System.Collections.Generic;
using System.Text;
using Braintree;

namespace Braintree.Tests
{
    public class TestPlan : Plan
    {
        new public List<AddOn> AddOns { get; set; }
        new public Int32? BillingDayOfMonth { get; set; }
        new public Int32? BillingFrequency { get; set; }
        new public string CurrencyIsoCode { get; set; }
        new public string Description { get; set; }
        new public List<Discount> Discounts { get; set; }
        new public string Id { get; set; }
        new public string Name { get; set; }
        new public Int32? NumberOfBillingCycles { get; set; }
        new public Decimal? Price { get; set; }
        new public bool? TrialPeriod { get; set; }
        new public Int32? TrialDuration { get; set; }
        new public PlanDurationUnit TrialDurationUnit { get; set; }

        internal TestPlan(NodeWrapper node) : base(node) {
        }
    }


    public class PlanFixture
    {
        public static TestPlan ADD_ON_DISCOUNT_PLAN = new TestPlan(null)
        {
            Description = "Plan for integration tests -- with add-ons and discounts",
            Id = "integration_plan_with_add_ons_and_discounts",
            NumberOfBillingCycles = 12,
            Price = 9.99M,
            BillingFrequency = 1,
            TrialPeriod = true,
            TrialDuration = 2,
            TrialDurationUnit = PlanDurationUnit.DAY
        };

        public static TestPlan BILLING_DAY_OF_MONTH_PLAN = new TestPlan(null)
        {
            Description = "Plan for integration tests -- with billing day of month",
            Id = "integration_plan_with_billing_day_of_month",
            NumberOfBillingCycles = 12,
            Price = 8.88M,
            BillingFrequency = 1,
            TrialPeriod = false,
            BillingDayOfMonth = 5
        };
    
        public static TestPlan PLAN_WITHOUT_TRIAL = new TestPlan(null)
        {
            Description = "Plan for integration tests -- without a trial",
            Id = "integration_trialless_plan",
            NumberOfBillingCycles = 12,
            Price = 12.34M,
            BillingFrequency = 1,
            TrialPeriod = false
        };
    
        public static TestPlan PLAN_WITH_TRIAL = new TestPlan(null)
        {
            Description = "Plan for integration tests -- with a trial",
            Id = "integration_trial_plan",
            NumberOfBillingCycles = 12,
            Price = 43.21M,
            BillingFrequency = 1,
            TrialPeriod = true,
            TrialDuration = 2,
            TrialDurationUnit = PlanDurationUnit.DAY
        };
    }
}
