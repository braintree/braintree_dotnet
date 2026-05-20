using Braintree.Exceptions;
using NUnit.Framework;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class MerchantIntegrationTest
    {
        private BraintreeGateway gateway;

        [SetUp]
        public void Setup()
        {
            gateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );
        }

        // NEXT_MAJOR_VERSION remove this test
        [Test]
        public void Create_ThrowsServerExceptionBecauseEndpointIsDisabled()
        {
            Assert.Throws<ServerException>(() => gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "GBR",
                PaymentMethods = new string[] {"credit_card", "paypal"}
            }));
        }
    }
}
