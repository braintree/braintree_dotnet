using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Braintree;

namespace Braintree.Tests
{
    [TestFixture]
    class ConfigurationTest
    {
        [Test]
        public void BaseMerchantURL_ReturnsDevelopmentURL()
        {
            Configuration.Environment = Environment.DEVELOPMENT;
            Configuration.MerchantID = "integration_merchant_id";
            Configuration.PublicKey = "integration_public_key";
            Configuration.PrivateKey = "integration_private_key";

            Assert.AreEqual("http://192.168.65.1:3000/merchants/integration_merchant_id", Configuration.BaseMerchantURL());
        }

        [Test]
        public void BaseMerchantURL_ReturnsSandboxURL()
        {
            Configuration.Environment = Environment.SANDBOX;
            Configuration.MerchantID = "integration_merchant_id";
            Configuration.PublicKey = "integration_public_key";
            Configuration.PrivateKey = "integration_private_key";

            Assert.AreEqual("https://sandbox.braintreegateway.com:443/merchants/integration_merchant_id", Configuration.BaseMerchantURL());
        }

        [Test]
        public void BaseMerchantURL_ReturnsProductionURL()
        {
            Configuration.Environment = Environment.PRODUCTION;
            Configuration.MerchantID = "integration_merchant_id";
            Configuration.PublicKey = "integration_public_key";
            Configuration.PrivateKey = "integration_private_key";


            Assert.AreEqual("https://www.braintreegateway.com:443/merchants/integration_merchant_id", Configuration.BaseMerchantURL());
        }

        [Test]
        public void GetAuthorizationHeader_ReturnsBase64EncodePublicAndPrivateKeys()
        {
            Configuration.Environment = Environment.DEVELOPMENT;
            Configuration.MerchantID = "integration_merchant_id";
            Configuration.PublicKey = "integration_public_key";
            Configuration.PrivateKey = "integration_private_key";
            Assert.AreEqual("Basic aW50ZWdyYXRpb25fcHVibGljX2tleTppbnRlZ3JhdGlvbl9wcml2YXRlX2tleQ==", Configuration.GetAuthorizationHeader());

        }
    }
}
