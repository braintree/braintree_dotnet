using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class ConfigurationTest
    {
        [Test]
        public void ConfigurationMissingEnvironment_ThrowsConfigurationException()
        {
            try {
                new Configuration(
                    null,
                    "integration_merchant_id",
                    "integration_public_key",
                    "integration_private_key"
                );
                Assert.Fail("Should throw ConfigurationException");
            } catch (ConfigurationException) {}
        }

        [Test]
        public void ConfigurationMissingMerchantId_ThrowsConfigurationException()
        {
            try {
                new Configuration(
                    Environment.DEVELOPMENT,
                    null,
                    "integration_public_key",
                    "integration_private_key"
                );
                Assert.Fail("Should throw ConfigurationException");
            } catch (ConfigurationException) {}
        }

        [Test]
        public void ConfigurationMissingPublicKey_ThrowsConfigurationException()
        {
            try {
                new Configuration(
                    Environment.DEVELOPMENT,
                    "integration_merchant_id",
                    null,
                    "integration_private_key"
                );
                Assert.Fail("Should throw ConfigurationException");
            } catch (ConfigurationException) {}
        }

        [Test]
        public void ConfigurationMissingPrivateKey_ThrowsConfigurationException()
        {
            try {
                new Configuration(
                    Environment.DEVELOPMENT,
                    "integration_merchant_id",
                    "integration_public_key",
                    null
                );
                Assert.Fail("Should throw ConfigurationException");
            } catch (ConfigurationException) {}
        }

        [Test]
        public void BaseMerchantURL_ReturnsDevelopmentURL()
        {
            BraintreeService service = new BraintreeService(new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            ));

            var host = System.Environment.GetEnvironmentVariable("GATEWAY_HOST") ?? "localhost";
            var port = System.Environment.GetEnvironmentVariable("GATEWAY_PORT") ?? "3000";
            var expected = string.Format("http://{0}:{1}/merchants/integration_merchant_id", host, port);

            Assert.AreEqual(expected, service.BaseMerchantURL());
        }

        [Test]
        public void BaseMerchantURL_ReturnsSandboxURL()
        {
            BraintreeService service = new BraintreeService(new Configuration(
                Environment.SANDBOX,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            ));

            Assert.AreEqual("https://api.sandbox.braintreegateway.com:443/merchants/integration_merchant_id", service.BaseMerchantURL());
        }

        [Test]
        public void BaseMerchantURL_ReturnsProductionURL()
        {
            BraintreeService service = new BraintreeService(new Configuration(
                Environment.PRODUCTION,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            ));

            Assert.AreEqual("https://api.braintreegateway.com:443/merchants/integration_merchant_id", service.BaseMerchantURL());
        }

        [Test]
        public void GetAuthorizationHeader_ReturnsBase64EncodePublicAndPrivateKeys()
        {
            BraintreeService service = new BraintreeService(new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            ));
            Assert.AreEqual("Basic aW50ZWdyYXRpb25fcHVibGljX2tleTppbnRlZ3JhdGlvbl9wcml2YXRlX2tleQ==", service.GetAuthorizationHeader());

        }
    }
}
