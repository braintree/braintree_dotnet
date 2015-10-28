using NUnit.Framework;
using System;
using System.Web;
using System.Text.RegularExpressions;
using Braintree.Exceptions;
using System.Collections.Specialized;

namespace Braintree.Tests
{
    [TestFixture]
    public class OAuthTest
    {
        private BraintreeGateway gateway;

        [SetUp]
        public void Setup()
        {
            gateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );
        }

        [Test]
        [Category("Integration")]
        public void CreateTokenFromCode_ReturnsOAuthCredentials()
        {
            string code = OAuthTestHelper.CreateGrant(gateway, "integration_merchant_id", "read_write");

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
        [Category("Integration")]
        public void CreateTokenFromRefreshToken_ExchangesRefreshTokenForAccessToken()
        {
            string code = OAuthTestHelper.CreateGrant(gateway, "integration_merchant_id", "read_write");

            ResultImpl<OAuthCredentials> accessTokenResult = gateway.OAuth.CreateTokenFromCode(new OAuthCredentialsRequest {
                Code = code,
                Scope = "read_write"
            });

            ResultImpl<OAuthCredentials> refreshTokenResult = gateway.OAuth.CreateTokenFromRefreshToken(new OAuthCredentialsRequest {
                RefreshToken = accessTokenResult.Target.RefreshToken,
                Scope = "read_write"
            });

            Assert.IsTrue(refreshTokenResult.IsSuccess());
            Assert.IsNotNull(refreshTokenResult.Target.AccessToken);
            Assert.IsNotNull(refreshTokenResult.Target.RefreshToken);
            Assert.IsNotNull(refreshTokenResult.Target.ExpiresAt);
            Assert.AreEqual("bearer", refreshTokenResult.Target.TokenType);
        }

        [Test]
        [Category("Integration")]
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

        [Test]
        [Category("Unit")]
        public void CreateTokenFromCode_RaisesIfWrongCredentials()
        {
            try {
                gateway = new BraintreeGateway(
                    "access_token$development$merchant_id$_oops_this_is_not_a_client_id_and_secret"
                );

                gateway.OAuth.CreateTokenFromCode(new OAuthCredentialsRequest());

                Assert.Fail("Should throw ConfigurationException");
            } catch (ConfigurationException) {}
        }

        [Test]
        [Category("Unit")]
        public void ConnectUrl_ReturnsCorrectUrl()
        {
            string url = gateway.OAuth.ConnectUrl(new OAuthConnectUrlRequest {
                MerchantId = "integration_merchant_id",
                RedirectUri = "http://bar.example.com",
                Scope = "read_write",
                State = "baz_state",
                User = new OAuthConnectUrlUserRequest {
                    Country = "USA",
                    Email = "foo@example.com",
                    FirstName = "Bob",
                    LastName = "Jones",
                    Phone = "555-555-5555",
                    DobYear = "1970",
                    DobMonth = "01",
                    DobDay = "01",
                    StreetAddress = "222 W Merchandise Mart",
                    Locality = "Chicago",
                    Region = "IL",
                    PostalCode = "60606",
                },
                Business = new OAuthConnectUrlBusinessRequest {
                    Name = "14 Ladders",
                    RegisteredAs = "14.0 Ladders",
                    Industry = "Ladders",
                    Description = "We sell the best ladders",
                    StreetAddress = "111 N Canal",
                    Locality = "Chicago",
                    Region = "IL",
                    PostalCode = "60606",
                    Country = "USA",
                    AnnualVolumeAmount = "1000000",
                    AverageTransactionAmount = "100",
                    MaximumTransactionAmount = "10000",
                    ShipPhysicalGoods = true,
                    FulfillmentCompletedIn = 7,
                    Currency = "USD",
                    Website = "http://example.com",
                },
            });

            var uri = new Uri(url);
            NameValueCollection query = HttpUtility.ParseQueryString(uri.Query);

            Assert.AreEqual("localhost", uri.Host);
            Assert.AreEqual("/oauth/connect", uri.AbsolutePath);

            Assert.AreEqual("integration_merchant_id", query["merchant_id"]);
            Assert.AreEqual("client_id$development$integration_client_id", query["client_id"]);
            Assert.AreEqual("http://bar.example.com", query["redirect_uri"]);
            Assert.AreEqual("read_write", query["scope"]);
            Assert.AreEqual("baz_state", query["state"]);

            Assert.AreEqual("USA", query["user[country]"]);
            Assert.AreEqual("foo@example.com", query["user[email]"]);
            Assert.AreEqual("Bob", query["user[first_name]"]);
            Assert.AreEqual("Jones", query["user[last_name]"]);
            Assert.AreEqual("555-555-5555", query["user[phone]"]);
            Assert.AreEqual("1970", query["user[dob_year]"]);
            Assert.AreEqual("01", query["user[dob_month]"]);
            Assert.AreEqual("01", query["user[dob_day]"]);
            Assert.AreEqual("222 W Merchandise Mart", query["user[street_address]"]);
            Assert.AreEqual("Chicago", query["user[locality]"]);
            Assert.AreEqual("IL", query["user[region]"]);
            Assert.AreEqual("60606", query["user[postal_code]"]);

            Assert.AreEqual("14 Ladders", query["business[name]"]);
            Assert.AreEqual("14.0 Ladders", query["business[registered_as]"]);
            Assert.AreEqual("Ladders", query["business[industry]"]);
            Assert.AreEqual("We sell the best ladders", query["business[description]"]);
            Assert.AreEqual("111 N Canal", query["business[street_address]"]);
            Assert.AreEqual("Chicago", query["business[locality]"]);
            Assert.AreEqual("IL", query["business[region]"]);
            Assert.AreEqual("60606", query["business[postal_code]"]);
            Assert.AreEqual("USA", query["business[country]"]);
            Assert.AreEqual("1000000", query["business[annual_volume_amount]"]);
            Assert.AreEqual("100", query["business[average_transaction_amount]"]);
            Assert.AreEqual("10000", query["business[maximum_transaction_amount]"]);
            Assert.AreEqual("true", query["business[ship_physical_goods]"]);
            Assert.AreEqual("7", query["business[fulfillment_completed_in]"]);
            Assert.AreEqual("USD", query["business[currency]"]);
            Assert.AreEqual("http://example.com", query["business[website]"]);

            Assert.AreEqual(64, query["signature"].Length);
            var hexRegex = new Regex("^[a-f0-9]+$");
            Assert.IsTrue(hexRegex.IsMatch(query["signature"]));
            Assert.AreEqual("SHA256", query["algorithm"]);
        }

        [Test]
        [Category("Unit")]
        public void ConnectUrl_WorksWithoutOptionalParameters()
        {
            string url = gateway.OAuth.ConnectUrl(new OAuthConnectUrlRequest());

            var uri = new Uri(url);
            NameValueCollection query = HttpUtility.ParseQueryString(uri.Query);

            Assert.IsNull(query["redirect_uri"]);
        }

        [Test]
        [Category("Unit")]
        public void ConnectUrl_WorksWithMultiplePaymentMethods()
        {
            string url = gateway.OAuth.ConnectUrl(new OAuthConnectUrlRequest {
                PaymentMethods = new string[] {"credit_card", "paypal"}
            });

            var uri = new Uri(url);
            NameValueCollection query = HttpUtility.ParseQueryString(uri.Query);

            Assert.AreEqual("credit_card,paypal", query["payment_methods[]"]);
        }

        [Test]
        [Category("Unit")]
        public void ComputeSignature_ReturnsCorrectSignature()
        {
            string url = "http://localhost:3000/oauth/connect?business%5Bname%5D=We+Like+Spaces&client_id=client_id%24development%24integration_client_id";
            string signature = gateway.OAuth.ComputeSignature(url);


            Assert.AreEqual("a36bcf10dd982e2e47e0d6a2cb930aea47ade73f954b7d59c58dae6167894d41", signature);
        }
    }
}
