using NUnit.Framework;
using System;

namespace Braintree.Tests
{
    [TestFixture]
    public class ShippingOptionRequestTest
    {
        [Test]
        public void ToXml_IncludesAllData()
        {
            var request = new ShippingOptionRequest()
            {
                Amount = 10,
                Id = "option1",
                Label = "fast",
                Selected = true,
                Type = "SHIPPING",

            };
            

            string xmlString = request.ToXml();

            Assert.IsTrue(xmlString.Contains("<shipping-option>"));
            Assert.IsTrue(xmlString.Contains("<amount>10.00</amount>"));
            Assert.IsTrue(xmlString.Contains("<id>option1</id>"));
            Assert.IsTrue(xmlString.Contains("<label>fast</label>"));
            Assert.IsTrue(xmlString.Contains("<selected>true</selected>"));
            Assert.IsTrue(xmlString.Contains("<type>SHIPPING</type>"));
            Assert.IsTrue(xmlString.Contains("</shipping-option>"));
        }
    }
}