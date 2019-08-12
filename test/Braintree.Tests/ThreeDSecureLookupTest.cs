using Braintree.Exceptions;
using NUnit.Framework;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Braintree.Tests
{
    [TestFixture]
    public class ThreeDSecureLookupTest
    {

        [Test]
        public void SetsPropertiesFromDynamicObject() {
            var rawJSON = @"{
                ""acsUrl"": ""https://braintreepayments.com"",
                ""threeDSecureVersion"": ""2.0"",
                ""transactionId"": ""123-txn-id"",
                ""unused"": ""value""
            }";
            var lookupResponse = JsonConvert.DeserializeObject<dynamic>(rawJSON);

            ThreeDSecureLookup lookup = new ThreeDSecureLookup(lookupResponse);

            Assert.AreEqual("https://braintreepayments.com", lookup.AcsUrl);
            Assert.AreEqual("2.0", lookup.ThreeDSecureVersion);
            Assert.AreEqual("123-txn-id", lookup.TransactionId);
        }
    }
}
