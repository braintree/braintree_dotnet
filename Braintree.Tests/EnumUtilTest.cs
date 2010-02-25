using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Braintree;

namespace Braintree.Tests
{
    [TestFixture]
    class EnumUtilTest
    {
        [Test]
        public void Find_FindsValidEnumValue()
        {
            Assert.AreEqual(TransactionStatus.AUTHORIZED, EnumUtil.Find(typeof(TransactionStatus), "authorized", "unrecognized"));
        }

        [Test]
        public void Find_ReturnsUNRECOGNIZEDForInValidEnumValue()
        {
            Assert.AreEqual(TransactionStatus.UNRECOGNIZED, EnumUtil.Find(typeof(TransactionStatus), "pants", "unrecognized"));
        }
    }
}
