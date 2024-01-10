using NUnit.Framework;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransactionLineItemRequestTest
    {
        [Test]
        public void toXml()
        {
            var request = new TransactionLineItemRequest()
            {
                CommodityCode = "250300",
                Description = "This is the item 1",
                DiscountAmount = 2.50M,
                LineItemKind = TransactionLineItemKind.DEBIT,
                Name = "Item 1",
                ProductCode = "RM 90",
                Quantity = 2M,
                TaxAmount = 2.85M,
                TotalAmount = 10.00M,
                UnitAmount = 10.00M,
                UnitOfMeasure = "gallons",
                UnitTaxAmount = 1.11M,
                UpcCode = "042100005264",
                UpcType = "UPC-A",
                Url = "https://example.com/products/RM-90",
                ImageUrl = "https://example.com/products/RM-90.jpeg"
            };

            string xml = request.ToXml("line-item");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            Assert.IsTrue(xml.Contains("<line-item>"));
            Assert.IsTrue(xml.Contains("<commodity-code>250300</commodity-code>"));
            Assert.IsTrue(xml.Contains("<description>This is the item 1</description>"));
            Assert.IsTrue(xml.Contains("<discount-amount>2.50</discount-amount>"));
            Assert.IsTrue(xml.Contains("<kind>debit</kind>"));
            Assert.IsTrue(xml.Contains("<name>Item 1</name>"));
            Assert.IsTrue(xml.Contains("<product-code>RM 90</product-code>"));
            Assert.IsTrue(xml.Contains("<quantity>2</quantity>"));
            Assert.IsTrue(xml.Contains("<tax-amount>2.85</tax-amount>"));
            Assert.IsTrue(xml.Contains("<total-amount>10.00</total-amount>"));
            Assert.IsTrue(xml.Contains("<unit-amount>10.00</unit-amount>"));
            Assert.IsTrue(xml.Contains("<unit-of-measure>gallons</unit-of-measure>"));
            Assert.IsTrue(xml.Contains("<unit-tax-amount>1.11</unit-tax-amount>"));
            Assert.IsTrue(xml.Contains("<upc-code>042100005264</upc-code>"));
            Assert.IsTrue(xml.Contains("<upc-type>UPC-A</upc-type>"));
            Assert.IsTrue(xml.Contains("<url>https://example.com/products/RM-90</url>"));
            Assert.IsTrue(xml.Contains("<image-url>https://example.com/products/RM-90.jpeg</image-url>"));
            Assert.IsTrue(xml.Contains("</line-item>"));
        }
    }
}
