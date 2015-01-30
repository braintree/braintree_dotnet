using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Braintree;
using Braintree.Exceptions;
using Braintree.Test;

using Params = System.Collections.Generic.Dictionary<string, object>;

namespace Braintree.Tests
{
    [TestFixture]
    public class PaymentMethodNonceTest
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
        public void Create_CreatesPaymentMethodNonce()
        {
            String nonce = TestHelper.GenerateUnlockedNonce(gateway);
            Result<Customer> customerResult = gateway.Customer.Create(new CustomerRequest());

            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(new PaymentMethodRequest
            {
                CustomerId = customerResult.Target.Id,
                PaymentMethodNonce = nonce
            });

            Result<PaymentMethodNonce> result = gateway.PaymentMethodNonce.Create(paymentMethodResult.Target.Token);
            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target);
            Assert.IsNotNull(result.Target.Nonce);
        }

        [Test]
        public void Find_RaisesNotFoundErrorWhenTokenDoesntExist()
        {
            try
            {
                gateway.PaymentMethodNonce.Create("notarealtoken");
                Assert.Fail("Should have raised NotFoundException");
            }
            catch(NotFoundException) {}
        }
    }
}
