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
                PaymentMethodNonce = nonce
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(paymentMethodResult.IsSuccess());

            Assert.IsInstanceOf(typeof(UsBankAccount), paymentMethodResult.Target);
            UsBankAccount usBankAccount = (UsBankAccount) paymentMethodResult.Target;

            UsBankAccountGateway usBankAccountGateway = new UsBankAccountGateway(gateway);
            usBankAccount = usBankAccountGateway.Find(usBankAccount.Token);

            Assert.AreEqual("021000021", usBankAccount.RoutingNumber);
            Assert.AreEqual("1234", usBankAccount.Last4);
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
                PaymentMethodNonce = nonce
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(paymentMethodResult.IsSuccess());

            Assert.IsInstanceOf(typeof(UsBankAccount), paymentMethodResult.Target);
            UsBankAccount usBankAccount = (UsBankAccount) paymentMethodResult.Target;

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

    }
}
