using NUnit.Framework;
using System;

namespace Braintree.Tests
{
    [TestFixture]
    public class PaymentMethodOptionsRequestTest
    {
        [Test]
        public void ToXml_IncludesAllProperties()
        {
            var request = new PaymentMethodOptionsRequest()
            {
                FailOnDuplicatePaymentMethod = false,
                MakeDefault = false,
                SkipAdvancedFraudChecking = false,
                VerificationAccountType = "type",
                VerificationAmount = "0.01",
                VerificationCurrencyIsoCode = "123",
                VerificationMerchantAccountId = "merchantaccount",
                VerifyCard = true
            };
            
            Assert.IsTrue(request.ToXml("payment-method-options").Contains("<fail-on-duplicate-payment-method>false</fail-on-duplicate-payment-method>"));
            Assert.IsTrue(request.ToXml("payment-method-options").Contains("<make-default>false</make-default>"));
            Assert.IsTrue(request.ToXml("payment-method-options").Contains("<skip-advanced-fraud-checking>false</skip-advanced-fraud-checking>"));
            Assert.IsTrue(request.ToXml("payment-method-options").Contains("<verification-account-type>type</verification-account-type>"));
            Assert.IsTrue(request.ToXml("payment-method-options").Contains("<verification-amount>0.01</verification-amount>"));
            Assert.IsTrue(request.ToXml("payment-method-options").Contains("<verification-currency-iso-code>123</verification-currency-iso-code>"));
            Assert.IsTrue(request.ToXml("payment-method-options").Contains("<verification-merchant-account-id>merchantaccount</verification-merchant-account-id>"));
            Assert.IsTrue(request.ToXml("payment-method-options").Contains("<verify-card>true</verify-card>"));
        }
    }
}
