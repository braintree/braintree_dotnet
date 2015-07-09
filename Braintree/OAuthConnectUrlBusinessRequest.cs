using System;

namespace Braintree
{
    public class OAuthConnectUrlBusinessRequest : Request
    {
        public string Name { get; set; }
        public string RegisteredAs { get; set; }
        public string Industry { get; set; }
        public string Description { get; set; }
        public string StreetAddress { get; set; }
        public string Locality { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string AnnualVolumeAmount { get; set; }
        public string AverageTransactionAmount { get; set; }
        public string MaximumTransactionAmount { get; set; }
        public bool ShipPhysicalGoods { get; set; }
        public int FulfillmentCompletedIn { get; set; }
        public string Currency { get; set; }
        public string Website { get; set; }

        public override string ToQueryString(string root)
        {
            var builder = new RequestBuilder("business");  // ignore the root as it is wrong
            builder.AddElement("name", Name);
            builder.AddElement("registered_as", RegisteredAs);
            builder.AddElement("industry", Industry);
            builder.AddElement("description", Description);
            builder.AddElement("street_address", StreetAddress);
            builder.AddElement("locality", Locality);
            builder.AddElement("region", Region);
            builder.AddElement("postal_code", PostalCode);
            builder.AddElement("country", Country);
            builder.AddElement("annual_volume_amount", AnnualVolumeAmount);
            builder.AddElement("average_transaction_amount", AverageTransactionAmount);
            builder.AddElement("maximum_transaction_amount", MaximumTransactionAmount);
            builder.AddElement("ship_physical_goods", ShipPhysicalGoods);
            builder.AddElement("fulfillment_completed_in", FulfillmentCompletedIn);
            builder.AddElement("currency", Currency);
            builder.AddElement("website", Website);
            return builder.ToQueryString();
        }
    }
}
