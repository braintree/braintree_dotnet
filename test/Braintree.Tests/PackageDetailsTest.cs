using Braintree.Exceptions;
using NUnit.Framework;
using System;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class PackageDetailsTest
    {
        private BraintreeGateway gateway;
        private BraintreeService service;

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

            service = new BraintreeService(gateway.Configuration);
        }

        [Test]
        public void DeserializesPackagesFromXml()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
            "<transaction>\n" +
            "  <id>recognized_transaction_id</id>\n" +
            "  <shipments><shipment>\n" +
            "  <id>track_id</id><tracking-number>tracking_number_1</tracking-number>\n" +
            // NEXT_MAJOR_VERSION Remove paypal-tracking-id from this response
            "  <carrier>UPS</carrier><paypal-tracking-id>pp_tracking_number_1</paypal-tracking-id>" +
            "  <paypal-tracker-id>pp_tracker_id</paypal-tracker-id>" +
            "  </shipment></shipments>\n" +
            "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);
            Assert.AreEqual(transaction.Packages[0].Id, "track_id");
            Assert.AreEqual(transaction.Packages[0].TrackingNumber, "tracking_number_1");
            Assert.AreEqual(transaction.Packages[0].Carrier, "UPS");
            // NEXT_MAJOR_VERSION Remove PaypalTrackingId assertion
            Assert.AreEqual(transaction.Packages[0].PaypalTrackingId, "pp_tracking_number_1");
            Assert.AreEqual(transaction.Packages[0].PaypalTrackerId, "pp_tracker_id");
        }

        [Test]
        public void DeserializesTransactionFromXml_IfNoPackages()
        {
            // testing if shipments tag is not present
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
            "<transaction>\n" +
            "  <id>recognized_transaction_id</id>\n" +
            "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);
            Assert.AreEqual(transaction.Packages.Length, 0);

            // testing if shipments tag is present but has no shipments
            xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
            "<transaction>\n" +
            "  <id>recognized_transaction_id</id>\n" +
            "<shipments></shipments>\n" +
            "</transaction>\n";

            doc = new XmlDocument();
            doc.LoadXml(xml);
            newNode = doc.DocumentElement;
            node = new NodeWrapper(newNode);

            transaction = new Transaction(node, gateway);
            Assert.AreEqual(transaction.Packages.Length, 0);
        }
    }
}