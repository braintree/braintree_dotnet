using System;
using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class CryptoTest
    {
        [Test]
        public void SecureCompare_ReturnsTrueForEqualStrings()
        {
            Assert.IsTrue(new Crypto().SecureCompare("a_string", "a_string"));
        }

        [Test]
        public void SecureCompare_ReturnsFalseForDifferentLengthStrings()
        {
            Assert.IsFalse (new Crypto ().SecureCompare ("a_string", "a_long_string"));
        }

        [Test]
        public void SecureCompare_ReturnsFalseForDifferentStringsOfSameLength()
        {
            Assert.IsFalse (new Crypto ().SecureCompare ("a_string", "a_strong"));
        }
    }
}
