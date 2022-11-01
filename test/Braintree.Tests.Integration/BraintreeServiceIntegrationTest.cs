using Braintree.Exceptions;
using Braintree.TestUtil;
using NUnit.Framework;
using System;
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
        [Ignore("unpend when proxy setup can be created")]
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
        [Ignore("unpend when proxy setup can be created")]
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

        [Test]
        public void BraintreeGateway_makesMultipleRequestsWithStaticClient()
        {
            var gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };

            string id = Guid.NewGuid().ToString();
            var createRequest = new CustomerRequest
            {
                Id = id,
            };

            Customer createdCustomer = gateway.Customer.Create(createRequest).Target;

            var updateRequest = new CustomerRequest
            {
                FirstName = "First"
            };

            gateway.Customer.Update(id, updateRequest);

            Customer customer = gateway.Customer.Find(createdCustomer.Id);

            Assert.AreEqual(id, customer.Id);
            Assert.AreEqual("First", customer.FirstName);
        }
#endif

        [Test]
        public void BraintreeGateway_ExceptionIfPathTraversal()
        {
            var gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };

            var batmanCustomerRequest = new CustomerRequest
            {
                FirstName = "Bruce",
                LastName = "Wayne"
            };

            Customer batman = gateway.Customer.Create(batmanCustomerRequest).Target;

            var batmanAddressRequest = new AddressRequest
            {
                Locality = "Gotham City"
            };

            Address batmanAddress = gateway.Address.Create(batman.Id, batmanAddressRequest).Target;

            var supermanCustomerRequest = new CustomerRequest
            {
                FirstName = "Clark",
                LastName = "Kent"
            };

            Customer superman = gateway.Customer.Create(supermanCustomerRequest).Target;

            var supermanAddressRequest = new AddressRequest
            {
                Locality = "Smallville",
                Region = "Kansas"
            };

            Address supermanAddress = gateway.Address.Create(superman.Id, supermanAddressRequest).Target;

            Assert.Throws<ArgumentException>(() => gateway.Address.Find(batman.Id, $"../../{superman.Id}/addresses/{supermanAddress.Id}"));

            Assert.Throws<ArgumentException>(() => gateway.Address.Find(batman.Id, $"%2E%2E/%2E%2E/{superman.Id}/addresses/{supermanAddress.Id}"));

            Assert.ThrowsAsync<ArgumentException>(() => gateway.Address.FindAsync(superman.Id, $"../../{batman.Id}/addresses/{batmanAddress.Id}"));

            Assert.ThrowsAsync<ArgumentException>(() => gateway.Address.FindAsync(superman.Id, $"%2E%2E/%2E%2E/{batman.Id}/addresses/{batmanAddress.Id}"));
        }
    }
    
}
