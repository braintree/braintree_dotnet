#pragma warning disable 1591

namespace Braintree
{
    public class IndustryDataRequest : Request
    {
        public string FolioNumber { get; set; }
        public string CheckInDate { get; set; }
        public string CheckOutDate { get; set; }
        public string RoomRate { get; set; }

        public string TravelPackage { get; set; }
        public string DepartureDate { get; set; }
        public string LodgingCheckInDate { get; set; }
        public string LodgingCheckOutDate { get; set; }
        public string LodgingName { get; set; }

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
