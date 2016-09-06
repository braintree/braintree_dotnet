using Braintree.Exceptions;
using NUnit.Framework;
using System;
#if netcoreapp10
using System.Net.Http;
using System.Net.Http.Headers;
#else
using System.Net;
#endif

namespace Braintree.Tests
{
    [TestFixture]
    public class ConfigurationTest
    {
        [Test]
        public void ConfigurationMissingEnvironment_ThrowsConfigurationException()
        {
            Environment environment = null;
            Assert.Throws<ConfigurationException>(() => new Configuration(
                environment,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            ));
        }

        [Test]
        public void ConfigurationMissingMerchantId_ThrowsConfigurationException()
        {
            Assert.Throws<ConfigurationException>(() => new Configuration(
                Environment.DEVELOPMENT,
                null,
                "integration_public_key",
                "integration_private_key"
            ));
        }

        [Test]
        public void ConfigurationMissingPublicKey_ThrowsConfigurationException()
        {
            Assert.Throws<ConfigurationException>(() => new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                null,
                "integration_private_key"
            ));
        }

        [Test]
        public void ConfigurationMissingPrivateKey_ThrowsConfigurationException()
        {
            Assert.Throws<ConfigurationException>(() => new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                null
            ));
        }

        [Test]
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

#if netcoreapp10
            Assert.AreEqual("aW50ZWdyYXRpb25fcHVibGljX2tleTppbnRlZ3JhdGlvbl9wcml2YXRlX2tleQ==", service.GetAuthorizationHeader());
#else
            Assert.AreEqual("Basic aW50ZWdyYXRpb25fcHVibGljX2tleTppbnRlZ3JhdGlvbl9wcml2YXRlX2tleQ==", service.GetAuthorizationHeader());
#endif
        }

        [Test]
        public void Proxy_ReturnsNullIfNotSpecified()
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
        public void Proxy_ReturnsProxyConfiguration()
        {
            Configuration configuration = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );
            Uri destination = new Uri("http://0.0.0.0");
            configuration.WebProxy = new WebProxy(new Uri("http://localhost:3000"));
            BraintreeService service = new BraintreeService(configuration);
            Assert.AreEqual("http://localhost:3000", service.GetWebProxy().GetProxy(destination).OriginalString);
        }

        [Test]
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
        public void HttpRequestMessageFactory_ReturnsDefaultIfNotSpecified()
        {
            Configuration configuration = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );

#if netcoreapp10
            Assert.IsNotNull(configuration.HttpRequestMessageFactory);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://webrequest.com");
            Assert.IsInstanceOf(httpRequestMessage.GetType(), configuration.HttpRequestMessageFactory(HttpMethod.Get, configuration.Environment.GatewayURL + "/merchants/integration_merchant_id"));
#else
            Assert.IsNotNull(configuration.HttpWebRequestFactory);
            HttpWebRequest httpWebRequest = WebRequest.Create("http://webrequest.com") as HttpWebRequest;
            Assert.IsInstanceOf(httpWebRequest.GetType(), configuration.HttpWebRequestFactory(configuration.Environment.GatewayURL + "/merchants/integration_merchant_id"));
#endif
        }

        [Test]
        public void HttpRequestMessageFactory_AcceptsCustomDelegate()
        {
            Configuration configuration = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );

#if netcoreapp10
            configuration.HttpRequestMessageFactory =
                delegate (HttpMethod method, string requestUriString)
                {
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, requestUriString);
                    httpRequestMessage.Headers.Range = new RangeHeaderValue(1024, 2048);
                    return httpRequestMessage;
                };
            Assert.IsNotNull(configuration.HttpRequestMessageFactory);

            var btWebRequest = configuration.HttpRequestMessageFactory(HttpMethod.Get, configuration.Environment.GatewayURL + "/merchants/integration_merchant_id");
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://webrequest.com");

            Assert.IsInstanceOf(requestMessage.GetType(), btWebRequest);
            var headerValue = btWebRequest.Headers.Range;
            StringAssert.Contains("1024", btWebRequest.Headers.Range.ToString());
#else
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
            
            Assert.IsInstanceOf(httpWebRequest.GetType(), btWebRequest);
            StringAssert.Contains("1024", btWebRequest.Headers["Range"]);
#endif
        }
    }
}
