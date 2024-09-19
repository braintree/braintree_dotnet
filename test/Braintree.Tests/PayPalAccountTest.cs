using Braintree.Exceptions;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Globalization;
using System.Text;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class PayPalAccountTest
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
            builder.Append("<paypal-account>");
            builder.Append("<billing-agreement-id>billingagreementid</billing-agreement-id>");
            builder.Append("<created-at type=\"datetime\">2018-04-12T19:54:16Z</created-at>");
            builder.Append("<customer-id>1396526238</customer-id>");
            builder.Append("<edit-paypal-vault-id>BA-ID1</edit-paypal-vault-id>");
            builder.Append("<email>some-email</email>");
            builder.Append("<funding-source-description>VISA 1234</funding-source-description>");
            builder.Append("<image-url>https://google.com/image.png</image-url>");
            builder.Append("<default>true</default>");
            builder.Append("<payer-id>1357</payer-id>");
            builder.Append("<revoked-at type=\"datetime\">2019-05-13T20:55:17Z</revoked-at>");
            builder.Append("<subscriptions type=\"array\">");
            builder.Append("</subscriptions>");
            builder.Append("<token>token</token>");
            builder.Append("<updated-at type=\"datetime\">2018-04-12T19:54:16Z</updated-at>");
            builder.Append("</paypal-account>");


            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());

            PayPalAccount paypalAccount = new PayPalAccount(new NodeWrapper(doc).GetNode("//paypal-account"), gateway);
            Assert.AreEqual("billingagreementid", paypalAccount.BillingAgreementId);
            Assert.AreEqual("4/12/2018 7:54:16 PM", paypalAccount.CreatedAt?.ToString(CultureInfo.GetCultureInfo("en-US")));
            Assert.AreEqual("1396526238", paypalAccount.CustomerId);
            Assert.AreEqual("BA-ID1", paypalAccount.EditPayPalVaultId);
            Assert.AreEqual("some-email", paypalAccount.Email);
            Assert.AreEqual("VISA 1234", paypalAccount.FundingSourceDescription);
            Assert.AreEqual("https://google.com/image.png", paypalAccount.ImageUrl);
            Assert.IsTrue(paypalAccount.IsDefault);
            Assert.AreEqual("1357", paypalAccount.PayerId);
            Assert.AreEqual("5/13/2019 8:55:17 PM", paypalAccount.RevokedAt?.ToString(CultureInfo.GetCultureInfo("en-US")));
            Assert.AreEqual(0, paypalAccount.Subscriptions.Length);
            Assert.AreEqual("token", paypalAccount.Token);
            Assert.AreEqual("4/12/2018 7:54:16 PM", paypalAccount.UpdatedAt?.ToString(CultureInfo.GetCultureInfo("en-US")));
        }
    }
}
