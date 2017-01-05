using Braintree.Exceptions;
using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class PaymentMethodTest
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
        public void ToXml_IncludesDeviceData()
        {
            var request = new PaymentMethodRequest()
            {
                DeviceData = "{\"device_session_id\":\"my_dsid\", \"fraud_merchant_id\":\"my_fmid\"}"
            };

            Assert.IsTrue(request.ToXml().Contains("device_session_id"));
            Assert.IsTrue(request.ToXml().Contains("my_dsid"));
            Assert.IsTrue(request.ToXml().Contains("fraud_merchant_id"));
            Assert.IsTrue(request.ToXml().Contains("my_fmid"));
        }


        [Test]
        public void Find_RaisesNotFoundErrorWhenTokenIsBlank()
        {
            Assert.Throws<NotFoundException>(() => gateway.PaymentMethod.Find(" "));
        }

        [Test]
        public void PaymentMethod_Delete_ToQueryString_IncludesRevokeAllGrants()
        {
            var request = new PaymentMethodDeleteRequest { RevokeAllGrants = true};
            Assert.IsTrue(request.ToQueryString().Contains("revoke_all_grants=true"));
        }
    }
}
