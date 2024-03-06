using Braintree.Exceptions;
using NUnit.Framework;
using System;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransactionSearchRequestTest
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
        public void Search_ByDebitNetwork(){
          string[] allowedNetworks = {
                Braintree.TransactionDebitNetwork.NYCE.GetDescription(),
                Braintree.TransactionDebitNetwork.ACCEL.GetDescription(),
                Braintree.TransactionDebitNetwork.MAESTRO.GetDescription(),
                Braintree.TransactionDebitNetwork.NYCE.GetDescription(),
                Braintree.TransactionDebitNetwork.PULSE.GetDescription(),
                Braintree.TransactionDebitNetwork.STAR.GetDescription(),
                Braintree.TransactionDebitNetwork.STAR_ACCESS.GetDescription()
            };
          TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is("1234").
                DebitNetwork.IncludedIn(allowedNetworks);
          
          Assert.AreEqual(searchRequest.ToXml(),"<search><id><is>1234</is></id><debit-network type=\"array\"><item>NYCE</item><item>ACCEL</item><item>MAESTRO</item><item>NYCE</item><item>PULSE</item><item>STAR</item><item>STAR_ACCESS</item></debit-network></search>");
        }
    }
}
