using Braintree.Exceptions;
using Braintree.Test;
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
            Assert.AreEqual(foundNonce.Details.Bin, "401288");
            Assert.AreEqual(foundNonce.Details.CardType, "Visa");
            Assert.AreEqual(foundNonce.Details.LastTwo, "81");
            Assert.AreEqual(foundNonce.Details.LastFour, "1881");
        }


        [Test]
        public void Find_ExposesDetailsForVenmoNonce()
        {
            string nonce = "fake-venmo-account-nonce";
            PaymentMethodNonce foundNonce = gateway.PaymentMethodNonce.Find(nonce);
            Assert.IsNotNull(foundNonce);
            Assert.AreEqual(foundNonce.Nonce, nonce);
            Assert.IsNotNull(foundNonce.Details);
            Assert.AreEqual(foundNonce.Details.Username, "venmojoe");
            Assert.AreEqual(foundNonce.Details.LastTwo, "99");
            Assert.AreEqual(foundNonce.Details.VenmoUserId, "Venmo-Joe-1");
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

            Assert.AreEqual(nonce, foundNonce.Nonce);
            Assert.AreEqual("CreditCard", foundNonce.Type);
            Assert.AreEqual("Y", info.Enrolled);
            Assert.AreEqual("authenticate_successful", info.Status);
            Assert.IsTrue(info.LiabilityShifted);
            Assert.IsTrue(info.LiabilityShiftPossible);
            Assert.AreEqual("test_cavv", info.Cavv);
            Assert.AreEqual("test_eci", info.EciFlag);
            Assert.AreEqual("1.0.2", info.ThreeDSecureVersion);
            Assert.AreEqual("test_xid", info.Xid);
        }

        [Test]
#if netcore
        public async Task FindAsync_ExposesBinData()
