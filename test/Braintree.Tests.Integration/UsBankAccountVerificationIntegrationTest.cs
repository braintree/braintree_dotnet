using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class UsBankAccountVerificationIntegrationTest
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
        public void Create_ReturnsSuccessfulResponse()
        {
            Result<Customer> customer = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(customer.IsSuccess());

            var request = new PaymentMethodRequest
            {
                CustomerId = customer.Target.Id,
                PaymentMethodNonce = TestHelper.GenerateValidUsBankAccountNonce(gateway),
                Options = new PaymentMethodOptionsRequest
                {
                    VerificationMerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID,
                    UsBankAccountVerificationMethod = UsBankAccountVerificationMethod.NETWORK_CHECK
                }
            };

            Result<PaymentMethod> result = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(result.IsSuccess());
            UsBankAccount usBankAccount = (UsBankAccount) result.Target;

            Assert.IsNotNull(usBankAccount.Token);
            UsBankAccountVerification verification = usBankAccount.Verifications[0];

            Assert.AreEqual(UsBankAccountVerificationMethod.NETWORK_CHECK, verification.VerificationMethod);
            Assert.AreEqual(UsBankAccountVerificationStatus.VERIFIED, verification.Status);
            Assert.NotNull(verification.Id);
            Assert.NotNull(verification.VerificationDeterminedAt);
        }

        [Test]
        public void Create_HandlesInvalidResponse()
        {
            Result<Customer> customer = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(customer.IsSuccess());

            var request = new PaymentMethodRequest
            {
                CustomerId = customer.Target.Id,
                PaymentMethodNonce = TestHelper.GenerateValidUsBankAccountNonce(gateway),
                Options = new PaymentMethodOptionsRequest
                {
                    VerificationMerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID,
                    UsBankAccountVerificationMethod = UsBankAccountVerificationMethod.UNRECOGNIZED
                }
            };

            Result<PaymentMethod> result = gateway.PaymentMethod.Create(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.PAYMENT_METHOD_OPTIONS_INVALID_US_BANK_ACCOUNT_VERIFICATION_METHOD,
                result.Errors.ForObject("payment-method").ForObject("options").OnField("us-bank-account-verification-method")[0].Code);
        }

        [Test]
        public void Search_OnMultipleValueFields()
        {
            Result<Customer> customer = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(customer.IsSuccess());

            var request1 = new PaymentMethodRequest
            {
                CustomerId = customer.Target.Id,
                PaymentMethodNonce = TestHelper.GenerateValidUsBankAccountNonce(gateway),
                Options = new PaymentMethodOptionsRequest
                {
                    VerificationMerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID,
                    UsBankAccountVerificationMethod = UsBankAccountVerificationMethod.NETWORK_CHECK
                }
            };

            var request2 = new PaymentMethodRequest
            {
                CustomerId = customer.Target.Id,
                PaymentMethodNonce = TestHelper.GenerateValidUsBankAccountNonce(gateway),
                Options = new PaymentMethodOptionsRequest
                {
                    VerificationMerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID,
                    UsBankAccountVerificationMethod = UsBankAccountVerificationMethod.INDEPENDENT_CHECK
                }
            };

            UsBankAccount result1 = (UsBankAccount) gateway.PaymentMethod.Create(request1).Target;
            UsBankAccount result2 = (UsBankAccount) gateway.PaymentMethod.Create(request2).Target;

            UsBankAccountVerification verification1 = gateway.UsBankAccountVerification.Find(result1.Verifications[0].Id);
            UsBankAccountVerification verification2 = gateway.UsBankAccountVerification.Find(result2.Verifications[0].Id);

            UsBankAccountVerificationSearchRequest searchRequest = new UsBankAccountVerificationSearchRequest().
                VerificationMethod.IncludedIn(UsBankAccountVerificationMethod.INDEPENDENT_CHECK,
                                              UsBankAccountVerificationMethod.NETWORK_CHECK).
                Ids.IncludedIn(verification1.Id, verification2.Id).
                Status.IncludedIn(UsBankAccountVerificationStatus.VERIFIED);

            ResourceCollection<UsBankAccountVerification> collection = gateway.UsBankAccountVerification.Search(searchRequest);

            Assert.AreEqual(2, collection.MaximumCount);
        }

        [Test]
