using Braintree.Exceptions;
using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class CustomerTest
    {
        private BraintreeGateway gateway;

        [SetUp]
        public void Setup()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
        }

        [Test]
        public void Find_RaisesIfIdIsBlank()
        {
            Assert.Throws<NotFoundException>(() => gateway.Customer.Find("  "));
        }
    }
}
