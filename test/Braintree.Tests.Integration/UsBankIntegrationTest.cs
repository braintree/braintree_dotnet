using Braintree.Exceptions;
using Braintree.Test;
using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Params = System.Collections.Generic.Dictionary<string, object>;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class UsBankIntegrationTest
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
        public void Find_FindsUsBankAccountWithToken()
        {
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());

            string nonce = TestHelper.GenerateValidUsBankAccountNonce(gateway);
            var request = new PaymentMethodRequest
            {
                CustomerId = result.Target.Id,
                PaymentMethodNonce = nonce,
                Options = new PaymentMethodOptionsRequest {
                  VerificationMerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID
                }
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(paymentMethodResult.IsSuccess());

            Assert.IsInstanceOf(typeof(UsBankAccount), paymentMethodResult.Target);
            UsBankAccount usBankAccount = (UsBankAccount) paymentMethodResult.Target;

            UsBankAccountGateway usBankAccountGateway = new UsBankAccountGateway(gateway);
            usBankAccount = usBankAccountGateway.Find(usBankAccount.Token);

            Assert.AreEqual("021000021", usBankAccount.RoutingNumber);
            Assert.AreEqual("0000", usBankAccount.Last4);
            Assert.AreEqual("checking", usBankAccount.AccountType);
            Assert.AreEqual("Dan Schulman", usBankAccount.AccountHolderName);
            Assert.IsTrue(Regex.IsMatch(usBankAccount.BankName, ".*CHASE.*"));
            AchMandate achMandate = usBankAccount.AchMandate;
            Assert.AreEqual("cl mandate text", achMandate.Text);
            Assert.AreEqual("DateTime", achMandate.AcceptedAt.GetType().Name);
            Assert.IsTrue(usBankAccount.IsDefault);
        }

        [Test]
        public void Find_RaisesNotFoundErrorWhenTokenDoesntExist()
        {
            UsBankAccountGateway usBankAccountGateway = new UsBankAccountGateway(gateway);
            string nonce = TestHelper.GenerateInvalidUsBankAccountNonce();
            Assert.Throws<NotFoundException>(() => usBankAccountGateway.Find(nonce));
        }

        [Test]
        public void Sale_TransactUsBankAccountWithToken()
        {
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());

            string nonce = TestHelper.GenerateValidUsBankAccountNonce(gateway);
            var request = new PaymentMethodRequest
            {
                CustomerId = result.Target.Id,
                PaymentMethodNonce = nonce,
                Options = new PaymentMethodOptionsRequest {
                  VerificationMerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID
                }
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(paymentMethodResult.IsSuccess());

            Assert.IsInstanceOf(typeof(UsBankAccount), paymentMethodResult.Target);
            UsBankAccount usBankAccount = (UsBankAccount) paymentMethodResult.Target;

            Assert.IsTrue(usBankAccount.IsVerified);
            Assert.AreEqual(1, usBankAccount.Verifications.Count);

            var transactionRequest = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID
            };

            UsBankAccountGateway usBankAccountGateway = new UsBankAccountGateway(gateway);
            Result<Transaction> transactionResult = usBankAccountGateway.Sale(usBankAccount.Token, transactionRequest);

            Assert.IsTrue(transactionResult.IsSuccess());
            Transaction transaction = transactionResult.Target;
            Assert.AreEqual(TransactionStatus.SETTLEMENT_PENDING, transaction.Status);

            UsBankAccountDetails usBankAccountDetails = transaction.UsBankAccountDetails;
            Assert.AreEqual(usBankAccount.RoutingNumber, usBankAccountDetails.RoutingNumber);
            Assert.AreEqual(usBankAccount.Last4, usBankAccountDetails.Last4);
            Assert.AreEqual(usBankAccount.AccountType, usBankAccountDetails.AccountType);
            Assert.AreEqual(usBankAccount.AccountHolderName, usBankAccountDetails.AccountHolderName);
            AchMandate achMandate = usBankAccountDetails.AchMandate;
            Assert.AreEqual(usBankAccount.AchMandate.Text, achMandate.Text);
            Assert.AreEqual("DateTime", achMandate.AcceptedAt.GetType().Name);
        }

        [Test]
        public void CompliantMerchant_FailsToTransactUnverifiedToken()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration2_merchant_id",
                PublicKey = "integration2_public_key",
                PrivateKey = "integration2_private_key"
            };

            Result<Customer> customerResult = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(customerResult.IsSuccess());

            string nonce = TestHelper.GenerateValidUsBankAccountNonce(gateway);
            var request = new PaymentMethodRequest
            {
                CustomerId = customerResult.Target.Id,
                PaymentMethodNonce = nonce,
                Options = new PaymentMethodOptionsRequest {
                  VerificationMerchantAccountId = "another_us_bank_merchant_account"
                }
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(paymentMethodResult.IsSuccess());

            Assert.IsInstanceOf(typeof(UsBankAccount), paymentMethodResult.Target);
            UsBankAccount usBankAccount = (UsBankAccount) paymentMethodResult.Target;

            Assert.IsFalse(usBankAccount.IsVerified);
            Assert.AreEqual(0, usBankAccount.Verifications.Count);

            var transactionRequest = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = "another_us_bank_merchant_account"
            };

            UsBankAccountGateway usBankAccountGateway = new UsBankAccountGateway(gateway);
            Result<Transaction> transactionResult = usBankAccountGateway.Sale(usBankAccount.Token, transactionRequest);

            Assert.IsFalse(transactionResult.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_US_BANK_ACCOUNT_MUST_BE_VERIFIED,
                transactionResult.Errors.ForObject("transaction").OnField("payment-method-token")[0].Code);
        }

        [Test]
        public void CompliantMerchant_FailsToTransactNonPlaidNonce()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration2_merchant_id",
                PublicKey = "integration2_public_key",
                PrivateKey = "integration2_private_key"
            };

            Result<Customer> customerResult = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(customerResult.IsSuccess());

            var transactionRequest = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CustomerId = customerResult.Target.Id,
                MerchantAccountId = "another_us_bank_merchant_account",
                PaymentMethodNonce = TestHelper.GenerateValidUsBankAccountNonce(gateway),
            };

            Result<Transaction> transactionResult = gateway.Transaction.Sale(transactionRequest);

            Assert.IsFalse(transactionResult.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_US_BANK_ACCOUNT_NONCE_MUST_BE_PLAID_VERIFIED,
                transactionResult.Errors.ForObject("transaction").OnField("payment-method-nonce")[0].Code);
        }
    }
}
