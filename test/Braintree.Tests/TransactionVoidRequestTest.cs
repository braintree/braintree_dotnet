using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransactionVoidRequestTest
    {
        [Test]
        public void ToXml_IncludesApiRequestKey()
        {
            TransactionVoidRequest request = new TransactionVoidRequest();
            request.ApiRequestKey = "test-api-key-123";

            string xml = request.ToXml();
            Assert.IsTrue(xml.Contains("<api-request-key>test-api-key-123</api-request-key>"));
        }

        [Test]
        public void ToXml_ExcludesApiRequestKeyWhenNull()
        {
            TransactionVoidRequest request = new TransactionVoidRequest();
            request.ApiRequestKey = null;

            string xml = request.ToXml();
            Assert.IsFalse(xml.Contains("api-request-key"));
        }

        [Test]
        public void ToXml_CreatesValidXml()
        {
            TransactionVoidRequest request = new TransactionVoidRequest();
            request.ApiRequestKey = "void-key-789";

            string xml = request.ToXml();
            Assert.IsTrue(xml.Contains("<transaction>"));
            Assert.IsTrue(xml.Contains("</transaction>"));
            Assert.IsTrue(xml.Contains("<api-request-key>void-key-789</api-request-key>"));
        }

        [Test]
        public void ToXml_EmptyRequestCreatesMinimalXml()
        {
            TransactionVoidRequest request = new TransactionVoidRequest();

            string xml = request.ToXml();
            Assert.IsTrue(xml.Contains("<transaction>"));
            Assert.IsTrue(xml.Contains("</transaction>"));
            Assert.IsFalse(xml.Contains("api-request-key"));
        }
    }
}
