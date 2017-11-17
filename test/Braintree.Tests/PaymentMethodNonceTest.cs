using Braintree.Exceptions;
using NUnit.Framework;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class PaymentMethodNonceTest
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
        public void ParsesNodeCorrectlyWithDetailsMissing()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<payment-method-nonce>" +
                "  <type>CreditCard</type>" +
                "  <nonce>fake-valid-nonce</nonce>" +
                "  <description>ending in 22</description>" +
                "  <consumed type=\"boolean\">false</consumed>" +
                "  <three-d-secure-info nil=\"true\"/>" +
                "</payment-method-nonce>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);

            var result = new ResultImpl<PaymentMethodNonce>(node, gateway);

            Assert.IsNotNull(result.Target);
            Assert.AreEqual("fake-valid-nonce", result.Target.Nonce);
            Assert.AreEqual("CreditCard", result.Target.Type);
            Assert.IsNull(result.Target.ThreeDSecureInfo);
            Assert.IsNull(result.Target.Details);
        }

        [Test]
        public void ParsesNodeCorrectlyWithBinData()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<payment-method-nonce>" +
                "  <type>CreditCard</type>" +
                "  <nonce>fake-valid-nonce</nonce>" +
                "  <description>ending in 22</description>" +
                "  <consumed type=\"boolean\">false</consumed>" +
                "  <three-d-secure-info nil=\"true\"/>" +
                "  <details nil=\"true\"/>" +
                "  <bin-data>" +
                "    <healthcare>Yes</healthcare>" +
                "    <debit>No</debit>" +
                "    <durbin-regulated>Unknown</durbin-regulated>" +
                "    <commercial>Unknown</commercial>" +
                "    <payroll>Unknown</payroll>" +
                "    <prepaid>NO</prepaid>" +
                "    <issuing-bank>Unknown</issuing-bank>" +
                "    <country-of-issuance>Something</country-of-issuance>" +
                "    <product-id>123</product-id>" +
                "  </bin-data>" +
                "</payment-method-nonce>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);

            var result = new ResultImpl<PaymentMethodNonce>(node, gateway);

            Assert.IsNotNull(result.Target);
            Assert.AreEqual("fake-valid-nonce", result.Target.Nonce);
            Assert.AreEqual("CreditCard", result.Target.Type);
            Assert.IsNull(result.Target.ThreeDSecureInfo);
            Assert.IsNull(result.Target.Details);
            Assert.IsNotNull(result.Target.BinData);
            Assert.AreEqual(Braintree.CreditCardCommercial.UNKNOWN, result.Target.BinData.Commercial);
            Assert.AreEqual(Braintree.CreditCardDebit.NO, result.Target.BinData.Debit);
            Assert.AreEqual(Braintree.CreditCardDurbinRegulated.UNKNOWN, result.Target.BinData.DurbinRegulated);
            Assert.AreEqual(Braintree.CreditCardHealthcare.YES, result.Target.BinData.Healthcare);
            Assert.AreEqual(Braintree.CreditCardPayroll.UNKNOWN, result.Target.BinData.Payroll);
            Assert.AreEqual(Braintree.CreditCardPrepaid.NO, result.Target.BinData.Prepaid);
            Assert.AreEqual("Something", result.Target.BinData.CountryOfIssuance);
            Assert.AreEqual("123", result.Target.BinData.ProductId);
            Assert.AreEqual("Unknown", result.Target.BinData.IssuingBank);
        }

        [Test]
        public void ParsesNodeCorrectlyWithNilValues()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<payment-method-nonce>" +
                "  <type>CreditCard</type>" +
                "  <nonce>fake-valid-nonce</nonce>" +
                "  <description>ending in 22</description>" +
                "  <consumed type=\"boolean\">false</consumed>" +
                "  <three-d-secure-info nil=\"true\"/>" +
                "  <details nil=\"true\"/>" +
                "</payment-method-nonce>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);

            var result = new ResultImpl<PaymentMethodNonce>(node, gateway);

            Assert.IsNotNull(result.Target);
            Assert.AreEqual("fake-valid-nonce", result.Target.Nonce);
            Assert.AreEqual("CreditCard", result.Target.Type);
            Assert.IsNull(result.Target.ThreeDSecureInfo);
            Assert.IsNull(result.Target.Details);
        }
    }
}
