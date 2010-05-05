using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Braintree;

namespace Braintree.Tests
{
    [TestFixture]
    public class CollectionUtilTest
    {
        [Test]
        public void Find_FindsValidCollectionValue()
        {
            Assert.AreEqual(SubscriptionStatus.ACTIVE, CollectionUtil.Find(SubscriptionStatus.STATUSES, "active", SubscriptionStatus.UNRECOGNIZED));
        }

        [Test]
        public void Find_ReturnsUNRECOGNIZEDForInValidEnumValue()
        {
            Assert.AreEqual(SubscriptionDurationUnit.UNRECOGNIZED, CollectionUtil.Find(SubscriptionDurationUnit.ALL, "pants", SubscriptionDurationUnit.UNRECOGNIZED));
        }
    }
}
