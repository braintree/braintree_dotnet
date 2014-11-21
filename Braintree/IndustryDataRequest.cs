#pragma warning disable 1591

using System;

namespace Braintree
{
    public class IndustryDataRequest : Request
    {
        public String FolioNumber { get; set; }
        public String CheckInDate { get; set; }
        public String CheckOutDate { get; set; }
        public String RoomRate { get; set; }

        public String TravelPackage { get; set; }
        public String DepartureDate { get; set; }
        public String LodgingCheckInDate { get; set; }
        public String LodgingCheckOutDate { get; set; }
        public String LodgingName { get; set; }

        public override String ToXml()
        {
            return ToXml("data");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        public override String ToQueryString(String root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            return new RequestBuilder(root).
                AddElement("folio-number", FolioNumber).
                AddElement("check-in-date", CheckInDate).
                AddElement("check-out-date", CheckOutDate).
                AddElement("travel-package", TravelPackage).
                AddElement("departure-date", DepartureDate).
                AddElement("lodging-check-in-date", LodgingCheckInDate).
                AddElement("lodging-check-out-date", LodgingCheckOutDate).
                AddElement("lodging-name", LodgingName).
                AddElement("room-rate", RoomRate);
        }
    }
}
