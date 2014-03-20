using System;
using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class SignatureServiceTest
    {
        class FakeHasher : Hasher
        {
            public String HmacHash(String key, String payload)
            {
              return payload + "-signed-with-" + key;
            }
        }

        [Test]
        public void Sign_SignsTheGivenPayload()
        {
            String signature = new SignatureService
            {
              Key = "secret-key",
              Hasher = new FakeHasher()
            }.Sign("my-payload");
            Assert.AreEqual("my-payload-signed-with-secret-key|my-payload", signature);
        }
    }
}
