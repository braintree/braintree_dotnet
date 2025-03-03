using Braintree.Exceptions;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Text;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class MetaCheckoutCardDetailsTest
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
            builder.Append("<transaction>");
            builder.Append("<bin>a-bin</bin>");
            builder.Append("<cardholder-name>Cardholder</cardholder-name>");
            builder.Append("<card-type>Visa</card-type>");
            builder.Append("<expiration-month>11</expiration-month>");
            builder.Append("<expiration-year>2024</expiration-year>");
            builder.Append("<customer-location>US</customer-location>");
            builder.Append("<default>false</default>");
            builder.Append("<expired>false</expired>");
            builder.Append("<unique-number-identifier>1234</unique-number-identifier>");
            builder.Append("<token>token1</token>");
            builder.Append("<created-at>2023-05-05T21:28:37Z</created-at>");
            builder.Append("<updated-at>2023-05-05T21:28:37Z</updated-at>");
            builder.Append("<prepaid>NO</prepaid>");
            builder.Append("<prepaid-reloadable>NO</prepaid-reloadable>");
            builder.Append("<payroll>NO</payroll>");
            builder.Append("<debit>NO</debit>");
            builder.Append("<commercial>NO</commercial>");
            builder.Append("<healthcare>NO</healthcare>");
            builder.Append("<container-id>a-container-id</container-id>" );
            builder.Append("<last-4>1234</last-4>");
            builder.Append("</transaction>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());

            MetaCheckoutCardDetails details = new MetaCheckoutCardDetails(new NodeWrapper(doc).GetNode("transaction"));

            Assert.AreEqual("11", details.ExpirationMonth);
            Assert.AreEqual("1234", details.LastFour);
            Assert.AreEqual("1234", details.UniqueNumberIdentifier);
            Assert.AreEqual("2024", details.ExpirationYear);
            Assert.AreEqual("a-bin", details.Bin);
            Assert.AreEqual("a-container-id", details.ContainerId);
            Assert.AreEqual("Cardholder", details.CardholderName);
            Assert.AreEqual("No", details.Commercial.GetDescription());
            Assert.AreEqual("No", details.Healthcare.GetDescription());
            Assert.AreEqual("No", details.Payroll.GetDescription());
            Assert.AreEqual("No", details.Prepaid.GetDescription());
            Assert.AreEqual("No", details.PrepaidReloadable.GetDescription());
            Assert.AreEqual("token1", details.Token);
            Assert.AreEqual("us", details.CustomerLocation.GetDescription());
            Assert.AreEqual("Visa", details.CardType.GetDescription());
            Assert.AreEqual(DateTime.Parse("2023-05-05T21:28:37Z"), details.CreatedAt);
            Assert.AreEqual(DateTime.Parse("2023-05-05T21:28:37Z"), details.UpdatedAt);
            Assert.AreEqual(false, details.IsExpired);
        }
    }
}
