using System;
using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class CoinbaseAccountTest
    {
        private BraintreeGateway gateway;

        [SetUp]
        public void Setup()
        {
            gateway = new BraintreeGateway();
        }

        [Test]
        public void TransactionCreate()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.Coinbase
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            if (result.IsSuccess())
            {
                CoinbaseDetails details = result.Target.CoinbaseDetails;
                Assert.IsNotNull(details);

                Assert.AreNotEqual("", details.UserEmail);
                Assert.IsNotNull(details.UserEmail);

                Assert.AreNotEqual("", details.UserName);
                Assert.IsNotNull(details.UserName);

                Assert.AreNotEqual("", details.UserId);
                Assert.IsNotNull(details.UserId);
            }
            else
                StringAssert.Contains("does not support", result.Message);
        }

        [Test]
        public void Vault()
        {
            Result<Customer> customerResult = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(customerResult.IsSuccess());

            PaymentMethodRequest request = new PaymentMethodRequest()
            {
                CustomerId = customerResult.Target.Id,
                PaymentMethodNonce = Nonce.Coinbase
            };
            var createResult = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(createResult.IsSuccess());

            var findResult = gateway.PaymentMethod.Find(createResult.Target.Token);
            Assert.IsTrue(findResult is CoinbaseAccount);
        }

        [Test]
        public void Customer()
        {
            CustomerRequest request = new CustomerRequest
            {
                PaymentMethodNonce = Nonce.Coinbase
            };
            Result<Customer> customerResult = gateway.Customer.Create(request);
            Assert.IsTrue(customerResult.IsSuccess());


            var customer = gateway.Customer.Find(customerResult.Target.Id);
            Assert.AreEqual(1, customer.CoinbaseAccounts.Length);

            CoinbaseAccount account = customer.CoinbaseAccounts[0];

            Assert.AreNotEqual("", account.UserEmail);
            Assert.IsNotNull(account.UserEmail);

            Assert.AreNotEqual("", account.UserName);
            Assert.IsNotNull(account.UserName);

            Assert.AreNotEqual("", account.UserId);
            Assert.IsNotNull(account.UserId);

            Assert.AreEqual(1, customer.PaymentMethods.Length);
            Assert.AreEqual(customer.PaymentMethods[0], customer.CoinbaseAccounts[0]);
        }
    }
}
