using NUnit.Framework;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class PackageTrackingRequestTest
    {
        [Test]
        public void SerializesToXml()
        {
            var lineItems = new TransactionLineItemRequest[2]
            {
                new TransactionLineItemRequest
                {
                    Quantity = 1,
                    Name = "Best Product Ever",
                    ProductCode = "ABC 01",
                    Description = "Best Description Ever",
                    UpcCode = "93486946",
                    UpcType = "UPC-A",
                    ImageUrl = "https://example.com/image.png"
                },
                new TransactionLineItemRequest
                {
                    Quantity = 1,
                    Name = "Best Product Ever",
                    ProductCode = "ABC 02",
                    Description = "Best Description Ever",
                    UpcCode = "4759867",
                    UpcType = "UPC-B",
                    ImageUrl = "https://example.com/image2.png"

                },
            };
            var request = new PackageTrackingRequest
            {
                TrackingNumber = "tracking_number_1",
                Carrier = "UPS",
                NotifyPayer = false,
                LineItems = lineItems,
            };

            Assert.AreEqual(
                "<shipment>" +
                    "<carrier>UPS</carrier>" +
                    "<line-items type=\"array\">" +
                    "<item>" +
                    "<description>Best Description Ever</description>" +
                    "<image-url>https://example.com/image.png</image-url>" +
                    "<name>Best Product Ever</name>" +
                    "<product-code>ABC 01</product-code>" +
                    "<quantity>1</quantity>" +
                    "<upc-code>93486946</upc-code>" +
                    "<upc-type>UPC-A</upc-type>" +
                    "</item>" +
                    "<item>" +
                    "<description>Best Description Ever</description>" +
                    "<image-url>https://example.com/image2.png</image-url>" +
                    "<name>Best Product Ever</name>" +
                    "<product-code>ABC 02</product-code>" +
                    "<quantity>1</quantity>" +
                    "<upc-code>4759867</upc-code>" +
                    "<upc-type>UPC-B</upc-type>" +
                    "</item>" +
                    "</line-items>" +
                    "<notify-payer>false</notify-payer>" +
                    "<tracking-number>tracking_number_1</tracking-number>" +
            "</shipment>", request.ToXml());
        }
    }
}
