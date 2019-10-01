using Braintree.Exceptions;
using NUnit.Framework;
using System;
using System.Xml;
using System.Text;

namespace Braintree.Tests
{
    [TestFixture]
    public class DisputeTest
    {
        private static readonly string TYPE_DATE = "type=\"date\"";
        private static readonly string TYPE_ARRAY = "type=\"array\"";
        private static readonly string NIL_TRUE = "nil=\"true\"";
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

        public string Payload_legacyAttributes()
        {
            return Node("dispute", 
                    Node("transaction",
                        Node("id", "transaction_id"),
                        Node("amount", "100.00")
                    ),
                    Node("id", "123456"),
                    Node("currency-iso-code", "USD"),
                    Node("status", "open"),
                    Node("amount", "100.00"),
                    NodeAttr("received-date", TYPE_DATE, "2016-02-22"),
                    NodeAttr("reply-by-date", TYPE_DATE, "2016-02-22"),
                    Node("reason", "fraud"),
                    NodeAttr("transaction-ids", TYPE_ARRAY,
                        Node("item", "asdf"),
                        Node("item", "qwer")
                    ),
                    NodeAttr("date-opened", TYPE_DATE, "2014-03-28"),
                    NodeAttr("date-won", TYPE_DATE, "2014-04-05"),
                    Node("kind", "chargeback")
                );
        }

        public string Payload_attributes()
        {
            return Node("dispute", 
                Node("id", "12345678"),
                Node("amount", "101.00"),
                Node("amount-disputed", "101.00"),
                Node("amount-won", "95.00"),
                Node("case-number", "CASE-12345"),
                NodeAttr("created-at", TYPE_DATE, "2017-06-16"),
                Node("currency-iso-code", "USD"),
                Node("processor-comments", "Processor comments"),
                Node("kind", "chargeback"),
                Node("merchant-account-id", "abc123"),
                Node("reason", "fraud"),
                Node("reason-code", "83"),
                Node("reason-description", "Reason code 83 description"),
                NodeAttr("received-date", TYPE_DATE, "2016-02-22"),
                NodeAttr("reply-by-date", TYPE_DATE, "2016-02-22"),
                Node("reference-number", "123456"),
                Node("status", "open"),
                NodeAttr("updated-at", TYPE_DATE, "2013-04-10"),
                Node("original-dispute-id", "original_dispute_id"),
                NodeAttr("status-history", TYPE_ARRAY,
                    Node("status-history",
                        Node("status", "open"),
                        NodeAttr("timestamp", TYPE_DATE, "2013-04-10T10:50:39Z"),
                        NodeAttr("effective-date", TYPE_DATE, "2013-04-10")
                    )
                ),
                NodeAttr("evidence", TYPE_ARRAY,
                    Node("evidence",
                        NodeAttr("created-at", TYPE_DATE, "2013-04-10T10:50:39Z"),
                        Node("id", "evidence1"),
                        Node("url", "url_of_file_evidence")
                    ),
                    Node("evidence",
                        NodeAttr("created-at", TYPE_DATE, "2013-04-10T10:50:39Z"),
                        Node("id", "evidence2"),
                        Node("comment", "text evidence"),
                        NodeAttr("sent-to-processor-at", TYPE_DATE, "2009-04-11")
                    )
                ),
                Node("transaction",
                    Node("id", "new_transaction_id"),
                    Node("amount", "101.00"),
                    NodeAttr("created-at", TYPE_DATE, "2017-06-21T20:44:41Z"),
                    NodeAttr("order-id", NIL_TRUE),
                    NodeAttr("purchase-order-number", NIL_TRUE),
                    Node("payment-instrument-subtype", "Visa")
                ),
                NodeAttr("date-opened", TYPE_DATE, "2014-03-28"),
                NodeAttr("date-won", TYPE_DATE, "2014-04-05")
            );
        }

        public string Payload_attributesWithEmptyValues()
        {
            return Node("dispute", 
                Node("id", "12345678"),
                Node("amount-disputed", "101.00"),
                Node("amount-won", "95.00"),
                Node("case-number", "CASE-12345"),
                NodeAttr("created-at", TYPE_DATE, "2017-06-16"),
                Node("currency-iso-code", "USD"),
                Node("processor-comments", "Processor comments"),
                Node("kind", "chargeback"),
                Node("merchant-account-id", "abc123"),
                Node("reason", "fraud"),
                Node("reason-code", "83"),
                Node("reason-description", "Reason code 83 description"),
                NodeAttr("received-date", TYPE_DATE, "2016-02-22"),
                Node("reference-number", "123456"),
                Node("status", "open"),
                NodeAttr("updated-at", TYPE_DATE, "2013-04-10"),
                Node("original-dispute-id", "original_dispute_id"),
                Node("transaction",
                    Node("id", "new_transaction_id"),
                    Node("amount", "101.00"),
                    NodeAttr("created-at", TYPE_DATE, "2017-06-21T20:44:41Z"),
                    NodeAttr("order-id", NIL_TRUE),
                    NodeAttr("purchase-order-number", NIL_TRUE),
                    Node("payment-instrument-subtype", "Visa")
                )
            );
        }

