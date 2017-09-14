using Braintree.Test;
using NUnit.Framework;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class CoinbaseIntegrationTest
    {
        private BraintreeGateway gateway;

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
        }

        [Test]
        public void TransactionCreate_NoLongerSupported()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.Coinbase
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.PAYMENT_METHOD_NO_LONGER_SUPPORTED , result.Errors.ForObject("Transaction").OnField("Base")[0].Code);
        }

        [Test]
        public void Vault_NoLongerSupported()
        {
            Result<Customer> customerResult = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(customerResult.IsSuccess());

            PaymentMethodRequest request = new PaymentMethodRequest()
            {
                CustomerId = customerResult.Target.Id,
                PaymentMethodNonce = Nonce.Coinbase
            };
            var createResult = gateway.PaymentMethod.Create(request);
            Assert.IsFalse(createResult.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.PAYMENT_METHOD_NO_LONGER_SUPPORTED, createResult.Errors.ForObject("CoinbaseAccount").OnField("Base")[0].Code);
        }

        [Test]
        public void Customer_NoLongerSupported()
        {
            CustomerRequest request = new CustomerRequest
            {
                PaymentMethodNonce = Nonce.Coinbase
            };
            Result<Customer> customerResult = gateway.Customer.Create(request);
            Assert.IsFalse(customerResult.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.PAYMENT_METHOD_NO_LONGER_SUPPORTED, customerResult.Errors.ForObject("CoinbaseAccount").OnField("Base")[0].Code);
        }
    }
}
