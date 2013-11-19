using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;
using System.Text.RegularExpressions;
using System.Web;
using System.Text;

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
