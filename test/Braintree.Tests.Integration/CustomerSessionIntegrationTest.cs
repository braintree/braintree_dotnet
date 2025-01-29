using Braintree;
using Braintree.GraphQL;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Braintree.Tests.Integration
{

    [TestFixture]
    public class CustomerSessionIntegrationTest
    {
        [Test]
        public void CreateCustomerSessionWithoutEmailAndPhone()
        {
            var input = CreateCustomerSessionInput.Builder()
            .MerchantAccountId("usd_pwpp_multi_account_merchant_account")
            .Build();

            var result = PwppGateway().CustomerSession.CreateCustomerSession(input);
            Assert.IsNotNull(result.Target);
        }

        [Test]
        public void CreateCustomerSessionWithMerchantProvidedSessionId()
        {
            string merchantSessionId = "11EF-A1E7-A5F5EE5C-A2E5-AFD2801469FC";
            var input = CreateCustomerSessionInput.Builder()
            .SessionId(merchantSessionId)
            .Build();

            var result = PwppGateway().CustomerSession.CreateCustomerSession(input);

            Assert.AreEqual(merchantSessionId, result.Target);
        }

        [Test]
        public void CreateCustomerSessionWithAPIDerivedSessionId()
        {
            Assert.IsNotNull(BuildCustomerSession(null).Target);
        }

        [Test]
        public void DoesNotCreateDuplicateCustomerSession()
        {
            string existingSessionId = "11EF-34BC-2702904B-9026-C3ECF4BAC765";

            var result = BuildCustomerSession(existingSessionId);

            Assert.IsFalse(result.IsSuccess());

            Assert.IsTrue(result.Errors.DeepAll()[0].Message.Contains("Session IDs must be unique per merchant"));
        }

        [Test]
        public void UpdateCustomerSession()
        {
            string sessionId = "11EF-A1E7-A5F5EE5C-A2E5-AFD2801469FC";
            var createInput = CreateCustomerSessionInput.Builder()
            .SessionId(sessionId)
            .MerchantAccountId("usd_pwpp_multi_account_merchant_account")
            .Build();

            PwppGateway().CustomerSession.CreateCustomerSession(createInput);

            var customer = BuildCustomerSessionInput("PR5_test@example.com", "4085005005");
            var input = UpdateCustomerSessionInput.Builder(sessionId)
            .Customer(customer)
            .Build();

            var result = PwppGateway().CustomerSession.UpdateCustomerSession(input);

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(sessionId, result.Target);
        }

        [Test]
        public void DoesNotUpdateNonExistentSession()
        {
            string sessionId = "11EF-34BC-2702904B-9026-C3ECF4BAC765";
            var customer = BuildCustomerSessionInput("PR9_test@example.com", "4085005009");
            var input = UpdateCustomerSessionInput.Builder(sessionId)
            .Customer(customer)
            .Build();

            var result = PwppGateway().CustomerSession.UpdateCustomerSession(input);
            Assert.IsFalse(result.IsSuccess());
            Assert.IsTrue(result.Errors.DeepAll()[0].Message.Contains("does not exist"));
        }
        [Test]
        public void GetCustomerRecommendations()
        {
            var customer = BuildCustomerSessionInput("PR5_test@example.com", "4085005005");
            var customerRecommendationsInput = CustomerRecommendationsInput.Builder(
                "11EF-A1E7-A5F5EE5C-A2E5-AFD2801469FC",
                new List<Recommendations> { Recommendations.PAYMENT_RECOMMENDATIONS }
                )
                .Customer(customer)
                .Build();

            var result = PwppGateway().CustomerSession.GetCustomerRecommendations(customerRecommendationsInput);

            Assert.IsTrue(result.IsSuccess());
            var payload = result.Target;
            Assert.IsTrue(payload.IsInPayPalNetwork);

            var recommendation = payload.Recommendations.PaymentOptions[0];
            Assert.AreEqual(RecommendedPaymentOption.PAYPAL, recommendation.PaymentOption);
            Assert.AreEqual(1, recommendation.RecommendedPriority);
        }

        [Test]
        public void DoesNotGetRecommendationsForNonExistentSessionId()
        {
            var customer = BuildCustomerSessionInput("PR9_test@example.com", "4085005009");
            var customerRecommendationsInput = CustomerRecommendationsInput.Builder(
                "11EF-34BC-2702904B-9026-C3ECF4BAC765",
                new List<Recommendations> { Recommendations.PAYMENT_RECOMMENDATIONS }
                )
                .Customer(customer)
                .Build();

            var result = PwppGateway().CustomerSession.GetCustomerRecommendations(customerRecommendationsInput);

            Assert.IsFalse(result.IsSuccess());
            Assert.IsTrue(result.Errors.DeepAll()[0].Message.Contains("does not exist"));
        }

        protected static BraintreeGateway PwppGateway()
        {
            return new BraintreeGateway(
                Environment.DEVELOPMENT,
                "pwpp_multi_account_merchant",
                "pwpp_multi_account_merchant_public_key",
                "pwpp_multi_account_merchant_private_key");
        }

        protected static Result<string> BuildCustomerSession(string sessionId)
        {
            var customer = BuildCustomerSessionInput("PR1_test@example.com", "4085005002");
            var builder = CreateCustomerSessionInput.Builder()
            .MerchantAccountId("usd_pwpp_multi_account_merchant_account")
            .Customer(customer);
            var input = (sessionId != null) ? builder.SessionId(sessionId).Build() : builder.Build();
            return PwppGateway().CustomerSession.CreateCustomerSession(input);
        }

        protected static CustomerSessionInput BuildCustomerSessionInput(string email, string phoneNumber)
        {
            var phoneInput = PhoneInput.Builder()
            .CountryPhoneCode("1")
            .PhoneNumber(phoneNumber)
            .Build();


            return CustomerSessionInput.Builder()
            .Email(email)
            .Phone(phoneInput)
            .DeviceFingerprintId("test")
            .PaypalAppInstalled(true)
            .VenmoAppInstalled(true)
            .UserAgent("Mozilla")
            .Build();
        }
    }
}