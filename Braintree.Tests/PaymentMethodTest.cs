using System;
using System.Collections.Generic;
using NUnit.Framework;
using Braintree;

namespace Braintree.Tests
{
    [TestFixture]
    public class PaymentMethodTest
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
        public void Create_CreatesPayPalAccountWithFuturePaymentNonce()
        {
            String nonce = TestHelper.GenerateFuturePaymentPayPalNonce(gateway);
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());

            var request = new PaymentMethodRequest
            {
                CustomerId = result.Target.Id,
                PaymentMethodNonce = nonce
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);

            Assert.IsTrue(paymentMethodResult.IsSuccess());
            Assert.IsNotNull(paymentMethodResult.Target.Token);
        }
    }
}
