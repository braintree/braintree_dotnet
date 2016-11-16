using Braintree.Exceptions;
using Braintree.Test;
using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Linq;
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
            UsBankAccount usbankAccount = (UsBankAccount) paymentMethodResult.Target;

            UsBankAccountGateway usBankAccountGateway = new UsBankAccountGateway(gateway);
            usbankAccount = usBankAccountGateway.Find(usbankAccount.Token);

            Assert.AreEqual("123456789", usbankAccount.RoutingNumber);
            Assert.AreEqual("1234", usbankAccount.Last4);
            Assert.AreEqual("checking", usbankAccount.AccountType);
            Assert.AreEqual("PayPal Checking - 1234", usbankAccount.AccountDescription);
            Assert.AreEqual("Dan Schulman", usbankAccount.AccountHolderName);
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
            UsBankAccount usbankAccount = (UsBankAccount) paymentMethodResult.Target;

            var transactionRequest = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID
            };

            UsBankAccountGateway usBankAccountGateway = new UsBankAccountGateway(gateway);
            Result<Transaction> transactionResult = usBankAccountGateway.Sale(usbankAccount.Token, transactionRequest);

            Assert.IsTrue(transactionResult.IsSuccess());
            Transaction transaction = transactionResult.Target;
            Assert.AreEqual(TransactionStatus.SETTLEMENT_PENDING, transaction.Status);

            UsBankAccountDetails usBankAccountDetails = transaction.UsBankAccountDetails;
            Assert.AreEqual(usbankAccount.RoutingNumber, usBankAccountDetails.RoutingNumber);
            Assert.AreEqual(usbankAccount.Last4, usBankAccountDetails.Last4);
            Assert.AreEqual(usbankAccount.AccountType, usBankAccountDetails.AccountType);
            Assert.AreEqual(usbankAccount.AccountDescription, usBankAccountDetails.AccountDescription);
            Assert.AreEqual(usbankAccount.AccountHolderName, usBankAccountDetails.AccountHolderName);
        }

    }
}
