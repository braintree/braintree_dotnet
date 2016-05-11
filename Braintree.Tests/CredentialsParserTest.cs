using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class CredentialsParserTest
    {
        [Test]
        [Category("Unit")]
        public void CredentialsParser_ParsesClientCredentials()
        {
            var parser = new CredentialsParser(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );

            Assert.AreEqual("client_id$development$integration_client_id", parser.ClientId);
            Assert.AreEqual("client_secret$development$integration_client_secret", parser.ClientSecret);
            Assert.AreEqual(Environment.DEVELOPMENT, parser.Environment);
        }

        [Test]
        [Category("Unit")]
        public void CredentialsParser_ParsesAccessToken()
        {
            var parser = new CredentialsParser(
                "access_token$development$merchant_id$access_token_id"
            );

            Assert.AreEqual("access_token$development$merchant_id$access_token_id", parser.AccessToken);
            Assert.AreEqual("merchant_id", parser.MerchantId);
            Assert.AreEqual(Environment.DEVELOPMENT, parser.Environment);
        }

        [Test]
        [Category("Unit")]
        [ExpectedException(typeof(ConfigurationException))]
        public void CredentialsParser_ThrowErrorOnInconsistentEnvironment()
        {
            new CredentialsParser(
                "client_id$development$integration_client_id",
                "client_secret$qa$integration_client_secret"
            );
        }

        [Test]
        [Category("Unit")]
        [ExpectedException(typeof(ConfigurationException))]
        public void CredentialsParser_ThrowErrorOnInvalidClientSecret()
        {
            new CredentialsParser(
                "client_id$development$integration_client_id",
                "client_id$development$integration_client_id"
            );
        }

        [Test]
        [Category("Unit")]
        [ExpectedException(typeof(ConfigurationException))]
        public void CredentialsParser_ThrowErrorOnInconsistentClientSecret()
        {
            new CredentialsParser(
                "client_secret$development$integration_client_secret",
                "client_secret$development$integration_client_secret"
            );
        }
    }
}
