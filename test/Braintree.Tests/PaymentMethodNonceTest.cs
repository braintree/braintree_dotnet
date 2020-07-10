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
        public void ParsesNodeCorrectlyWithPayPalDetails()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<payment-method-nonce>" +
                "  <type>PayPalAccount</type>" +
                "  <nonce>fake-paypal-account-nonce</nonce>" +
                "  <description></description>" +
                "  <consumed type=\"boolean\">false</consumed>" +
                "  <details>" +
                "    <payer-info>" +
                "      <email>jane.doe@paypal.com</email>" +
                "      <first-name>first</first-name>" +
                "      <last-name>last</last-name>" +
                "      <payer-id>pay-123</payer-id>" +
                "      <country-code>US</country-code>" +
                "    </payer-info>" +
                "  </details>" +
                "</payment-method-nonce>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);

            var result = new ResultImpl<PaymentMethodNonce>(node, gateway);

            Assert.IsNotNull(result.Target);
            Assert.AreEqual("fake-paypal-account-nonce", result.Target.Nonce);
            Assert.AreEqual("PayPalAccount", result.Target.Type);
            Assert.IsNotNull(result.Target.Details);
            Assert.IsNotNull(result.Target.Details.PayerInfo);
            Assert.AreEqual("jane.doe@paypal.com", result.Target.Details.PayerInfo.Email);
            Assert.AreEqual("first", result.Target.Details.PayerInfo.FirstName);
            Assert.AreEqual("last", result.Target.Details.PayerInfo.LastName);
            Assert.AreEqual("pay-123", result.Target.Details.PayerInfo.PayerId);
            Assert.AreEqual("US", result.Target.Details.PayerInfo.CountryCode);
        }

        [Test]
        public void ParsesNodeCorrectlyWithVenmoDetails()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<payment-method-nonce>" +
                "  <type>VenmoAccount</type>" +
                "  <nonce>fake-venmo-account-nonce</nonce>" +
                "  <description></description>" +
                "  <consumed type=\"boolean\">false</consumed>" +
                "  <details>" +
                "    <last-two>99</last-two>" +
                "    <username>venmojoe</username>" +
                "    <venmo-user-id>Venmo-Joe-1</venmo-user-id>" +
                "  </details>" +
                "</payment-method-nonce>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);

            var result = new ResultImpl<PaymentMethodNonce>(node, gateway);

            Assert.IsNotNull(result.Target);
            Assert.AreEqual("fake-venmo-account-nonce", result.Target.Nonce);
            Assert.AreEqual("VenmoAccount", result.Target.Type);
            Assert.IsNotNull(result.Target.Details);
            Assert.AreEqual("99", result.Target.Details.LastTwo);
            Assert.AreEqual("venmojoe", result.Target.Details.Username);
            Assert.AreEqual("Venmo-Joe-1", result.Target.Details.VenmoUserId);
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

        [Test]
        public void ParseThreeDSecureInfo()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<payment-method-nonce>" +
                "  <type>CreditCard</type>" +
                "  <nonce>fake-valid-nonce</nonce>" +
                "  <description>ending in 22</description>" +
                "  <consumed type=\"boolean\">false</consumed>" +
                "  <three-d-secure-info>" +
                "    <enrolled>Y</enrolled>" +
                "    <status>authenticate_successful</status>" +
                "    <liability-shifted>true</liability-shifted>" +
                "    <liability-shift-possible>true</liability-shift-possible>" +
                "    <cavv>cavv-value</cavv>" +
                "    <xid>xid-value</xid>" +
                "    <ds-transaction-id>ds-trx-id-value</ds-transaction-id>" +
                "    <eci-flag>06</eci-flag>" +
                "    <three-d-secure-version>2.0.1</three-d-secure-version>" +
                "  </three-d-secure-info>" +
                "  <details nil=\"true\"/>" +
                "</payment-method-nonce>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);

            var result = new ResultImpl<PaymentMethodNonce>(node, gateway);
            var threeDSecureInfo = result.Target.ThreeDSecureInfo;
            Assert.IsNotNull(result.Target.ThreeDSecureInfo);
            Assert.AreEqual("Y", threeDSecureInfo.Enrolled);
            Assert.AreEqual("authenticate_successful", threeDSecureInfo.Status);
            Assert.IsTrue(threeDSecureInfo.LiabilityShifted);
            Assert.IsTrue(threeDSecureInfo.LiabilityShiftPossible);
            Assert.AreEqual("cavv-value", threeDSecureInfo.Cavv);
            Assert.AreEqual("xid-value", threeDSecureInfo.Xid);
            Assert.AreEqual("ds-trx-id-value", threeDSecureInfo.DsTransactionId);
            Assert.AreEqual("06", threeDSecureInfo.EciFlag);
            Assert.AreEqual("2.0.1", threeDSecureInfo.ThreeDSecureVersion);
        }

        [Test]
        public void ParseAuthenticationInsights()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<payment-method-nonce>" +
                "  <authentication-insight>" +
                "    <regulation-environment>bar</regulation-environment>" +
                "    <sca-indicator>foo</sca-indicator>" +
                "  </authentication-insight>" +
                "</payment-method-nonce>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);

            var result = new ResultImpl<PaymentMethodNonce>(node, gateway);

            Assert.IsNotNull(result.Target);
            Assert.AreEqual("foo", result.Target.AuthenticationInsight.ScaIndicator);
            Assert.AreEqual("bar", result.Target.AuthenticationInsight.RegulationEnvironment);
        }
    }
}
