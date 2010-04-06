using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using Braintree;

namespace Braintree.Tests
{
    [TestFixture]
    public class PagedCollectionTest
    {
        [Test]
        public void TotalPages_WithFullPages()
        {
            PagedCollection<Subscription> collection = TestHelper.MockPagedCollection<Subscription>(1, 10, 5);
            Assert.AreEqual(2, collection.TotalPages());
        }

        [Test]
        public void TotalPages_WithPartialPages()
        {
            PagedCollection<Subscription> collection = TestHelper.MockPagedCollection<Subscription>(1, 11, 5);
            Assert.AreEqual(3, collection.TotalPages());
        }

        [Test]
        public void IsLastPage_OnFirstPage()
        {
            PagedCollection<Subscription> collection = TestHelper.MockPagedCollection<Subscription>(1, 10, 5);
            Assert.IsFalse(collection.IsLastPage());
        }

        [Test]
        public void IsLastPage_OnLastPage()
        {
            PagedCollection<Subscription> collection = TestHelper.MockPagedCollection<Subscription>(2, 10, 5);
            Assert.IsTrue(collection.IsLastPage());
        }

        [Test]
        public void IsLastPage_OnRandomPage()
        {
            PagedCollection<Subscription> collection = TestHelper.MockPagedCollection<Subscription>(2, 50, 5);
            Assert.IsFalse(collection.IsLastPage());
        }
    }
}
