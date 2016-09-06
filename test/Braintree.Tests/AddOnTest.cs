using Braintree.Exceptions;
using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class AddOnTest
    {
        private BraintreeGateway gateway;

        [Test]
        public void All_RaisesIfMissingCredentials()
        {
            gateway = new BraintreeGateway
            {
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };

            Assert.Throws<ConfigurationException> (() => gateway.AddOn.All());
        }
    }
}

