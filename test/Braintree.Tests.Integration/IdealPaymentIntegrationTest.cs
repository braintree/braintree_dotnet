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
    public class IdealPaymentIntegrationTest
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
        public void Find_FindsIdealPaymentWithId()
        {
            string idealPaymentId = TestHelper.GenerateValidIdealPaymentId(gateway);

            IdealPaymentGateway idealPaymentGateway = new IdealPaymentGateway(gateway);
            var idealPayment = idealPaymentGateway.Find(idealPaymentId);

            Assert.IsTrue(Regex.IsMatch(idealPayment.Id, "^idealpayment_\\w{6,}$"));
            Assert.IsTrue(Regex.IsMatch(idealPayment.IdealTransactionId, "^\\d{16,}$"));
            Assert.IsNotNull(idealPayment.Currency);
            Assert.IsNotNull(idealPayment.Amount);
            Assert.IsNotNull(idealPayment.Status);
            Assert.IsNotNull(idealPayment.OrderId);
            Assert.IsNotNull(idealPayment.Issuer);
            Assert.IsTrue(idealPayment.ApprovalUrl.StartsWith("https://"));
            Assert.IsNotNull(idealPayment.IbanBankAccount.AccountHolderName);
            Assert.IsNotNull(idealPayment.IbanBankAccount.Bic);
            Assert.IsNotNull(idealPayment.IbanBankAccount.MaskedIban);
            Assert.IsTrue(Regex.IsMatch(idealPayment.IbanBankAccount.IbanAccountNumberLast4, "^\\d{4}$"));
            Assert.IsNotNull(idealPayment.IbanBankAccount.IbanCountry);
            Assert.IsNotNull(idealPayment.IbanBankAccount.Description);
        }

        [Test]
        public void Find_RaisesNotFoundErrorWhenPaymentDoesntExist()
        {
            IdealPaymentGateway idealPaymentGateway = new IdealPaymentGateway(gateway);
            string idealPaymentId = TestHelper.GenerateInvalidIdealPaymentId();
            Assert.Throws<NotFoundException>(() => idealPaymentGateway.Find(idealPaymentId));
        }

        [Test]
        public void Sale_TransactIdealPaymentWithId()
        {
            string idealPaymentId = TestHelper.GenerateValidIdealPaymentId(gateway);
            var transactionRequest = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = "ideal_merchant_account",
                OrderId = "ABC123"
            };

            IdealPaymentGateway idealPaymentGateway = new IdealPaymentGateway(gateway);
            Result<Transaction> transactionResult = idealPaymentGateway.Sale(idealPaymentId, transactionRequest);

            Assert.IsTrue(transactionResult.IsSuccess());
            Transaction transaction = transactionResult.Target;
            Assert.AreEqual(TransactionStatus.SETTLED, transaction.Status);

            IdealPaymentDetails idealPaymentDetails = transaction.IdealPaymentDetails;
            Assert.AreEqual(TransactionStatus.SETTLED, transaction.Status);
            Assert.IsTrue(Regex.IsMatch(idealPaymentDetails.IdealPaymentId, "^idealpayment_\\w{6,}$"));
            Assert.IsTrue(Regex.IsMatch(idealPaymentDetails.IdealTransactionId, "^\\d{16,}$"));
            Assert.IsTrue(idealPaymentDetails.ImageUrl.StartsWith("https://"));
            Assert.IsNotNull(idealPaymentDetails.MaskedIban);
            Assert.IsNotNull(idealPaymentDetails.Bic);
        }
    }
}
