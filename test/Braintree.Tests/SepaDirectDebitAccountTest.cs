
using System.Xml;
using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class SepaDirectDebitAccountTest
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
            string xml = "<sepa-debit-account>" 
                + "<bank-reference-token>a-bank-reference-token</bank-reference-token>" 
                + "<customer-global-id>a-customer-global-id</customer-global-id>"
                + "<customer-id>a-customer-id</customer-id>"
                + "<created-at>2021-05-05T21:28:37Z</created-at>"
                + "<default type=\"boolean\">true</default>"
                + "<global-id>a-global-id</global-id>"
                + "<image-url>https://jsdk.docker.dev:9000/payment_method_logo/sepa_debit_account.png?environment=test</image-url>"
                + "<last-4>1234</last-4>"
                + "<mandate-type>RECURRENT</mandate-type>"
                + "<merchant-account-id>a-merchant-account-id</merchant-account-id>"
                + "<merchant-or-partner-customer-id>a-mp-customer-id</merchant-or-partner-customer-id>"
                + "<token>ch6byss</token>"
                + "<updated-at>2021-05-05T21:28:37Z</updated-at>"
                + "<view-mandate-url>https://paypal.com/</view-mandate-url>"
                + "</sepa-debit-account>"; 
            var node = nodeFromXml(xml);


            BraintreeGateway gw = new BraintreeGateway();
            SepaDirectDebitAccount account = new SepaDirectDebitAccount(node, gw);

            Assert.AreEqual("a-bank-reference-token", account.BankReferenceToken);
            Assert.AreEqual("a-customer-global-id", account.CustomerGlobalId);
            Assert.AreEqual("a-customer-id", account.CustomerId);
            Assert.AreEqual(true, account.IsDefault);
            Assert.AreEqual("a-global-id", account.GlobalId);
            Assert.AreEqual("https://jsdk.docker.dev:9000/payment_method_logo/sepa_debit_account.png?environment=test", account.ImageUrl);
            Assert.AreEqual("1234", account.Last4);
            Assert.AreEqual(MandateType.RECURRENT, account.MandateType);
            Assert.AreEqual("a-merchant-account-id", account.MerchantAccountId);
            Assert.AreEqual("a-mp-customer-id", account.MerchantOrPartnerCustomerId);
            Assert.AreEqual("ch6byss", account.Token);
            Assert.AreEqual("https://paypal.com/", account.ViewMandateUrl);
            Assert.NotNull(account.CreatedAt);
            Assert.NotNull(account.UpdatedAt);
        }
    }
}
