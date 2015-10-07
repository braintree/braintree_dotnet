using System;
using System.Reflection;
using System.Text;
using System.Xml;
using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class DisbursementTest
    {
        private BraintreeGateway gateway;
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

            XmlDocument attributesXml = CreateAttributesXml();
            attributes = new NodeWrapper(attributesXml).GetNode("//disbursement");
        }

        private XmlDocument CreateAttributesXml(){
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<disbursement>");
            builder.Append("<id>123456</id>");
            builder.Append("<transaction-ids type=\"array\">");
            builder.Append("<item>sub_merchant_transaction</item>");
            builder.Append("</transaction-ids>");
            builder.Append("<success type=\"boolean\">false</success>");
            builder.Append("<retry type=\"boolean\">false</retry>");
            builder.Append("<merchant-account>");
            builder.Append("<id>sandbox_sub_merchant_account</id>");
            builder.Append("<currency-iso-code>usd</currency-iso-code>");
            builder.Append("<sub-merchant-account type=\"boolean\">false</sub-merchant-account>");
            builder.Append("<status>active</status>");
            builder.Append("</merchant-account>");
            builder.Append("<amount>100.00</amount>");
            builder.Append("<disbursement-date type=\"date\">2014-02-10</disbursement-date>");
            builder.Append("<exception-message>bank_rejected</exception-message>");
            builder.Append("<follow-up-action>update_funding_information</follow-up-action>");
            builder.Append("</disbursement>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());

            return doc;
        }

        [Test]
        public void Transactions()
        {
          Disbursement disbursement = new Disbursement(attributes, gateway);
          ResourceCollection<Transaction> transactions = disbursement.Transactions();
          Assert.IsNotNull(transactions);
          Assert.AreEqual(1, transactions.MaximumCount);
          Assert.AreEqual("sub_merchant_transaction", transactions.FirstItem.Id);
        }
    }
}
