using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Braintree;

namespace Braintree.Tests
{
    [TestFixture]
    public class EnumUtilTest
    {
        [Test]
        public void Find_FindsValidEnumValue()
        {
            Assert.AreEqual(SubscriptionDurationUnit.DAY, EnumUtil.Find(typeof(SubscriptionDurationUnit), "day", "unrecognized"));
        }

        [Test]
        public void Find_ReturnsUNRECOGNIZEDForInValidEnumValue()
        {
            Assert.AreEqual(SubscriptionDurationUnit.UNRECOGNIZED, EnumUtil.Find(typeof(SubscriptionDurationUnit), "pants", "unrecognized"));
        }
    }
}
