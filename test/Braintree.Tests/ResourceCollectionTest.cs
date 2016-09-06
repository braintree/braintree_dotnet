using NUnit.Framework;
using System.Collections.Generic;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class ResourceCollectionTest
    {
        private string[] values = new string[] { "a", "b", "c", "d", "e" };

        [Test]
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

        [Test]
        public void Ids_ReturnsAllIdsInCollection()
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
                return new List<string>();
            });

            List<string> assertIds = new List<string>() {"0","1","2","3","4"};
            Assert.AreEqual(resourceCollection.Ids, assertIds);
        }

        [Test]
        public void Ids_ReturnsEmptyListWhenNoIds()
        {
            string body = @"<search-results>
                              <page-size>2</page-size>
                              <ids type='array'>
                              </ids>
                            </search-results>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(body);
            NodeWrapper xml = new NodeWrapper(doc.ChildNodes[0]);

            ResourceCollection<string> resourceCollection = new ResourceCollection<string>(xml, delegate(string[] ids) {
                return new List<string>();
            });

            List<string> assertIds = new List<string>();
            Assert.AreEqual(resourceCollection.Ids, assertIds);
        }
    }
}
