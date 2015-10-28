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
        private string[] values = new string[] { "a", "b", "c", "d", "e" };

        [Test]
        [Category("Unit")]
        public void ResourceCollection_IteratesOverCollectionsProperly()
        {
            string body = @"<search-results>
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

            ResourceCollection<string> resourceCollection = new ResourceCollection<string>(xml, delegate(string[] ids) {
                List<string> results = new List<string>();

                foreach (string id in ids)
                {
                    results.Add(values[int.Parse(id)]);
                }

                return results;
            });

            int index = 0;
            int count = 0;
            foreach (string item in resourceCollection)
            {
                Assert.AreEqual(values[index], item);
                index++;
                count++;
            }

            Assert.AreEqual(values.Length, count);
        }
    }
}
