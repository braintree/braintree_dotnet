using System;
using System.Collections.Generic;
using System.Text;
using Braintree;

namespace Braintree.Tests
{
    public class Plan
    {
        public Int32 BillingFrequency { get; set; }
        public String Description { get; set; }
        public String Id { get; set; }
        public Int32 NumberOfBillingCycles { get; set; }
        public Decimal Price { get; set; }
        public Boolean TrialPeriod { get; set; }
        public Int32 TrialDuration { get; set; }
        public SubscriptionDurationUnit TrialDurationUnit { get; set; }
    
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
