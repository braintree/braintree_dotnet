using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransparentRedirectRequestTest
    {

        [SetUp]
        public void Setup()
        {
            Configuration.Environment = Environment.DEVELOPMENT;
            Configuration.MerchantId = "integration_merchant_id";
            Configuration.PublicKey = "integration_public_key";
            Configuration.PrivateKey = "integration_private_key";
        }

        [Test]
        public void Constructor_RaisesForgedQueryStringExceptionIfGivenInvalidQueryString() 
        {
            try
            {
                new TransparentRedirectRequest("http_status=200&id=7kdj469tw7yck32j&hash=99c9ff20cd7910a1c1e793ff9e3b2d15586dc6b9" + "this makes it invalid");
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
                new TransparentRedirectRequest("http_status=500&id=6kdj469tw7yck32j&hash=a839a44ca69d59a3d6f639c294794989676632dc");
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
                new TransparentRedirectRequest("?http_status=500&id=6kdj469tw7yck32j&hash=a839a44ca69d59a3d6f639c294794989676632dc");
                Assert.Fail("Expected ServerException.");
            }
            catch (ServerException)
            {
                // expected
            }
        }


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

            try {
                CustomerRequest trParams = new CustomerRequest();
                CustomerRequest request = new CustomerRequest
                {
                    FirstName = "John",
                    LastName = "Doe"
                };

                String queryString = TestHelper.QueryStringForTR(trParams, request, Configuration.BaseMerchantURL() + "/test/maintenance");
                gateway.Customer.ConfirmTransparentRedirect(queryString);
                Assert.Fail("Expected DownForMaintenanceException");
            } catch (Braintree.Exceptions.DownForMaintenanceException) {
                // expected
            }
        }

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

            try {
                CustomerRequest trParams = new CustomerRequest();
                CustomerRequest request = new CustomerRequest
                {
                    FirstName = "John",
                    LastName = "Doe"
                };

                String queryString = TestHelper.QueryStringForTR(trParams, request, gateway.Customer.TransparentRedirectURLForCreate());
                gateway.Customer.ConfirmTransparentRedirect(queryString);
                Assert.Fail("Expected AuthenticationException");
            } catch (Braintree.Exceptions.AuthenticationException) {
                // expected
            }
        }
    }
}
