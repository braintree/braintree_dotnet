using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Braintree;

namespace Braintree.Tests
{
    [TestFixture]
    public class CryptoTest
    {
        [Test]
        public void HmacHash_ReturnsCorrectHash()
        {
            String actual = new Crypto().HmacHash("secretKey", "hello world");
            Assert.AreEqual("D503D7A1A6ADBA1E6474E9FF2C4167F9DFDF4247", actual);
        }

        [Test]
        public void Sha1Bytes_ReturnsCorrectHash()
        {
            byte[] bytes = new Crypto().Sha1Bytes("hello world");
            String hex = BitConverter.ToString(bytes);
            String actual = hex.Replace("-", "");
            Assert.AreEqual("2AAE6C35C94FCFB415DBE95F408B9CE91EE846ED", actual);
        }
    }
}
