using Braintree.Exceptions;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Globalization;
using System.Text;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class ApplePayCardTest
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
        public void ConstructFromXMLResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<payment-method>");
            builder.Append("<merchant-token-identifier>merchant-token-123</merchant-token-identifier>");
            builder.Append("<source-card-last4>1234</source-card-last4>");
            builder.Append("</payment-method>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());

            ApplePayCard card = new ApplePayCard(new NodeWrapper(doc).GetNode("payment-method"), gateway);
           
            Assert.AreEqual("merchant-token-123", card.MerchantTokenIdentifier);
            Assert.AreEqual("1234", card.SourceCardLast4);
        }
    }
}
