using Braintree.Exceptions;
using NUnit.Framework;
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
        public void SaleTrData_ReturnsValidTrDataHash()
        {
            string trData = gateway.Transaction.SaleTrData(new TransactionRequest(), "http://example.com");
            Assert.IsTrue(trData.Contains("sale"));
            Assert.IsTrue(TrUtil.IsTrDataValid(trData, service));
        }

        [Test]
        public void CreditTrData_ReturnsValidTrDataHash()
        {
            string trData = gateway.Transaction.CreditTrData(new TransactionRequest(), "http://example.com");
            Assert.IsTrue(trData.Contains("credit"));
            Assert.IsTrue(TrUtil.IsTrDataValid(trData, service));
        }

        [Test]
        public void TrData_QueryStringParams()
        {
            string trData = gateway.Transaction.SaleTrData(new TransactionRequest {
                Options = new TransactionOptionsRequest
                {
                    StoreInVault = true,
                    AddBillingAddressToPaymentMethod = true,
                    StoreShippingAddressInVault = true,
                    SubmitForSettlement = true
                }
            }, "http://example.com");
            Assert.IsTrue(trData.Contains("store_in_vault"));
            Assert.IsTrue(trData.Contains("add_billing_address_to_payment_method"));
            Assert.IsTrue(trData.Contains("store_shipping_address_in_vault"));
            Assert.IsTrue(trData.Contains("submit_for_settlement"));

            trData = gateway.Transaction.SaleTrData(new TransactionRequest {
                Options = new TransactionOptionsRequest
                {
                }
            }, "http://example.com");
            Assert.IsFalse(trData.Contains("store_in_vault"));
            Assert.IsFalse(trData.Contains("add_billing_address_to_payment_method"));
            Assert.IsFalse(trData.Contains("store_shipping_address_in_vault"));
            Assert.IsFalse(trData.Contains("submit_for_settlement"));

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
    }
}
