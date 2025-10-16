using NUnit.Framework;
using System.Text;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class CreditCardVerificationTest
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
        public void ConstructFromResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<api-error-response>");
            builder.Append("  <verification>");
            builder.Append("    <amount>1.02</amount>");
            builder.Append("    <currency-iso-code>USD</currency-iso-code>");
            builder.Append("    <avs-error-response-code nil=\"true\"></avs-error-response-code>");
            builder.Append("    <avs-postal-code-response-code>I</avs-postal-code-response-code>");
            builder.Append("    <status>processor_declined</status>");
            builder.Append("    <processor-response-code>2000</processor-response-code>");
            builder.Append("    <avs-street-address-response-code>I</avs-street-address-response-code>");
            builder.Append("    <processor-response-text>Do Not Honor</processor-response-text>");
            builder.Append("    <network-response-code>05</network-response-code>");
            builder.Append("    <network-response-text>Do not Honor</network-response-text>");
            builder.Append("    <network-transaction-id>123456789012345</network-transaction-id>");
            builder.Append("    <cvv-response-code>M</cvv-response-code>");
            builder.Append("  </verification>");
            builder.Append("  <errors>");
            builder.Append("    <errors type=\"array\"/>");
            builder.Append("  </errors>");
            builder.Append("</api-error-response>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());

            CreditCardVerification verification = new CreditCardVerification(new NodeWrapper(doc).GetNode("//verification"), gateway);
            Assert.AreEqual(decimal.Parse("1.02"), verification.Amount);
            Assert.AreEqual("USD", verification.CurrencyIsoCode);
            Assert.AreEqual(null, verification.AvsErrorResponseCode);
            Assert.AreEqual("I", verification.AvsPostalCodeResponseCode);
            Assert.AreEqual(VerificationStatus.PROCESSOR_DECLINED, verification.Status);
            Assert.AreEqual("2000", verification.ProcessorResponseCode);
            Assert.AreEqual("I", verification.AvsStreetAddressResponseCode);
            Assert.AreEqual("Do Not Honor", verification.ProcessorResponseText);
            Assert.AreEqual("M", verification.CvvResponseCode);
            Assert.AreEqual("05", verification.NetworkResponseCode);
            Assert.AreEqual("Do not Honor", verification.NetworkResponseText);
            Assert.AreEqual("123456789012345", verification.NetworkTransactionId);
        }

        [Test]
        public void ConstructFromResponseWithNoVerification()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<api-error-response>");
            builder.Append("  <errors>");
            builder.Append("    <errors type=\"array\"/>");
            builder.Append("  </errors>");
            builder.Append("</api-error-response>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());

            CreditCardVerification verification = new CreditCardVerification(new NodeWrapper(doc).GetNode("//verification"), gateway);
            Assert.AreEqual(null, verification.Amount);
            Assert.AreEqual(null, verification.CurrencyIsoCode);
            Assert.AreEqual(null, verification.AvsErrorResponseCode);
            Assert.AreEqual(null, verification.AvsPostalCodeResponseCode);
            Assert.AreEqual(null, verification.Status);
            Assert.AreEqual(null, verification.ProcessorResponseCode);
            Assert.AreEqual(null, verification.AvsStreetAddressResponseCode);
            Assert.AreEqual(null, verification.ProcessorResponseText);
            Assert.AreEqual(null, verification.CvvResponseCode);
        }

        [Test]
        public void DeserializesVisaAniResponseCodeFromXml()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            builder.Append("<verification>\n");
            builder.Append("  <ani-first-name-response-code>M</ani-first-name-response-code>\n");
            builder.Append("  <ani-last-name-response-code>M</ani-last-name-response-code>\n");
            builder.Append("</verification>\n");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            CreditCardVerification verification = new CreditCardVerification(node, gateway);
            Assert.AreEqual("M", verification.AniFirstNameResponseCode);
            Assert.AreEqual("M", verification.AniLastNameResponseCode);
        }

        [Test]
        public void PaymentAccountReference_AccessibleThroughCreditCardDetails()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            builder.Append("<verification>\n");
            builder.Append("  <credit-card>\n");
            builder.Append("    <last-4>1234</last-4>\n");
            builder.Append("    <payment-account-reference>V0010013019339005665779448477</payment-account-reference>\n");
            builder.Append("  </credit-card>\n");
            builder.Append("</verification>\n");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            CreditCardVerification verification = new CreditCardVerification(node, gateway);
            Assert.AreEqual("V0010013019339005665779448477", verification.CreditCard.PaymentAccountReference);
        }

    }
}
