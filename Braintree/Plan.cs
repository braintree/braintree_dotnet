using System;
using System.Collections.Generic;

namespace Braintree
{
    public class PlanDurationUnit : Enumeration
    {
        public static readonly PlanDurationUnit DAY = new PlanDurationUnit("day");
        public static readonly PlanDurationUnit MONTH = new PlanDurationUnit("month");
        public static readonly PlanDurationUnit UNRECOGNIZED = new PlanDurationUnit("unrecognized");
        public static readonly PlanDurationUnit[] ALL = { DAY, MONTH };
        protected PlanDurationUnit(String name) : base(name) {}
    }

    public class Plan
    {
        public List<AddOn> AddOns { get; protected set; }
        public Int32? BillingDayOfMonth { get; protected set; }
        public Int32? BillingFrequency { get; protected set; }
        public String CurrencyIsoCode { get; protected set; }
        public String Description { get; protected set; }
        public List<Discount> Discounts { get; protected set; }
        public String Id { get; protected set; }
        public String Name { get; protected set; }
        public Int32? NumberOfBillingCycles { get; protected set; }
        public Decimal? Price { get; protected set; }
        public Boolean? TrialPeriod { get; protected set; }
        public Int32? TrialDuration { get; protected set; }
        public PlanDurationUnit TrialDurationUnit { get; protected set; }

        public Plan(NodeWrapper node)
        {
            if (node == null) return;
            BillingDayOfMonth = node.GetInteger("billing-day-of-month");
            BillingFrequency = node.GetInteger("billing-frequency");
            CurrencyIsoCode = node.GetString("currency-iso-code");
            Description = node.GetString("description");
            Id = node.GetString("id");
            Name = node.GetString("name");
            NumberOfBillingCycles = node.GetInteger("number-of-billing-cycles");
            Price = node.GetDecimal("price");
            TrialPeriod = node.GetBoolean("trial-period");
            TrialDuration = node.GetInteger("trial-duration");
            String trialDurationUnitStr = node.GetString("trial-duration-unit");
            if (trialDurationUnitStr != null) {
                TrialDurationUnit = (PlanDurationUnit) CollectionUtil.Find(PlanDurationUnit.ALL, trialDurationUnitStr, PlanDurationUnit.UNRECOGNIZED);
            }
            AddOns = new List<AddOn> ();
            foreach (NodeWrapper addOnResponse in node.GetList("add-ons/modification")) {
                AddOns.Add(new AddOn(addOnResponse));
            }
            Discounts = new List<Discount> ();
            foreach (NodeWrapper discountResponse in node.GetList("discounts/modification")) {
                Discounts.Add(new Discount(discountResponse));
            }
        }
    }
}

