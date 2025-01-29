using Braintree;
using Braintree.GraphQL;
using Braintree.TestUtil;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;

namespace Braintree.Tests.GraphQL
{
    [TestFixture]
    public class CreateCustomerSessionInputTest
    {
        [Test]
        public void TestToGraphQLVariables()
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
            .VenmoAppInstalled(false)
            .Build();
            var input = CreateCustomerSessionInput.Builder()
                        .MerchantAccountId("merchant-account-id")
                        .SessionId("session-id")
                        .Customer(customerSessionInput)
                        .Domain("a-domain")
                        .Build();

            var dict = input.ToGraphQLVariables();

            Assert.AreEqual("merchant-account-id", dict["merchantAccountId"]);
            Assert.AreEqual("session-id", dict["sessionId"]);

            CollectionAssert.AreEquivalent(
                customerSessionInput.ToGraphQLVariables(),
                (System.Collections.IEnumerable)dict["customer"]
            );


            CollectionAssert.AreEquivalent(
                customerSessionInput.Phone.ToGraphQLVariables(),
                (System.Collections.IEnumerable)((Dictionary<string, object>)dict["customer"])["phone"]
            );

            Assert.AreEqual("a-domain", dict["domain"]);
        }
    }
}
