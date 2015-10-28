using System;
using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class Sha1HasherTest
    {
        [Test]
        [Category("Unit")]
        public void HmacHash_ReturnsCorrectHash()
        {
            string actual = new Sha1Hasher().HmacHash("secretKey", "hello world");
            Assert.AreEqual("D503D7A1A6ADBA1E6474E9FF2C4167F9DFDF4247", actual);
        }

        [Test]
        [Category("Unit")]
        public void Sha1Bytes_ReturnsCorrectHash()
        {
            byte[] bytes = new Sha1Hasher().Sha1Bytes("hello world");
            string hex = BitConverter.ToString(bytes);
            string actual = hex.Replace("-", "");
            Assert.AreEqual("2AAE6C35C94FCFB415DBE95F408B9CE91EE846ED", actual);
        }
    }
}
