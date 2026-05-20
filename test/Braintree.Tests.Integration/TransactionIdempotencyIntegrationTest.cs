using Braintree.Test;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class TransactionIdempotencyIntegrationTest
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
        public void Sale_WithApiRequestKey_ReturnsOriginalTransactionOnDuplicateRequest()
        {
            string apiRequestKey = "idempotency-key-" + new Random().Next(1000000);

            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                ApiRequestKey = apiRequestKey,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2035"
                }
            };

            Result<Transaction> result1 = gateway.Transaction.Sale(request);
            Assert.IsTrue(result1.IsSuccess());
            Transaction transaction1 = result1.Target;
            Assert.IsNotNull(transaction1.Id);

            Result<Transaction> result2 = gateway.Transaction.Sale(request);
            Assert.IsTrue(result2.IsSuccess());
            Transaction transaction2 = result2.Target;

            Assert.AreEqual(transaction1.Status, transaction2.Status);
            Assert.AreEqual(transaction1.Id, transaction2.Id);
        }

        [Test]
        public void Sale_WithApiRequestKey_FailsWhenDifferentRequestUsedWithSameKey()
        {
            string apiRequestKey = "idempotency-key-" + new Random().Next(1000000);

            var request1 = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                ApiRequestKey = apiRequestKey,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2035"
                }
            };

            Result<Transaction> result1 = gateway.Transaction.Sale(request1);
            Assert.IsTrue(result1.IsSuccess());

            var request2 = new TransactionRequest
            {
                Amount = 200.00M,
                ApiRequestKey = apiRequestKey,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2035"
                }
            };

            Result<Transaction> result2 = gateway.Transaction.Sale(request2);

            Assert.IsFalse(result2.IsSuccess());
            Assert.IsNotNull(result2.Errors);
            Assert.IsNotNull(result2.Errors.DeepAll());
            Assert.Greater(result2.Errors.DeepAll().Count, 0);
            Assert.AreEqual(ValidationErrorCode.API_REQUEST_KEY_CAN_BE_REUSED_ONLY_WITH_THE_SAME_REQUEST,
                result2.Errors.DeepAll()[0].Code);
        }

        [Test]
        public void Sale_SameSalesWithDifferentApiRequestKey()
        {
            string apiRequestKey1 = "idempotency-key-" + new Random().Next(1000000);

            var request1 = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                ApiRequestKey = apiRequestKey1,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2035"
                }
            };

            Result<Transaction> result1 = gateway.Transaction.Sale(request1);
            Assert.IsTrue(result1.IsSuccess());
            Transaction transaction1 = result1.Target;
            Assert.IsNotNull(transaction1.Id);

            string apiRequestKey2 = "idempotency-key-" + new Random().Next(1000000);
            var request2 = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                ApiRequestKey = apiRequestKey2,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2035"
                }
            };

            Result<Transaction> result2 = gateway.Transaction.Sale(request2);
            Assert.IsTrue(result2.IsSuccess());
            Transaction transaction2 = result2.Target;

            Assert.AreNotEqual(transaction1.Id, transaction2.Id);
        }

        [Test]
        public void Sale_WithApiRequestKey_FailsWhenApiRequestKeyIsTooLong()
        {
            var request1 = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                ApiRequestKey = new string('a', 255),
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2035"
                }
            };

            Result<Transaction> result1 = gateway.Transaction.Sale(request1);
            Assert.IsTrue(result1.IsSuccess());

            var request2 = new TransactionRequest
            {
                Amount = 200.00M,
                ApiRequestKey = new string('b', 256),
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2035"
                }
            };

            Result<Transaction> result2 = gateway.Transaction.Sale(request2);

            Assert.IsFalse(result2.IsSuccess());
            Assert.IsNotNull(result2.Errors);
            Assert.IsNotNull(result2.Errors.DeepAll());
            Assert.Greater(result2.Errors.DeepAll().Count, 0);
            Assert.AreEqual(ValidationErrorCode.API_REQUEST_KEY_TOO_LONG,
                result2.Errors.DeepAll()[0].Code);
        }

        [Test]
        public void SubmitForPartialSettlement_WithApiRequestKey_ReturnsOriginalOnDuplicateRequest()
        {
            string apiRequestKey = "partial-settlement-idempotency-key-" + new Random().Next(1000000);

            var saleRequest = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2035"
                }
            };

            Result<Transaction> saleResult = gateway.Transaction.Sale(saleRequest);
            Assert.IsTrue(saleResult.IsSuccess());
            string transactionId = saleResult.Target.Id;

            decimal partialAmount = 50.00M;
            var partialSettlementRequest = new TransactionRequest
            {
                Amount = partialAmount,
                ApiRequestKey = apiRequestKey
            };

            Result<Transaction> partialSettlementResult1 = gateway.Transaction.SubmitForPartialSettlement(transactionId, partialSettlementRequest);
            Assert.IsTrue(partialSettlementResult1.IsSuccess());
            Transaction partialSettlementTransaction1 = partialSettlementResult1.Target;
            Assert.AreEqual(partialAmount, partialSettlementTransaction1.Amount);
            Assert.IsNotNull(partialSettlementTransaction1.Id);

            Result<Transaction> partialSettlementResult2 = gateway.Transaction.SubmitForPartialSettlement(transactionId, partialSettlementRequest);
            Assert.IsTrue(partialSettlementResult2.IsSuccess());
            Transaction partialSettlementTransaction2 = partialSettlementResult2.Target;

            Assert.AreEqual(partialSettlementTransaction1.Id, partialSettlementTransaction2.Id);
            Assert.AreEqual(partialSettlementTransaction1.Amount, partialSettlementTransaction2.Amount);
        }

        [Test]
        public void SubmitForSettlement_WithApiRequestKey_ReturnsOriginalOnDuplicateRequest()
        {
            string apiRequestKey = "settlement-idempotency-key-" + new Random().Next(1000000);

            var saleRequest = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2035"
                }
            };

            Result<Transaction> saleResult = gateway.Transaction.Sale(saleRequest);
            Assert.IsTrue(saleResult.IsSuccess());
            string transactionId = saleResult.Target.Id;
            decimal originalAmount = saleResult.Target.Amount.Value;

            var settlementRequest = new TransactionRequest
            {
                ApiRequestKey = apiRequestKey
            };

            Result<Transaction> settlementResult1 = gateway.Transaction.SubmitForSettlement(transactionId, settlementRequest);
            Assert.IsTrue(settlementResult1.IsSuccess());
            Transaction settlementTransaction1 = settlementResult1.Target;
            Assert.AreEqual(originalAmount, settlementTransaction1.Amount);
            Assert.IsNotNull(settlementTransaction1.Id);

            Result<Transaction> settlementResult2 = gateway.Transaction.SubmitForSettlement(transactionId, settlementRequest);
            Assert.IsTrue(settlementResult2.IsSuccess());
            Transaction settlementTransaction2 = settlementResult2.Target;

            Assert.AreEqual(settlementTransaction1.Id, settlementTransaction2.Id);
            Assert.AreEqual(settlementTransaction1.Amount, settlementTransaction2.Amount);
        }

        [Test]
        public void Void_WithApiRequestKey_ReturnsOriginalVoidOnDuplicateRequest()
        {
            string apiRequestKey = "void-idempotency-key-" + new Random().Next(1000000);

            var saleRequest = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2035"
                }
            };

            Result<Transaction> saleResult = gateway.Transaction.Sale(saleRequest);
            Assert.IsTrue(saleResult.IsSuccess());
            string transactionId = saleResult.Target.Id;

            var voidRequest = new TransactionVoidRequest
            {
                ApiRequestKey = apiRequestKey
            };

            Result<Transaction> voidResult1 = gateway.Transaction.Void(transactionId, voidRequest);
            Assert.IsTrue(voidResult1.IsSuccess());
            Transaction voidedTransaction1 = voidResult1.Target;
            Assert.AreEqual(TransactionStatus.VOIDED, voidedTransaction1.Status);

            Result<Transaction> voidResult2 = gateway.Transaction.Void(transactionId, voidRequest);
            Assert.IsTrue(voidResult2.IsSuccess());
            Transaction voidedTransaction2 = voidResult2.Target;

            Assert.AreEqual(voidedTransaction1.Id, voidedTransaction2.Id);
            Assert.AreEqual(voidedTransaction1.Status, voidedTransaction2.Status);
            Assert.AreEqual(TransactionStatus.VOIDED, voidedTransaction2.Status);
        }

        [Test]
        public void Refund_WithApiRequestKey_ReturnsOriginalRefundOnDuplicateRequest()
        {
            string apiRequestKey = "refund-idempotency-key-" + new Random().Next(1000000);

            var saleRequest = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2035"
                },
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> saleResult = gateway.Transaction.Sale(saleRequest);
            Assert.IsTrue(saleResult.IsSuccess());
            string transactionId = saleResult.Target.Id;

            Transaction settledTransaction = gateway.TestTransaction.Settle(transactionId);
            Assert.AreEqual(TransactionStatus.SETTLED, settledTransaction.Status);

            var refundRequest = new TransactionRefundRequest
            {
                ApiRequestKey = apiRequestKey
            };

            Result<Transaction> refundResult1 = gateway.Transaction.Refund(transactionId, refundRequest);
            Assert.IsTrue(refundResult1.IsSuccess());
            Transaction refundTransaction1 = refundResult1.Target;
            Assert.AreEqual(TransactionType.CREDIT, refundTransaction1.Type);
            Assert.IsNotNull(refundTransaction1.Id);

            Result<Transaction> refundResult2 = gateway.Transaction.Refund(transactionId, refundRequest);
            Assert.IsTrue(refundResult2.IsSuccess());
            Transaction refundTransaction2 = refundResult2.Target;

            Assert.AreEqual(refundTransaction1.Id, refundTransaction2.Id);
            Assert.AreEqual(refundTransaction1.Type, refundTransaction2.Type);
        }

        [Test]
        public void Credit_WithApiRequestKey_ReturnsOriginalOnDuplicateRequest()
        {
            string apiRequestKey = "credit-idempotency-key-" + new Random().Next(1000000);

            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                ApiRequestKey = apiRequestKey,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2035"
                }
            };

            Result<Transaction> creditResult1 = gateway.Transaction.Credit(request);
            Assert.IsTrue(creditResult1.IsSuccess());
            Transaction creditTransaction1 = creditResult1.Target;
            Assert.AreEqual(TransactionType.CREDIT, creditTransaction1.Type);
            Assert.IsNotNull(creditTransaction1.Id);

            Result<Transaction> creditResult2 = gateway.Transaction.Credit(request);
            Assert.IsTrue(creditResult2.IsSuccess());
            Transaction creditTransaction2 = creditResult2.Target;

            Assert.AreEqual(creditTransaction1.Id, creditTransaction2.Id);
            Assert.AreEqual(creditTransaction1.Type, creditTransaction2.Type);
        }
    }
}
