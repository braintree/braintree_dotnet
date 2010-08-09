using System;
using System.Collections.Generic;
using System.Text;
using Braintree;

namespace Braintree.Tests
{
    public class Plan
    {
        public Int32 BillingDayOfMonth { get; set; }
        public Int32 BillingFrequency { get; set; }
        public String Description { get; set; }
        public String Id { get; set; }
        public Int32 NumberOfBillingCycles { get; set; }
        public Decimal Price { get; set; }
        public Boolean TrialPeriod { get; set; }
        public Int32 TrialDuration { get; set; }
        public SubscriptionDurationUnit TrialDurationUnit { get; set; }

        public static Plan ADD_ON_DISCOUNT_PLAN = new Plan
        {
            Description = "Plan for integration tests -- with add-ons and discounts",
            Id = "integration_plan_with_add_ons_and_discounts",
            NumberOfBillingCycles = 12,
            Price = 9.99M,
            BillingFrequency = 1,
            TrialPeriod = true,
            TrialDuration = 2,
            TrialDurationUnit = SubscriptionDurationUnit.DAY
        };

        public static Plan BILLING_DAY_OF_MONTH_PLAN = new Plan
        {
            Description = "Plan for integration tests -- with billing day of month",
            Id = "integration_plan_with_billing_day_of_month",
            NumberOfBillingCycles = 12,
            Price = 8.88M,
            BillingFrequency = 1,
            TrialPeriod = false,
            BillingDayOfMonth = 5
        };
    
        public static Plan PLAN_WITHOUT_TRIAL = new Plan
        {
            Description = "Plan for integration tests -- without a trial",
            Id = "integration_trialless_plan",
            NumberOfBillingCycles = 12,
            Price = 12.34M,
            BillingFrequency = 1,
            TrialPeriod = false
        };
    
        public static Plan PLAN_WITH_TRIAL = new Plan
            {
            Description = "Plan for integration tests -- with a trial",
            Id = "integration_trial_plan",
            NumberOfBillingCycles = 12,
            Price = 43.21M,
            BillingFrequency = 1,
            TrialPeriod = true,
            TrialDuration = 2,
            TrialDurationUnit = SubscriptionDurationUnit.DAY
        };

        private Plan() { }
    }
}
