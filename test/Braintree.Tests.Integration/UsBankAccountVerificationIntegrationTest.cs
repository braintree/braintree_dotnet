using Braintree.TestUtil;
using Braintree.Test;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

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
        public void SuccessfullyConfirmSettled_MicroTransferVerification()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration2_merchant_id",
                PublicKey = "integration2_public_key",
                PrivateKey = "integration2_private_key"
            };

            Result<Customer> customer = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(customer.IsSuccess());

            var request = new PaymentMethodRequest
            {
                CustomerId = customer.Target.Id,
                PaymentMethodNonce = TestHelper.GenerateValidUsBankAccountNonce(gateway, "1000000000"),
                Options = new PaymentMethodOptionsRequest
                {
                    VerificationMerchantAccountId = MerchantAccountIDs.ANOTHER_US_BANK_MERCHANT_ACCOUNT_ID,
                    UsBankAccountVerificationMethod = UsBankAccountVerificationMethod.MICRO_TRANSFERS
                }
            };

            Result<PaymentMethod> result = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(result.IsSuccess());
            UsBankAccount usBankAccount = (UsBankAccount) result.Target;

            Assert.IsNotNull(usBankAccount.Token);
            UsBankAccountVerification verification = usBankAccount.Verifications[0];

            Assert.AreEqual(UsBankAccountVerificationMethod.MICRO_TRANSFERS, verification.VerificationMethod);
            Assert.AreEqual(UsBankAccountVerificationStatus.PENDING, verification.Status);

            var confirmRequest = new UsBankAccountVerificationConfirmRequest
            {
                DepositAmounts = new int[] { 17, 29 }
            };

            var confirmResult = gateway.UsBankAccountVerification.ConfirmMicroTransferAmounts(verification.Id, confirmRequest);

            Assert.IsTrue(confirmResult.IsSuccess());

            verification = (UsBankAccountVerification) confirmResult.Target;

            Assert.AreEqual(UsBankAccountVerificationStatus.VERIFIED, verification.Status);

            usBankAccount = (UsBankAccount) gateway.PaymentMethod.Find(verification.UsBankAccount.Token);

            Assert.IsTrue(usBankAccount.IsVerified);
        }

        [Test]
        public void SuccessfullyConfirmUnsettled_MicroTransferVerification()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration2_merchant_id",
                PublicKey = "integration2_public_key",
                PrivateKey = "integration2_private_key"
            };

            Result<Customer> customer = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(customer.IsSuccess());

            var request = new PaymentMethodRequest
            {
                CustomerId = customer.Target.Id,
                PaymentMethodNonce = TestHelper.GenerateValidUsBankAccountNonce(gateway, "1000000001"),
                Options = new PaymentMethodOptionsRequest
                {
                    VerificationMerchantAccountId = MerchantAccountIDs.ANOTHER_US_BANK_MERCHANT_ACCOUNT_ID,
                    UsBankAccountVerificationMethod = UsBankAccountVerificationMethod.MICRO_TRANSFERS
                }
            };

            Result<PaymentMethod> result = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(result.IsSuccess());
            UsBankAccount usBankAccount = (UsBankAccount) result.Target;

            Assert.IsNotNull(usBankAccount.Token);
            UsBankAccountVerification verification = usBankAccount.Verifications[0];

            Assert.AreEqual(UsBankAccountVerificationMethod.MICRO_TRANSFERS, verification.VerificationMethod);
            Assert.AreEqual(UsBankAccountVerificationStatus.PENDING, verification.Status);

            var confirmRequest = new UsBankAccountVerificationConfirmRequest
            {
                DepositAmounts = new int[] { 17, 29 }
            };

            var confirmResult = gateway.UsBankAccountVerification.ConfirmMicroTransferAmounts(verification.Id, confirmRequest);

            Assert.IsTrue(confirmResult.IsSuccess());

            verification = (UsBankAccountVerification) confirmResult.Target;

            Assert.AreEqual(UsBankAccountVerificationStatus.PENDING, verification.Status);
        }

        [Test]
        public void MultipleAttemptConfirm_MicroTransferVerification()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration2_merchant_id",
                PublicKey = "integration2_public_key",
                PrivateKey = "integration2_private_key"
            };

            Result<Customer> customer = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(customer.IsSuccess());

            var request = new PaymentMethodRequest
            {
                CustomerId = customer.Target.Id,
                PaymentMethodNonce = TestHelper.GenerateValidUsBankAccountNonce(gateway, "1000000000"),
                Options = new PaymentMethodOptionsRequest
                {
                    VerificationMerchantAccountId = MerchantAccountIDs.ANOTHER_US_BANK_MERCHANT_ACCOUNT_ID,
                    UsBankAccountVerificationMethod = UsBankAccountVerificationMethod.MICRO_TRANSFERS
                }
            };

            Result<PaymentMethod> result = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(result.IsSuccess());
            UsBankAccount usBankAccount = (UsBankAccount) result.Target;

            Assert.IsNotNull(usBankAccount.Token);
            UsBankAccountVerification verification = usBankAccount.Verifications[0];

            Assert.AreEqual(UsBankAccountVerificationMethod.MICRO_TRANSFERS, verification.VerificationMethod);
            Assert.AreEqual(UsBankAccountVerificationStatus.PENDING, verification.Status);

            var confirmRequest = new UsBankAccountVerificationConfirmRequest
            {
                DepositAmounts = new int[] { 1, 1 }
            };

            for(int i = 0; i < 4; i++)
            {
                var r = gateway.UsBankAccountVerification.ConfirmMicroTransferAmounts(verification.Id, confirmRequest);
                Assert.IsFalse(r.IsSuccess());
                Assert.AreEqual(
                    ValidationErrorCode.US_BANK_ACCOUNT_VERIFICATION_AMOUNTS_DO_NOT_MATCH,
                    r.Errors.ForObject("us-bank-account-verification").OnField("base")[0].Code);
            }

            var confirmResult = gateway.UsBankAccountVerification.ConfirmMicroTransferAmounts(verification.Id, confirmRequest);
            Assert.IsFalse(confirmResult.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.US_BANK_ACCOUNT_VERIFICATION_TOO_MANY_CONFIRMATION_ATTEMPTS,
                confirmResult.Errors.ForObject("us-bank-account-verification").OnField("base")[0].Code);
        }

        [Test]
        public void Create_ReturnsSuccessfulResponse()
        {
            Result<Customer> customer = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(customer.IsSuccess());

            var request = new PaymentMethodRequest
            {
                CustomerId = customer.Target.Id,
                PaymentMethodNonce = Nonce.UsBankAccount,
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
        public void CreateWithAddOns_ReturnsSuccessfulResponse()
        {
            Result<Customer> customer = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(customer.IsSuccess());

            var request = new PaymentMethodRequest
            {
                CustomerId = customer.Target.Id,
                PaymentMethodNonce = Nonce.UsBankAccount,
                Options = new PaymentMethodOptionsRequest
                {
                    VerificationMerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID,
                    UsBankAccountVerificationMethod = UsBankAccountVerificationMethod.NETWORK_CHECK,
                    VerificationAddOns = VerificationAddOns.CUSTOMER_VERIFICATION
                }
            };

            Result<PaymentMethod> result = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(result.IsSuccess());
            UsBankAccount usBankAccount = (UsBankAccount) result.Target;

            Assert.AreEqual("0000", usBankAccount.Last4);
            Assert.AreEqual("Wells Fargo", usBankAccount.BankName);
            Assert.AreEqual("Marty McFly", usBankAccount.AccountHolderName);

            Assert.IsNotNull(usBankAccount.Token);
            UsBankAccountVerification verification = usBankAccount.Verifications[0];

            Assert.AreEqual(UsBankAccountVerificationMethod.NETWORK_CHECK, verification.VerificationMethod);
            Assert.AreEqual(UsBankAccountVerificationStatus.VERIFIED, verification.Status);
            Assert.NotNull(verification.Id);
            Assert.NotNull(verification.VerificationDeterminedAt);
            Assert.AreEqual(verification.ProcessorResponseCode, "1000");
        }

        [Test]
        public void Create_ReturnsAdditionalProcessorResponse()
        {
            Result<Customer> customer = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(customer.IsSuccess());

            var request = new PaymentMethodRequest
            {
                CustomerId = customer.Target.Id,
                PaymentMethodNonce = TestHelper.GenerateValidUsBankAccountNonce(gateway, "1000000005"),
                Options = new PaymentMethodOptionsRequest
                {
                    VerificationMerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID,
                    UsBankAccountVerificationMethod = UsBankAccountVerificationMethod.NETWORK_CHECK
                }
            };

            Result<PaymentMethod> result = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(result.IsSuccess());
            UsBankAccount usBankAccount = (UsBankAccount) result.Target;

            Assert.AreEqual("0005", usBankAccount.Last4);
            Assert.AreEqual("JPMORGAN CHASE", usBankAccount.BankName);
            Assert.AreEqual("Dan Schulman", usBankAccount.AccountHolderName);

            Assert.IsNotNull(usBankAccount.Token);
            UsBankAccountVerification verification = usBankAccount.Verifications[0];

            Assert.AreEqual(UsBankAccountVerificationMethod.NETWORK_CHECK, verification.VerificationMethod);
            Assert.AreEqual(UsBankAccountVerificationStatus.PROCESSOR_DECLINED, verification.Status);
            Assert.NotNull(verification.Id);
            Assert.NotNull(verification.VerificationDeterminedAt);
            Assert.AreEqual(verification.ProcessorResponseCode, "2061");
            Assert.AreEqual(verification.AdditionalProcessorResponse, "Invalid routing number");
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
