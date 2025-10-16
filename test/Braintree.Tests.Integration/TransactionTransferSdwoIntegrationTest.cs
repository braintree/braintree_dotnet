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
    public class TransactionTransferSdwoIntegrationTest
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
        public void Sale_ShouldCreateTransactionWithTransfer()
        {
            string[] transferTypes = { "account_to_account", "boleto_ticket", "person_to_person", "wallet_transfer"};
    
            foreach (string transferType in transferTypes)
            {
                var transactionRequest = new TransactionRequest
                {
                    Amount = 100.00M,
                    MerchantAccountId = MerchantAccountIDs.CARD_PROCESSOR_BRAZIL_SDWO_MERCHANT_ACCOUNT_ID,
                    CreditCard = new TransactionCreditCardRequest
                    {
                        Number = "4111111111111111",
                        ExpirationDate = "06/2026",
                        CVV = "123"
                    },
                    Descriptor = new DescriptorRequest
                    {
                        Name = "companynme12*product1",
                        Phone = "1232344444",
                        Url = "example.com"
                    },
                    BillingAddress = new AddressRequest
                    {
                        FirstName = "Bob James",
                        CountryCodeAlpha2 = "CA",
                        ExtendedAddress = "",
                        Locality = "Trois-Rivires",
                        Region = "QC",
                        PostalCode = "G8Y 156",
                        StreetAddress = "2346 Boul Lane"
                    },
                    Transfer = new TransferRequest
                    {
                        Type = transferType,
                        Sender = new SenderRequest
                        {
                            FirstName = "Alice",
                            LastName = "Silva",
                            AccountReferenceNumber = "1000012345",
                            TaxId = "12345678900",
                            Address = new AddressRequest
                            {
                                StreetAddress = "Rua das Flores, 100",
                                ExtendedAddress = "2B",
                                Locality = "São Paulo",
                                Region = "SP",
                                PostalCode = "01001-000",
                                CountryCodeAlpha2 = "BR",
                                InternationalPhone = new InternationalPhoneRequest
                                {
                                    CountryCode = "55",
                                    NationalNumber = "1234567890"
                                }
                            }
                        },
                        Receiver = new ReceiverRequest
                        {
                            FirstName = "Bob",
                            LastName = "Souza",
                            AccountReferenceNumber = "2000012345",
                            TaxId = "98765432100",
                            Address = new AddressRequest
                            {
                                StreetAddress = "Avenida Brasil, 200",
                                ExtendedAddress = "2B",
                                Locality = "Rio de Janeiro",
                                Region = "RJ",
                                PostalCode = "20040-002",
                                CountryCodeAlpha2 = "BR",
                                InternationalPhone = new InternationalPhoneRequest
                                {
                                    CountryCode = "55",
                                    NationalNumber = "9876543210"
                                }
                            }
                        }
                    },
                    Options = new TransactionOptionsRequest
                    {
                        StoreInVaultOnSuccess = true
                    }
                };
                try
                {
                    var result = gateway.Transaction.Sale(transactionRequest);
                    Assert.IsTrue(result.IsSuccess());
                    Assert.AreEqual(TransactionStatus.AUTHORIZED, result.Target.Status);
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.ToString());
                }
            }
        }

        [Test]
        public void Sale_ShouldNotCreateTransactionWithInvalidTransferType()
        {
            var transactionRequest = new TransactionRequest
            {
                Amount = 100.00M,
                MerchantAccountId = MerchantAccountIDs.CARD_PROCESSOR_BRAZIL_SDWO_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = "4111111111111111",
                    ExpirationDate = "06/2026",
                    CVV = "123"
                },
                Transfer = new TransferRequest
                {
                    Type = "invalid_transfer_type",
                    Sender = new SenderRequest
                    {
                        FirstName = "Alice",
                        LastName = "Silva",
                        AccountReferenceNumber = "1000012345",
                        TaxId = "12345678900",
                        Address = new AddressRequest
                        {
                            StreetAddress = "Rua das Flores, 100",
                            ExtendedAddress = "2B",
                            Locality = "São Paulo",
                            Region = "SP",
                            PostalCode = "01001-000",
                            CountryCodeAlpha2 = "BR",
                            InternationalPhone = new InternationalPhoneRequest
                            {
                                CountryCode = "55",
                                NationalNumber = "1234567890"
                            }
                        }
                    },
                    Receiver = new ReceiverRequest
                    {
                        FirstName = "Bob",
                        LastName = "Souza",
                        AccountReferenceNumber = "2000012345",
                        TaxId = "98765432100",
                        Address = new AddressRequest
                        {
                            StreetAddress = "Avenida Brasil, 200",
                            ExtendedAddress = "2B",
                            Locality = "Rio de Janeiro",
                            Region = "RJ",
                            PostalCode = "20040-002",
                            CountryCodeAlpha2 = "BR",
                            InternationalPhone = new InternationalPhoneRequest
                            {
                                CountryCode = "55",
                                NationalNumber = "9876543210"
                            }
                        }
                    }
                }
            };

            var result = gateway.Transaction.Sale(transactionRequest);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_TRANSFER_TYPE_INVALID, result.Errors.DeepAll()[0].Code);
            Assert.AreEqual("Transfer type is invalid.", result.Errors.DeepAll()[0].Message);
        }

        [Test]
        public void Sale_ShouldNotCreateTransactionWhenTransferDetailsRequiredButMissing()
        {
            var transactionRequest = new TransactionRequest
            {
                Amount = 100.00M,
                MerchantAccountId = MerchantAccountIDs.CARD_PROCESSOR_BRAZIL_SDWO_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = "4111111111111111",
                    ExpirationDate = "06/2026",
                    CVV = "123"
                }
            };

            var result = gateway.Transaction.Sale(transactionRequest);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_TRANSFER_DETAILS_REQUIRED, result.Errors.DeepAll()[0].Code);
            Assert.AreEqual("Transfer details are required for this merchant account.", result.Errors.DeepAll()[0].Message);
        }

        [Test]
        public void Sale_ShouldCreateTransactionWithTransferBlockButMissingTransferType()
        {
            var transactionRequest = new TransactionRequest
            {
                Amount = 100.00M,
                MerchantAccountId = MerchantAccountIDs.CARD_PROCESSOR_BRAZIL_SDWO_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = "4111111111111111",
                    ExpirationDate = "06/2026",
                    CVV = "123"
                },
                Descriptor = new DescriptorRequest
                {
                    Name = "companynme12*product1",
                    Phone = "1232344444",
                    Url = "example.com"
                },
                BillingAddress = new AddressRequest
                {
                    FirstName = "Bob James",
                    CountryCodeAlpha2 = "CA",
                    ExtendedAddress = "",
                    Locality = "Trois-Rivires",
                    Region = "QC",
                    PostalCode = "G8Y 156",
                    StreetAddress = "2346 Boul Lane"
                },
                Transfer = new TransferRequest
                {
                    Sender = new SenderRequest
                    {
                        FirstName = "Alice",
                        LastName = "Silva",
                        AccountReferenceNumber = "1000012345",
                        TaxId = "12345678900",
                        Address = new AddressRequest
                        {
                            StreetAddress = "Rua das Flores, 100",
                            ExtendedAddress = "2B",
                            Locality = "São Paulo",
                            Region = "SP",
                            PostalCode = "01001-000",
                            CountryCodeAlpha2 = "BR",
                            InternationalPhone = new InternationalPhoneRequest
                            {
                                CountryCode = "55",
                                NationalNumber = "1234567890"
                            }
                        }
                    },
                    Receiver = new ReceiverRequest
                    {
                        FirstName = "Bob",
                        LastName = "Souza",
                        AccountReferenceNumber = "2000012345",
                        TaxId = "98765432100",
                        Address = new AddressRequest
                        {
                            StreetAddress = "Avenida Brasil, 200",
                            ExtendedAddress = "2B",
                            Locality = "Rio de Janeiro",
                            Region = "RJ",
                            PostalCode = "20040-002",
                            CountryCodeAlpha2 = "BR",
                            InternationalPhone = new InternationalPhoneRequest
                            {
                                CountryCode = "55",
                                NationalNumber = "9876543210"
                            }
                        }
                    }
                },
                Options = new TransactionOptionsRequest
                {
                    StoreInVaultOnSuccess = true
                }
            };

            try
            {
                var result = gateway.Transaction.Sale(transactionRequest);
                Assert.IsTrue(result.IsSuccess());
                Assert.AreEqual(TransactionStatus.AUTHORIZED, result.Target.Status);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }
    }
}
