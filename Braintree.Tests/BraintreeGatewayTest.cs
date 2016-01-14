using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Braintree;

namespace Braintree.Tests
{
    [TestFixture]
    public class BraintreeGatewayTest
    {
        [Test]
        [Category("Unit")]
        public void SetConfigurationEnvironment_WithEnvironment()
        {
            BraintreeGateway gateway = new BraintreeGateway(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );

            Assert.AreEqual(gateway.Environment, Environment.DEVELOPMENT);
        }

        [Test]
        [Category("Unit")]
        public void SetConfigurationEnvironment_WithString()
        {
            BraintreeGateway gateway = new BraintreeGateway(
                "development",
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );

            Assert.AreEqual(gateway.Environment, Environment.DEVELOPMENT);
        }
    }
}
