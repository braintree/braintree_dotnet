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
            "<authorization-id>12345</authorization-id>" +
            "<billing-agreement-id>billingagreementid</billing-agreement-id>" +
            "<capture-id>2468</capture-id>" +
            "<custom-field>so custom much wow</custom-field>" +
            "<debug-id>12345</debug-id>" +
            "<description>item</description>" +
            "<image-url>www.image-url.com</image-url>" +
            "<implicitly-vaulted-payment-method-token>implicittoken</implicitly-vaulted-payment-method-token>" +
            "<implicitly-vaulted-payment-method-global-id>implicitglobalid</implicitly-vaulted-payment-method-global-id>" +
            "<payer-email>abc@test.com</payer-email>" +
            "<payment-id>1234567890</payment-id>" +
            "<payee-id>6789</payee-id>" +
            "<payee-email>payee@test.com</payee-email>" +
            "<payer-id>1357</payer-id>" +
            "<payer-first-name>Grace</payer-first-name>" +
            "<payer-last-name>Hopper</payer-last-name>" +
            "<payer-status>status</payer-status>" +
            "<recipient-email>test@paypal.com</recipient-email>" + 
            "<recipient-phone>" + 
                "<country-code>1</country-code>" + 
                "<national-number>4082222222</national-number>" + 
            "</recipient-phone>" +
            "<refund-from-transaction-fee-amount>2.00</refund-from-transaction-fee-amount>" +
            "<refund-from-transaction-fee-currency-iso-code>123</refund-from-transaction-fee-currency-iso-code>" +
            "<refund-id>8675309</refund-id>" +
            "<seller-protection-status>12345</seller-protection-status>" +
            "<tax-id>taxid</tax-id>" +
            "<tax-id-type>taxidtype</tax-id-type>" +
            "<token>token</token>" +
            "<transaction-fee-amount>10.00</transaction-fee-amount>" +
            "<transaction-fee-currency-iso-code>123</transaction-fee-currency-iso-code>" +
            "</paypal-details>";
            var node = nodeFromXml(xml);

            PayPalDetails details = new PayPalDetails(node);

            Assert.AreEqual("12345", details.AuthorizationId);
            Assert.AreEqual("billingagreementid", details.BillingAgreementId);
            Assert.AreEqual("2468", details.CaptureId);
            Assert.AreEqual("so custom much wow", details.CustomField);
            Assert.AreEqual("12345", details.DebugId);
            Assert.AreEqual("item", details.Description);
            Assert.AreEqual("www.image-url.com", details.ImageUrl);
            Assert.AreEqual("implicittoken", details.ImplicitlyVaultedPaymentMethodToken);
            Assert.AreEqual("implicitglobalid", details.ImplicitlyVaultedPaymentMethodGlobalId);
            Assert.AreEqual("abc@test.com", details.PayerEmail);
            Assert.AreEqual("1234567890", details.PaymentId);
            Assert.AreEqual("6789", details.PayeeId);
            Assert.AreEqual("payee@test.com", details.PayeeEmail);
            Assert.AreEqual("1357", details.PayerId);
            Assert.AreEqual("Grace", details.PayerFirstName);
            Assert.AreEqual("Hopper", details.PayerLastName);
            Assert.AreEqual("status", details.PayerStatus);
            Assert.AreEqual("test@paypal.com", details.RecipientEmail); 
            Assert.AreEqual("1", details.RecipientPhone.CountryCode); 
            Assert.AreEqual("4082222222", details.RecipientPhone.NationalNumber);
            Assert.AreEqual("2.00", details.RefundFromTransactionFeeAmount);
            Assert.AreEqual("123", details.RefundFromTransactionFeeCurrencyIsoCode);
            Assert.AreEqual("8675309", details.RefundId); 
            Assert.AreEqual("12345", details.SellerProtectionStatus);
            Assert.AreEqual("taxid", details.TaxId);
            Assert.AreEqual("taxidtype", details.TaxIdType);
            Assert.AreEqual("token", details.Token);
            Assert.AreEqual("10.00", details.TransactionFeeAmount);
            Assert.AreEqual("123", details.TransactionFeeCurrencyIsoCode);
        }
    }
}
