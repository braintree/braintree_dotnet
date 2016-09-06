using Braintree.Exceptions;
using NUnit.Framework;

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
    }
}
