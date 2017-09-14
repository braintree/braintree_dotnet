using Braintree.Exceptions;
using NUnit.Framework;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class FacilitatedDetailsTest
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
            string nonce = "4004b83f-2559-0d79-2de0-c096d1ed9b92";
            string xml = "<facilitated-details>" +
                "<merchant-id>abc123</merchant-id>" +
                "<merchant-name>Cool Store</merchant-name>" +
                "<payment-method-nonce>" + nonce + "</payment-method-nonce>" +
                "</facilitated-details>";
            var node = nodeFromXml(xml);

            FacilitatedDetails details = new FacilitatedDetails(node);

            Assert.AreEqual("abc123", details.MerchantId);
            Assert.AreEqual("Cool Store", details.MerchantName);
            Assert.AreEqual(nonce, details.PaymentMethodNonce);
        }

        [Test]
        public void CanOmitPaymentMethodNonce()
        {
            string xml = "<facilitated-details>" +
                "<merchant-id>abc123</merchant-id>" +
                "<merchant-name>Cool Store</merchant-name>" +
                "</facilitated-details>";
            var node = nodeFromXml(xml);

            FacilitatedDetails details = new FacilitatedDetails(node);

            Assert.AreEqual(null, details.PaymentMethodNonce);
        }
    }
}
