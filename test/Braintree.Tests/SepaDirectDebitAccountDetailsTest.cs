
using System.Xml;
using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class SepaDirectDebitAccountDetailsTest
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
            string xml = "<sepa-debit-account-detail>"
                + "<paypal-v2-order-id>123456</paypal-v2-order-id>" 
                + "<mandate-type>ONE_OFF</mandate-type>" 
                + "<settlement-type>instant</settlement-type>" 
                + "<bank-reference-token>123456789</bank-reference-token>" 
                + "<debug-id>ABC123</debug-id>"
                + "<token>ch6byss</token>"
                + "<capture-id>ABC123</capture-id>"
                + "<refund-id>a-refund-id</refund-id>"
                + "<merchant-or-partner-customer-id>123456789</merchant-or-partner-customer-id>"
                + "<transaction-fee-amount>12.34</transaction-fee-amount>"
                + "<transaction-fee-currency-iso-code>EUR</transaction-fee-currency-iso-code>"
                + "<refund-from-transaction-fee-amount>1.34</refund-from-transaction-fee-amount>"
                + "<refund-from-transaction-fee-currency-iso-code>EUR</refund-from-transaction-fee-currency-iso-code>"
                + "</sepa-debit-account-detail>";
            var node = nodeFromXml(xml);

            SepaDirectDebitAccountDetails details = new SepaDirectDebitAccountDetails(node);

            Assert.AreEqual("123456", details.PayPalV2OrderId);
            Assert.AreEqual(MandateType.ONE_OFF, details.MandateType);
            Assert.AreEqual("123456789", details.BankReferenceToken);
            Assert.AreEqual("123456789", details.MerchantOrPartnerCustomerId);
            Assert.AreEqual("ABC123", details.DebugId);
            Assert.AreEqual("a-refund-id", details.RefundId);
            Assert.AreEqual("ABC123", details.CaptureId);
            Assert.AreEqual("12.34", details.TransactionFeeAmount);
            Assert.AreEqual("EUR", details.TransactionFeeCurrencyIsoCode);
            Assert.AreEqual("1.34", details.RefundFromTransactionFeeAmount);
            Assert.AreEqual("EUR", details.RefundFromTransactionFeeCurrencyIsoCode);
            Assert.AreEqual(SettlementType.INSTANT, details.SettlementType);
            Assert.AreEqual("ch6byss", details.Token);
        }
    }
}
