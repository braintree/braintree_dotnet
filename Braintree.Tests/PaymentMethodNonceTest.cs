using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Braintree.Exceptions;

using Params = System.Collections.Generic.Dictionary<string, object>;

namespace Braintree.Tests
{
    //NOTE: good
    [TestFixture]
    public class PaymentMethodNonceTest
    {
        private BraintreeGateway gateway;

        [SetUp]
        public void Setup()
        {
            gateway = new BraintreeGateway();
        }

        [Test]
        public void Create_CreatesPaymentMethodNonce()
        {
            string nonce = TestHelper.GenerateUnlockedNonce(gateway);
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
        public void Create_RaisesNotFoundErrorWhenTokenDoesntExist()
        {
            try
            {
                gateway.PaymentMethodNonce.Create("notarealtoken");
                Assert.Fail("Should have raised NotFoundException");
            }
            catch(NotFoundException) {}
        }

        [Test]
        public void Find_ExposesThreeDSecureInfo()
        {
            try {
                PaymentMethodNonce nonce = gateway.PaymentMethodNonce.Find("threedsecurednonce");
                ThreeDSecureInfo info = nonce.ThreeDSecureInfo;

                Assert.AreEqual(nonce.Nonce, "threedsecurednonce");
                Assert.AreEqual(nonce.Type, "CreditCard");
                Assert.AreEqual(info.Enrolled, "Y");
                Assert.AreEqual(info.Status, "authenticate_successful");
                Assert.AreEqual(info.LiabilityShifted, true);
                Assert.AreEqual(info.LiabilityShiftPossible, true);
            }
            catch(NotFoundException ex)
            {
                Assert.Inconclusive(ex.Message);
            }
        }

        [Test]
        public void Find_ExposesNullThreeDSecureInfoIfBlank()
        {
            string nonce = TestHelper.GenerateUnlockedNonce(gateway);

            PaymentMethodNonce foundNonce = gateway.PaymentMethodNonce.Find(nonce);

            Assert.IsNull(foundNonce.ThreeDSecureInfo);
        }

        [Test]
        public void Find_RaisesNotFoundErrorWhenTokenDoesntExist()
        {
            try
            {
                gateway.PaymentMethodNonce.Find("notarealnonce");
                Assert.Fail("Should have raised NotFoundException");
            }
            catch(NotFoundException) {}
        }

    }
}
