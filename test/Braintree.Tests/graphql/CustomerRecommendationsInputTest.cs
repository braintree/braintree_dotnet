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
            var recommendations = new List<Recommendations> { Recommendations.PAYMENT_RECOMMENDATIONS };

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

            var input = CustomerRecommendationsInput.Builder("session-id", recommendations)
            .MerchantAccountId("merchant-account-id")
            .Customer(customerSessionInput)
            .Build();



            var dict = input.ToGraphQLVariables();

            Assert.AreEqual("merchant-account-id", dict["merchantAccountId"]);
            Assert.AreEqual("session-id", dict["sessionId"]);
            CollectionAssert.AreEqual(recommendations[0].ToString(), ((System.Collections.IList)dict["recommendations"])[0].ToString()); 
            CollectionAssert.AreEquivalent(
                customerSessionInput.ToGraphQLVariables(),
                (System.Collections.IEnumerable)dict["customer"]
            );
        }
    }
}
