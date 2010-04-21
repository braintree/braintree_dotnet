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
        public void Test_FirstItem_IsNull()
        {
            PagedCollection<Subscription> collection = TestHelper.MockPagedCollection<Subscription>(0);
            Assert.IsNull(collection.FirstItem);
        }

        [Test]
        public void Test_FirstItem_WithResults()
        {
            List<String> strings = new List<String>();
            strings.Add("abc");
            strings.Add("def");
            strings.Add("ghi");

            PagedCollection<String> collection = new PagedCollection<String>(strings, 3, null);
            Assert.AreEqual("abc", collection.FirstItem);
        }
    }
}
