using NUnit.Framework;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class CredentialsParserTest
    {
        [Test]
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
        public void CredentialsParser_ThrowErrorOnInconsistentEnvironment()
        {
            try {
                new CredentialsParser(
                    "client_id$development$integration_client_id",
                    "client_secret$qa$integration_client_secret"
                );

                Assert.Fail("Should throw ConfigurationException");
            } catch (ConfigurationException) {}
        }

        [Test]
        public void CredentialsParser_ThrowErrorOnInvalidClientSecret()
        {
            try {
                new CredentialsParser(
                    "client_id$development$integration_client_id",
                    "client_id$development$integration_client_id"
                );

                Assert.Fail("Should throw ConfigurationException");
            } catch (ConfigurationException) {}
        }

        [Test]
        public void CredentialsParser_ThrowErrorOnInconsistentClientSecret()
        {
            try {
                new CredentialsParser(
                    "client_secret$development$integration_client_secret",
                    "client_secret$development$integration_client_secret"
                );

                Assert.Fail("Should throw ConfigurationException");
            } catch (ConfigurationException) {}
        }
    }
}
