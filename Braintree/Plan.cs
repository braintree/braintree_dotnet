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
        protected PlanDurationUnit(string name) : base(name) {}
    }

    public class Plan
    {
        public virtual List<AddOn> AddOns { get; protected set; }
        public virtual int? BillingDayOfMonth { get; protected set; }
        public virtual int? BillingFrequency { get; protected set; }
        public virtual string CurrencyIsoCode { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual List<Discount> Discounts { get; protected set; }
        public virtual string Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual int? NumberOfBillingCycles { get; protected set; }
        public virtual decimal? Price { get; protected set; }
        public virtual bool? TrialPeriod { get; protected set; }
        public virtual int? TrialDuration { get; protected set; }
        public virtual PlanDurationUnit TrialDurationUnit { get; protected set; }

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
            string trialDurationUnitStr = node.GetString("trial-duration-unit");
            if (trialDurationUnitStr != null) {
                TrialDurationUnit = (PlanDurationUnit) CollectionUtil.Find(PlanDurationUnit.ALL, trialDurationUnitStr, PlanDurationUnit.UNRECOGNIZED);
            }
            AddOns = new List<AddOn> ();
            foreach (var addOnResponse in node.GetList("add-ons/add-on")) {
                AddOns.Add(new AddOn(addOnResponse));
            }
            Discounts = new List<Discount> ();
            foreach (var discountResponse in node.GetList("discounts/discount")) {
                Discounts.Add(new Discount(discountResponse));
            }
        }

        [Obsolete("Mock Use Only")]
        protected internal Plan() { }
    }
}
