using NUnit.Framework;
using System;

namespace Braintree.Tests
{
    [TestFixture]
    public class CreditCardOptionsRequestTest
    {
        [Test]
        public void ToXml_IncludesAllProperties()
        {
            var request = new CreditCardOptionsRequest()
            {
                FailOnDuplicatePaymentMethod = false,
                FailOnDuplicatePaymentMethodForCustomer = false,
                MakeDefault = false,
                SkipAdvancedFraudChecking = false,
                UpdateExistingToken = "foo",
                // NEXT_MAJOR_VERSION Remove VenmoSdkSession
                // The old venmo SDK integration has been deprecated
                VenmoSdkSession = "session",
                VerificationAccountType = "type",
                VerificationAmount = "0.01",
                VerificationCurrencyIsoCode = "123",
                VerificationMerchantAccountId = "merchantaccount",
                VerifyCard = true
            };

            Assert.IsTrue(request.ToXml("credit-card-options").Contains("<fail-on-duplicate-payment-method>false</fail-on-duplicate-payment-method>"));
            Assert.IsTrue(request.ToXml("credit-card-options").Contains("<fail-on-duplicate-payment-method-for-customer>false</fail-on-duplicate-payment-method-for-customer>"));
            Assert.IsTrue(request.ToXml("credit-card-options").Contains("<make-default>false</make-default>"));
            Assert.IsTrue(request.ToXml("credit-card-options").Contains("<skip-advanced-fraud-checking>false</skip-advanced-fraud-checking>"));
            Assert.IsTrue(request.ToXml("credit-card-options").Contains("<update-existing-token>foo</update-existing-token>"));
            // NEXT_MAJOR_VERSION Remove this assertion
            Assert.IsTrue(request.ToXml("credit-card-options").Contains("<venmo-sdk-session>session</venmo-sdk-session>"));
            Assert.IsTrue(request.ToXml("credit-card-options").Contains("<verification-account-type>type</verification-account-type>"));
            Assert.IsTrue(request.ToXml("credit-card-options").Contains("<verification-amount>0.01</verification-amount>"));
            Assert.IsTrue(request.ToXml("credit-card-options").Contains("<verification-currency-iso-code>123</verification-currency-iso-code>"));
            Assert.IsTrue(request.ToXml("credit-card-options").Contains("<verification-merchant-account-id>merchantaccount</verification-merchant-account-id>"));
            Assert.IsTrue(request.ToXml("credit-card-options").Contains("<verify-card>true</verify-card>"));
        }
    }
}
