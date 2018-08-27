using NUnit.Framework;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class OAuthAccessRevocationTest
    {
        [Test]
        public void AssignsMerchantId()
        {
            string xml = "<oauth-application-revocation>" +
                "<merchant-id>abc123def</merchant-id>" +
                "</oauth-application-revocation>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);

            OAuthAccessRevocation revocation = new OAuthAccessRevocation(node);

            Assert.AreEqual(revocation.MerchantId, "abc123def");
        }
    }
}
