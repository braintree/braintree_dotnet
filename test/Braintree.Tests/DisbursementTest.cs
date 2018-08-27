using Braintree.Exceptions;
using NUnit.Framework;
using System.Xml;

namespace Braintree.Tests
{

    [TestFixture]
    public class DisbursementTest
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
        public void TestDisbursementTypeIsSet()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                + "<dibursement>"
                + "   <id>12345</id>"
                + "   <merchant-account>"
                + "       <status>active</status>"
                + "       <id>merchant_account_token</id>"
                + "       <currency-iso-code>USD</currency-iso-code>"
                + "       <default type=\"boolean\">false</default>"
                + "       <sub-merchant-account type=\"boolean\">false</sub-merchant-account>"
                + "   </merchant-account>"
                + "   <exception-message nil=\"true\"/>"
                + "   <amount>100.00</amount>"
                + "   <disbursement-type>credit</disbursement-type>"
                + "   <success type=\"boolean\">true</success>"
                + "   <retry type=\"boolean\">true</retry>"
                + "   <transaction-ids type=\"array\">"
                + "       <item>asdf</item>"
                + "       <item>qwer</item>"
                + "   </transaction-ids>"
                + "</dibursement>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);
            Disbursement disbursement = new Disbursement(node, gateway);

            Assert.AreEqual(DisbursementType.CREDIT, disbursement.DisbursementType);
        }

        [Test]
        public void TestDisbursementTypeIsSetWhenNotPresent()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                + "<dibursement>"
                + "   <id>12345</id>"
                + "   <merchant-account>"
                + "       <status>active</status>"
                + "       <id>merchant_account_token</id>"
                + "       <currency-iso-code>USD</currency-iso-code>"
                + "       <default type=\"boolean\">false</default>"
                + "       <sub-merchant-account type=\"boolean\">false</sub-merchant-account>"
                + "   </merchant-account>"
                + "   <exception-message nil=\"true\"/>"
                + "   <amount>100.00</amount>"
                + "   <success type=\"boolean\">true</success>"
                + "   <retry type=\"boolean\">true</retry>"
                + "   <transaction-ids type=\"array\">"
                + "       <item>asdf</item>"
                + "       <item>qwer</item>"
                + "   </transaction-ids>"
                + "</dibursement>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);
            Disbursement disbursement = new Disbursement(node, gateway);

            Assert.AreEqual(DisbursementType.UNKNOWN, disbursement.DisbursementType);
        }
    }
}
