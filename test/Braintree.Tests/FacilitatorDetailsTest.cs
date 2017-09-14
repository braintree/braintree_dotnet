using Braintree.Exceptions;
using NUnit.Framework;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class FacilitatorDetailsTest
    {
        private NodeWrapper nodeFromXml(string xml) {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            return new NodeWrapper(newNode);
        }

        [Test]
        public void IncludesFields()
        {
            string xml = "<facilitator-details>" +
                "<oauth-application-client-id>abc123</oauth-application-client-id>" +
                "<oauth-application-name>Fun Shop</oauth-application-name>" +
                "<source-payment-method-token>abc9xyz</source-payment-method-token>" +
                "</facilitator-details>";
            var node = nodeFromXml(xml);

            FacilitatorDetails details = new FacilitatorDetails(node);

            Assert.AreEqual("abc123", details.OauthApplicationClientId);
            Assert.AreEqual("Fun Shop", details.OauthApplicationName);
            Assert.AreEqual("abc9xyz", details.SourcePaymentMethodToken);
        }

        [Test]
        public void CanOmitSourcePaymentMethodToken()
        {
            string xml = "<facilitator-details>" +
                "<oauth-application-client-id>abc123</oauth-application-client-id>" +
                "<oauth-application-name>Fun Shop</oauth-application-name>" +
                "</facilitator-details>";
            var node = nodeFromXml(xml);

            FacilitatorDetails details = new FacilitatorDetails(node);

            Assert.AreEqual(null, details.SourcePaymentMethodToken);
        }
    }
}
