using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Braintree;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class MerchantTest
    {
        private BraintreeGateway gateway;

        [SetUp]
        public void Setup()
        {
            gateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );

        }

        [Test]
        public void Create_ReturnsMerchantAndCredentials()
        {
            ResultImpl<Merchant> result = gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "USA",
                PaymentMethods = new string[] {"credit_card", "paypal"}
            });

            Assert.IsTrue(result.IsSuccess());
            Assert.IsFalse(string.IsNullOrEmpty(result.Target.Id));
            Assert.AreEqual("name@email.com", result.Target.Email);
            Assert.AreEqual("name@email.com", result.Target.CompanyName);
            Assert.AreEqual("USA", result.Target.CountryCodeAlpha3);
            Assert.AreEqual("US", result.Target.CountryCodeAlpha2);
            Assert.AreEqual("840", result.Target.CountryCodeNumeric);
            Assert.AreEqual("United States of America", result.Target.CountryName);

            Assert.IsTrue(result.Target.Credentials.AccessToken.StartsWith("access_token$"));
            Assert.IsTrue(result.Target.Credentials.ExpiresAt > DateTime.Now);
            Assert.IsTrue(string.IsNullOrEmpty(result.Target.Credentials.RefreshToken));
            Assert.AreEqual("bearer", result.Target.Credentials.TokenType);
        }

        [Test]
        public void Create_FailsWithInvalidPaymentMethods()
        {
            ResultImpl<Merchant> result = gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "USA",
                PaymentMethods = new string[] {"fake_money"}
            });

            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.MERCHANT_PAYMENT_METHODS_ARE_INVALID,
                result.Errors.ForObject("merchant").OnField("payment-methods")[0].Code
            );
        }
    }
}