#else
        public void FindAsync_ExposesBinData()
        {
            Task.Run(async () =>
#endif
        {
            string inputNonce = Nonce.TransactableUnknownIndicators;
            PaymentMethodNonce nonce = await gateway.PaymentMethodNonce.FindAsync(inputNonce);
            Assert.AreEqual(nonce.Nonce, inputNonce);
            Assert.IsNotNull(nonce.BinData);
            Assert.AreEqual(nonce.BinData.Commercial, Braintree.CreditCardCommercial.UNKNOWN);
            Assert.AreEqual(nonce.BinData.Debit, Braintree.CreditCardDebit.UNKNOWN);
            Assert.AreEqual(nonce.BinData.DurbinRegulated, Braintree.CreditCardDurbinRegulated.UNKNOWN);
            Assert.AreEqual(nonce.BinData.Healthcare, Braintree.CreditCardHealthcare.UNKNOWN);
            Assert.AreEqual(nonce.BinData.Payroll, Braintree.CreditCardPayroll.UNKNOWN);
            Assert.AreEqual(nonce.BinData.Prepaid, Braintree.CreditCardPrepaid.UNKNOWN);
            Assert.AreEqual(nonce.BinData.CountryOfIssuance, "Unknown");
            Assert.AreEqual(nonce.BinData.IssuingBank, "Unknown");
            Assert.AreEqual(nonce.BinData.ProductId,"Unknown");
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
#if netcore
        public async Task FindAsync_ExposesBinDataPrepaidValue()
#else
        public void FindAsync_ExposesBinDataPrepaidValue()
        {
            Task.Run(async () =>
#endif
        {
            string inputNonce = Nonce.TransactablePrepaid;
            PaymentMethodNonce nonce = await gateway.PaymentMethodNonce.FindAsync(inputNonce);
            Assert.AreEqual(nonce.Nonce, inputNonce);
            Assert.IsNotNull(nonce.BinData);
            Assert.AreEqual(nonce.BinData.Prepaid, Braintree.CreditCardPrepaid.YES);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
#if netcore
        public async Task FindAsync_ExposesBinDataCommercial()
#else
        public void FindAsync_ExposesBinDataCommercial()
        {
            Task.Run(async () =>
#endif
        {
            string inputNonce = Nonce.TransactableCommercial;
            PaymentMethodNonce nonce = await gateway.PaymentMethodNonce.FindAsync(inputNonce);
            Assert.AreEqual(nonce.Nonce, inputNonce);
            Assert.IsNotNull(nonce.BinData);
            Assert.AreEqual(nonce.BinData.Commercial, Braintree.CreditCardCommercial.YES);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
#if netcore
        public async Task FindAsync_ExposesBinDataDebit()
#else
        public void FindAsync_ExposesBinDataDebit()
        {
            Task.Run(async () =>
#endif
        {
            string inputNonce = Nonce.TransactableDebit;
            PaymentMethodNonce nonce = await gateway.PaymentMethodNonce.FindAsync(inputNonce);
            Assert.AreEqual(nonce.Nonce, inputNonce);
            Assert.IsNotNull(nonce.BinData);
            Assert.AreEqual(nonce.BinData.Debit, Braintree.CreditCardDebit.YES);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
#if netcore
        public async Task FindAsync_ExposesBinDataDurbinRegulated()
#else
        public void FindAsync_ExposesBinDataDurbinRegulated()
        {
            Task.Run(async () =>
#endif
        {
            string inputNonce = Nonce.TransactableDurbinRegulated;
            PaymentMethodNonce nonce = await gateway.PaymentMethodNonce.FindAsync(inputNonce);
            Assert.AreEqual(nonce.Nonce, inputNonce);
            Assert.IsNotNull(nonce.BinData);
            Assert.AreEqual(nonce.BinData.DurbinRegulated, Braintree.CreditCardDurbinRegulated.YES);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
#if netcore
        public async Task FindAsync_ExposesBinDataHealthcare()
#else
        public void FindAsync_ExposesBinDataHealthcare()
        {
            Task.Run(async () =>
#endif
        {
            string inputNonce = Nonce.TransactableHealthcare;
            PaymentMethodNonce nonce = await gateway.PaymentMethodNonce.FindAsync(inputNonce);
            Assert.AreEqual(nonce.Nonce, inputNonce);
            Assert.IsNotNull(nonce.BinData);
            Assert.AreEqual(nonce.BinData.Healthcare, Braintree.CreditCardHealthcare.YES);
            Assert.AreEqual(nonce.BinData.ProductId, "J3");
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
#if netcore
        public async Task FindAsync_ExposesBinDataPayroll()
#else
        public void FindAsync_ExposesBinDataPayroll()
        {
            Task.Run(async () =>
#endif
        {
            string inputNonce = Nonce.TransactablePayroll;
            PaymentMethodNonce nonce = await gateway.PaymentMethodNonce.FindAsync(inputNonce);
            Assert.AreEqual(nonce.Nonce, inputNonce);
            Assert.IsNotNull(nonce.BinData);
            Assert.AreEqual(nonce.BinData.Payroll, Braintree.CreditCardPayroll.YES);
            Assert.AreEqual(nonce.BinData.ProductId, "MSA");
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
#if netcore
        public async Task FindAsync_ExposesBinDataIssuingBank()
#else
        public void FindAsync_ExposesBinDataIssuingBank()
        {
            Task.Run(async () =>
#endif
        {
            string inputNonce = Nonce.TransactableIssuingBankNetworkOnly;
            PaymentMethodNonce nonce = await gateway.PaymentMethodNonce.FindAsync(inputNonce);
            Assert.AreEqual(nonce.Nonce, inputNonce);
            Assert.IsNotNull(nonce.BinData);
            Assert.AreEqual(nonce.BinData.IssuingBank, "NETWORK ONLY");
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
#if netcore
        public async Task FindAsync_ExposesBinDataCountryOfIssuance()
#else
        public void FindAsync_ExposesBinDataCountryOfIssuance()
        {
            Task.Run(async () =>
#endif
        {
            string inputNonce = Nonce.TransactableCountryOfIssuanceUSA;
            PaymentMethodNonce nonce = await gateway.PaymentMethodNonce.FindAsync(inputNonce);
            Assert.AreEqual(nonce.Nonce, inputNonce);
            Assert.IsNotNull(nonce.BinData);
            Assert.AreEqual(nonce.BinData.CountryOfIssuance, "USA");
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

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
