using Braintree.Exceptions;
using NUnit.Framework;
using System;
using System.Net;

namespace Braintree.Tests
{
    [TestFixture]
    public class BraintreeServiceTest
    {
        private Configuration configuration;
        private BraintreeService service;

        [SetUp]
        public void Setup()
        {
            configuration = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );

            service = new BraintreeService(configuration);
        }

        [Test]
        public void GetWebProxy_ReturnsNullIfNotSpecified()
        {
            BraintreeService service = new BraintreeService(new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            ));
            Assert.AreEqual(null, service.GetWebProxy());
        }

        [Test]
        public void GetWebProxy_ReturnsProxyConfiguration()
        {
            Configuration configuration = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );

            configuration.WebProxy = new WebProxy(new Uri("http://localhost:3000"));
            BraintreeService service = new BraintreeService(configuration);
            Uri destination = new Uri("http://0.0.0.0");

            Assert.AreEqual("http://localhost:3000", service.GetWebProxy().GetProxy(destination).OriginalString);
        }

        [Test]
        public void WebProxy_ReturnsWebProxyConfiguration()
        {
            Configuration configuration = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );

            configuration.WebProxy = new WebProxy(new Uri("http://localhost:3000"));
            BraintreeService service = new BraintreeService(configuration);
            Assert.AreEqual(configuration.WebProxy, service.GetWebProxy());
        }

        [Test]
        public void ThrowExceptionIfErrorStatusCodeIsUpgradeRequired()
        {
            Assert.Throws<UpgradeRequiredException>(() => BraintreeService.ThrowExceptionIfErrorStatusCode((HttpStatusCode) 426, null));
        }

        [Test]
        public void ThrowExceptionIfErrorStatusCodeIsTooManyRequests()
        {
            Assert.Throws<TooManyRequestsException>(() => BraintreeService.ThrowExceptionIfErrorStatusCode((HttpStatusCode) 429, null));
        }

        [Test]
        public void ThrowExceptionIfErrorStatusCodeIsDownForMaintenance()
        {
            Assert.Throws<DownForMaintenanceException> (() => BraintreeService.ThrowExceptionIfErrorStatusCode((HttpStatusCode) 503, null));
        }

        [Test]
        public void GetAuthorizationSchema_ReturnsBasicHeader()
        {
            string schema = service.GetAuthorizationSchema();
            Assert.AreEqual("Basic", schema);
        }

        [Test]
        public void GetAuthorizationHeader_ReturnsCredentials()
        {
            var headers = service.GetAuthorizationHeader();
            Assert.IsNotEmpty(headers);
        }
    }
}
