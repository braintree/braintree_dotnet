using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class BraintreeGatewayTest
    {
        [Test]
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
