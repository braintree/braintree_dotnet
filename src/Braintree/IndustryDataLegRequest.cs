#pragma warning disable 1591

using System;

namespace Braintree
{
    public class IndustryDataLegRequest : Request
    {
        public string ConjunctionTicket { get; set; }
        public string ExchangeTicket { get; set; }
        public string CouponNumber { get; set; }
        public string ServiceClass { get; set; }
        public string CarrierCode { get; set; }
        public string FareBasisCode { get; set; }
        public string FlightNumber { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string DepartureAirportCode { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalAirportCode { get; set; }
        public string ArrivalTime { get; set; }
        public bool? StopoverPermitted { get; set; }
        public decimal? FareAmount { get; set; }
        public decimal? FeeAmount { get; set; }
        public decimal? TaxAmount { get; set; }
        public string EndorsementOrRestrictions { get; set; }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            var builder = new RequestBuilder(root).
                AddElement("conjunction-ticket", ConjunctionTicket).
                AddElement("exchange-ticket", ExchangeTicket).
                AddElement("coupon-number", CouponNumber).
                AddElement("service-class", ServiceClass).
                AddElement("carrier-code", CarrierCode).
                AddElement("fare-basis-code", FareBasisCode).
                AddElement("flight-number", FlightNumber).
                AddElement("departure-airport-code", DepartureAirportCode).
                AddElement("departure-time", DepartureTime).
                AddElement("arrival-airport-code", ArrivalAirportCode).
                AddElement("arrival-time", ArrivalTime).
                AddElement("endorsement-or-restrictions", EndorsementOrRestrictions);

            if (DepartureDate != null)
                builder.AddElement("departure-date", DepartureDate);
            if (StopoverPermitted != null)
                builder.AddElement("stopover-permitted", StopoverPermitted);
            if (FareAmount != null)
                builder.AddElement("fare-amount", FareAmount);
            if (FeeAmount != null)
                builder.AddElement("fee-amount", FeeAmount);
            if (TaxAmount != null)
                builder.AddElement("tax-amount", TaxAmount);
            return builder;
        }
    }
}
