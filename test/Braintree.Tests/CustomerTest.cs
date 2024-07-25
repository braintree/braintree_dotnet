using Braintree.Exceptions;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Globalization;
using System.Text;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class CustomerTest
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
        public void Find_RaisesIfIdIsBlank()
        {
            Assert.Throws<NotFoundException>(() => gateway.Customer.Find("  "));
        }

        [Test]
        public void ConstructFromXMLResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<customer>");
            builder.Append("<first-name>Christine</first-name>");
            builder.Append("<last-name>Darden</last-name>");
            builder.Append("<company>NASA</company>");
            builder.Append("<email>test@paypal.com</email>");
            builder.Append("<website>www.paypal.com</website>");
            builder.Append("<phone>555-555-5555</phone>");
            builder.Append("<fax>555-555-5556</fax>");
            builder.Append("<international-phone><country-code>1</country-code><national-number>3121234567</national-number></international-phone>");
            builder.Append("</customer>");


            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());

            Customer customer = new Customer(new NodeWrapper(doc).GetNode("//customer"), gateway);
            Assert.AreEqual("Christine", customer.FirstName);
            Assert.AreEqual("Darden", customer.LastName);
            Assert.AreEqual("NASA", customer.Company);
            Assert.AreEqual("test@paypal.com", customer.Email);
            Assert.AreEqual("www.paypal.com", customer.Website);
            Assert.AreEqual("555-555-5555", customer.Phone);
            Assert.AreEqual("555-555-5556", customer.Fax);
            Assert.AreEqual("1", customer.InternationalPhone.CountryCode);
            Assert.AreEqual("3121234567", customer.InternationalPhone.NationalNumber);
        }
    }
}
