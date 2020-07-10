using Braintree.Exceptions;
using NUnit.Framework;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class AuthenticationInsightResponseTest
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
        public void ParsesNodeCorrectlyWithScaIndicator()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<authentication-insight>" +
                "  <regulation-environment>bar</regulation-environment>" +
                "  <sca-indicator>foo</sca-indicator>" +
                "</authentication-insight>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);

            var result = new ResultImpl<AuthenticationInsightResponse>(node, gateway);

            Assert.IsNotNull(result.Target);
            Assert.AreEqual("foo", result.Target.ScaIndicator);
            Assert.AreEqual("bar", result.Target.RegulationEnvironment);
        }

        [Test]
        public void ParsesNodeCorrectlyWithoutScaIndicator()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<authentication-insight>" +
                "  <regulation-environment>bar</regulation-environment>" +
                "</authentication-insight>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);

            var result = new ResultImpl<AuthenticationInsightResponse>(node, gateway);

            Assert.IsNotNull(result.Target);
            Assert.AreEqual(null, result.Target.ScaIndicator);
            Assert.AreEqual("bar", result.Target.RegulationEnvironment);
        }
    }
}
