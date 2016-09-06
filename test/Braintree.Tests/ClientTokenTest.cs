using NUnit.Framework;
using System;
using System.Text.RegularExpressions;

namespace Braintree.Tests
{
    [TestFixture]
    public class ClientTokenTest
    {
        [Test]
        public void Generate_RaisesExceptionIfVerifyCardIsIncludedWithoutCustomerId()
        {
            BraintreeGateway gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
            Exception exception = null;
            try {
                gateway.ClientToken.generate(
                    new ClientTokenRequest
                    {
                        Options = new ClientTokenOptionsRequest
                        {
                            VerifyCard = true
                        }
                    }
                );
            } catch (Exception tempException) {
                exception = tempException;
            }
            Assert.IsNotNull(exception);
            Assert.IsTrue(Regex.Match(exception.Message, @"VerifyCard").Success);
            Assert.IsInstanceOf(typeof(ArgumentException), exception);
        }

        [Test]
        public void Generate_RaisesExceptionIfMakeDefaultIsIncludedWithoutCustomerId()
        {
            BraintreeGateway gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
            Exception exception = null;
            try {
                gateway.ClientToken.generate(
                    new ClientTokenRequest
                    {
                        Options = new ClientTokenOptionsRequest
                        {
                            MakeDefault = true
                        }
                    }
                );
            } catch (Exception tempException) {
                exception = tempException;
            }
            Assert.IsNotNull(exception);
            Assert.IsTrue(Regex.Match(exception.Message, @"MakeDefault").Success);
            Assert.IsInstanceOf(typeof(ArgumentException), exception);
        }

        [Test]
        public void Generate_RaisesExceptionIfFailOnDuplicatePaymentMethodIsIncludedWithoutCustomerId()
        {
            BraintreeGateway gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
            Exception exception = null;
            try {
                gateway.ClientToken.generate(
                    new ClientTokenRequest
                    {
                        Options = new ClientTokenOptionsRequest
                        {
                            FailOnDuplicatePaymentMethod = true
                        }
                    }
                );
            } catch (Exception tempException) {
                exception = tempException;
            }
            Assert.IsNotNull(exception);
            Assert.IsTrue(Regex.Match(exception.Message, @"FailOnDuplicatePaymentMethod").Success);
            Assert.IsInstanceOf(typeof(ArgumentException), exception);
        }
    }
}
