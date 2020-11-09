using System.Xml;
using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class PayPalDetailsTest
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
            string xml = "<paypal-details>" +
            "<payer-email>abc@test.com</payer-email>" +
            "<payment-id>1234567890</payment-id>" +
            "<authorization-id>12345</authorization-id>" +
            "<token>token</token>" +
            "<image-url>www.image-url.com</image-url>" +
            "<debug-id>12345</debug-id>" +
            "<payee-id>6789</payee-id>" +
            "<payee-email>payee@test.com</payee-email>" +
            "<custom-field>so custom much wow</custom-field>" +
            "<payer-id>1357</payer-id>" +
            "<payer-first-name>Grace</payer-first-name>" +
            "<payer-last-name>Hopper</payer-last-name>" +
            "<payer-status>status</payer-status>" +
            "<seller-protection-status>12345</seller-protection-status>" +
            "<refund-id>8675309</refund-id>" +
            "<capture-id>2468</capture-id>" +
            "<transaction-fee-amount>10.00</transaction-fee-amount>" +
            "<transaction-fee-currency-iso-code>123</transaction-fee-currency-iso-code>" +
            "<refund-from-transaction-fee-amount>2.00</refund-from-transaction-fee-amount>" +
            "<refund-from-transaction-fee-currency-iso-code>123</refund-from-transaction-fee-currency-iso-code>" +
            "<description>item</description>" +
            "<implicitly-vaulted-payment-method-token>implicittoken</implicitly-vaulted-payment-method-token>" +
            "<implicitly-vaulted-payment-method-global-id>implicitglobalid</implicitly-vaulted-payment-method-global-id>" +
            "<billing-agreement-id>billingagreementid</billing-agreement-id>" +
            "</paypal-details>";
            var node = nodeFromXml(xml);

            PayPalDetails details = new PayPalDetails(node);

            Assert.AreEqual("abc@test.com", details.PayerEmail);
            Assert.AreEqual("1234567890", details.PaymentId);
            Assert.AreEqual("12345", details.AuthorizationId);
            Assert.AreEqual("token", details.Token);
            Assert.AreEqual("www.image-url.com", details.ImageUrl);
            Assert.AreEqual("12345", details.DebugId);
            Assert.AreEqual("6789", details.PayeeId);
            Assert.AreEqual("payee@test.com", details.PayeeEmail);
            Assert.AreEqual("so custom much wow", details.CustomField);
            Assert.AreEqual("1357", details.PayerId);
            Assert.AreEqual("Grace", details.PayerFirstName);
            Assert.AreEqual("Hopper", details.PayerLastName);
            Assert.AreEqual("status", details.PayerStatus);
            Assert.AreEqual("12345", details.SellerProtectionStatus);
            Assert.AreEqual("8675309", details.RefundId);
            Assert.AreEqual("2468", details.CaptureId);
            Assert.AreEqual("10.00", details.TransactionFeeAmount);
            Assert.AreEqual("123", details.TransactionFeeCurrencyIsoCode);
            Assert.AreEqual("2.00", details.RefundFromTransactionFeeAmount);
            Assert.AreEqual("123", details.RefundFromTransactionFeeCurrencyIsoCode);
            Assert.AreEqual("item", details.Description);
            Assert.AreEqual("implicittoken", details.ImplicitlyVaultedPaymentMethodToken);
            Assert.AreEqual("implicitglobalid", details.ImplicitlyVaultedPaymentMethodGlobalId);
            Assert.AreEqual("billingagreementid", details.BillingAgreementId);
        }
    }
}
