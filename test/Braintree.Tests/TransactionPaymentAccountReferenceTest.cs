using NUnit.Framework;
using System.Text;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransactionPaymentAccountReferenceTest
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
        public void Transaction_CreditCard_PaymentAccountReference_ReturnsValue()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<transaction>");
            builder.Append("  <id>transaction_id</id>");
            builder.Append("  <type>sale</type>");
            builder.Append("  <amount>100.00</amount>");
            builder.Append("  <status>settled</status>");
            builder.Append("  <credit-card>");
            builder.Append("    <last-4>1234</last-4>");
            builder.Append("    <card-type>Visa</card-type>");
            builder.Append("    <payment-account-reference>V0010013019339005665779448477</payment-account-reference>");
            builder.Append("  </credit-card>");
            builder.Append("</transaction>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);
            Assert.AreEqual("V0010013019339005665779448477", transaction.CreditCard.PaymentAccountReference);
        }

        [Test]
        public void Transaction_ApplePayDetails_PaymentAccountReference_ReturnsValue()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<transaction>");
            builder.Append("  <id>transaction_id</id>");
            builder.Append("  <type>sale</type>");
            builder.Append("  <amount>100.00</amount>");
            builder.Append("  <status>settled</status>");
            builder.Append("  <apple-pay>");
            builder.Append("    <last-4>1234</last-4>");
            builder.Append("    <card-type>Visa</card-type>");
            builder.Append("    <payment-account-reference>V0010013019339005665779448477</payment-account-reference>");
            builder.Append("  </apple-pay>");
            builder.Append("</transaction>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);
            Assert.AreEqual("V0010013019339005665779448477", transaction.ApplePayDetails.PaymentAccountReference);
        }

        [Test]
        public void Transaction_AndroidPayDetails_PaymentAccountReference_ReturnsValue()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<transaction>");
            builder.Append("  <id>transaction_id</id>");
            builder.Append("  <type>sale</type>");
            builder.Append("  <amount>100.00</amount>");
            builder.Append("  <status>settled</status>");
            builder.Append("  <android-pay-card>");
            builder.Append("    <virtual-card-last-4>1234</virtual-card-last-4>");
            builder.Append("    <virtual-card-type>Visa</virtual-card-type>");
            builder.Append("    <payment-account-reference>V0010013019339005665779448477</payment-account-reference>");
            builder.Append("  </android-pay-card>");
            builder.Append("</transaction>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);
            Assert.AreEqual("V0010013019339005665779448477", transaction.AndroidPayDetails.PaymentAccountReference);
        }

    }
}