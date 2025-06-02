using Braintree;
using Braintree.GraphQL;
using Braintree.TestUtil;
using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Braintree.Tests.GraphQL
{
    [TestFixture]
    public class CustomerSessionInputTest
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
            .HashedEmail("a-hashed-email-address")
            .Phone(phoneInput)
            .HashedPhoneNumber("a-hashed-phone-number")
            .DeviceFingerprintId("device-fingerprint-id")
            .PaypalAppInstalled(false)
            .VenmoAppInstalled(true)
            .UserAgent("Mozilla")
            .Build();

            var dict = customerSessionInput.ToGraphQLVariables();

            Assert.AreEqual("nobody@nowehwere.com", dict["email"]);
            CollectionAssert.AreEquivalent(
                phoneInput.ToGraphQLVariables(),
                (System.Collections.IEnumerable)dict["phone"]
            );
            Assert.AreEqual("a-hashed-email-address", dict["hashedEmail"]);
            Assert.AreEqual("a-hashed-phone-number", dict["hashedPhoneNumber"]);
            Assert.IsFalse((bool)dict["paypalAppInstalled"]);
            Assert.IsTrue((bool)dict["venmoAppInstalled"]);
            Assert.AreEqual("Mozilla", dict["userAgent"]);
        }
    }
}
