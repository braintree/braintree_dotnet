using System;
using NUnit.Framework;

namespace Braintree.Tests
{
    //NOTE: good
    [TestFixture]
    public class Sha256HasherTest
    {
        [Test]
        public void HmacHash_ReturnsCorrectHash()
        {
            string actual = new Sha256Hasher().HmacHash("secretKey", "hello world");
            Assert.AreEqual("a31288ecf77d266463fc7e2a63799cb1ce6dcff156610373f722fa298e932340", actual);
        }
    }
}
