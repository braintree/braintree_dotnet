using Braintree.Exceptions;
using Braintree.Test;
using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class TransactionTransferTypeIntegrationTest
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
        public void Sale_ShouldCreateTransactionWithTransferType()
        {
            TransactionRequest transactionRequest = new TransactionRequest
            {
                Amount = 100.00M,
                MerchantAccountId = MerchantAccountIDs.AFT_FIRST_DATA_WALLET_TRANSFER,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = "4111111111111111",
                    ExpirationDate = "06/2026",
                    CVV = "123"
                },
                Transfer = new TransferRequest
                {
                    Type = "wallet_transfer",
                    Sender = new SenderRequest
                    {
                        FirstName = "Alice",
                        LastName = "Silva",
                        MiddleName = "A",
                        DateOfBirth = new DateTime(2009, 1, 1),
                        AccountReferenceNumber = "1000012345",
                        Address = new AddressRequest
                        {
                            StreetAddress = "1st Main, door 12th",
                            Locality = "LA",
                            Region = "CA",
                            CountryCodeAlpha2 = "US"
                        }
                    },
                    Receiver = new ReceiverRequest
                    {
                        FirstName = "Bob",
                        LastName = "Souza",
                        MiddleName = "A",
                        Address = new AddressRequest
                        {
                            StreetAddress = "1st Main, door 12th",
                            Locality = "LA",
                            Region = "CA",
                            CountryCodeAlpha2 = "US"
                        }
                    }
                }
            };

            var result = gateway.Transaction.Sale(transactionRequest);
            Assert.IsTrue(result.IsSuccess());
            var transaction = result.Target;
            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.Status);
            Assert.IsTrue(transaction.AccountFundingTransaction);
        }

        [Test]
        public void Sale_ShouldNotCreateTransactionWithInvalidTransferType()
        {

            var transactionRequest = new TransactionRequest
            {
                Amount = 100.00M,
                MerchantAccountId = "aft_first_data_wallet_transfer",
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = "4111111111111111",
                    ExpirationDate = "06/2026",
                    CVV = "123"
                },
                Transfer = new TransferRequest
                {
                    Type = "invalid_transfer",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(transactionRequest);
            Assert.IsFalse(result.IsSuccess());
        }
    }
}

