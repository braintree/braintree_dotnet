using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class AmountBreakdownRequestTest
    {
        [Test]
        public void ToXml_IncludesAllData()
        {
            var request = new AmountBreakdownRequest()
            {
                Discount = 15,
                Handling = 0,
                Insurance = 5,
                ItemTotal = 45,
                Shipping = 10,
                ShippingDiscount = 0,
                TaxTotal = 10,
            };
            

            string xmlString = request.ToXml();

            Assert.IsTrue(xmlString.Contains("<amount-breakdown>"));
            Assert.IsTrue(xmlString.Contains("<discount>15.00</discount>"));
            Assert.IsTrue(xmlString.Contains("<handling>0.00</handling>"));
            Assert.IsTrue(xmlString.Contains("<insurance>5.00</insurance>"));
            Assert.IsTrue(xmlString.Contains("<item-total>45.00</item-total>"));
            Assert.IsTrue(xmlString.Contains("<shipping>10.00</shipping>"));
            Assert.IsTrue(xmlString.Contains("<shipping-discount>0.00</shipping-discount>"));
            Assert.IsTrue(xmlString.Contains("<tax-total>10.00</tax-total>"));
            Assert.IsTrue(xmlString.Contains("</amount-breakdown>"));
        }
    }
}