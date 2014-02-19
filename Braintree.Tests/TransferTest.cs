using System;
using System.Text;
using System.Xml;
using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransferTest
    {
        private BraintreeGateway gateway;
        private BraintreeService service;
        private NodeWrapper attributes;

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

            XmlDocument attributesXml = CreateAttributesXml();
            attributes = new NodeWrapper(attributesXml).GetNode("//transfer");
        }

        private XmlDocument CreateAttributesXml(){
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<transfer>");
            builder.Append("<merchant-account-id>sandbox_sub_merchant_account</merchant-account-id>");
            builder.Append("<id>123456</id>");
            builder.Append("<message>invalid-account-number</message>");
            builder.Append("<amount>100.00</amount>");
            builder.Append("<disbursement-date>2013-04-10</disbursement-date>");
            builder.Append("<follow-up-action>update</follow-up-action>");
            builder.Append("</transfer>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());

            return doc;
        }

        [Test]
        public void MerchantAccount()
        {
          Transfer transfer = new Transfer(attributes, service);
          Assert.IsNotNull(transfer.MerchantAccount());
          Assert.AreEqual("sandbox_sub_merchant_account", transfer.MerchantAccount().Id);
        }

        [Test]
        public void Transactions()
        {
          Transfer transfer = new Transfer(attributes, service);
          Assert.IsNotNull(transfer.Transactions());
          Assert.AreEqual(1, transfer.Transactions().MaximumCount);
          Assert.AreEqual("sub_merchant_transaction", transfer.Transactions().FirstItem.Id);
        }
    }
}
