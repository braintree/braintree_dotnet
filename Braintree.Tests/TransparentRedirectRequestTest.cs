using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NUnit.Framework;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransparentRedirectRequestTest
    {
        private BraintreeService service;

        [SetUp]
        public void Setup()
        {
            service = new BraintreeService(new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            ));
        }

        [Test]
        public void Constructor_RaisesForgedQueryStringExceptionIfGivenInvalidQueryString() 
        {
            try
            {
                new TransparentRedirectRequest("http_status=200&id=7kdj469tw7yck32j&hash=99c9ff20cd7910a1c1e793ff9e3b2d15586dc6b9" + "this makes it invalid", service);
                Assert.Fail("Expected ForgedQueryStringException.");
            }
            catch(ForgedQueryStringException)
            {
                // expected
            }
        }


        [Test]
        public void Constructor_RaisesNotFoundExceptionIfStatusIs500()
        {
            try
            {
                new TransparentRedirectRequest("http_status=500&id=6kdj469tw7yck32j&hash=a839a44ca69d59a3d6f639c294794989676632dc", service);
                Assert.Fail("Expected ServerException.");
            }
            catch (ServerException)
            {
                // expected
            }
        }

        [Test]
        public void Constructor_RaisesNotFoundExceptionIfStatusIs500WithLeadQuestionMark()
        {
            try
            {
                new TransparentRedirectRequest("?http_status=500&id=6kdj469tw7yck32j&hash=a839a44ca69d59a3d6f639c294794989676632dc", service);
                Assert.Fail("Expected ServerException.");
            }
            catch (ServerException)
            {
                // expected
            }
        }

        [Test]
        public void Constructor_RaisesAuthorizationExceptionIfStatusIs403()
        {
            try
            {
                new TransparentRedirectRequest("http_status=403&id=6kdj469tw7yck32j&hash=a839a44ca69d59a3d6f639c294794989676632dc", service);
                Assert.Fail("Expected ServerException.");
            }
            catch (AuthorizationException)
            {
                // expected
            }
        }

        [Test]
        public void Constructor_RaisesAuthorizationExceptionWithMessageIfStatusIs403AndBtMessageIsInQueryString()
        {
            string message = "Invalid params: transaction[bad]";

            try
            {
                new TransparentRedirectRequest(string.Format("http_status=403&bt_message={0}&id=6kdj469tw7yck32j&hash=a839a44ca69d59a3d6f639c294794989676632dc", HttpUtility.UrlEncode(message)), service);
                Assert.Fail("Expected ServerException.");
            }
            catch (AuthorizationException e)
            {
                Assert.AreEqual(message, e.Message);
            }
        }

        #pragma warning disable 0618
        [Test]
        public void Constructor_RaisesDownForMaintenanceExceptionIfDownForMaintenance()
        {
            BraintreeGateway gateway = new BraintreeGateway()
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
            BraintreeService service = new BraintreeService(gateway.Configuration);

            try {
                CustomerRequest trParams = new CustomerRequest();
                CustomerRequest request = new CustomerRequest
                {
                    FirstName = "John",
                    LastName = "Doe"
                };

                string queryString = TestHelper.QueryStringForTR(trParams, request, service.BaseMerchantURL() + "/test/maintenance", service);
                gateway.Customer.ConfirmTransparentRedirect(queryString);
                Assert.Fail("Expected DownForMaintenanceException");
            } catch (Braintree.Exceptions.DownForMaintenanceException) {
                // expected
            }
        }
        #pragma warning restore 0618

        #pragma warning disable 0618
        [Test]
        public void Constructor_AuthenticationExceptionIfBadCredentials()
        {
            BraintreeGateway gateway = new BraintreeGateway()
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "bad_key"
            };
            BraintreeService service = new BraintreeService(gateway.Configuration);

            try {
                CustomerRequest trParams = new CustomerRequest();
                CustomerRequest request = new CustomerRequest
                {
                    FirstName = "John",
                    LastName = "Doe"
                };

                string queryString = TestHelper.QueryStringForTR(trParams, request, gateway.Customer.TransparentRedirectURLForCreate(), service);
                gateway.Customer.ConfirmTransparentRedirect(queryString);
                Assert.Fail("Expected AuthenticationException");
            } catch (Braintree.Exceptions.AuthenticationException) {
                // expected
            }
        }
        #pragma warning restore 0618
    }
}