        [Test]
        public void Constructor_legacyAttributes()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Payload_legacyAttributes());
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);
            var result = new Dispute(node);

            Assert.AreEqual( "123456", result.Id);
            Assert.AreEqual(100m, result.Amount);
            Assert.AreEqual("USD", result.CurrencyIsoCode);
            Assert.AreEqual(DisputeReason.FRAUD, result.Reason);
            Assert.AreEqual(DisputeStatus.OPEN, result.Status);
            Assert.AreEqual("transaction_id", result.TransactionDetails.Id);
            Assert.AreEqual("100.00", result.TransactionDetails.Amount);
            Assert.AreEqual(DateTime.Parse("2014-03-28"), result.DateOpened);
            Assert.AreEqual(DateTime.Parse("2014-04-05"), result.DateWon);
            Assert.AreEqual(DisputeKind.CHARGEBACK, result.Kind);
        }

        [Test]
        public void Constructor_newAttributes()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Payload_attributes());
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);
            var result = new Dispute(node);
            Assert.AreEqual( "12345678", result.Id);
            Assert.AreEqual(101m, result.Amount);
            Assert.AreEqual("USD", result.CurrencyIsoCode);
            Assert.AreEqual(DisputeReason.FRAUD, result.Reason);
            Assert.AreEqual(DisputeStatus.OPEN, result.Status);
            Assert.AreEqual("new_transaction_id", result.TransactionDetails.Id);
            Assert.AreEqual("101.00", result.TransactionDetails.Amount);
            Assert.AreEqual(DateTime.Parse("2014-03-28"), result.DateOpened);
            Assert.AreEqual(DateTime.Parse("2014-04-05"), result.DateWon);
            Assert.AreEqual(DisputeKind.CHARGEBACK, result.Kind);
        }

        [Test]
        public void Constructor_populatesNewFields()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Payload_attributes());
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);
            var result = new Dispute(node);
            Assert.AreEqual(101m, result.AmountDisputed);
            Assert.AreEqual(95m, result.AmountWon);
            Assert.AreEqual("CASE-12345", result.CaseNumber);
            Assert.AreEqual(DateTime.Parse("2017-06-16"), result.CreatedAt);
            Assert.AreEqual("Processor comments", result.ProcessorComments);
            Assert.AreEqual("abc123", result.MerchantAccountId);
            Assert.AreEqual("original_dispute_id", result.OriginalDisputeId);
            Assert.AreEqual("83", result.ReasonCode);
            Assert.AreEqual("Reason code 83 description", result.ReasonDescription);
            Assert.AreEqual("123456", result.ReferenceNumber);
            Assert.AreEqual(DateTime.Parse("2013-04-10"), result.UpdatedAt);
            Assert.AreEqual(DisputeStatus.OPEN, result.StatusHistory[0].Status);
            Assert.AreEqual(DateTime.Parse("2013-04-10T10:50:39Z"), result.StatusHistory[0].Timestamp);
            Assert.AreEqual(DateTime.Parse("2013-04-10"), result.StatusHistory[0].EffectiveDate);
            Assert.AreEqual("evidence1", result.Evidence[0].Id);
            Assert.AreEqual("url_of_file_evidence", result.Evidence[0].Url);
            Assert.AreEqual(DateTime.Parse("2013-04-10T10:50:39Z"), result.Evidence[0].CreatedAt);
            Assert.IsNull(result.Evidence[0].Comment);
            Assert.IsNull(result.Evidence[0].SentToProcessorAt);
            Assert.AreEqual("evidence2", result.Evidence[1].Id);
            Assert.AreEqual("text evidence", result.Evidence[1].Comment);
            Assert.AreEqual(DateTime.Parse("2013-04-10T10:50:39Z"), result.Evidence[1].CreatedAt);
            Assert.AreEqual(DateTime.Parse("2009-04-11"), result.Evidence[1].SentToProcessorAt);
            Assert.IsNull(result.Evidence[1].Url);
        }

        [Test]
        public void Constructor_handlesEmptyFields()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Payload_attributesWithEmptyValues());
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);
            var result = new Dispute(node);
            Assert.IsNull(result.Amount);
            Assert.IsNull(result.DateOpened);
            Assert.IsNull(result.DateWon);
            Assert.IsEmpty(result.Evidence);
            Assert.IsNull(result.ReplyByDate);
            Assert.IsEmpty(result.StatusHistory);
        }

        [Test]
        public void Constructor_populatesTransaction()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Payload_attributes());
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);
            var result = new Dispute(node);
            Assert.AreEqual("new_transaction_id", result.Transaction.Id);
            Assert.AreEqual(101m, result.Transaction.Amount);
            Assert.IsNull(result.Transaction.OrderId);
            Assert.AreEqual("Visa", result.Transaction.PaymentInstrumentSubtype);
            Assert.IsNull(result.Transaction.PurchaseOrderNumber);
            Assert.AreEqual(DateTime.Parse("2017-06-21T20:44:41Z"), result.Transaction.CreatedAt);
        }

        private static string Node(string name, params string[] contents) {
            return NodeAttr(name, null, contents);
        }

        private static string NodeAttr(string name, string attributes, params string[] contents) {
            StringBuilder buffer = new StringBuilder();
            buffer.Append('<').Append(name);
            if (attributes != null)
            {
                buffer.Append(" ").Append(attributes);
            }
            buffer.Append('>');
            foreach (string content in contents)
            {
                buffer.Append(content);
            }
            buffer.Append("</").Append(name).Append('>');
            return buffer.ToString();
        }
    }
}
