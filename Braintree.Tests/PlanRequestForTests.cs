using System;
using System.Collections.Generic;

namespace Braintree.Tests
{
    public class PlanRequestForTests : Request
    {
        public Int32? BillingDayOfMonth { get; set; }
        public Int32? BillingFrequency { get; set; }
        public String CurrencyIsoCode { get; set; }
        public String Description { get; set; }
        public String Id { get; set; }
        public String Name { get; set; }
        public Int32? NumberOfBillingCycles { get; set; }
        public Decimal? Price { get; set; }
        public Boolean? TrialPeriod { get; set; }
        public Int32? TrialDuration { get; set; }
        public PlanDurationUnit TrialDurationUnit { get; set; }
        public List<AddOn> AddOns { get; set; }
        public List<Discount> Discounts { get; set; }

        public override String ToXml()
        {
            return ToXml("plan");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        public virtual RequestBuilder BuildRequest(String root)
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
            builder.AddElement("trial-duration-unit", TrialDurationUnit);
            builder.AddElement("add-ons", AddOns);
            builder.AddElement("discounts", Discounts);
            return builder;
        }
    }
}

