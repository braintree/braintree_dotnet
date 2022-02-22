using Braintree.Exceptions;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Text;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class AddressTest
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
        public void Find_FindsErrorsOutOnWhitespaceAddressId()
        {
            Assert.Throws<NotFoundException>(() => gateway.Address.Find(" ", "address_id"));
        }

        [Test]
        public void Find_FindsErrorsOutOnWhitespaceCustomerId()
        {
            Assert.Throws<NotFoundException>(() => gateway.Address.Find("customer_id", " "));
        }

        [Test]
        public void ConstructFromXMLResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<address>");
            builder.Append("<id>id</id>");
            builder.Append("<customer-id>custid</customer-id>");
            builder.Append("<first-name>Christine</first-name>");
            builder.Append("<last-name>Darden</last-name>");
            builder.Append("<company>NASA</company>");
            builder.Append("<street-address>123 xyz street</street-address>");
            builder.Append("<extended-address>floor 2</extended-address>");
            builder.Append("<locality>Orlando</locality>");
            builder.Append("<region>FL</region>");
            builder.Append("<postal-code>11111</postal-code>");
            builder.Append("<country-code-alpha2>US</country-code-alpha2>");
            builder.Append("<country-code-alpha3>USA</country-code-alpha3>");
            builder.Append("<country-code-numeric>1</country-code-numeric>");
            builder.Append("<country-name>United States</country-name>");
            builder.Append("<phone-number>555-555-5555</phone-number>");
            builder.Append("<created-at type='datetime'>2018-10-10T22:46:41Z</created-at>");
            builder.Append("<updated-at type='datetime'>2020-10-10T22:46:41Z</updated-at>");
            builder.Append("</address>");


            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());

            Address address = new Address(new NodeWrapper(doc).GetNode("//address"));
            Assert.AreEqual("id", address.Id);
            Assert.AreEqual("custid", address.CustomerId);
            Assert.AreEqual("Christine", address.FirstName);
            Assert.AreEqual("Darden", address.LastName);
            Assert.AreEqual("NASA", address.Company);
            Assert.AreEqual("123 xyz street", address.StreetAddress);
            Assert.AreEqual("floor 2", address.ExtendedAddress);
            Assert.AreEqual("Orlando", address.Locality);
            Assert.AreEqual("FL", address.Region);
            Assert.AreEqual("11111", address.PostalCode);
            Assert.AreEqual("US", address.CountryCodeAlpha2);
            Assert.AreEqual("USA", address.CountryCodeAlpha3);
            Assert.AreEqual("1", address.CountryCodeNumeric);
            Assert.AreEqual("United States", address.CountryName);
            Assert.AreEqual("555-555-5555", address.PhoneNumber);
            Assert.AreEqual("10/10/2018 22:46:41", address.CreatedAt.ToString());
            Assert.AreEqual("10/10/2020 22:46:41", address.UpdatedAt.ToString());
        }
    }
}
