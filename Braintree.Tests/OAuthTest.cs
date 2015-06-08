using NUnit.Framework;
using System;

namespace Braintree.Tests
{
    [TestFixture]
    public class OAuthTest
    {
        private BraintreeGateway gateway;

        [SetUp]
        public void Setup()
        {
            gateway = new BraintreeGateway {
                Environment = Environment.DEVELOPMENT,
                ClientId = "client_id$development$integration_client_id",
                ClientSecret = "client_secret$development$integration_client_secret"
            };
        }

        [Test]
        public void CreateTokenFromCode_ReturnsOAuthCredentials()
        {
            String code = OAuthTestHelper.CreateGrant(gateway, "integration_merchant_id", "read_write");

            ResultImpl<OAuthCredentials> result = gateway.OAuth.CreateTokenFromCode(new OAuthCredentialsRequest {
                Code = code,
                Scope = "read_write"
            });

            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target.AccessToken);
            Assert.IsNotNull(result.Target.RefreshToken);
            Assert.IsNotNull(result.Target.ExpiresAt);
            Assert.AreEqual("bearer", result.Target.TokenType);
        }

        [Test]
        public void CreateTokenFromBadCode_ReturnsFailureCode()
        {
            ResultImpl<OAuthCredentials> result = gateway.OAuth.CreateTokenFromCode(new OAuthCredentialsRequest {
                Code = "bad_code",
                Scope = "read_write"
            });

            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.OAUTH_INVALID_GRANT,
                result.Errors.ForObject("Credentials").OnField("Code")[0].Code
            );
            Assert.AreEqual(
                "Invalid grant: code not found",
                result.Errors.ForObject("Credentials").OnField("Code")[0].Message
            );
        }
    }
}
