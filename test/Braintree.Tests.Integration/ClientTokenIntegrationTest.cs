using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class ClientTokenIntegrationTest
    {
        private BraintreeGateway gateway;

        [SetUp]
        public void Setup()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
        }

        [Test]
        public void Generate_ValidDomainsAccepted()
        {
            var ClientTokenRequest = new ClientTokenRequest
            {
                Domains = new string[] {"example.com"}
            };
            var encodedClientToken = gateway.ClientToken.Generate(ClientTokenRequest);
            Assert.NotNull(encodedClientToken);
        }

        [Test]
        public void Generate_InvalidDomainsNotAccepted()
        {
            var ClientTokenRequest = new ClientTokenRequest
            {
                Domains = new string[] {"example"}
            };
            ArgumentException invalidFormatException = Assert.Throws<ArgumentException>(() =>  gateway.ClientToken.Generate(ClientTokenRequest));
            Assert.AreEqual(invalidFormatException.Message, "Client token domains must be valid domain names (RFC 1035), e.g. example.com");
        }

        [Test]
        public void Generate_TooManyDomainsNotAccepted()
        {
            var ClientTokenRequest = new ClientTokenRequest
            {
                Domains = new string[] {
                    "example1.com",
                    "example2.com",
                    "example3.com",
                    "example4.com",
                    "example5.com",
                    "example6.com"
                }
            };
            ArgumentException tooManyException = Assert.Throws<ArgumentException>(() =>  gateway.ClientToken.Generate(ClientTokenRequest));
            Assert.AreEqual(tooManyException.Message, "Cannot specify more than 5 client token domains");
        }

        [Test]
        public void Generate_GeneratesFingerprintAcceptedByGateway()
        {
            var encodedClientToken = gateway.ClientToken.Generate();
            var decodedClientToken = Encoding.UTF8.GetString(Convert.FromBase64String(encodedClientToken));
            var clientToken = Regex.Unescape(decodedClientToken);
            var authorizationFingerprint = TestHelper.extractParamFromJson("authorizationFingerprint", clientToken);

            var builder = new RequestBuilder();
            builder.
                AddTopLevelElement("authorization_fingerprint", authorizationFingerprint).
                AddTopLevelElement("shared_customer_identifier", "test-identifier").
                AddTopLevelElement("shared_customer_identifier_type", "testing");

            var response = new TestUtil.BraintreeTestHttpService().Get(
                gateway.MerchantId,
                "v1/payment_methods?" + builder.ToQueryString());

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
#if netcore
        public async Task GenerateAsync_GeneratesFingerprintAcceptedByGateway()
#else
        public void GenerateAsync_GeneratesFingerprintAcceptedByGateway()
        {
            Task.Run(async() =>
#endif
        {
            var encodedClientToken = await gateway.ClientToken.GenerateAsync();
            var decodedClientToken = Encoding.UTF8.GetString(Convert.FromBase64String(encodedClientToken));
            var clientToken = Regex.Unescape(decodedClientToken);
            var authorizationFingerprint = TestHelper.extractParamFromJson("authorizationFingerprint", clientToken);

            var builder = new RequestBuilder();
            builder.
                AddTopLevelElement("authorization_fingerprint", authorizationFingerprint).
                AddTopLevelElement("shared_customer_identifier", "test-identifier").
                AddTopLevelElement("shared_customer_identifier_type", "testing");

            var response = new TestUtil.BraintreeTestHttpService().Get(
                gateway.MerchantId,
                "v1/payment_methods?" + builder.ToQueryString());

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif
    }
}
