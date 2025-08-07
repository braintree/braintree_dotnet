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
                       }
            };

            string xml = transactionRequest.ToXml();

            Assert.IsTrue(xml.Contains("<transfer>"));
            Assert.IsTrue(xml.Contains("<type>wallet_transfer</type>"));
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

