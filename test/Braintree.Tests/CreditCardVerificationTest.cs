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
            builder.Append("    <avs-error-response-code nil=\"true\"></avs-error-response-code>");
            builder.Append("    <avs-postal-code-response-code>I</avs-postal-code-response-code>");
            builder.Append("    <status>processor_declined</status>");
            builder.Append("    <processor-response-code>2000</processor-response-code>");
            builder.Append("    <avs-street-address-response-code>I</avs-street-address-response-code>");
            builder.Append("    <processor-response-text>Do Not Honor</processor-response-text>");
            builder.Append("    <cvv-response-code>M</cvv-response-code>");
            builder.Append("  </verification>");
            builder.Append("  <errors>");
            builder.Append("    <errors type=\"array\"/>");
            builder.Append("  </errors>");
            builder.Append("</api-error-response>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());

            CreditCardVerification verification = new CreditCardVerification(new NodeWrapper(doc).GetNode("//verification"), gateway);
            Assert.AreEqual(null, verification.AvsErrorResponseCode);
            Assert.AreEqual("I", verification.AvsPostalCodeResponseCode);
            Assert.AreEqual(VerificationStatus.PROCESSOR_DECLINED, verification.Status);
            Assert.AreEqual("2000", verification.ProcessorResponseCode);
            Assert.AreEqual("I", verification.AvsStreetAddressResponseCode);
            Assert.AreEqual("Do Not Honor", verification.ProcessorResponseText);
            Assert.AreEqual("M", verification.CvvResponseCode);
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
            Assert.AreEqual(null, verification.AvsErrorResponseCode);
            Assert.AreEqual(null, verification.AvsPostalCodeResponseCode);
            Assert.AreEqual(null, verification.Status);
            Assert.AreEqual(null, verification.ProcessorResponseCode);
            Assert.AreEqual(null, verification.AvsStreetAddressResponseCode);
            Assert.AreEqual(null, verification.ProcessorResponseText);
            Assert.AreEqual(null, verification.CvvResponseCode);
        }
    }
}
