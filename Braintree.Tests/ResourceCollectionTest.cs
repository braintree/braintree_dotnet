using System;
using System.Text;
using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;
using Braintree;

namespace Braintree.Tests
{
    [TestFixture]
    public class ResourceCollectionTest
    {
        private String[] values = new String[] { "a", "b", "c", "d", "e" };

        [Test]
        public void ResourceCollection_IteratesOverCollectionsProperly()
        {
            String body = @"<search-results>
                              <page-size>2</page-size>
                              <ids type='array'>
                                <items>0</items>
                                <items>1</items>
                                <items>2</items>
                                <items>3</items>
                                <items>4</items>
                              </ids>
                            </search-results>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(body);
            NodeWrapper xml = new NodeWrapper(doc.ChildNodes[0]);

            ResourceCollection<String> resourceCollection = new ResourceCollection<String>(xml, delegate(String[] ids) {
                List<String> results = new List<String>();

                foreach (String id in ids)
                {
                    results.Add(values[Int32.Parse(id)]);
                }

                return results;
            });

            int index = 0;
            int count = 0;
            foreach (String item in resourceCollection)
            {
                Assert.AreEqual(values[index], item);
                index++;
                count++;
            }

            Assert.AreEqual(values.Length, count);
        }
    }
}
