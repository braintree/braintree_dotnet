using Braintree.Exceptions;
using Braintree.TestUtil;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class PaymentMethodNonceTest
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
        public void Create_CreatesPaymentMethodNonce()
        { 
            string nonce = TestHelper.GenerateUnlockedNonce(gateway);
            Result<Customer> customerResult = gateway.Customer.Create(new CustomerRequest());

            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(new PaymentMethodRequest
            {
                CustomerId = customerResult.Target.Id,
                PaymentMethodNonce = nonce
            });

            Result<PaymentMethodNonce> result = gateway.PaymentMethodNonce.Create(paymentMethodResult.Target.Token);
            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target);
            Assert.IsNotNull(result.Target.Nonce);
        }
        
        [Test]
#if netcore
        public async Task CreateAsync_CreatesPaymentMethodNonce()
#else
        public void CreateAsync_CreatesPaymentMethodNonce()
        {
            Task.Run(async () =>
#endif
        {
            string nonce = TestHelper.GenerateUnlockedNonce(gateway);
            Result<Customer> customerResult = await gateway.Customer.CreateAsync(new CustomerRequest());

            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(new PaymentMethodRequest
            {
                CustomerId = customerResult.Target.Id,
                PaymentMethodNonce = nonce
            });

            Result<PaymentMethodNonce> result = await gateway.PaymentMethodNonce.CreateAsync(paymentMethodResult.Target.Token);
            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target);
            Assert.IsNotNull(result.Target.Nonce);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Create_RaisesNotFoundErrorWhenTokenDoesntExist()
        {
            Assert.Throws<NotFoundException>(() => gateway.PaymentMethodNonce.Create("notarealtoken"));
        }

        [Test]
        public void Find_ExposesDetailsForCreditCardNonce()
        {
            string nonce = "fake-valid-nonce";
            PaymentMethodNonce foundNonce = gateway.PaymentMethodNonce.Find(nonce);
            Assert.IsNotNull(foundNonce);
            Assert.AreEqual(foundNonce.Nonce, nonce);
            Assert.AreEqual(foundNonce.Type, "CreditCard");
            Assert.IsNotNull(foundNonce.Details);
            Assert.AreEqual(foundNonce.Details.CardType, "Visa");
            Assert.AreEqual(foundNonce.Details.LastTwo, "81");
        }

        [Test]
        public void Find_ExposesThreeDSecureInfo()
        {
            BraintreeService service = new BraintreeService(gateway.Configuration);
            CreditCardRequest creditCardRequest = new CreditCardRequest
            {
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationMonth = "05",
                ExpirationYear = "2020"
            };
            string nonce = TestHelper.Generate3DSNonce(service, creditCardRequest);

            PaymentMethodNonce foundNonce = gateway.PaymentMethodNonce.Find(nonce);
            ThreeDSecureInfo info = foundNonce.ThreeDSecureInfo;

            Assert.AreEqual(foundNonce.Nonce, nonce);
            Assert.AreEqual(foundNonce.Type, "CreditCard");
            Assert.AreEqual(info.Enrolled, "Y");
            Assert.AreEqual(info.Status, "authenticate_successful");
            Assert.AreEqual(info.LiabilityShifted, true);
            Assert.AreEqual(info.LiabilityShiftPossible, true);
        }
        
        [Test]
#if netcore
        public async Task FindAsync_ExposesThreeDSecureInfo()
#else
        public void FindAsync_ExposesThreeDSecureInfo()
        {
            Task.Run(async () =>
#endif
        {
            BraintreeService service = new BraintreeService(gateway.Configuration);
            CreditCardRequest creditCardRequest = new CreditCardRequest
            {
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationMonth = "05",
                ExpirationYear = "2020"
            };
            string nonce = TestHelper.Generate3DSNonce(service, creditCardRequest);

            PaymentMethodNonce foundNonce = await gateway.PaymentMethodNonce.FindAsync(nonce);
            ThreeDSecureInfo info = foundNonce.ThreeDSecureInfo;

            Assert.AreEqual(foundNonce.Nonce, nonce);
            Assert.AreEqual(foundNonce.Type, "CreditCard");
            Assert.AreEqual(info.Enrolled, "Y");
            Assert.AreEqual(info.Status, "authenticate_successful");
            Assert.AreEqual(info.LiabilityShifted, true);
            Assert.AreEqual(info.LiabilityShiftPossible, true);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Find_ExposesNullThreeDSecureInfoIfBlank()
        {
            string nonce = TestHelper.GenerateUnlockedNonce(gateway);

            PaymentMethodNonce foundNonce = gateway.PaymentMethodNonce.Find(nonce);

            Assert.IsNull(foundNonce.ThreeDSecureInfo);
        }

        [Test]
        public void Find_RaisesNotFoundErrorWhenTokenDoesntExist()
        {
            Assert.Throws<NotFoundException>(() => gateway.PaymentMethodNonce.Find("notarealnonce"));
        }
    }
}
