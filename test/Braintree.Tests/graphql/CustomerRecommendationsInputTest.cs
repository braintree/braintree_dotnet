using Braintree;
using Braintree.GraphQL;
using NUnit.Framework;
using System.Collections.Generic;

namespace Braintree.Tests.GraphQL
{
    [TestFixture]
    public class CustomerRecommendationsInputTest
    {
        [Test]
        public void ToGraphQLVariables()
        {

            var phoneInput = PhoneInput.Builder()
            .CountryPhoneCode("1")
            .PhoneNumber("5555555555")
            .ExtensionNumber("5555")
            .Build();

            var customerSessionInput = CustomerSessionInput.Builder()
            .Email("nobody@nowehwere.com")
            .Phone(phoneInput)
            .DeviceFingerprintId("device-fingerprint-id")
            .PaypalAppInstalled(false)
            .VenmoAppInstalled(true)
            .Build();

            var payee = PayPalPayeeInput.Builder()
            .EmailAddress("test@example.com")
            .ClientId("merchant-public-id")
            .Build();

            var amount = new MonetaryAmountInput("300.0", "USD");

            var purchaseUnit = PayPalPurchaseUnitInput.Builder(amount)
            .Payee(payee)
            .Build();

            var purchaseUnits = new List<PayPalPurchaseUnitInput>
            {
                purchaseUnit
            };

            var input = CustomerRecommendationsInput.Builder()
            .SessionId("session-id")
            .Customer(customerSessionInput)
            .PurchaseUnits(purchaseUnits)
            .Domain("test.com")
            .MerchantAccountId("merchant-account-id")
            .Build();

            var dict = input.ToGraphQLVariables();

            Assert.AreEqual("session-id", dict["sessionId"]);

            CollectionAssert.AreEquivalent(
                customerSessionInput.ToGraphQLVariables(),
                (System.Collections.IEnumerable)dict["customer"]
            );

            CollectionAssert.AreEquivalent(
                purchaseUnits[0].ToGraphQLVariables(),
                (System.Collections.IEnumerable)((System.Collections.IList)dict["purchaseUnits"])[0]
            );

            Assert.AreEqual("test.com", dict["domain"]);

            Assert.AreEqual("merchant-account-id", dict["merchantAccountId"]);
        }
    }
}
