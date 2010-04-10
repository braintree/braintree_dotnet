using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Braintree;

namespace Braintree.Tests
{
    [TestFixture]
    public class ConfigurationTest
    {
        [Test]
        public void BaseMerchantURL_ReturnsDevelopmentURL()
        {
            Configuration.Environment = Environment.DEVELOPMENT;
            Configuration.MerchantId = "integration_merchant_id";
            Configuration.PublicKey = "integration_public_key";
            Configuration.PrivateKey = "integration_private_key";

            var host = System.Environment.GetEnvironmentVariable("GATEWAY_HOST") ?? "localhost";
            var port = System.Environment.GetEnvironmentVariable("GATEWAY_PORT") ?? "3000";
            var expected = String.Format("http://{0}:{1}/merchants/integration_merchant_id", host, port);

            Assert.AreEqual(expected, Configuration.BaseMerchantURL());
        }

        [Test]
        public void BaseMerchantURL_ReturnsSandboxURL()
        {
            Configuration.Environment = Environment.SANDBOX;
            Configuration.MerchantId = "integration_merchant_id";
            Configuration.PublicKey = "integration_public_key";
            Configuration.PrivateKey = "integration_private_key";

            Assert.AreEqual("https://sandbox.braintreegateway.com:443/merchants/integration_merchant_id", Configuration.BaseMerchantURL());
        }

        [Test]
        public void BaseMerchantURL_ReturnsProductionURL()
        {
            Configuration.Environment = Environment.PRODUCTION;
            Configuration.MerchantId = "integration_merchant_id";
            Configuration.PublicKey = "integration_public_key";
            Configuration.PrivateKey = "integration_private_key";


            Assert.AreEqual("https://www.braintreegateway.com:443/merchants/integration_merchant_id", Configuration.BaseMerchantURL());
        }

        [Test]
        public void GetAuthorizationHeader_ReturnsBase64EncodePublicAndPrivateKeys()
        {
            Configuration.Environment = Environment.DEVELOPMENT;
            Configuration.MerchantId = "integration_merchant_id";
            Configuration.PublicKey = "integration_public_key";
            Configuration.PrivateKey = "integration_private_key";
            Assert.AreEqual("Basic aW50ZWdyYXRpb25fcHVibGljX2tleTppbnRlZ3JhdGlvbl9wcml2YXRlX2tleQ==", Configuration.GetAuthorizationHeader());

        }
    }
}
