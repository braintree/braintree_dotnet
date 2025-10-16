using NUnit.Framework;
using System.Text;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class PaymentAccountReferenceTest
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
        public void CreditCard_PaymentAccountReference_ReturnsValue()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<credit-card>");
            builder.Append("  <last-4>1234</last-4>");
            builder.Append("  <payment-account-reference>V0010013019339005665779448477</payment-account-reference>");
            builder.Append("</credit-card>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            CreditCard creditCard = new CreditCard(node, gateway);
            Assert.AreEqual("V0010013019339005665779448477", creditCard.PaymentAccountReference);
        }

        [Test]
        public void ApplePayDetails_PaymentAccountReference_ReturnsValue()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<apple-pay-details>");
            builder.Append("  <last-4>1234</last-4>");
            builder.Append("  <payment-account-reference>V0010013019339005665779448477</payment-account-reference>");
            builder.Append("</apple-pay-details>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            ApplePayDetails details = new ApplePayDetails(node);
            Assert.AreEqual("V0010013019339005665779448477", details.PaymentAccountReference);
        }

        [Test]
        public void AndroidPayDetails_PaymentAccountReference_ReturnsValue()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<android-pay-details>");
            builder.Append("  <virtual-card-last-4>1234</virtual-card-last-4>");
            builder.Append("  <payment-account-reference>V0010013019339005665779448477</payment-account-reference>");
            builder.Append("</android-pay-details>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            AndroidPayDetails details = new AndroidPayDetails(node);
            Assert.AreEqual("V0010013019339005665779448477", details.PaymentAccountReference);
        }

    }
}