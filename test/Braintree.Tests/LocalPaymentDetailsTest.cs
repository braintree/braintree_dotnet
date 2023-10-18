using System.Xml;
using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class LocalPaymentDetailsTest
    {
        private NodeWrapper nodeFromXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            return new NodeWrapper(newNode);
        }

        [Test]
        public void IncludesFields()
        {
            string xml = "<local-payment>" +

            "<capture-id>CAT-1234</capture-id>" +
            "<custom-field>whatever</custom-field>" +
            "<debug-id>DEB-1234</debug-id>" +
            "<description>Detailed text</description>" +
            "<funding-source>ideal</funding-source>" +
            "<implicitly-vaulted-payment-method-global-id>abcdefgabcdefg</implicitly-vaulted-payment-method-global-id>" +
            "<implicitly-vaulted-payment-method-token>cx9rav</implicitly-vaulted-payment-method-token>" +
            "<payer-id>nothing</payer-id>" +
            "<payment-id>ABC12345</payment-id>" +
            "<refund-from-transaction-fee-amount>2.00</refund-from-transaction-fee-amount>" +
            "<refund-from-transaction-fee-currency-iso-code>EUR</refund-from-transaction-fee-currency-iso-code>" +
            "<refund-id>REF-1234</refund-id>" +
            "<transaction-fee-amount>10.00</transaction-fee-amount>" +
            "<transaction-fee-currency-iso-code>EUR</transaction-fee-currency-iso-code>" +
            "</local-payment>";
            var node = nodeFromXml(xml);

            LocalPaymentDetails details = new LocalPaymentDetails(node);

            Assert.AreEqual("CAT-1234", details.CaptureId);
            Assert.AreEqual("whatever", details.CustomField);
            Assert.AreEqual("DEB-1234", details.DebugId);
            Assert.AreEqual("Detailed text", details.Description);
            Assert.AreEqual("ideal", details.FundingSource);
            Assert.AreEqual("abcdefgabcdefg", details.ImplicitlyVaultedPaymentMethodGlobalId);
            Assert.AreEqual("cx9rav", details.ImplicitlyVaultedPaymentMethodToken);
            Assert.AreEqual("nothing", details.PayerId);
            Assert.AreEqual("ABC12345", details.PaymentId);
            Assert.AreEqual("2.00", details.RefundFromTransactionFeeAmount);
            Assert.AreEqual("EUR", details.RefundFromTransactionFeeCurrencyIsoCode);
            Assert.AreEqual("REF-1234", details.RefundId);
            Assert.AreEqual("10.00", details.TransactionFeeAmount);
            Assert.AreEqual("EUR", details.TransactionFeeCurrencyIsoCode);
        }
    }
}
