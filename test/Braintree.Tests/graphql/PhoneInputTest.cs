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
    public class PhoneInputTest
    {
        [Test]
        public void TestToGraphQLVariables()
        {
            var phoneInput = PhoneInput.Builder()
            .CountryPhoneCode("1")
            .PhoneNumber("5555555555")
            .ExtensionNumber("5555")
            .Build();


            var dict = phoneInput.ToGraphQLVariables();
            
            Assert.AreEqual("1", dict["countryPhoneCode"]);
            Assert.AreEqual("5555555555", dict["phoneNumber"]);
            Assert.AreEqual("5555", dict["extensionNumber"]);
        }
    }
}
