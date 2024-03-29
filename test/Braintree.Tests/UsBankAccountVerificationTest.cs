using NUnit.Framework;
using System.Text;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class UsBankAccountVerificationTest
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
            builder.Append("<thing>");
            builder.Append("  <verification>");
            builder.Append("    <verification-method>network_check</verification-method>");
            builder.Append("    <verification-add-ons>customer_verification</verification-add-ons>");
            builder.Append("    <status>processor_declined</status>");
            builder.Append("    <processor-response-code>2000</processor-response-code>");
            builder.Append("    <additional-processor-response>Invalid routing number</additional-processor-response>");
            builder.Append("    <processor-response-text>Do Not Honor</processor-response-text>");
            builder.Append("  </verification>");
            builder.Append("</thing>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());

            UsBankAccountVerification verification = new UsBankAccountVerification(
                new NodeWrapper(doc).GetNode("//verification")
            );

            Assert.AreEqual(UsBankAccountVerificationMethod.NETWORK_CHECK, verification.VerificationMethod);
            Assert.AreEqual(VerificationAddOns.CUSTOMER_VERIFICATION, verification.VerificationAddOns);
            Assert.AreEqual(UsBankAccountVerificationStatus.PROCESSOR_DECLINED, verification.Status);
            Assert.AreEqual("2000", verification.ProcessorResponseCode);
            Assert.AreEqual("Do Not Honor", verification.ProcessorResponseText);
            Assert.AreEqual("Invalid routing number", verification.AdditionalProcessorResponse);
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

            UsBankAccountVerification verification = new UsBankAccountVerification(
                    new NodeWrapper(doc).GetNode("//verification")
            );

            Assert.AreEqual(null, verification.ProcessorResponseCode);
            Assert.AreEqual(null, verification.ProcessorResponseText);
        }
    }
}
