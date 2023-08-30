#pragma warning disable 1591

using System;

namespace Braintree
{
    public class IndustryDataRequest : Request
    {
        public bool? AdvancedDeposit { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string CheckInDate { get; set; }
        public string CheckOutDate { get; set; }
        public string CustomerCode { get; set; }
        public string DepartureDate { get; set; }
        public decimal? FareAmount { get; set; }
        public decimal? FeeAmount { get; set; }
        public bool? FireSafe { get; set; }
        public string FolioNumber { get; set; }
        public DateTime? IssuedDate { get; set; }
        public string IssuingCarrierCode { get; set; }
        public string LodgingCheckInDate { get; set; }
        public string LodgingCheckOutDate { get; set; }
        public string LodgingName { get; set; }
        public bool? NoShow { get; set; }
        public string PassengerFirstName { get; set; }
        public string PassengerLastName { get; set; }
        public string PassengerMiddleInitial { get; set; }
        public string PassengerTitle { get; set; }
        public string PropertyPhone { get; set; }
        public bool? RestrictedTicket { get; set; }
        public Decimal RoomRate { get; set; }
        public decimal? RoomTax { get; set; }
        public decimal? TaxAmount { get; set; }
        public string TicketIssuerAddress { get; set; }
        public string TicketNumber { get; set; }
        public string TravelAgencyCode { get; set; }
        public string TravelAgencyName { get; set; }
        public string TravelPackage { get; set; }

        public IndustryDataLegRequest[] Legs { get; set; }
        public IndustryDataAdditionalChargeRequest[] AdditionalCharges { get; set; }

        public override string ToXml()
        {
            return ToXml("data");
        }

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
                AddElement("folio-number", FolioNumber).
                AddElement("check-in-date", CheckInDate).
                AddElement("check-out-date", CheckOutDate).
                AddElement("travel-package", TravelPackage).
                AddElement("departure-date", DepartureDate).
                AddElement("lodging-check-in-date", LodgingCheckInDate).
                AddElement("lodging-check-out-date", LodgingCheckOutDate).
                AddElement("lodging-name", LodgingName).
                AddElement("passenger-first-name", PassengerFirstName).
                AddElement("passenger-last-name", PassengerLastName).
                AddElement("passenger-middle-initial", PassengerMiddleInitial).
                AddElement("passenger-title", PassengerTitle).
                AddElement("travel-agency-name", TravelAgencyName).
                AddElement("travel-agency-code", TravelAgencyCode).
                AddElement("ticket-number", TicketNumber).
                AddElement("issuing-carrier-code", IssuingCarrierCode).
                AddElement("customer-code", CustomerCode).
                AddElement("property-phone", PropertyPhone);

            if (RoomRate != null)
                builder.AddElement("room-rate", RoomRate.ToString());
            if (RoomTax != null)
                builder.AddElement("room-tax", RoomTax.ToString());
            if (IssuedDate != null)
                builder.AddElement("issued-date", IssuedDate);
            if (FareAmount != null)
                builder.AddElement("fare-amount", FareAmount);
            if (FeeAmount != null)
                builder.AddElement("fee-amount", FeeAmount);
            if (TaxAmount != null)
                builder.AddElement("tax-amount", TaxAmount);
            if (RestrictedTicket != null)
                builder.AddElement("restricted-ticket", RestrictedTicket);
            if (NoShow != null)
                builder.AddElement("no-show", NoShow);
            if (AdvancedDeposit != null)
                builder.AddElement("advanced-deposit", AdvancedDeposit);
            if (FireSafe != null)
                builder.AddElement("fire-safe", FireSafe);
            if (Legs != null)
                builder.AddElement("legs", Legs);
            if (AdditionalCharges != null)
                builder.AddElement("additional-charges", AdditionalCharges);
            if (ArrivalDate != null)
                builder.AddElement("arrival-date", ArrivalDate);
            if (TicketIssuerAddress != null)
                builder.AddElement("ticket-issuer-address", TicketIssuerAddress);
            return builder;
        }
    }
}
