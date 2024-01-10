using Braintree.Exceptions;
using NUnit.Framework;
using System;
using System.Xml;
using System.Collections.Generic;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransactionLineItemTest
    {
        private BraintreeGateway gateway;

        [SetUp]
        public void Setup() {}

        [Test]
        public void DeserializesPackagesFromXml()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<line-items><line-item>\n" +
                " <commodity-code>250300</commodity-code>\n" +
                " <description>This is the item 1</description>\n" +
                " <discount-amount>2.50</discount-amount>\n" +
                " <kind>debit</kind>\n" +
                " <name>Item 1</name>\n" +
                " <product-code>RM 90</product-code>\n" +
                " <quantity>2</quantity>\n" +
                " <tax-amount>2.85</tax-amount>\n" +
                " <total-amount>10.00</total-amount>\n" +
                " <unit-amount>10</unit-amount>\n" +
                " <unit-of-measure>gallons</unit-of-measure>\n" +
                " <unit-tax-amount>1.11</unit-tax-amount>\n" +
                " <upc-code>042100005264</upc-code>\n" +
                " <upc-type>UPC-A</upc-type>\n" +
                " <url>https://example.com/products/RM-90</url>\n" +
                " <image-url>https://example.com/products/RM-90.jpeg</image-url>\n" +
                " </line-item>\n" +
                " <line-item>\n" +
                " <commodity-code>250311</commodity-code>\n" +
                " <description>This is the description</description>\n" +
                " <discount-amount>1.50</discount-amount>\n" +
                " <kind>unrecognized</kind>\n" +
                " <name>This is the name</name>\n" +
                " <product-code>RM 91</product-code>\n" +
                " <quantity>1</quantity>\n" +
                " <tax-amount>1.50</tax-amount>\n" +
                " <total-amount>8.00</total-amount>\n" +
                " <unit-amount>8</unit-amount>\n" +
                " <unit-of-measure>count</unit-of-measure>\n" +
                " <unit-tax-amount>1.50</unit-tax-amount>\n" +
                " <upc-code>042100005212</upc-code>\n" +
                " <upc-type>UPC-A</upc-type>\n" +
                " <url>https://example.com/products/RM-91</url>\n" +
                " <image-url>https://example.com/products/RM-91.jpeg</image-url>\n" +
                " </line-item></line-items>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var response = new NodeWrapper(newNode);

            var transactionLineItems = new List<TransactionLineItem>();

            foreach (var node in response.GetList("line-item"))
            {
                transactionLineItems.Add(new TransactionLineItem(node));
            }

            Assert.AreEqual(transactionLineItems[0].CommodityCode, "250300");
            Assert.AreEqual(transactionLineItems[0].Description, "This is the item 1");
            Assert.AreEqual(transactionLineItems[0].DiscountAmount, 2.50);
            Assert.AreEqual(transactionLineItems[0].Kind, TransactionLineItemKind.DEBIT);
            Assert.AreEqual(transactionLineItems[0].Name, "Item 1");
            Assert.AreEqual(transactionLineItems[0].ProductCode, "RM 90");
            Assert.AreEqual(transactionLineItems[0].Quantity, 2);
            Assert.AreEqual(transactionLineItems[0].TaxAmount, 2.85);
            Assert.AreEqual(transactionLineItems[0].TotalAmount, 10);
            Assert.AreEqual(transactionLineItems[0].UnitAmount, 10);
            Assert.AreEqual(transactionLineItems[0].UnitOfMeasure, "gallons");
            Assert.AreEqual(transactionLineItems[0].UnitTaxAmount, 1.11);
            Assert.AreEqual(transactionLineItems[0].UpcCode, "042100005264");
            Assert.AreEqual(transactionLineItems[0].UpcType, "UPC-A");
            Assert.AreEqual(transactionLineItems[0].Url, "https://example.com/products/RM-90");
            Assert.AreEqual(transactionLineItems[0].ImageUrl, "https://example.com/products/RM-90.jpeg");

            Assert.AreEqual(transactionLineItems[1].CommodityCode, "250311");
            Assert.AreEqual(transactionLineItems[1].Description, "This is the description");
            Assert.AreEqual(transactionLineItems[1].DiscountAmount, 1.50);
            Assert.AreEqual(transactionLineItems[1].Kind, TransactionLineItemKind.UNRECOGNIZED);
            Assert.AreEqual(transactionLineItems[1].Name, "This is the name");
            Assert.AreEqual(transactionLineItems[1].ProductCode, "RM 91");
            Assert.AreEqual(transactionLineItems[1].Quantity, 1);
            Assert.AreEqual(transactionLineItems[1].TaxAmount, 1.50);
            Assert.AreEqual(transactionLineItems[1].TotalAmount, 8);
            Assert.AreEqual(transactionLineItems[1].UnitAmount, 8);
            Assert.AreEqual(transactionLineItems[1].UnitOfMeasure, "count");
            Assert.AreEqual(transactionLineItems[1].UnitTaxAmount, 1.50);
            Assert.AreEqual(transactionLineItems[1].UpcCode, "042100005212");
            Assert.AreEqual(transactionLineItems[1].UpcType, "UPC-A");
            Assert.AreEqual(transactionLineItems[1].Url, "https://example.com/products/RM-91");
            Assert.AreEqual(transactionLineItems[1].ImageUrl, "https://example.com/products/RM-91.jpeg");
        }
    }
}