#if netcore
        public async Task SearchAsync_OnMultipleValueFields()
#else
        public void SearchAsync_OnMultipleValueFields()
        {
            Task.Run(async () =>
#endif
        {
            Result<Customer> customer = await gateway.Customer.CreateAsync(new CustomerRequest());
            Assert.IsTrue(customer.IsSuccess());

            var request1 = new PaymentMethodRequest
            {
                CustomerId = customer.Target.Id,
                PaymentMethodNonce = TestHelper.GenerateValidUsBankAccountNonce(gateway),
                Options = new PaymentMethodOptionsRequest
                {
                    VerificationMerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID,
                    UsBankAccountVerificationMethod = UsBankAccountVerificationMethod.NETWORK_CHECK
                }
            };

            var request2 = new PaymentMethodRequest
            {
                CustomerId = customer.Target.Id,
                PaymentMethodNonce = TestHelper.GenerateValidUsBankAccountNonce(gateway),
                Options = new PaymentMethodOptionsRequest
                {
                    VerificationMerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID,
                    UsBankAccountVerificationMethod = UsBankAccountVerificationMethod.INDEPENDENT_CHECK
                }
            };

            UsBankAccount result1 = (UsBankAccount) (await gateway.PaymentMethod.CreateAsync(request1)).Target;
            UsBankAccount result2 = (UsBankAccount) (await gateway.PaymentMethod.CreateAsync(request2)).Target;

            UsBankAccountVerification verification1 = gateway.UsBankAccountVerification.Find(result1.Verifications[0].Id);
            UsBankAccountVerification verification2 = gateway.UsBankAccountVerification.Find(result2.Verifications[0].Id);

            UsBankAccountVerificationSearchRequest searchRequest = new UsBankAccountVerificationSearchRequest().
                VerificationMethod.IncludedIn(UsBankAccountVerificationMethod.INDEPENDENT_CHECK,
                                              UsBankAccountVerificationMethod.NETWORK_CHECK).
                Ids.IncludedIn(verification1.Id, verification2.Id).
                Status.IncludedIn(UsBankAccountVerificationStatus.VERIFIED);

            ResourceCollection<UsBankAccountVerification> collection = await gateway.UsBankAccountVerification.SearchAsync(searchRequest);

            Assert.AreEqual(2, collection.MaximumCount);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Search_OnTextFields()
        {
            Result<Customer> customer = gateway.Customer.Create(new CustomerRequest{
                Email = "mike.a@example.com",
            });
            Assert.IsTrue(customer.IsSuccess());

            var request = new PaymentMethodRequest
            {
                CustomerId = customer.Target.Id,
                PaymentMethodNonce = TestHelper.GenerateValidUsBankAccountNonce(gateway),
                Options = new PaymentMethodOptionsRequest
                {
                    VerificationMerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID,
                    UsBankAccountVerificationMethod = UsBankAccountVerificationMethod.NETWORK_CHECK
                }
            };

            UsBankAccount result = (UsBankAccount) gateway.PaymentMethod.Create(request).Target;

            string token = result.Token;
            string accountHolderName = result.AccountHolderName;
            string accountType = result.AccountType;
            string customerId = customer.Target.Id;
            string customerEmail = customer.Target.Email;

            UsBankAccountVerificationSearchRequest searchRequest = new UsBankAccountVerificationSearchRequest().
                PaymentMethodToken.Is(token).
                CustomerId.Is(customerId).
                CustomerEmail.Is(customerEmail).
                AccountHolderName.Is(accountHolderName).
                AccountType.Is(accountType);

            ResourceCollection<UsBankAccountVerification> collection = gateway.UsBankAccountVerification.Search(searchRequest);
            UsBankAccountVerification verification = collection.FirstItem;

            Assert.AreEqual(1, collection.MaximumCount);
            Assert.AreEqual(accountHolderName, verification.UsBankAccount.AccountHolderName);
            Assert.AreEqual(accountType, verification.UsBankAccount.AccountType);
        }
    }
}
