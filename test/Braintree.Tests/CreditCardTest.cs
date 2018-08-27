using Braintree.Exceptions;
using NUnit.Framework;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class CreditCardTest
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
        public void TrData_ReturnsValidTrDataHash()
        {
            string trData = gateway.TrData(new CreditCardRequest(), "http://example.com");
            Assert.IsTrue(TrUtil.IsTrDataValid(trData, service));
        }

        [Test]
        public void Find_FindsErrorsOutOnWhitespaceIds()
        {
            Assert.Throws<NotFoundException>(() => gateway.CreditCard.Find(" "));
        }

        [Test]
        public void VerificationIsLatestVerification()
        {
            string xml = "<credit-card>"
                          + "<verifications>"
                          + "    <verification>"
                          + "        <created-at type=\"datetime\">2014-11-20T17:27:15Z</created-at>"
                          + "        <id>123</id>"
                          + "    </verification>"
                          + "    <verification>"
                          + "        <created-at type=\"datetime\">2014-11-20T17:27:18Z</created-at>"
                          + "        <id>932</id>"
                          + "    </verification>"
                          + "    <verification>"
                          + "        <created-at type=\"datetime\">2014-11-20T17:27:17Z</created-at>"
                          + "        <id>456</id>"
                          + "    </verification>"
                          + "</verifications>"
                        + "</credit-card>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);

            var result = new ResultImpl<CreditCard>(node, gateway);

            Assert.AreEqual("932", result.Target.Verification.Id);
        }
    }
}
