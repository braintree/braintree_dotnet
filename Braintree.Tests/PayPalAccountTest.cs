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
    }
}
