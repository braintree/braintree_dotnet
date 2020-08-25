using Braintree.Exceptions;
using NUnit.Framework;
using System;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransactionTest
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
        public void Find_FindsErrorsOutOnWhitespaceIds()
        {
            Assert.Throws<NotFoundException>(() => gateway.Transaction.Find(" "));
        }

        [Test]
        public void TransactionRequest_ToXml_Includes_SkipAdvancedFraudChecking()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2016",
                },
                Options = new TransactionOptionsRequest
                {
                    SkipAdvancedFraudChecking = false
                }
            };
            Assert.IsTrue(request.ToXml().Contains("<skip-advanced-fraud-checking>false</skip-advanced-fraud-checking>"));
        }

        [Test]
        public void UnrecognizedValuesAreCategorizedAsSuch()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "  <id>unrecognized_transaction_id</id>\n" +
                "  <status>unrecognizable status</status>\n" +
                "  <type>sale</type>\n" +
                "  <customer></customer>\n" +
                "  <billing></billing>\n" +
                "  <shipping></shipping>\n" +
                "  <custom-fields/>\n" +
                "  <gateway-rejection-reason>unrecognizable gateway rejection reason</gateway-rejection-reason>\n" +
                "  <credit-card></credit-card>\n" +
                "  <status-history type=\"array\"></status-history>\n" +
                "  <subscription></subscription>\n" +
                "  <descriptor></descriptor>\n" +
                "  <escrow-status>unrecognizable escrow status</escrow-status>\n" +
                "  <disbursement-details></disbursement-details>\n" +
                "  <payment-instrument-type>credit_card</payment-instrument-type>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);

            Assert.AreEqual(TransactionGatewayRejectionReason.UNRECOGNIZED, transaction.GatewayRejectionReason);
            Assert.AreEqual(TransactionEscrowStatus.UNRECOGNIZED, transaction.EscrowStatus);
            Assert.AreEqual(TransactionStatus.UNRECOGNIZED, transaction.Status);
        }

        [Test]
        public void RecognizesTokenIssuanceGatewayRejectReason()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "  <id></id>\n" +
                "  <status></status>\n" +
                "  <type>sale</type>\n" +
                "  <customer></customer>\n" +
                "  <billing></billing>\n" +
                "  <shipping></shipping>\n" +
                "  <custom-fields/>\n" +
                "  <gateway-rejection-reason>token_issuance</gateway-rejection-reason>\n" +
                "  <credit-card></credit-card>\n" +
                "  <status-history type=\"array\"></status-history>\n" +
                "  <subscription></subscription>\n" +
                "  <descriptor></descriptor>\n" +
                "  <escrow-status></escrow-status>\n" +
                "  <disbursement-details></disbursement-details>\n" +
                "  <payment-instrument-type>credit_card</payment-instrument-type>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);

            Assert.AreEqual(TransactionGatewayRejectionReason.TOKEN_ISSUANCE, transaction.GatewayRejectionReason);
            Assert.AreEqual(TransactionEscrowStatus.UNRECOGNIZED, transaction.EscrowStatus);
            Assert.AreEqual(TransactionStatus.UNRECOGNIZED, transaction.Status);
        }

        [Test]
        public void DeserializesLevel3SummaryFieldsFromXml()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "  <shipping-amount>1.00</shipping-amount>\n" +
                "  <discount-amount>2.00</discount-amount>\n" +
                "  <ships-from-postal-code>12345</ships-from-postal-code>\n" +
                "  <disbursement-details></disbursement-details>\n" +
                "  <subscription></subscription>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);

            Assert.AreEqual(1.00M, transaction.ShippingAmount);
            Assert.AreEqual(2.00M, transaction.DiscountAmount);
            Assert.AreEqual("12345", transaction.ShipsFromPostalCode);
        }

        [Test]
        public void DeserializesAuthorizationAdjustmentsFromXml()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "  <authorization-adjustments>\n" +
                "    <authorization-adjustment>\n" +
                "      <amount>10.00</amount>\n" +
                "      <success>true</success>\n" +
                "      <timestamp>2018-05-16T12:00:00+00:00</timestamp>\n" +
                "      <processor-response-code>1000</processor-response-code>\n" +
                "      <processor-response-text>Approved</processor-response-text>\n" +
                "    </authorization-adjustment>\n" +
                "  </authorization-adjustments>\n" +
                "  <disbursement-details></disbursement-details>\n" +
                "  <subscription></subscription>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);

            Assert.AreEqual(10.00M, transaction.AuthorizationAdjustments[0].Amount);
            Assert.AreEqual(true, transaction.AuthorizationAdjustments[0].Success);
            Assert.AreEqual(DateTime.Parse("5/16/18 12:00:00 PM"), transaction.AuthorizationAdjustments[0].Timestamp);
            Assert.AreEqual("1000", transaction.AuthorizationAdjustments[0].ProcessorResponseCode);
            Assert.AreEqual("Approved", transaction.AuthorizationAdjustments[0].ProcessorResponseText);
        }

        [Test]
        public void DeserializesNetworkTransactionIdFromXml()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "  <network-transaction-id>123456789012345</network-transaction-id>\n" +
                "  <disbursement-details></disbursement-details>\n" +
                "  <subscription></subscription>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);

            Assert.AreEqual("123456789012345", transaction.NetworkTransactionId);
        }

        [Test]
        public void DeserializesNetworkResponseCodeAndTextFromXml()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "  <network-response-code>00</network-response-code>\n" +
                "  <network-response-text>Approved</network-response-text>\n" +
                "  <disbursement-details></disbursement-details>\n" +
                "  <subscription></subscription>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);

            Assert.AreEqual("00", transaction.NetworkResponseCode);
            Assert.AreEqual("Approved", transaction.NetworkResponseText);
        }
    }
}
