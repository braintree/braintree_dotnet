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
        public void Generate_GeneratesFingerprintAcceptedByGateway()
        {
            var encodedClientToken = gateway.ClientToken.generate();
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
            var encodedClientToken = await gateway.ClientToken.generateAsync();
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
