using System.Collections.Generic;

namespace Braintree.Tests
{
    public class PlanRequestForTests : Request
    {   
        public int? BillingDayOfMonth { get; set; }
        public int? BillingFrequency { get; set; }
        public string CurrencyIsoCode { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int? NumberOfBillingCycles { get; set; }
        public decimal? Price { get; set; }
        public bool? TrialPeriod { get; set; }
        public int? TrialDuration { get; set; }
        public PlanDurationUnit? TrialDurationUnit { get; set; }
        public List<AddOn> AddOns { get; set; }
        public List<Discount> Discounts { get; set; }

        public override string ToXml()
        {
            return ToXml("plan");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public virtual RequestBuilder BuildRequest(string root)
        {
            var builder = new RequestBuilder(root);

            builder.AddElement("billing-day-of-month", BillingDayOfMonth);
            builder.AddElement("billing-frequency", BillingFrequency);
            builder.AddElement("currency-iso-code", CurrencyIsoCode);
            builder.AddElement("description", Description);
            builder.AddElement("id", Id);
            builder.AddElement("name", Name);
            builder.AddElement("number-of-billing-cycles", NumberOfBillingCycles);
            builder.AddElement("price", Price);
            builder.AddElement("trial-period", TrialPeriod);
            builder.AddElement("trial-duration", TrialDuration);
            if (TrialDurationUnit != null)
            {
                builder.AddElement("trial-duration-unit", TrialDurationUnit.GetDescription());
            }
            builder.AddElement("add-ons", AddOns);
            builder.AddElement("discounts", Discounts);
            return builder;
        }
    }
}

