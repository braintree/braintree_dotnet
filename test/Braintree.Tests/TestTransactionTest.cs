using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture()]
    public class TestTransactionTest {

        [Test]
        public void FailsInProduction()
        {
            var productionGateway = new BraintreeGateway
            {
                Environment = Environment.PRODUCTION,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };

            Assert.Throws<Braintree.Exceptions.TestOperationPerformedInProductionException>(() => productionGateway.TestTransaction.Settle("production_transaction_id"));
        }
    }
}
