using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class ConfigurationTest
    {
        [Test]
        [Category("Unit")]
        [ExpectedException(typeof(ConfigurationException))]
        public void ConfigurationMissingEnvironment_ThrowsConfigurationException()
        {
            Environment environment = null;
            new Configuration(
                environment,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );
        }

        [Test]
        [Category("Unit")]
        [ExpectedException(typeof(ConfigurationException))]
        public void ConfigurationMissingMerchantId_ThrowsConfigurationException()
        {
            new Configuration(
                Environment.DEVELOPMENT,
                null,
                "integration_public_key",
                "integration_private_key"
            );
        }

        [Test]
        [Category("Unit")]
        [ExpectedException(typeof(ConfigurationException))]
        public void ConfigurationMissingPublicKey_ThrowsConfigurationException()
        {
            new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                null,
                "integration_private_key"
            );
        }

        [Test]
        [Category("Unit")]
        [ExpectedException(typeof(ConfigurationException))]
        public void ConfigurationMissingPrivateKey_ThrowsConfigurationException()
        {
            new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                null
            );
        }

        [Test]
        [Category("Unit")]
        public void ConfigurationWithStringEnvironment_Initializes()
        {
            Configuration config = new Configuration(
                "development",
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );
            BraintreeService service = new BraintreeService(config);

            var host = System.Environment.GetEnvironmentVariable("GATEWAY_HOST") ?? "localhost";
            var port = System.Environment.GetEnvironmentVariable("GATEWAY_PORT") ?? "3000";
            var expected = string.Format("http://{0}:{1}/merchants/integration_merchant_id", host, port);

            Assert.AreEqual(expected, service.BaseMerchantURL());
        }

        [Test]
        [Category("Unit")]
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
        [Category("Unit")]
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
        [Category("Unit")]
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
        [Category("Unit")]
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

        [Test]
        [Category("Unit")]
        public void Proxy_ReturnsNullIfNotSpecified()
        {
            BraintreeService service = new BraintreeService(new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            ));
            Assert.AreEqual(null, service.GetProxy());
        }

        [Test]
        [Category("Unit")]
        public void Proxy_ReturnsProxyConfiguration()
        {
            Configuration configuration = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );

            configuration.Proxy = "http://localhost:3000";
            BraintreeService service = new BraintreeService(configuration);
            Assert.AreEqual("http://localhost:3000", service.GetProxy());
        }

        [Test]
        [Category("Unit")]
        public void Timeout_DefaultsToSixtySeconds()
        {
            Configuration configuration = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );

            Assert.AreEqual(60000, configuration.Timeout);
        }

        [Test]
        [Category("Unit")]
        public void Timeout_ReturnsTheSetValue()
        {
            Configuration configuration = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );

            configuration.Timeout = 1;

            Assert.AreEqual(1, configuration.Timeout);
        }

        [Test]
        [Category("Unit")]
        public void HttpWebRequestFactory_ReturnsDefaultIfNotSpecified()
        {
            Configuration configuration = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );

            Assert.IsNotNull(configuration.HttpWebRequestFactory);

            HttpWebRequest httpWebRequest = WebRequest.Create("http://webrequest.com") as HttpWebRequest;
            Assert.IsInstanceOfType(httpWebRequest.GetType(), configuration.HttpWebRequestFactory(configuration.Environment.GatewayURL + "/merchants/integration_merchant_id"));
        }

        [Test]
        [Category("Unit")]
        public void HttpWebRequestFactory_AcceptsCustomDelegate()
        {
            Configuration configuration = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );

            configuration.HttpWebRequestFactory =
                delegate(String requestUriString)
                {
                    var webRequest = WebRequest.Create(requestUriString) as HttpWebRequest;
                    webRequest.AddRange(1024);
                    return webRequest;
                };

            Assert.IsNotNull(configuration.HttpWebRequestFactory);

            var btWebRequest = configuration.HttpWebRequestFactory(configuration.Environment.GatewayURL + "/merchants/integration_merchant_id");
            HttpWebRequest httpWebRequest = WebRequest.Create("http://webrequest.com") as HttpWebRequest;

            Assert.IsInstanceOfType(httpWebRequest.GetType(), btWebRequest);
            StringAssert.Contains("1024", btWebRequest.Headers["Range"]);
        }
    }
}
