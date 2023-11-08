using Braintree.Exceptions;
using NUnit.Framework;
using System;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class IndustryDataRequestTest
    {
        private BraintreeGateway gateway;

        [SetUp]
        public void Setup()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
        }

        [Test]
        public void ToXml_IncludesAllProperties()
        {
            var request = new IndustryRequest
            {
                IndustryType = TransactionIndustryType.TRAVEL_AND_FLIGHT,
                IndustryData = new IndustryDataRequest
                {
                    PassengerFirstName = "John",
                    PassengerLastName = "Doe",
                    PassengerMiddleInitial = "M",
                    PassengerTitle = "Mr.",
                    IssuedDate = new DateTime(2018, 1, 1),
                    TravelAgencyName = "Expedia",
                    TravelAgencyCode = "12345678",
                    TicketNumber = "ticket-number",
                    IssuingCarrierCode = "AA",
                    CustomerCode = "customer-code",
                    FareAmount = 7000M,
                    FeeAmount = 1000M,
                    TaxAmount = 2000M,
                    RestrictedTicket = false,
                    ArrivalDate = new DateTime(2018, 1, 1),
                    TicketIssuerAddress = "tkt-issuer-address",
                    DateOfBirth = "2012-12-12",
                    CountryCode = "US",
                    Legs = new IndustryDataLegRequest[]
                    {
                        new IndustryDataLegRequest
                        {
                            ConjunctionTicket = "CJ0001",
                            ExchangeTicket = "ET0001",
                            CouponNumber = "1",
                            ServiceClass = "Y",
                            CarrierCode = "AA",
                            FareBasisCode = "W",
                            FlightNumber = "AA100",
                            DepartureDate = new DateTime(2018, 1, 2),
                            DepartureAirportCode = "MDW",
                            DepartureTime = "08:00",
                            ArrivalAirportCode = "ATX",
                            ArrivalTime = "10:00",
                            StopoverPermitted = false,
                            FareAmount = 3500M,
                            FeeAmount = 500M,
                            TaxAmount = 1000M,
                            EndorsementOrRestrictions = "NOT REFUNDABLE",
                        }
                    }
                }
            };

            Assert.IsTrue(request.ToXml().Contains("<arrival-date type=\"datetime\">2018-01-01 00:00:00Z</arrival-date>"));
            Assert.IsTrue(request.ToXml().Contains("<ticket-issuer-address>tkt-issuer-address</ticket-issuer-address>"));
            Assert.IsTrue(request.ToXml().Contains("<country-code>US</country-code>"));
            Assert.IsTrue(request.ToXml().Contains("<date-of-birth>2012-12-12</date-of-birth>"));
        }
    }
}
