using NUnit.Framework;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class NodeWrapperTest
    {
        [Test]
        public void GetEnum_ReturnsEnumValue()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                + "<outer-element>"
                + "  <element>second</element>"
                + "</outer-element>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode node = doc.DocumentElement;
            NodeWrapper nodeWrapper = new NodeWrapper(node);

            TestEnum value = nodeWrapper.GetEnum("element", TestEnum.UNRECOGNIZED);
            Assert.AreEqual(TestEnum.SECOND, value);
        }

        [Test]
        public void GetEnum_WhenElementNotFound_ReturnsDefaultValue()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                + "<outer-element></outer-element>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode node = doc.DocumentElement;
            NodeWrapper nodeWrapper = new NodeWrapper(node);

            TestEnum value = nodeWrapper.GetEnum("missing-element", TestEnum.UNRECOGNIZED);
            Assert.AreEqual(TestEnum.UNRECOGNIZED, value);
        }

        [Test]
        public void GetEnum_WhenDescriptionNotFound_ReturnsDefaultValue()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                + "<outer-element>"
                + "  <element>invalid</element>"
                + "</outer-element>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode node = doc.DocumentElement;
            NodeWrapper nodeWrapper = new NodeWrapper(node);

            TestEnum value = nodeWrapper.GetEnum("element", TestEnum.UNRECOGNIZED);
            Assert.AreEqual(TestEnum.UNRECOGNIZED, value);
        }

        [Test]
        public void NullableGetEnum_ReturnsEnumValue()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                + "<outer-element>"
                + "  <element>second</element>"
                + "</outer-element>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode node = doc.DocumentElement;
            NodeWrapper nodeWrapper = new NodeWrapper(node);

            TestEnum? value = nodeWrapper.GetEnum<TestEnum>("element");
            Assert.AreEqual(TestEnum.SECOND, value);
        }

        [Test]
        public void NullableGetEnum_WhenElementNotFound_ReturnsDefaultValue()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                + "<outer-element></outer-element>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode node = doc.DocumentElement;
            NodeWrapper nodeWrapper = new NodeWrapper(node);

            TestEnum? value = nodeWrapper.GetEnum("missing-element", TestEnum.UNRECOGNIZED);
            Assert.AreEqual(TestEnum.UNRECOGNIZED, value);
        }

        [Test]
        public void NullableGetEnum_WhenDescriptionNotFound_ReturnsDefaultValue()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                + "<outer-element>"
                + "  <element>invalid</element>"
                + "</outer-element>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode node = doc.DocumentElement;
            NodeWrapper nodeWrapper = new NodeWrapper(node);

            TestEnum? value = nodeWrapper.GetEnum("element", TestEnum.UNRECOGNIZED);
            Assert.AreEqual(TestEnum.UNRECOGNIZED, value);
        }

        [Test]
        public void NullableGetEnum_WhenDescriptionNotFound_AndDefaultIsOmitted_ReturnsNull()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                + "<outer-element>"
                + "  <element>invalid</element>"
                + "</outer-element>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode node = doc.DocumentElement;
            NodeWrapper nodeWrapper = new NodeWrapper(node);

            TestEnum? value = nodeWrapper.GetEnum<TestEnum>("invalid");
            Assert.Null(value);
        }
    }
}
