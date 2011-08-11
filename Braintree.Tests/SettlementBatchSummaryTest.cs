using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Braintree;

namespace Braintree.Tests
{
    [TestFixture]
    public class SettlementBatchSummaryTest
    {
        private BraintreeGateway gateway;
        private BraintreeService service;

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

            service = new BraintreeService(gateway.Configuration);
        }

        [Test]
        public void Generate_ReturnsAnEmptyCollectionIfThereIsNoData()
        {
            Result<SettlementBatchSummary> result = gateway.SettlementBatchSummary.Generate(DateTime.Parse("1979-01-01"));
            Assert.AreEqual(0, result.Target.Results.Count);
        }

        [Test]
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
            TestHelper.Settle(service, transaction.Id);
            transaction = gateway.Transaction.Find(transaction.Id);

            var result = gateway.SettlementBatchSummary.Generate(DateTime.Now);
            var visas = new List<IDictionary<String,String>>();
            foreach (var row in result.Target.Results)
            {
                if (Braintree.CreditCardCardType.VISA.ToString().Equals(row["card_type"]))
                {
                    visas.Add(row);

                }
            }

            Assert.AreEqual(1, visas.Count);
        }

        [Test]
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
                CustomFields = new Dictionary<String, String>
                {
                    { "store_me", "custom value" }
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            TestHelper.Settle(service, transaction.Id);
            transaction = gateway.Transaction.Find(transaction.Id);

            var result = gateway.SettlementBatchSummary.Generate(DateTime.Now, "store_me");
            var customValues = new List<IDictionary<String, String>>();
            foreach (var row in result.Target.Results)
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

