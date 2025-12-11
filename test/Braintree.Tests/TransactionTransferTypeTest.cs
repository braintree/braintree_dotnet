using Braintree.Exceptions;
using NUnit.Framework;
using System;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransactionTransferTypeTest
    {
        private BraintreeGateway gateway;
        private BraintreeService service;

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
            service = new BraintreeService(gateway.Configuration);
        }

        [Test]
        public void BuildRequest_Transfer_IncludedWhenNotNull()
        {
            var transactionRequest = new TransactionRequest
            {
                Amount = 100.00m,
                Transfer = new TransferRequest
                {
                    Type = "wallet_transfer",
                    Sender = new SenderRequest
                    {
                        FirstName = "Alice",
                        LastName = "Silva",
                        MiddleName = "A",
                        AccountReferenceNumber = "1000012345",
                        DateOfBirth = new DateTime(2009, 1, 1),
                        Address = new AddressRequest
                        {
                            StreetAddress = "Door 12, 12th Main",
                            Locality = "Los Angeles",
                            Region = "CA",
                            CountryCodeAlpha2 = "US"
                        }
                    },
                    Receiver = new ReceiverRequest
                    {
                        FirstName = "Bob",
                        LastName = "Souza",
                        MiddleName = "A",
                        Address = new AddressRequest
                        {
                            StreetAddress = "Door 10, 10th Main",
                            Locality = "Los Angeles",
                            Region = "CA",
                            CountryCodeAlpha2 = "US"
                        }
                    }
                }
            };

            string xml = transactionRequest.ToXml();
            Assert.IsTrue(xml.Contains("<transfer>"));
            Assert.IsTrue(xml.Contains("<type>wallet_transfer</type>"));
            Assert.IsTrue(xml.Contains("<sender>"));
            Assert.IsTrue(xml.Contains("<first-name>Alice</first-name>"));
            Assert.IsTrue(xml.Contains("<last-name>Silva</last-name>"));
            Assert.IsTrue(xml.Contains("<middle-name>A</middle-name>"));
            Assert.IsTrue(xml.Contains("<date-of-birth type=\"datetime\">2009-01-01 00:00:00Z</date-of-birth>"));
            Assert.IsTrue(xml.Contains("<account-reference-number>1000012345</account-reference-number>"));
            Assert.IsTrue(xml.Contains("<address>"));
            Assert.IsTrue(xml.Contains("</sender>"));
            Assert.IsTrue(xml.Contains("<receiver>"));
            Assert.IsTrue(xml.Contains("<first-name>Bob</first-name>"));
            Assert.IsTrue(xml.Contains("<last-name>Souza</last-name>"));
            Assert.IsTrue(xml.Contains("<middle-name>A</middle-name>"));
            Assert.IsTrue(xml.Contains("</receiver>"));
            Assert.IsTrue(xml.Contains("</transfer>"));
        }

        [Test]
        public void BuildRequest_Transfer_NotIncludedWhenNull()
        {
            var transactionRequest = new TransactionRequest
            {
                Transfer = null
            };

            var xml = transactionRequest.ToXml();

            Assert.IsFalse(xml.Contains("<transfer>"));
        }
    }
}

