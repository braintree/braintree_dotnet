using Braintree.Exceptions;
using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class AddressTest
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
        public void Find_FindsErrorsOutOnWhitespaceAddressId()
        {
            Assert.Throws<NotFoundException>(() => gateway.Address.Find(" ", "address_id"));
        }

        [Test]
        public void Find_FindsErrorsOutOnWhitespaceCustomerId()
        {
            Assert.Throws<NotFoundException>(() => gateway.Address.Find("customer_id", " "));
        }
    }
}
