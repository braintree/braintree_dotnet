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
    public class MetaCheckoutTokenTest
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
            builder.Append("<bin>a-bin</bin>");
            builder.Append("<business>NO</business>");
            builder.Append("<card-type>Visa</card-type>");
            builder.Append("<cardholder-name>Cardholder</cardholder-name>");
            builder.Append("<commercial>NO</commercial>");
            builder.Append("<consumer>NO</consumer>");
            builder.Append("<container-id>a-container-id</container-id>" );
            builder.Append("<corporate>NO</corporate>");
            builder.Append("<created-at>2023-05-05T21:28:37Z</created-at>");
            builder.Append("<cryptogram>AlhlvxmN2ZKuAAESNFZ4GoABFA==</cryptogram>");
            builder.Append("<customer-id>customer-id</customer-id>");
            builder.Append("<customer-location>US</customer-location>");
            builder.Append("<debit>NO</debit>");
            builder.Append("<default>false</default>");
            builder.Append("<ecommerce-indicator>07</ecommerce-indicator>");
            builder.Append("<expiration-month>11</expiration-month>");
            builder.Append("<expiration-year>2024</expiration-year>");
            builder.Append("<expired>false</expired>");
            builder.Append("<healthcare>NO</healthcare>");
            builder.Append("<last-4>1234</last-4>");
            builder.Append("<payroll>NO</payroll>");
            builder.Append("<prepaid-reloadable>NO</prepaid-reloadable>");
            builder.Append("<prepaid>NO</prepaid>");
            builder.Append("<purchase>NO</purchase>");
            builder.Append("<token>token1</token>");
            builder.Append("<unique-number-identifier>1234</unique-number-identifier>");
            builder.Append("<updated-at>2023-05-05T21:28:37Z</updated-at>");
            builder.Append("</payment-method>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());

            MetaCheckoutToken card = new MetaCheckoutToken(new NodeWrapper(doc).GetNode("payment-method"), gateway);

            Assert.AreEqual("07", card.ECommerceIndicator);
            Assert.AreEqual("11", card.ExpirationMonth);
            Assert.AreEqual("1234", card.LastFour);
            Assert.AreEqual("1234", card.UniqueNumberIdentifier);
            Assert.AreEqual("2024", card.ExpirationYear);
            Assert.AreEqual("a-bin", card.Bin);
            Assert.AreEqual("a-container-id", card.ContainerId);
            Assert.AreEqual("AlhlvxmN2ZKuAAESNFZ4GoABFA==", card.Cryptogram);
            Assert.AreEqual("Cardholder", card.CardholderName);
            Assert.AreEqual("customer-id", card.CustomerId);
            Assert.AreEqual("No", card.Business.GetDescription());
            Assert.AreEqual("No", card.Commercial.GetDescription());
            Assert.AreEqual("No", card.Consumer.GetDescription());
            Assert.AreEqual("No", card.Corporate.GetDescription());
            Assert.AreEqual("No", card.Healthcare.GetDescription());
            Assert.AreEqual("No", card.Payroll.GetDescription());
            Assert.AreEqual("No", card.Prepaid.GetDescription());
            Assert.AreEqual("No", card.PrepaidReloadable.GetDescription());
            Assert.AreEqual("No", card.Purchase.GetDescription());
            Assert.AreEqual("token1", card.Token);
            Assert.AreEqual("us", card.CustomerLocation.GetDescription());
            Assert.AreEqual("Visa", card.CardType.GetDescription());
            Assert.AreEqual(DateTime.Parse("2023-05-05T21:28:37Z"), card.CreatedAt);
            Assert.AreEqual(DateTime.Parse("2023-05-05T21:28:37Z"), card.UpdatedAt);
            Assert.AreEqual(false, card.IsDefault);
            Assert.AreEqual(false, card.IsExpired);
        }
    }
}
