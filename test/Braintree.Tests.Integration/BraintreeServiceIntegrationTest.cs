using Braintree.Exceptions;
using Braintree.TestUtil;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class BraintreeServiceIntegrationTest
    {
        [Test]
        public void SandboxSSLCertificateSuccessful()
        {
            Assert.Throws<AuthenticationException>(() => new BraintreeService(new Configuration(Environment.SANDBOX, "dummy", "dummy", "dummy")).Get("/merchants/not_our_merchant_id"));
        }

        [Test]
        public void ProductionSSLCertificateSuccessful()
        {
            Assert.Throws<AuthenticationException>(() => new BraintreeService(new Configuration(Environment.PRODUCTION, "dummy", "dummy", "dummy")).Get("/merchants/not_our_merchant_id"));
        }

        [Test]
        public void GetAuthorizationSchema_ReturnsBearerHeader()
        {

            BraintreeGateway oauthGateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );

            string code = OAuthTestHelper.CreateGrant(oauthGateway, "integration_merchant_id", "read_write");

            ResultImpl<OAuthCredentials> accessTokenResult = oauthGateway.OAuth.CreateTokenFromCode(new OAuthCredentialsRequest
            {
                Code = code,
                Scope = "read_write"
            });

            BraintreeGateway gateway = new BraintreeGateway(accessTokenResult.Target.AccessToken);
            Configuration oauthConfiguration = gateway.Configuration;
            BraintreeService oauthService = new BraintreeService(oauthConfiguration);
            string schema = oauthService.GetAuthorizationSchema();
            Assert.AreEqual("Bearer", schema);
        }

        [Test]
        public void GetAuthorizationHeader_ReturnsAccessToken()
        {
            BraintreeGateway oauthGateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );

            string code = OAuthTestHelper.CreateGrant(oauthGateway, "integration_merchant_id", "read_write");

            ResultImpl<OAuthCredentials> accessTokenResult = oauthGateway.OAuth.CreateTokenFromCode(new OAuthCredentialsRequest
            {
                Code = code,
                Scope = "read_write"
            });

            BraintreeGateway gateway = new BraintreeGateway(accessTokenResult.Target.AccessToken);
            Configuration oauthConfiguration = gateway.Configuration;
            BraintreeService oauthService = new BraintreeService(oauthConfiguration);
            var headers = oauthService.GetAuthorizationHeader();
#if netcore
            Assert.AreEqual(oauthConfiguration.AccessToken, headers);
#else
            Assert.AreEqual(oauthConfiguration.AccessToken, headers.Split(' ')[1]);
#endif
        }

#if netcore
        [Test]
        public void SetWebProxy_DoesNotThrowUriException()
        {
            Configuration configuration = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );
            configuration.WebProxy = new WebProxy("http://localhost:3000");

            BraintreeService service = new BraintreeService(configuration);
            try {
                service.Get(service.MerchantPath() + "/non-existent-route");
                Assert.Fail("Should have thrown exception");
            } catch (System.UriFormatException) {
                Assert.Fail("Setting WebProxy should not throw a URI exception");
            } catch (NotFoundException) {
                // expected
            }
        }

        [Test]
        public async Task SetWebProxyAsync_DoesNotThrowUriException()
        {
            Configuration configuration = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );
            configuration.WebProxy = new WebProxy("http://localhost:3000");

            BraintreeService service = new BraintreeService(configuration);
            try {
                await service.GetAsync(service.MerchantPath() + "/non-existent-route");
                Assert.Fail("Should have thrown exception");
            } catch (System.UriFormatException) {
                Assert.Fail("Setting WebProxy should not throw a URI exception");
            } catch (NotFoundException) {
                // expected
            }
        }
#endif
    }
}
