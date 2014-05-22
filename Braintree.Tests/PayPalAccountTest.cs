using System;
using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;
using Braintree.Test;

namespace Braintree.Tests
{
    [TestFixture]
    public class PayPalAccountTest
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
        public void Find_ReturnsPayPalAccountByToken()
        {
            Result<Customer> customerResult = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(customerResult.IsSuccess());

            var request = new PaymentMethodRequest
            {
                CustomerId = customerResult.Target.Id,
                PaymentMethodNonce = Nonce.PayPalFuturePayment
            };
            Result<PaymentMethod> result = gateway.PaymentMethod.Create(request);

            Assert.IsTrue(result.IsSuccess());

            PayPalAccount found = gateway.PayPalAccount.Find(result.Target.Token);
            Assert.IsNotNull(found);
            Assert.IsNotNull(found.Email);
            Assert.AreEqual(found.Email, ((PayPalAccount) result.Target).Email);
        }

        [Test]
        public void Find_RaisesNotFoundErrorForUnknownToken()
        {
            try {
                gateway.PayPalAccount.Find(" ");
                Assert.Fail("Should throw NotFoundException");
            } catch (NotFoundException) {}
        }

        [Test]
        public void Find_RaisesNotFoundErrorForCreditCardToken()
        {
            var createRequest = new CustomerRequest
            {
                CreditCard = new CreditCardRequest
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/12",
                    CVV = "123",
                }
            };

            Customer customer = gateway.Customer.Create(createRequest).Target;

            try {
                gateway.PayPalAccount.Find(customer.CreditCards[0].Token);
                Assert.Fail("Should throw NotFoundException");
            } catch (NotFoundException) {}
        }

        [Test]
        public void Delete_ReturnsPayPalAccountByToken()
        {
            Result<Customer> customerResult = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(customerResult.IsSuccess());

            var request = new PaymentMethodRequest
            {
                CustomerId = customerResult.Target.Id,
                PaymentMethodNonce = Nonce.PayPalFuturePayment
            };
            Result<PaymentMethod> result = gateway.PaymentMethod.Create(request);

            Assert.IsTrue(result.IsSuccess());

            gateway.PayPalAccount.Delete(result.Target.Token);
        }

        [Test]
        public void Delete_RaisesNotFoundErrorForUnknownToken()
        {
            try {
                gateway.PayPalAccount.Delete(" ");
                Assert.Fail("Should throw NotFoundException");
            } catch (NotFoundException) {}
        }

        [Test]
        public void Update_CanUpdateToken()
        {
            Result<Customer> customerResult = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(customerResult.IsSuccess());

            var request = new PaymentMethodRequest
            {
                CustomerId = customerResult.Target.Id,
                PaymentMethodNonce = Nonce.PayPalFuturePayment
            };
            Result<PaymentMethod> result = gateway.PaymentMethod.Create(request);

            Assert.IsTrue(result.IsSuccess());

            String newToken = Guid.NewGuid().ToString();
            var updateRequest = new PayPalAccountRequest
            {
                Token = newToken
            };
            var updateResult = gateway.PayPalAccount.Update(result.Target.Token, updateRequest);

            Assert.IsTrue(updateResult.IsSuccess());
            Assert.AreEqual(newToken, updateResult.Target.Token);
        }
    }
}
