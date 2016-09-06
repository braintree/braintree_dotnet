using Braintree.Exceptions;
using Braintree.TestUtil;
using NUnit.Framework;
using System;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class TransparentRedirectRequestIntegrationTest
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

            Exception exception = null;

            try {
                CustomerRequest trParams = new CustomerRequest();
                CustomerRequest request = new CustomerRequest
                {
                    FirstName = "John",
                    LastName = "Doe"
                };

                string queryString = TestHelper.QueryStringForTR(trParams, request, service.BaseMerchantURL() + "/test/maintenance", service);
                gateway.Customer.ConfirmTransparentRedirect(queryString);
            } catch (Exception localException) {
                exception = localException;
            }

            Assert.IsNotNull(exception);
            Assert.IsInstanceOf(typeof(DownForMaintenanceException), exception);
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

            Exception exception = null;
            try {
                CustomerRequest trParams = new CustomerRequest();
                CustomerRequest request = new CustomerRequest
                {
                    FirstName = "John",
                    LastName = "Doe"
                };

                string queryString = TestHelper.QueryStringForTR(trParams, request, gateway.Customer.TransparentRedirectURLForCreate(), service);
                gateway.Customer.ConfirmTransparentRedirect(queryString);
            } catch (Exception localException) {
                exception = localException;
            }

            Assert.IsNotNull(exception);
            Assert.IsInstanceOf(typeof(AuthenticationException), exception);
        }
        #pragma warning restore 0618
    }
}
