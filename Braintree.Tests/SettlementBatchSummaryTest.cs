using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using NUnit.Framework;
using Braintree;
using System.Threading;

namespace Braintree.Tests
{
    [TestFixture]
    public class SettlementBatchSummaryTest
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
        [Category("Integration")]
        public void Generate_ReturnsAnEmptyCollectionIfThereIsNoData()
        {
            Result<SettlementBatchSummary> result = gateway.SettlementBatchSummary.Generate(DateTime.Parse("1979-01-01"));
            Assert.AreEqual(0, result.Target.Records.Count);
        }

        [Test]
        [Category("Integration")]
        public void Generate_ReturnsTransactionsSettledOnAGivenDay()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = 1000M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = "4111111111111111",
                    ExpirationDate = "05/2012",
                    CardholderName = "Tom Smith",
                },
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                },
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            Transaction settlementResult = gateway.TestTransaction.Settle(transaction.Id);
            var settlementDate = settlementResult.SettlementBatchId.Substring(0,10);
            transaction = gateway.Transaction.Find(transaction.Id);
            var result = gateway.SettlementBatchSummary.Generate(System.DateTime.Parse(settlementDate));
            var visas = new List<IDictionary<string,string>>();
            foreach (var row in result.Target.Records)
            {
                if (Braintree.CreditCardCardType.VISA.ToString().Equals(row["card_type"]))
                {
                    visas.Add(row);

                }
            }

            Assert.AreEqual(1, visas.Count);
        }

        [Test]
        [Category("Integration")]
        public void Generate_AcceptsDatesInNonUSFormats()
        {
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;
            CultureInfo australianCulture = new CultureInfo("en-AU");
            Thread.CurrentThread.CurrentCulture = australianCulture;

            DateTime date = new DateTime(2014, 8, 20);
            var result = gateway.SettlementBatchSummary.Generate(date);

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(australianCulture, Thread.CurrentThread.CurrentCulture);
            Thread.CurrentThread.CurrentCulture = originalCulture;
        }

        [Test]
        [Category("Integration")]
        public void Generate_CanBeGroupedByACustomField()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = 1000M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = "4111111111111111",
                    ExpirationDate = "05/2012",
                    CardholderName = "Tom Smith",
                },
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                },
                CustomFields = new Dictionary<string, string>
                {
                    { "store_me", "custom value" }
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            Transaction settlementResult = gateway.TestTransaction.Settle(transaction.Id);
            var settlementDate = settlementResult.SettlementBatchId.Substring(0,10);
            transaction = gateway.Transaction.Find(transaction.Id);

            var result = gateway.SettlementBatchSummary.Generate(System.DateTime.Parse(settlementDate), "store_me");
            var customValues = new List<IDictionary<string, string>>();
            foreach (var row in result.Target.Records)
            {
                if ("custom value".Equals(row["store_me"]))
                {
                    customValues.Add(row);

                }
            }

            Assert.AreEqual(1, customValues.Count);
        }
    }
}

