using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransactionTest
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
        public void SaleTrData_ReturnsValidTrDataHash()
        {
            String trData = gateway.Transaction.SaleTrData(new TransactionRequest(), "http://example.com");
            Assert.IsTrue(trData.Contains("sale"));
            Assert.IsTrue(TrUtil.IsTrDataValid(trData, service));
        }

        [Test]
        public void CreditTrData_ReturnsValidTrDataHash()
        {
            String trData = gateway.Transaction.CreditTrData(new TransactionRequest(), "http://example.com");
            Assert.IsTrue(trData.Contains("credit"));
            Assert.IsTrue(TrUtil.IsTrDataValid(trData, service));
        }

        [Test]
        public void TrData_QueryStringParams()
        {
            String trData = gateway.Transaction.SaleTrData(new TransactionRequest {
                Options = new TransactionOptionsRequest
                {
                    StoreInVault = true,
                    AddBillingAddressToPaymentMethod = true,
                    StoreShippingAddressInVault = true,
                    SubmitForSettlement = true
                }
            }, "http://example.com");
            Assert.IsTrue(trData.Contains("store_in_vault"));
            Assert.IsTrue(trData.Contains("add_billing_address_to_payment_method"));
            Assert.IsTrue(trData.Contains("store_shipping_address_in_vault"));
            Assert.IsTrue(trData.Contains("submit_for_settlement"));

            trData = gateway.Transaction.SaleTrData(new TransactionRequest {
                Options = new TransactionOptionsRequest
                {
                }
            }, "http://example.com");
            Assert.IsFalse(trData.Contains("store_in_vault"));
            Assert.IsFalse(trData.Contains("add_billing_address_to_payment_method"));
            Assert.IsFalse(trData.Contains("store_shipping_address_in_vault"));
            Assert.IsFalse(trData.Contains("submit_for_settlement"));

        }

        [Test]
        public void Search_OnAllTextFields()
        {
            String creditCardToken = String.Format("cc{0}", new Random().Next(1000000).ToString());
            String firstName = String.Format("Tim{0}", new Random().Next(1000000).ToString());

            TransactionRequest request = new TransactionRequest
            {
                Amount = 1000M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = "4111111111111111",
                    ExpirationDate = "05/2012",
                    CardholderName = "Tom Smith",
                    Token = creditCardToken
                },
                BillingAddress = new AddressRequest
                {
                    Company = "Braintree",
                    CountryName = "United States of America",
                    ExtendedAddress = "Suite 123",
                    FirstName = firstName,
                    LastName = "Smith",
                    Locality = "Chicago",
                    PostalCode = "12345",
                    Region = "IL",
                    StreetAddress = "123 Main St"
                },
                Customer = new CustomerRequest
                {
                    Company = "Braintree",
                    Email = "smith@example.com",
                    Fax = "5551231234",
                    FirstName = "Tom",
                    LastName = "Smith",
                    Phone = "5551231234",
                    Website = "http://example.com"
                },
                Options = new TransactionOptionsRequest
                {
                    StoreInVault = true,
                    SubmitForSettlement = true
                },
                OrderId = "myorder",
                ShippingAddress = new AddressRequest
                {
                    Company = "Braintree P.S.",
                    CountryName = "Mexico",
                    ExtendedAddress = "Apt 456",
                    FirstName = "Thomas",
                    LastName = "Smithy",
                    Locality = "Braintree",
                    PostalCode = "54321",
                    Region = "MA",
                    StreetAddress = "456 Road"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            TestHelper.Settle(service, transaction.Id);
            transaction = gateway.Transaction.Find(transaction.Id);

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                BillingCompany.Is("Braintree").
                BillingCountryName.Is("United States of America").
                BillingExtendedAddress.Is("Suite 123").
                BillingFirstName.Is(firstName).
                BillingLastName.Is("Smith").
                BillingLocality.Is("Chicago").
                BillingPostalCode.Is("12345").
                BillingRegion.Is("IL").
                BillingStreetAddress.Is("123 Main St").
                CreditCardCardholderName.Is("Tom Smith").
                CreditCardExpirationDate.Is("05/2012").
                CreditCardNumber.Is(SandboxValues.CreditCardNumber.VISA).
                Currency.Is("USD").
                CustomerCompany.Is("Braintree").
                CustomerEmail.Is("smith@example.com").
                CustomerFax.Is("5551231234").
                CustomerFirstName.Is("Tom").
                CustomerId.Is(transaction.Customer.Id).
                CustomerLastName.Is("Smith").
                CustomerPhone.Is("5551231234").
                CustomerWebsite.Is("http://example.com").
                OrderId.Is("myorder").
                PaymentMethodToken.Is(creditCardToken).
                ProcessorAuthorizationCode.Is(transaction.ProcessorAuthorizationCode).
                SettlementBatchId.Is(transaction.SettlementBatchId).
                ShippingCompany.Is("Braintree P.S.").
                ShippingCountryName.Is("Mexico").
                ShippingExtendedAddress.Is("Apt 456").
                ShippingFirstName.Is("Thomas").
                ShippingLastName.Is("Smithy").
                ShippingLocality.Is("Braintree").
                ShippingPostalCode.Is("54321").
                ShippingRegion.Is("MA").
                ShippingStreetAddress.Is("456 Road");

            ResourceCollection<Transaction> collection = gateway.Transaction.Search(searchRequest);

            Assert.AreEqual(1, collection.MaximumCount);
            Assert.AreEqual(transaction.Id, collection.FirstItem.Id);
        }

        [Test]
        public void Search_OnTextNodeOperators() {
            var request = new TransactionRequest
            {
                Amount = 1000M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = "4111111111111111",
                    ExpirationDate = "05/2012",
                    CardholderName = "Tom Smith"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            var searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                CreditCardCardholderName.StartsWith("Tom");

            ResourceCollection<Transaction> collection = gateway.Transaction.Search(searchRequest);
            Assert.AreEqual(1, collection.MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                CreditCardCardholderName.EndsWith("Smith");

            collection = gateway.Transaction.Search(searchRequest);
            Assert.AreEqual(1, collection.MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                CreditCardCardholderName.Contains("m Sm");

            collection = gateway.Transaction.Search(searchRequest);
            Assert.AreEqual(1, collection.MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                CreditCardCardholderName.IsNot("Tom Smith");

            collection = gateway.Transaction.Search(searchRequest);
            Assert.AreEqual(0, collection.MaximumCount);
        }

        [Test]
        public void Search_OnCreatedUsing()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2010"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                CreatedUsing.Is(TransactionCreatedUsing.FULL_INFORMATION);

            ResourceCollection<Transaction> collection = gateway.Transaction.Search(searchRequest);

            Assert.AreEqual(1, collection.MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                CreatedUsing.IncludedIn(TransactionCreatedUsing.FULL_INFORMATION, TransactionCreatedUsing.TOKEN);

            collection = gateway.Transaction.Search(searchRequest);

            Assert.AreEqual(1, collection.MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                CreatedUsing.Is(TransactionCreatedUsing.TOKEN);

            collection = gateway.Transaction.Search(searchRequest);

            Assert.AreEqual(0, collection.MaximumCount);
        }

        [Test]
        public void Search_OnCreditCardCustomerLocation()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2010"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                CreditCardCustomerLocation.Is(CreditCardCustomerLocation.US);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                CreditCardCustomerLocation.IncludedIn(CreditCardCustomerLocation.US, CreditCardCustomerLocation.INTERNATIONAL);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                CreditCardCustomerLocation.Is(CreditCardCustomerLocation.INTERNATIONAL);

            Assert.AreEqual(0, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Search_OnMerchantAccountId()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2010"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                MerchantAccountId.Is(transaction.MerchantAccountId);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                MerchantAccountId.IncludedIn(transaction.MerchantAccountId, "badmerchantaccountid");

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                MerchantAccountId.Is("badmerchantaccountid");

            Assert.AreEqual(0, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Search_OnCreditCardType()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2010"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                CreditCardCardType.Is(CreditCardCardType.VISA);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                CreditCardCardType.Is(transaction.CreditCard.CardType);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                CreditCardCardType.IncludedIn(CreditCardCardType.VISA, CreditCardCardType.MASTER_CARD);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                CreditCardCardType.Is(CreditCardCardType.MASTER_CARD);

            Assert.AreEqual(0, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Search_OnStatus()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2010"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                Status.Is(TransactionStatus.AUTHORIZED);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                Status.IncludedIn(TransactionStatus.AUTHORIZED, TransactionStatus.GATEWAY_REJECTED);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                Status.Is(TransactionStatus.GATEWAY_REJECTED);

            Assert.AreEqual(0, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Search_OnAuthorizationExpiredStatus()
        {
            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Status.Is(TransactionStatus.AUTHORIZATION_EXPIRED);

            ResourceCollection<Transaction> results = gateway.Transaction.Search(searchRequest);

            Assert.IsTrue(results.MaximumCount > 0);
            Assert.AreEqual(TransactionStatus.AUTHORIZATION_EXPIRED, results.FirstItem.Status);
        }

        [Test]
        public void Search_OnSource()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2010"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                Source.Is(TransactionSource.API);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                Source.IncludedIn(TransactionSource.API, TransactionSource.CONTROL_PANEL);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                Source.Is(TransactionSource.CONTROL_PANEL);

            Assert.AreEqual(0, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Search_OnTransactionType()
        {
            String name = new Random().Next(1000000).ToString();
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2010",
                    CardholderName = name
                },
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Transaction creditTransaction = gateway.Transaction.Credit(request).Target;

            Transaction saleTransaction = gateway.Transaction.Sale(request).Target;

            TestHelper.Settle(service, saleTransaction.Id);
            Transaction refundTransaction = gateway.Transaction.Refund(saleTransaction.Id).Target;

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                CreditCardCardholderName.Is(name).
                Type.Is(TransactionType.CREDIT);

            Assert.AreEqual(2, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                CreditCardCardholderName.Is(name).
                Type.Is(TransactionType.CREDIT).
                Refund.Is(true);

            ResourceCollection<Transaction> collection = gateway.Transaction.Search(searchRequest);

            Assert.AreEqual(1, collection.MaximumCount);
            Assert.AreEqual(refundTransaction.Id, collection.FirstItem.Id);

            searchRequest = new TransactionSearchRequest().
                CreditCardCardholderName.Is(name).
                Type.Is(TransactionType.CREDIT).
                Refund.Is(false);

            collection = gateway.Transaction.Search(searchRequest);

            Assert.AreEqual(1, collection.MaximumCount);
            Assert.AreEqual(creditTransaction.Id, collection.FirstItem.Id);
        }

        [Test]
        public void Search_OnAmount()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = 1000M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2010"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                Amount.Between(500M, 1500M);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                Amount.GreaterThanOrEqualTo(500M);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                Amount.LessThanOrEqualTo(1500M);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                Amount.Between(500M, 900M);

            Assert.AreEqual(0, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Search_OnCreatedAt()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2010"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            DateTime createdAt = transaction.CreatedAt.Value;
            DateTime threeDaysEarlier = createdAt.AddDays(-3);
            DateTime oneDayEarlier = createdAt.AddDays(-1);
            DateTime oneDayLater = createdAt.AddDays(1);

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                CreatedAt.Between(oneDayEarlier, oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                CreatedAt.GreaterThanOrEqualTo(oneDayEarlier);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                CreatedAt.LessThanOrEqualTo(oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                CreatedAt.Between(threeDaysEarlier, oneDayEarlier);

            Assert.AreEqual(0, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Search_OnCreatedAtUsingLocalTime()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2010"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            DateTime oneDayEarlier = DateTime.Now.AddDays(-1);
            DateTime oneDayLater = DateTime.Now.AddDays(1);

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                CreatedAt.Between(oneDayEarlier, oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Search_OnDisbursementDate()
        {
            DateTime disbursementDate = DateTime.Parse("2013-04-10");
            DateTime threeDaysEarlier = disbursementDate.AddDays(-3);
            DateTime oneDayEarlier = disbursementDate.AddDays(-1);
            DateTime oneDayLater = disbursementDate.AddDays(1);

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is("deposittransaction").
                DisbursementDate.Between(oneDayEarlier, oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is("deposittransaction").
                DisbursementDate.GreaterThanOrEqualTo(oneDayEarlier);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is("deposittransaction").
                DisbursementDate.LessThanOrEqualTo(oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is("deposittransaction").
                DisbursementDate.Between(threeDaysEarlier, oneDayEarlier);

            Assert.AreEqual(0, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Search_OnDisputeDate()
        {
            DateTime disputeDate = DateTime.Parse("2014-03-01");
            DateTime threeDaysEarlier = disputeDate.AddDays(-3);
            DateTime oneDayEarlier = disputeDate.AddDays(-1);
            DateTime oneDayLater = disputeDate.AddDays(1);

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is("disputedtransaction").
                DisputeDate.Between(oneDayEarlier, oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is("2disputetransaction").
                DisputeDate.GreaterThanOrEqualTo(oneDayEarlier);

            Assert.AreEqual(2, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is("disputedtransaction").
                DisputeDate.LessThanOrEqualTo(oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is("disputedtransaction").
                DisputeDate.Between(threeDaysEarlier, oneDayEarlier);

            Assert.AreEqual(0, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Search_OnAuthorizedAt()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2010"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            DateTime threeDaysEarlier = DateTime.Now.AddDays(-3);
            DateTime oneDayEarlier = DateTime.Now.AddDays(-1);
            DateTime oneDayLater = DateTime.Now.AddDays(1);

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                AuthorizedAt.Between(oneDayEarlier, oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                AuthorizedAt.GreaterThanOrEqualTo(oneDayEarlier);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                AuthorizedAt.LessThanOrEqualTo(oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                AuthorizedAt.Between(threeDaysEarlier, oneDayEarlier);

            Assert.AreEqual(0, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Search_OnAuthorizationExpiredAt()
        {
            DateTime threeDaysEarlier = DateTime.Now.AddDays(-3);
            DateTime oneDayEarlier = DateTime.Now.AddDays(-1);
            DateTime oneDayLater = DateTime.Now.AddDays(1);

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                AuthorizationExpiredAt.Between(threeDaysEarlier, oneDayEarlier);

            Assert.AreEqual(0, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                AuthorizationExpiredAt.Between(oneDayEarlier, oneDayLater);

            var results = gateway.Transaction.Search(searchRequest);
            Assert.IsTrue(results.MaximumCount > 0);
            Assert.AreEqual(TransactionStatus.AUTHORIZATION_EXPIRED, results.FirstItem.Status);
        }

        [Test]
        public void Search_OnFailedAt()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.FAILED,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2010"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Transaction;

            DateTime threeDaysEarlier = DateTime.Now.AddDays(-3);
            DateTime oneDayEarlier = DateTime.Now.AddDays(-1);
            DateTime oneDayLater = DateTime.Now.AddDays(1);

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                FailedAt.Between(oneDayEarlier, oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                FailedAt.GreaterThanOrEqualTo(oneDayEarlier);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                FailedAt.LessThanOrEqualTo(oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                FailedAt.Between(threeDaysEarlier, oneDayEarlier);

            Assert.AreEqual(0, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Search_OnGatewayRejectedAt()
        {
            BraintreeGateway processingRulesGateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "processing_rules_merchant_id",
                PublicKey = "processing_rules_public_key",
                PrivateKey = "processing_rules_private_key"
            };

            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2010",
                    CVV = "200"
                }
            };

            Transaction transaction = processingRulesGateway.Transaction.Sale(request).Transaction;

            DateTime threeDaysEarlier = DateTime.Now.AddDays(-3);
            DateTime oneDayEarlier = DateTime.Now.AddDays(-1);
            DateTime oneDayLater = DateTime.Now.AddDays(1);

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                GatewayRejectedAt.Between(oneDayEarlier, oneDayLater);

            Assert.AreEqual(1, processingRulesGateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                GatewayRejectedAt.GreaterThanOrEqualTo(oneDayEarlier);

            Assert.AreEqual(1, processingRulesGateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                GatewayRejectedAt.LessThanOrEqualTo(oneDayLater);

            Assert.AreEqual(1, processingRulesGateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                GatewayRejectedAt.Between(threeDaysEarlier, oneDayEarlier);

            Assert.AreEqual(0, processingRulesGateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Search_OnProcessorDeclinedAt()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.DECLINE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2010"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Transaction;

            DateTime threeDaysEarlier = DateTime.Now.AddDays(-3);
            DateTime oneDayEarlier = DateTime.Now.AddDays(-1);
            DateTime oneDayLater = DateTime.Now.AddDays(1);

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                ProcessorDeclinedAt.Between(oneDayEarlier, oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                ProcessorDeclinedAt.GreaterThanOrEqualTo(oneDayEarlier);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                ProcessorDeclinedAt.LessThanOrEqualTo(oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                ProcessorDeclinedAt.Between(threeDaysEarlier, oneDayEarlier);

            Assert.AreEqual(0, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Search_OnSettledAt()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2010"
                },
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            TestHelper.Settle(service, transaction.Id);
            transaction = gateway.Transaction.Find(transaction.Id);

            DateTime threeDaysEarlier = DateTime.Now.AddDays(-3);
            DateTime oneDayEarlier = DateTime.Now.AddDays(-1);
            DateTime oneDayLater = DateTime.Now.AddDays(1);

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                SettledAt.Between(oneDayEarlier, oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                SettledAt.GreaterThanOrEqualTo(oneDayEarlier);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                SettledAt.LessThanOrEqualTo(oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                SettledAt.Between(threeDaysEarlier, oneDayEarlier);

            Assert.AreEqual(0, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Search_OnSubmittedForSettlementAt()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2010"
                },
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            DateTime threeDaysEarlier = DateTime.Now.AddDays(-3);
            DateTime oneDayEarlier = DateTime.Now.AddDays(-1);
            DateTime oneDayLater = DateTime.Now.AddDays(1);

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                SubmittedForSettlementAt.Between(oneDayEarlier, oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                SubmittedForSettlementAt.GreaterThanOrEqualTo(oneDayEarlier);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                SubmittedForSettlementAt.LessThanOrEqualTo(oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                SubmittedForSettlementAt.Between(threeDaysEarlier, oneDayEarlier);

            Assert.AreEqual(0, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Search_OnVoidedAt()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2010"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            transaction = gateway.Transaction.Void(transaction.Id).Target;

            DateTime threeDaysEarlier = DateTime.Now.AddDays(-3);
            DateTime oneDayEarlier = DateTime.Now.AddDays(-1);
            DateTime oneDayLater = DateTime.Now.AddDays(1);

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                VoidedAt.Between(oneDayEarlier, oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                VoidedAt.GreaterThanOrEqualTo(oneDayEarlier);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                VoidedAt.LessThanOrEqualTo(oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                VoidedAt.Between(threeDaysEarlier, oneDayEarlier);

            Assert.AreEqual(0, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Search_OnMultipleStatuses()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2010"
                },
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            DateTime threeDaysEarlier = DateTime.Now.AddDays(-3);
            DateTime oneDayEarlier = DateTime.Now.AddDays(-1);
            DateTime oneDayLater = DateTime.Now.AddDays(1);

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                AuthorizedAt.Between(oneDayEarlier, oneDayLater).
                SubmittedForSettlementAt.Between(oneDayEarlier, oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                AuthorizedAt.Between(threeDaysEarlier, oneDayEarlier).
                SubmittedForSettlementAt.Between(threeDaysEarlier, oneDayEarlier);

            Assert.AreEqual(0, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        [ExpectedException(typeof(Braintree.Exceptions.DownForMaintenanceException))]
        public void Search_ReturnsErrorOnTimeout()
        {
          TransactionSearchRequest searchRequest = new TransactionSearchRequest().
              Amount.Is(-5);
          gateway.Transaction.Search(searchRequest);
        }

        [Test]
        public void Sale_ReturnsSuccessfulResponse()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(1000.00, transaction.Amount);
            Assert.AreEqual(TransactionType.SALE, transaction.Type);
            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.Status);
            Assert.AreEqual(DateTime.Now.Year, transaction.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, transaction.UpdatedAt.Value.Year);
            Assert.IsNotNull(transaction.ProcessorAuthorizationCode);
            Assert.AreEqual(TransactionGatewayRejectionReason.UNRECOGNIZED, transaction.GatewayRejectionReason);

            CreditCard creditCard = transaction.CreditCard;
            Assert.AreEqual("411111", creditCard.Bin);
            Assert.AreEqual("1111", creditCard.LastFour);
            Assert.AreEqual("05", creditCard.ExpirationMonth);
            Assert.AreEqual("2009", creditCard.ExpirationYear);
            Assert.AreEqual("05/2009", creditCard.ExpirationDate);
        }

        [Test]
        public void Sale_WithDeviceData()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                DeviceData = "{\"device_session_id\":\"my_dsid\", \"fraud_merchant_id\":\"7\"}"
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(1000.00, transaction.Amount);
            Assert.AreEqual(TransactionType.SALE, transaction.Type);
            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.Status);
            Assert.AreEqual(DateTime.Now.Year, transaction.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, transaction.UpdatedAt.Value.Year);
            Assert.IsNotNull(transaction.ProcessorAuthorizationCode);
            Assert.AreEqual(TransactionGatewayRejectionReason.UNRECOGNIZED, transaction.GatewayRejectionReason);

            CreditCard creditCard = transaction.CreditCard;
            Assert.AreEqual("411111", creditCard.Bin);
            Assert.AreEqual("1111", creditCard.LastFour);
            Assert.AreEqual("05", creditCard.ExpirationMonth);
            Assert.AreEqual("2009", creditCard.ExpirationYear);
            Assert.AreEqual("05/2009", creditCard.ExpirationDate);
        }

        [Test]
        public void Sale_WithAllAttributes()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                Channel = "MyShoppingCartProvider",
                OrderId = "123",
                Recurring = true,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    CVV = "321",
                    ExpirationDate = "05/2009",
                    CardholderName = "John Doe"
                },
                Customer = new CustomerRequest
                {
                    FirstName = "Dan",
                    LastName = "Smith",
                    Company = "Braintree Payment Solutions",
                    Email = "dan@example.com",
                    Phone = "419-555-1234",
                    Fax = "419-555-1235",
                    Website = "http://braintreepayments.com"
                },
                BillingAddress = new AddressRequest
                {
                    FirstName = "Carl",
                    LastName = "Jones",
                    Company = "Braintree",
                    StreetAddress = "123 E Main St",
                    ExtendedAddress = "Suite 403",
                    Locality = "Chicago",
                    Region = "IL",
                    PostalCode = "60622",
                    CountryName = "United States of America",
                    CountryCodeAlpha2 = "US",
                    CountryCodeAlpha3 = "USA",
                    CountryCodeNumeric = "840"
                },
                ShippingAddress = new AddressRequest
                {
                    FirstName = "Andrew",
                    LastName = "Mason",
                    Company = "Braintree Shipping",
                    StreetAddress = "456 W Main St",
                    ExtendedAddress = "Apt 2F",
                    Locality = "Bartlett",
                    Region = "MA",
                    PostalCode = "60103",
                    CountryName = "Mexico",
                    CountryCodeAlpha2 = "MX",
                    CountryCodeAlpha3 = "MEX",
                    CountryCodeNumeric = "484"
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(1000.00, transaction.Amount);
            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.Status);
            Assert.AreEqual("MyShoppingCartProvider", transaction.Channel);
            Assert.AreEqual("123", transaction.OrderId);
            Assert.IsTrue(transaction.Recurring.Value);
            Assert.IsNull(transaction.GetVaultCreditCard());
            Assert.IsNull(transaction.GetVaultCustomer());
            Assert.IsNull(transaction.AvsErrorResponseCode);
            Assert.AreEqual("M", transaction.AvsPostalCodeResponseCode);
            Assert.AreEqual("M", transaction.AvsStreetAddressResponseCode);
            Assert.IsFalse(transaction.TaxExempt.Value);
            Assert.AreEqual("M", transaction.CvvResponseCode);
            Assert.AreEqual("USD", transaction.CurrencyIsoCode);

            Assert.IsNull(transaction.GetVaultCreditCard());
            CreditCard creditCard = transaction.CreditCard;
            Assert.AreEqual("411111", creditCard.Bin);
            Assert.AreEqual("1111", creditCard.LastFour);
            Assert.AreEqual("05", creditCard.ExpirationMonth);
            Assert.AreEqual("2009", creditCard.ExpirationYear);
            Assert.AreEqual("05/2009", creditCard.ExpirationDate);
            Assert.AreEqual("John Doe", creditCard.CardholderName);

            Assert.IsNull(transaction.GetVaultCustomer());
            Customer customer = transaction.Customer;
            Assert.AreEqual("Dan", customer.FirstName);
            Assert.AreEqual("Smith", customer.LastName);
            Assert.AreEqual("Braintree Payment Solutions", customer.Company);
            Assert.AreEqual("dan@example.com", customer.Email);
            Assert.AreEqual("419-555-1234", customer.Phone);
            Assert.AreEqual("419-555-1235", customer.Fax);
            Assert.AreEqual("http://braintreepayments.com", customer.Website);

            Assert.IsNull(transaction.GetVaultBillingAddress());
            Address billingAddress = transaction.BillingAddress;
            Assert.AreEqual("Carl", billingAddress.FirstName);
            Assert.AreEqual("Jones", billingAddress.LastName);
            Assert.AreEqual("Braintree", billingAddress.Company);
            Assert.AreEqual("123 E Main St", billingAddress.StreetAddress);
            Assert.AreEqual("Suite 403", billingAddress.ExtendedAddress);
            Assert.AreEqual("Chicago", billingAddress.Locality);
            Assert.AreEqual("IL", billingAddress.Region);
            Assert.AreEqual("60622", billingAddress.PostalCode);
            Assert.AreEqual("United States of America", billingAddress.CountryName);
            Assert.AreEqual("US", billingAddress.CountryCodeAlpha2);
            Assert.AreEqual("USA", billingAddress.CountryCodeAlpha3);
            Assert.AreEqual("840", billingAddress.CountryCodeNumeric);

            Assert.IsNull(transaction.GetVaultShippingAddress());
            Address shippingAddress = transaction.ShippingAddress;
            Assert.AreEqual("Andrew", shippingAddress.FirstName);
            Assert.AreEqual("Mason", shippingAddress.LastName);
            Assert.AreEqual("Braintree Shipping", shippingAddress.Company);
            Assert.AreEqual("456 W Main St", shippingAddress.StreetAddress);
            Assert.AreEqual("Apt 2F", shippingAddress.ExtendedAddress);
            Assert.AreEqual("Bartlett", shippingAddress.Locality);
            Assert.AreEqual("MA", shippingAddress.Region);
            Assert.AreEqual("60103", shippingAddress.PostalCode);
            Assert.AreEqual("Mexico", shippingAddress.CountryName);
            Assert.AreEqual("MX", shippingAddress.CountryCodeAlpha2);
            Assert.AreEqual("MEX", shippingAddress.CountryCodeAlpha3);
            Assert.AreEqual("484", shippingAddress.CountryCodeNumeric);
        }

        [Test]
        public void Sale_WithSecurityParams()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                DeviceSessionId = "abc123",
                FraudMerchantId = "456",
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void Sale_SpecifyingMerchantAccountId()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID, transaction.MerchantAccountId);
        }

        [Test]
        public void Sale_WithoutSpecifyingMerchantAccountIdFallsBackToDefault()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(MerchantAccountIDs.DEFAULT_MERCHANT_ACCOUNT_ID, transaction.MerchantAccountId);
        }

        [Test]
        public void Sale_WithStoreInVaultAndSpecifyingToken()
        {
            String customerId = new Random().Next(1000000).ToString();
            String paymentToken = new Random().Next(1000000).ToString();

            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Token = paymentToken,
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009"
                },
                Customer = new CustomerRequest
                {
                    Id = customerId,
                    FirstName = "Jane",
                },
                Options = new TransactionOptionsRequest
                {
                    StoreInVault = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            CreditCard creditCard = transaction.CreditCard;
            Assert.AreEqual(paymentToken, creditCard.Token);
            Assert.AreEqual("05/2009", transaction.GetVaultCreditCard().ExpirationDate);

            Customer customer = transaction.Customer;
            Assert.AreEqual(customerId, customer.Id);
            Assert.AreEqual("Jane", transaction.GetVaultCustomer().FirstName);
        }

        [Test]
        public void Sale_WithVaultCustomerAndNewCreditCard()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()
            {
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Some Company",
                Email = "hansolo64@compuserve.com",
                Phone = "312.555.1111",
                Fax = "312.555.1112",
                Website = "www.disney.com"
            }).Target;

            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    CardholderName = "Bob the Builder",
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009"
                },
                CustomerId = customer.Id
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual("Bob the Builder", transaction.CreditCard.CardholderName);
            Assert.IsNull(transaction.GetVaultCreditCard());
        }

        [Test]
        public void Sale_WithVaultCustomerAndNewCreditCardStoresInVault()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()
            {
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Some Company",
                Email = "hansolo64@compuserver.com",
                Phone = "312.555.1111",
                Fax = "312.555.1112",
                Website = "www.disney.com"
            }).Target;

            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    CardholderName = "Bob the Builder",
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009"
                },
                CustomerId = customer.Id,
                Options = new TransactionOptionsRequest
                {
                    StoreInVault = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual("Bob the Builder", transaction.CreditCard.CardholderName);
            Assert.AreEqual("Bob the Builder", transaction.GetVaultCreditCard().CardholderName);
        }

        [Test]
        public void Sale_WithStoreInVaultWithoutSpecifyingToken()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                Customer = new CustomerRequest
                {
                    FirstName = "Jane"
                },
                Options = new TransactionOptionsRequest
                {
                    StoreInVault = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            CreditCard creditCard = transaction.CreditCard;
            Assert.IsNotNull(creditCard.Token);
            Assert.AreEqual("05/2009", transaction.GetVaultCreditCard().ExpirationDate);

            Customer customer = transaction.Customer;
            Assert.IsNotNull(customer.Id);
            Assert.AreEqual("Jane", transaction.GetVaultCustomer().FirstName);
        }

        [Test]
        public void Sale_WithStoreInVaultOnSuccessWhenTransactionSuccessful()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                Customer = new CustomerRequest
                {
                    FirstName = "Jane"
                },
                Options = new TransactionOptionsRequest
                {
                    StoreInVaultOnSuccess = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            CreditCard creditCard = transaction.CreditCard;
            Assert.IsNotNull(creditCard.Token);
            Assert.AreEqual("05/2009", transaction.GetVaultCreditCard().ExpirationDate);

            Customer customer = transaction.Customer;
            Assert.IsNotNull(customer.Id);
            Assert.AreEqual("Jane", transaction.GetVaultCustomer().FirstName);
        }



        [Test]
        public void Sale_WithStoreInVaultOnSuccessWhenTransactionFails()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.DECLINE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                Customer = new CustomerRequest
                {
                    FirstName = "Jane"
                },
                Options = new TransactionOptionsRequest
                {
                    StoreInVaultOnSuccess = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Transaction transaction = result.Transaction;

            CreditCard creditCard = transaction.CreditCard;
            Assert.IsNull(creditCard.Token);
            Assert.IsNull(transaction.GetVaultCreditCard());

            Customer customer = transaction.Customer;
            Assert.IsNull(customer.Id);
            Assert.IsNull(transaction.GetVaultCustomer());
        }

        [Test]
        public void Sale_WithStoreInVaultForBillingAndShipping()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                BillingAddress = new AddressRequest
                {
                    FirstName = "Carl",
                },
                ShippingAddress = new AddressRequest
                {
                    FirstName = "Andrew"
                },
                Options = new TransactionOptionsRequest
                {
                    StoreInVault = true,
                    AddBillingAddressToPaymentMethod = true,
                    StoreShippingAddressInVault = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            CreditCard creditCard = transaction.GetVaultCreditCard();
            Assert.AreEqual("Carl", creditCard.BillingAddress.FirstName);
            Assert.AreEqual("Carl", transaction.GetVaultBillingAddress().FirstName);
            Assert.AreEqual("Andrew", transaction.GetVaultShippingAddress().FirstName);

            Customer customer = transaction.GetVaultCustomer();
            Assert.AreEqual(2, customer.Addresses.Length);

            Assert.AreEqual("Carl", customer.Addresses[0].FirstName);
            Assert.AreEqual("Andrew", customer.Addresses[1].FirstName);

            Assert.IsNotNull(transaction.BillingAddress.Id);
            Assert.IsNotNull(transaction.ShippingAddress.Id);
        }

        [Test]
        public void Sale_WithThreeDSecureToken()
        {
            var three_d_secure_token = TestHelper.Create3DSVerification(service, MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID, new ThreeDSecureRequestForTests() {
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationMonth = "05",
                ExpirationYear = "2009"
            });

            var request = new TransactionRequest
            {
                MerchantAccountId = MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID,
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                ThreeDSecureToken = three_d_secure_token,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.Status);
        }

        [Test]
        public void Sale_ErrorThreeDSecureTransactionDataDoesNotMatch()
        {
            var three_d_secure_token = TestHelper.Create3DSVerification(service, MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID, new ThreeDSecureRequestForTests() {
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationMonth = "05",
                ExpirationYear = "2009",
            });

            var request = new TransactionRequest
            {
                MerchantAccountId = MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID,
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                ThreeDSecureToken = three_d_secure_token,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.MASTER_CARD,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_THREE_D_SECURE_TRANSACTION_DATA_DOESNT_MATCH_VERIFY, result.Errors.ForObject("Transaction").OnField("Three-D-Secure-Token")[0].Code);
        }

        [Test]
        public void Sale_ErrorWithNullThreeDSecureToken()
        {
            String three_d_secure_token = null;

            var request = new TransactionRequest
            {
                MerchantAccountId = MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID,
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                ThreeDSecureToken = three_d_secure_token,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_THREE_D_SECURE_TOKEN_IS_INVALID, result.Errors.ForObject("Transaction").OnField("Three-D-Secure-Token")[0].Code);
        }

        [Test]
        public void Sale_Declined()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.DECLINE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009"
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Transaction transaction = result.Transaction;

            Assert.AreEqual(2000.00, transaction.Amount);
            Assert.AreEqual(TransactionStatus.PROCESSOR_DECLINED, transaction.Status);
            Assert.AreEqual("2000", transaction.ProcessorResponseCode);
            Assert.IsNotNull(transaction.ProcessorResponseText);
            Assert.IsNull(transaction.VoiceReferralNumber);

            CreditCard creditCard = transaction.CreditCard;
            Assert.AreEqual("411111", creditCard.Bin);
            Assert.AreEqual("1111", creditCard.LastFour);
            Assert.AreEqual("05", creditCard.ExpirationMonth);
            Assert.AreEqual("2009", creditCard.ExpirationYear);
            Assert.AreEqual("05/2009", creditCard.ExpirationDate);
        }

        [Test]
        public void Sale_GatewayRejectedForAvs()
        {
            BraintreeGateway processingRulesGateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "processing_rules_merchant_id",
                PublicKey = "processing_rules_public_key",
                PrivateKey = "processing_rules_private_key"
            };

            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                BillingAddress = new AddressRequest
                {
                    StreetAddress = "200 Fake Street"
                },
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009"
                }
            };

            Result<Transaction> result = processingRulesGateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Transaction transaction = result.Transaction;

            Assert.AreEqual(TransactionGatewayRejectionReason.AVS, transaction.GatewayRejectionReason);
        }

        [Test]
        public void Sale_GatewayRejectedForAvsAndCvv()
        {
            BraintreeGateway processingRulesGateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "processing_rules_merchant_id",
                PublicKey = "processing_rules_public_key",
                PrivateKey = "processing_rules_private_key"
            };

            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                BillingAddress = new AddressRequest
                {
                    PostalCode = "20000"
                },
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                    CVV = "200"
                }
            };

            Result<Transaction> result = processingRulesGateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Transaction transaction = result.Transaction;

            Assert.AreEqual(TransactionGatewayRejectionReason.AVS_AND_CVV, transaction.GatewayRejectionReason);
        }

        [Test]
        public void Sale_GatewayRejectedForCvv()
        {
            BraintreeGateway processingRulesGateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "processing_rules_merchant_id",
                PublicKey = "processing_rules_public_key",
                PrivateKey = "processing_rules_private_key"
            };

            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                    CVV = "200"
                }
            };

            Result<Transaction> result = processingRulesGateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Transaction transaction = result.Transaction;

            Assert.AreEqual(TransactionGatewayRejectionReason.CVV, transaction.GatewayRejectionReason);
        }

        [Test]
        public void Sale_GatewayRejectedForFraud()
        {
           BraintreeGateway processingRulesGateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "processing_rules_merchant_id",
                PublicKey = "processing_rules_public_key",
                PrivateKey = "processing_rules_private_key"
            };

            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.FRAUD,
                    ExpirationDate = "05/2017",
                    CVV = "333"
                }
            };

            Result<Transaction> result = processingRulesGateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Transaction transaction = result.Transaction;

            Assert.AreEqual(TransactionGatewayRejectionReason.FRAUD, transaction.GatewayRejectionReason);
        }

        [Test]
        public void UnrecognizedValuesAreCategorizedAsSuch()
        {
          Transaction transaction = gateway.Transaction.Find("unrecognized_transaction_id");

          Assert.AreEqual(TransactionGatewayRejectionReason.UNRECOGNIZED, transaction.GatewayRejectionReason);
          Assert.AreEqual(TransactionEscrowStatus.UNRECOGNIZED, transaction.EscrowStatus);
          Assert.AreEqual(TransactionStatus.UNRECOGNIZED, transaction.Status);
        }

        [Test]
        public void Sale_WithCustomFields()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CustomFields = new Dictionary<String, String>
                {
                    { "store_me", "custom value" },
                    { "another_stored_field", "custom value2" }
                },
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009"
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual("custom value", transaction.CustomFields["store_me"]);
            Assert.AreEqual("custom value2", transaction.CustomFields["another_stored_field"]);
        }

        [Test]
        public void Sale_WithUnregisteredCustomField()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.DECLINE,
                CustomFields = new Dictionary<String, String>
                {
                    { "unkown_custom_field", "custom value" }
                },
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009"
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_CUSTOM_FIELD_IS_INVALID, result.Errors.ForObject("Transaction").OnField("CustomFields")[0].Code);
        }

        [Test]
        public void Sale_WithPaymentMethodToken()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CVV = "123",
                Number = "5105105105105100",
                ExpirationDate = "05/12"
            };

            CreditCard creditCard = gateway.CreditCard.Create(creditCardRequest).Target;

            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodToken = creditCard.Token
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(creditCard.Token, transaction.CreditCard.Token);
            Assert.AreEqual("510510", transaction.CreditCard.Bin);
            Assert.AreEqual("05/2012", transaction.CreditCard.ExpirationDate);
        }

        [Test]
        public void Sale_WithPaymentMethodTokenAndCvv()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5105105105105100",
                ExpirationDate = "05/12"
            };

            CreditCard creditCard = gateway.CreditCard.Create(creditCardRequest).Target;

            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodToken = creditCard.Token,
                CreditCard = new TransactionCreditCardRequest
                {
                    CVV = "301"
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual("S", transaction.CvvResponseCode);
            Assert.AreEqual(creditCard.Token, transaction.CreditCard.Token);
            Assert.AreEqual("510510", transaction.CreditCard.Bin);
            Assert.AreEqual("05/2012", transaction.CreditCard.ExpirationDate);
        }

        [Test]
        public void Sale_UsesShippingAddressFromVault()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            gateway.CreditCard.Create(new CreditCardRequest
            {
                CustomerId = customer.Id,
                CVV = "123",
                Number = "5105105105105100",
                ExpirationDate = "05/12"
            });

            Address shippingAddress = gateway.Address.Create(customer.Id, new AddressRequest { FirstName = "Carl" }).Target;

            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CustomerId = customer.Id,
                ShippingAddressId = shippingAddress.Id
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(shippingAddress.Id, transaction.ShippingAddress.Id);
            Assert.AreEqual("Carl", transaction.ShippingAddress.FirstName);
        }

        [Test]
        public void Sale_UsesBillingAddressFromVault()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            gateway.CreditCard.Create(new CreditCardRequest
            {
                CustomerId = customer.Id,
                CVV = "123",
                Number = "5105105105105100",
                ExpirationDate = "05/12"
            });

            Address billingAddress = gateway.Address.Create(customer.Id, new AddressRequest { FirstName = "Carl" }).Target;

            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CustomerId = customer.Id,
                BillingAddressId = billingAddress.Id
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(billingAddress.Id, transaction.BillingAddress.Id);
            Assert.AreEqual("Carl", transaction.BillingAddress.FirstName);
        }

        [Test]
        public void Sale_WithValidationError()
        {
            TransactionRequest request = new TransactionRequest
            {
                CreditCard = new TransactionCreditCardRequest
                {
                    ExpirationMonth = "05",
                    ExpirationYear = "2010"
                },
                BillingAddress = new AddressRequest
                {
                    CountryName = "zzzzzz",
                    CountryCodeAlpha2 = "zz",
                    CountryCodeAlpha3 = "zzz",
                    CountryCodeNumeric = "000"
                },
                ShippingAddress = new AddressRequest
                {
                    CountryName = "zzzzz",
                    CountryCodeAlpha2 = "zz",
                    CountryCodeAlpha3 = "zzz",
                    CountryCodeNumeric = "000"
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.IsNull(result.Target);
            Assert.IsNull(result.Transaction);
            Assert.IsNull(result.CreditCardVerification);

            Assert.AreEqual(ValidationErrorCode.TRANSACTION_AMOUNT_IS_REQUIRED, result.Errors.ForObject("Transaction").OnField("Amount")[0].Code);
            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_COUNTRY_CODE_ALPHA2_IS_NOT_ACCEPTED,
                result.Errors.ForObject("Transaction").ForObject("Billing").OnField("CountryCodeAlpha2")[0].Code
            );
            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_COUNTRY_CODE_ALPHA3_IS_NOT_ACCEPTED,
                result.Errors.ForObject("Transaction").ForObject("Billing").OnField("CountryCodeAlpha3")[0].Code
            );
            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_COUNTRY_CODE_NUMERIC_IS_NOT_ACCEPTED,
                result.Errors.ForObject("Transaction").ForObject("Billing").OnField("CountryCodeNumeric")[0].Code
            );
            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_COUNTRY_NAME_IS_NOT_ACCEPTED,
                result.Errors.ForObject("Transaction").ForObject("Billing").OnField("CountryName")[0].Code
            );

            Dictionary<String, String> parameters = result.Parameters;
            Assert.IsFalse(parameters.ContainsKey("transaction[amount]"));
            Assert.AreEqual("05", parameters["transaction[credit_card][expiration_month]"]);
            Assert.AreEqual("2010", parameters["transaction[credit_card][expiration_year]"]);
        }

        [Test]
        public void Sale_WithTooLongPurchaseOrderNumberAttributes()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                TaxExempt = true,
                TaxAmount = 10M,
                PurchaseOrderNumber = "aaaaaaaaaaaaaaaaaa",
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_PURCHASE_ORDER_NUMBER_IS_TOO_LONG,
                result.Errors.ForObject("Transaction").OnField("PurchaseOrderNumber")[0].Code
            );
        }

        [Test]
        public void Sale_WithInvalidPurchaseOrderNumberAttributes()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                TaxExempt = true,
                TaxAmount = 10M,
                PurchaseOrderNumber = "\u00c3\u009f\u00c3\u00a5\u00e2\u0088\u0082",
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_PURCHASE_ORDER_NUMBER_IS_INVALID,
                result.Errors.ForObject("Transaction").OnField("PurchaseOrderNumber")[0].Code
            );
        }
        [Test]
        public void Sale_WithLevel2Validations()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                TaxExempt = true,
                TaxAmount = 10M,
                PurchaseOrderNumber = "12345",
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.IsTrue(transaction.TaxExempt.Value);
            Assert.AreEqual(10M, transaction.TaxAmount.Value);
            Assert.AreEqual("12345", transaction.PurchaseOrderNumber);
        }

        [Test]
        public void Sale_WithServiceFee()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_SUB_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ServiceFeeAmount = 1M
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Transaction transaction = result.Target;
            Assert.AreEqual(1M, transaction.ServiceFeeAmount);
        }

        [Test]
        public void Sale_WithZeroServiceFee()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_SUB_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ServiceFeeAmount = 0M
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Transaction transaction = result.Target;
            Assert.AreEqual(0M, transaction.ServiceFeeAmount);
        }

        [Test]
        public void Sale_WithServiceFeeWithTooLargeAmount()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = 1M,
                MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_SUB_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ServiceFeeAmount = 2M
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_SERVICE_FEE_AMOUNT_IS_TOO_LARGE, result.Errors.ForObject("Transaction").OnField("ServiceFeeAmount")[0].Code);
        }

        [Test]
        public void Sale_WithMerchantAccountIdAndWithoutServiceFeeAmount()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = 1M,
                MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_SUB_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_SUB_MERCHANT_ACCOUNT_REQUIRES_SERVICE_FEE_AMOUNT, result.Errors.ForObject("Transaction").OnField("MerchantAccountId")[0].Code);
        }

        [Test]
        public void Sale_WithServiceFeeAmountOnMasterMerchantAccount()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = 4M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ServiceFeeAmount = 2M
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_SERVICE_FEE_AMOUNT_NOT_ALLOWED_ON_MASTER_MERCHANT_ACCOUNT , result.Errors.ForObject("Transaction").OnField("ServiceFeeAmount")[0].Code);
        }

        [Test]
        public void Sale_WithHoldInEscrow()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = 4M,
                MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_SUB_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ServiceFeeAmount = 2M,
                Options = new TransactionOptionsRequest
                {
                    HoldInEscrow = true
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;
            Assert.AreEqual(TransactionEscrowStatus.HOLD_PENDING, transaction.EscrowStatus);
        }

        [Test]
        public void Sale_WithHoldInEscrowFailsForMasterMerchantAccount()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = 4M,
                MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                Options = new TransactionOptionsRequest
                {
                    HoldInEscrow = true
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_CANNOT_HOLD_IN_ESCROW,
                result.Errors.ForObject("Transaction").OnField("Base")[0].Code
            );
        }

        [Test]
        public void HoldInEscrow_AfterSale()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = 4M,
                MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_SUB_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ServiceFeeAmount = 1M
            };
            Result<Transaction> saleResult = gateway.Transaction.Sale(request);
            Transaction saleTransaction = saleResult.Target;
            Result<Transaction> result = gateway.Transaction.HoldInEscrow(saleTransaction.Id);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;
            Assert.AreEqual(
                TransactionEscrowStatus.HOLD_PENDING,
                transaction.EscrowStatus
            );
        }

        [Test]
        public void HoldInEscrow_AfterSaleFailsForMasterMerchantAccount()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = 4M,
                MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
            };
            Result<Transaction> saleResult = gateway.Transaction.Sale(request);
            Transaction saleTransaction = saleResult.Target;
            Result<Transaction> result = gateway.Transaction.HoldInEscrow(saleTransaction.Id);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_CANNOT_HOLD_IN_ESCROW,
                result.Errors.ForObject("Transaction").OnField("Base")[0].Code
            );
        }

        [Test]
        public void ReleaseFromEscrow()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = 4M,
                MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_SUB_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                Options = new TransactionOptionsRequest
                {
                    HoldInEscrow = true
                },
                ServiceFeeAmount = 1M
            };
            Result<Transaction> saleResult = gateway.Transaction.Sale(request);
            Transaction saleTransaction = saleResult.Target;
            Assert.IsTrue(saleResult.IsSuccess());
            TestHelper.Escrow(service, saleTransaction.Id);
            Result<Transaction> result = gateway.Transaction.ReleaseFromEscrow(saleTransaction.Id);

            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;
            Assert.AreEqual(
                TransactionEscrowStatus.RELEASE_PENDING,
                transaction.EscrowStatus
            );
        }

        [Test]
        public void ReleaseFromEscrow_FailsForNonSubmittableTransaction()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = 4M,
                MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                }
            };
            Result<Transaction> saleResult = gateway.Transaction.Sale(request);
            Transaction saleTransaction = saleResult.Target;

            Result<Transaction> result = gateway.Transaction.ReleaseFromEscrow(saleTransaction.Id);

            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_CANNOT_RELEASE_FROM_ESCROW,
                result.Errors.ForObject("Transaction").OnField("Base")[0].Code
            );
        }

        [Test]
        public void CancelRelease()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = 4M,
                MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_SUB_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                Options = new TransactionOptionsRequest
                {
                    HoldInEscrow = true
                },
                ServiceFeeAmount = 1M
            };
            Result<Transaction> saleResult = gateway.Transaction.Sale(request);
            Transaction saleTransaction = saleResult.Target;
            Assert.IsTrue(saleResult.IsSuccess());
            TestHelper.Escrow(service, saleTransaction.Id);
            Result<Transaction> result = gateway.Transaction.ReleaseFromEscrow(saleTransaction.Id);
            Assert.IsTrue(result.IsSuccess());

            Transaction releasedTransaction = result.Target;

            Result<Transaction> cancelResult = gateway.Transaction.CancelRelease(releasedTransaction.Id);
            Assert.IsTrue(cancelResult.IsSuccess());
            Transaction transaction = cancelResult.Target;
            Assert.AreEqual(
                TransactionEscrowStatus.HELD,
                transaction.EscrowStatus
            );
        }

        [Test]
        public void CancelRelease_FailsForTransactionsNotPendingRelease()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = 4M,
                MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_SUB_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                Options = new TransactionOptionsRequest
                {
                    HoldInEscrow = true
                },
                ServiceFeeAmount = 1M
            };
            Result<Transaction> saleResult = gateway.Transaction.Sale(request);
            Transaction saleTransaction = saleResult.Target;
            Assert.IsTrue(saleResult.IsSuccess());
            TestHelper.Escrow(service, saleTransaction.Id);

            Result<Transaction> cancelResult = gateway.Transaction.CancelRelease(saleTransaction.Id);
            Assert.IsFalse(cancelResult.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_CANNOT_CANCEL_RELEASE,
                cancelResult.Errors.ForObject("Transaction").OnField("Base")[0].Code
            );
        }

        [Test]
        public void Sale_WithDescriptor()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                Descriptor = new DescriptorRequest
                {
                  Name = "123*123456789012345678",
                  Phone = "3334445555"
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual("123*123456789012345678", transaction.Descriptor.Name);
            Assert.AreEqual("3334445555", transaction.Descriptor.Phone);
        }

        [Test]
        public void ConfirmTransparentRedirect_SpecifyingDescriptor()
        {
            TransactionRequest trParams = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                Type = TransactionType.SALE,
                Descriptor = new DescriptorRequest
                {
                  Name = "123*123456789012345678",
                  Phone = "3334445555"
                }
            };

            TransactionRequest request = new TransactionRequest
            {
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009"
                }
            };

            String queryString = TestHelper.QueryStringForTR(trParams, request, gateway.TransparentRedirect.Url, service);
            Result<Transaction> result = gateway.TransparentRedirect.ConfirmTransaction(queryString);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual("123*123456789012345678", transaction.Descriptor.Name);
            Assert.AreEqual("3334445555", transaction.Descriptor.Phone);
        }

        [Test]
        public void Sale_WithDescriptorValidation()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                Descriptor = new DescriptorRequest
                {
                  Name = "badcompanyname12*badproduct12",
                  Phone = "%bad4445555"
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.DESCRIPTOR_NAME_FORMAT_IS_INVALID,
                result.Errors.ForObject("Transaction").ForObject("Descriptor").OnField("Name")[0].Code
            );

            Assert.AreEqual(
                ValidationErrorCode.DESCRIPTOR_PHONE_FORMAT_IS_INVALID,
                result.Errors.ForObject("Transaction").ForObject("Descriptor").OnField("Phone")[0].Code
            );
        }

        [Test]
        public void Sale_WithVenmoSdkPaymentMethodCode()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                VenmoSdkPaymentMethodCode = SandboxValues.VenmoSdk.VISA_PAYMENT_METHOD_CODE
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Transaction transaction = result.Target;
            Assert.AreEqual("1111", transaction.CreditCard.LastFour);
        }

        [Test]
        public void Sale_WithVenmoSdkSession()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                Options = new TransactionOptionsRequest
                {
                    VenmoSdkSession = SandboxValues.VenmoSdk.SESSION
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Transaction transaction = result.Target;
            Assert.IsTrue(transaction.CreditCard.IsVenmoSdk.Value);
        }

        #pragma warning disable 0618
        [Test]
        public void ConfirmTransparentRedirect_CreatesTheTransaction()
        {
            TransactionRequest trParams = new TransactionRequest
            {
                Type = TransactionType.SALE
            };

            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009"
                },
                BillingAddress = new CreditCardAddressRequest
                {
                    CountryName = "United States of America",
                    CountryCodeAlpha2 = "US",
                    CountryCodeAlpha3 = "USA",
                    CountryCodeNumeric = "840"
                }
            };

            String queryString = TestHelper.QueryStringForTR(trParams, request, gateway.Transaction.TransparentRedirectURLForCreate(), service);
            Result<Transaction> result = gateway.Transaction.ConfirmTransparentRedirect(queryString);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(1000.00, transaction.Amount);
            Assert.AreEqual(TransactionType.SALE, transaction.Type);
            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.Status);
            Assert.AreEqual(DateTime.Now.Year, transaction.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, transaction.UpdatedAt.Value.Year);

            CreditCard creditCard = transaction.CreditCard;
            Assert.AreEqual("411111", creditCard.Bin);
            Assert.AreEqual("1111", creditCard.LastFour);
            Assert.AreEqual("05", creditCard.ExpirationMonth);
            Assert.AreEqual("2009", creditCard.ExpirationYear);
            Assert.AreEqual("05/2009", creditCard.ExpirationDate);

            Address address = transaction.BillingAddress;
            Assert.AreEqual("US", address.CountryCodeAlpha2);
            Assert.AreEqual("USA", address.CountryCodeAlpha3);
            Assert.AreEqual("840", address.CountryCodeNumeric);
            Assert.AreEqual("United States of America", address.CountryName);
        }
        #pragma warning restore 0618

        #pragma warning disable 0618
        [Test]
        public void ConfirmTransparentRedirect_SpecifyingMerchantAccountId()
        {
            TransactionRequest trParams = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                Type = TransactionType.SALE,
                MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID
            };

            TransactionRequest request = new TransactionRequest
            {
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009"
                }
            };

            String queryString = TestHelper.QueryStringForTR(trParams, request, gateway.Transaction.TransparentRedirectURLForCreate(), service);
            Result<Transaction> result = gateway.Transaction.ConfirmTransparentRedirect(queryString);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID, transaction.MerchantAccountId);
        }
        #pragma warning restore 0618

        [Test]
        public void Credit_WithValidParams()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009"
                }
            };

            Result<Transaction> result = gateway.Transaction.Credit(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(1000.00, transaction.Amount);
            Assert.AreEqual(TransactionType.CREDIT, transaction.Type);
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, transaction.Status);

            CreditCard creditCard = transaction.CreditCard;
            Assert.AreEqual("411111", creditCard.Bin);
            Assert.AreEqual("1111", creditCard.LastFour);
            Assert.AreEqual("05", creditCard.ExpirationMonth);
            Assert.AreEqual("2009", creditCard.ExpirationYear);
            Assert.AreEqual("05/2009", creditCard.ExpirationDate);
        }

        [Test]
        public void Credit_SpecifyingMerchantAccountId()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Credit(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID, transaction.MerchantAccountId);
        }

        [Test]
        public void Credit_WithoutSpecifyingMerchantAccountIdFallsBackToDefault()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Credit(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(MerchantAccountIDs.DEFAULT_MERCHANT_ACCOUNT_ID, transaction.MerchantAccountId);
        }

        [Test]
        public void Credit_WithCustomFields()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CustomFields = new Dictionary<String, String>
                {
                    { "store_me", "custom value"},
                    { "another_stored_field", "custom value2" }
                },
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009"
                }
            };

            Result<Transaction> result = gateway.Transaction.Credit(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual("custom value", transaction.CustomFields["store_me"]);
            Assert.AreEqual("custom value2", transaction.CustomFields["another_stored_field"]);
        }

        [Test]
        public void Credit_WithValidationError()
        {
            TransactionRequest request = new TransactionRequest
            {
                CreditCard = new TransactionCreditCardRequest
                {
                    ExpirationMonth = "05",
                    ExpirationYear = "2010"
                }
            };

            Result<Transaction> result = gateway.Transaction.Credit(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.IsNull(result.Target);

            Assert.AreEqual(ValidationErrorCode.TRANSACTION_AMOUNT_IS_REQUIRED, result.Errors.ForObject("Transaction").OnField("Amount")[0].Code);

            Dictionary<String, String> parameters = result.Parameters;
            Assert.IsFalse(parameters.ContainsKey("transaction[amount]"));
            Assert.AreEqual("05", parameters["transaction[credit_card][expiration_month]"]);
            Assert.AreEqual("2010", parameters["transaction[credit_card][expiration_year]"]);
        }

        [Test]
        public void Credit_WithServiceFeeIsDisallowed()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ServiceFeeAmount = 1M
            };

            Result<Transaction> result = gateway.Transaction.Credit(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_SERVICE_FEE_IS_NOT_ALLOWED_ON_CREDITS,
                result.Errors.ForObject("Transaction").OnField("Base")[0].Code
            );
        }

        [Test]
        public void Find_WithAValidTransactionId()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2008"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            Transaction foundTransaction = gateway.Transaction.Find(transaction.Id);

            Assert.AreEqual(transaction.Id, foundTransaction.Id);
            Assert.AreEqual(TransactionStatus.AUTHORIZED, foundTransaction.Status);
            Assert.AreEqual("05/2008", foundTransaction.CreditCard.ExpirationDate);
        }

        [Test]
        public void Find_WithBadId()
        {
            try
            {
                gateway.Transaction.Find("badId");
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException)
            {
                // expected
            }
        }

        [Test]
        public void Find_ExposesDisbursementDetails()
        {
            Transaction transaction = gateway.Transaction.Find("deposittransaction");

            Assert.AreEqual(transaction.IsDisbursed(), true);

            DisbursementDetails details = transaction.DisbursementDetails;
            Assert.AreEqual(details.DisbursementDate, DateTime.Parse("2013-04-10"));
            Assert.AreEqual(details.SettlementAmount, Decimal.Parse("100.00"));
            Assert.AreEqual(details.SettlementCurrencyIsoCode, "USD");
            Assert.AreEqual(details.SettlementCurrencyExchangeRate, "1");
            Assert.AreEqual(details.FundsHeld, false);
            Assert.AreEqual(details.Success, true);
        }

        [Test]
        public void Find_ExposesDisputes()
        {
            Transaction transaction = gateway.Transaction.Find("disputedtransaction");

            List<Dispute> disputes = transaction.Disputes;
            Dispute dispute = disputes[0];

            Assert.AreEqual(dispute.ReceivedDate, DateTime.Parse("2014-03-01"));
            Assert.AreEqual(dispute.ReplyByDate, DateTime.Parse("2014-03-21"));
            Assert.AreEqual(dispute.Amount, Decimal.Parse("250.00"));
            Assert.AreEqual(dispute.CurrencyIsoCode, "USD");
            Assert.AreEqual(dispute.Reason, DisputeReason.FRAUD);
            Assert.AreEqual(dispute.Status, DisputeStatus.WON);
        }

        [Test]
        public void Find_IsDisbursedFalse()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2008"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            Assert.AreEqual(false, transaction.IsDisbursed());
        }

        [Test]
        public void Find_FindsErrorsOutOnWhitespaceIds()
        {
            try {
                gateway.Transaction.Find(" ");
                Assert.Fail("Should throw NotFoundException");
            } catch (NotFoundException) {}
        }


        [Test]
        public void Void_VoidsTheTransaction()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2008"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            Result<Transaction> result = gateway.Transaction.Void(transaction.Id);
            Assert.IsTrue(result.IsSuccess());

            Assert.AreEqual(transaction.Id, result.Target.Id);
            Assert.AreEqual(TransactionStatus.VOIDED, result.Target.Status);
        }

        [Test]
        public void Void_WithBadId()
        {
            try
            {
                gateway.Transaction.Void("badId");
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException)
            {
                // expected
            }
        }

        [Test]
        public void Void_WithValidationError()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2008"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            Result<Transaction> result = gateway.Transaction.Void(transaction.Id);
            Assert.IsTrue(result.IsSuccess());

            result = gateway.Transaction.Void(transaction.Id);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_CANNOT_BE_VOIDED, result.Errors.ForObject("Transaction").OnField("Base")[0].Code);
        }

        [Test]
        public void SubmitForSettlement_WithoutAmount()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2008"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            Result<Transaction> result = gateway.Transaction.SubmitForSettlement(transaction.Id);

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, result.Target.Status);
            Assert.AreEqual(SandboxValues.TransactionAmount.AUTHORIZE, result.Target.Amount);
            Assert.IsNull(result.Message);
        }

        [Test]
        public void SubmitForSettlement_WithAmount()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2008"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            Result<Transaction> result = gateway.Transaction.SubmitForSettlement(transaction.Id, Decimal.Parse("50.00"));

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, result.Target.Status);
            Assert.AreEqual(50.00, result.Target.Amount);
        }

        [Test]
        public void SubmitForSettlement_TransactionAmountLessThanServiceFeeAmount()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = 100M,
                MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_SUB_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "06/2008"
                },
                ServiceFeeAmount = 50M
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            Result<Transaction> result = gateway.Transaction.SubmitForSettlement(transaction.Id, 25M);

            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_SETTLEMENT_AMOUNT_IS_LESS_THAN_SERVICE_FEE_AMOUNT,
                result.Errors.ForObject("Transaction").OnField("Amount")[0].Code
            );
        }

        [Test]
        public void StatusHistory_HasCorrectValues()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2008"
                }
            };

            String transactionId = gateway.Transaction.Sale(request).Target.Id;
            Transaction transaction = gateway.Transaction.SubmitForSettlement(transactionId, Decimal.Parse("50.00")).Target;

            Assert.AreEqual(2, transaction.StatusHistory.Length);
            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.StatusHistory[0].Status);
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, transaction.StatusHistory[1].Status);
        }

        [Test]
        public void SubmitForSettlement_WithValidationError()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2008"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            Result<Transaction> result = gateway.Transaction.SubmitForSettlement(transaction.Id);
            Assert.IsTrue(result.IsSuccess());

            result = gateway.Transaction.SubmitForSettlement(transaction.Id);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_CANNOT_SUBMIT_FOR_SETTLEMENT, result.Errors.ForObject("Transaction").OnField("Base")[0].Code);
            Assert.AreEqual("Cannot submit for settlement unless status is authorized.", result.Message);
        }

        [Test]
        public void SubmitForSettlement_WithBadId()
        {
            try
            {
                gateway.Transaction.SubmitForSettlement("badId");
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException)
            {
                // expected
            }
        }

        #pragma warning disable 0618
        [Test]
        public void Refund_WithABasicTransaction()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2008"
                },
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            TestHelper.Settle(service, transaction.Id);

            Result<Transaction> result;
            try
            {
                result = gateway.Transaction.Refund(transaction.Id);
            }
            catch(Exception e)
            {
                System.Console.WriteLine("Got exception! " + e.Source);
                throw e;
            }

            Assert.IsTrue(result.IsSuccess());
            var refund = result.Target;

            Assert.AreEqual(TransactionType.CREDIT, refund.Type);
            Assert.AreEqual(transaction.Amount, refund.Amount);

            Transaction firstTransaction = gateway.Transaction.Find(transaction.Id);
            Assert.AreEqual(refund.Id, firstTransaction.RefundId);
            Assert.AreEqual(firstTransaction.Id, refund.RefundedTransactionId);
        }
        #pragma warning restore 0618

        [Test]
        public void Refund_WithAPartialAmount()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2008"
                },
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            TestHelper.Settle(service, transaction.Id);

            Result<Transaction> result = gateway.Transaction.Refund(transaction.Id, Decimal.Parse("500.00"));
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionType.CREDIT, result.Target.Type);
            Assert.AreEqual(Decimal.Parse("500.00"), result.Target.Amount);
        }

        [Test]
        public void Refund_MultipleRefundsWithPartialAmounts()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2008"
                },
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            TestHelper.Settle(service, transaction.Id);

            Transaction refund1 = gateway.Transaction.Refund(transaction.Id, 500M).Target;
            Assert.AreEqual(TransactionType.CREDIT, refund1.Type);
            Assert.AreEqual(500M, refund1.Amount);

            Transaction refund2 = gateway.Transaction.Refund(transaction.Id, 500M).Target;
            Assert.AreEqual(TransactionType.CREDIT, refund2.Type);
            Assert.AreEqual(500M, refund2.Amount);

            Transaction refundedTransaction = gateway.Transaction.Find(transaction.Id);
            Assert.AreEqual(2, refundedTransaction.RefundIds.Count);
            Assert.Contains(refund1.Id, refundedTransaction.RefundIds);
            Assert.Contains(refund2.Id, refundedTransaction.RefundIds);
        }

        [Test]
        public void Settle_RefundFailsWithNonSettledTransaction()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2008"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.Status);

            Result<Transaction> result;
            try
            {
                result = gateway.Transaction.Refund(transaction.Id);
            }
            catch(Exception e)
            {
                System.Console.WriteLine("Got exception! " + e.Source);
                throw e;
            }
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.TRANSACTION_CANNOT_REFUND_UNLESS_SETTLED, result.Errors.ForObject("Transaction").OnField("Base")[0].Code);
        }

        [Test]
        public void SnapshotPlanIdAddOnsAndDiscountsFromSubscription()
        {
            CustomerRequest customerRequest = new CustomerRequest
            {
                CreditCard = new CreditCardRequest
                {
                    CardholderName = "Fred Jones",
                    Number = "5105105105105100",
                    ExpirationDate = "05/12"
                }
            };

            CreditCard creditCard = gateway.Customer.Create(customerRequest).Target.CreditCards[0];

            SubscriptionRequest request = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = PlanFixture.PLAN_WITHOUT_TRIAL.Id,
                AddOns = new AddOnsRequest
                {
                    Add = new AddAddOnRequest[]
                    {
                        new AddAddOnRequest
                        {
                            InheritedFromId = "increase_10",
                            Amount = 11M,
                            NumberOfBillingCycles = 5,
                            Quantity = 2,
                        },
                        new AddAddOnRequest
                        {
                            InheritedFromId = "increase_20",
                            Amount = 21M,
                            NumberOfBillingCycles = 6,
                            Quantity = 3,
                        }
                    }
                },
                Discounts = new DiscountsRequest
                {
                    Add = new AddDiscountRequest[]
                    {
                        new AddDiscountRequest
                        {
                            InheritedFromId = "discount_7",
                            Amount = 7.50M,
                            Quantity = 2,
                            NeverExpires = true
                        },
                    }
                }
            };

            Transaction transaction = gateway.Subscription.Create(request).Target.Transactions[0];

            Assert.AreEqual(PlanFixture.PLAN_WITHOUT_TRIAL.Id, transaction.PlanId);

            List<AddOn> addOns = transaction.AddOns;
            addOns.Sort(TestHelper.CompareModificationsById);

            Assert.AreEqual(2, addOns.Count);

            Assert.AreEqual("increase_10", addOns[0].Id);
            Assert.AreEqual(11M, addOns[0].Amount);
            Assert.AreEqual(2, addOns[0].Quantity);
            Assert.IsFalse(addOns[0].NeverExpires.Value);
            Assert.AreEqual(5, addOns[0].NumberOfBillingCycles);

            Assert.AreEqual("increase_20", addOns[1].Id);
            Assert.AreEqual(21M, addOns[1].Amount);
            Assert.AreEqual(3, addOns[1].Quantity);
            Assert.IsFalse(addOns[1].NeverExpires.Value);
            Assert.AreEqual(6, addOns[1].NumberOfBillingCycles);

            List<Discount> discounts = transaction.Discounts;
            Assert.AreEqual(1, discounts.Count);

            Assert.AreEqual("discount_7", discounts[0].Id);
            Assert.AreEqual(7.50M, discounts[0].Amount);
            Assert.AreEqual(2, discounts[0].Quantity);
            Assert.IsTrue(discounts[0].NeverExpires.Value);
            Assert.IsNull(discounts[0].NumberOfBillingCycles);
        }

        [Test]
        public void CloneTransaction()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                Customer = new CustomerRequest
                {
                    FirstName = "Dan",
                },
                BillingAddress = new AddressRequest
                {
                    FirstName = "Carl",
                },
                ShippingAddress = new AddressRequest
                {
                    FirstName = "Andrew",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            TransactionCloneRequest cloneRequest = new TransactionCloneRequest
            {
                Amount = 123.45M,
                Channel = "MyShoppingCartProvider",
                Options = new TransactionOptionsCloneRequest
                {
                    SubmitForSettlement = false
                }
            };

            Result<Transaction> cloneResult = gateway.Transaction.CloneTransaction(transaction.Id, cloneRequest);
            Assert.IsTrue(cloneResult.IsSuccess());
            Transaction cloneTransaction = cloneResult.Target;

            Assert.AreEqual(123.45, cloneTransaction.Amount);
            Assert.AreEqual("MyShoppingCartProvider", cloneTransaction.Channel);
            Assert.AreEqual(TransactionStatus.AUTHORIZED, cloneTransaction.Status);
            Assert.IsNull(cloneTransaction.GetVaultCreditCard());
            Assert.IsNull(cloneTransaction.GetVaultCustomer());
            Assert.IsNull(cloneTransaction.GetVaultCreditCard());
            Assert.IsNull(cloneTransaction.GetVaultBillingAddress());
            Assert.IsNull(cloneTransaction.GetVaultShippingAddress());

            CreditCard creditCard = cloneTransaction.CreditCard;
            Assert.AreEqual("411111", creditCard.Bin);
            Assert.AreEqual("1111", creditCard.LastFour);
            Assert.AreEqual("05/2009", creditCard.ExpirationDate);

            Assert.AreEqual("Dan", cloneTransaction.Customer.FirstName);
            Assert.AreEqual("Carl", cloneTransaction.BillingAddress.FirstName);
            Assert.AreEqual("Andrew", cloneTransaction.ShippingAddress.FirstName);

        }

        [Test]
        public void CloneTransactionAndSubmitForSettlement()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            TransactionCloneRequest cloneRequest = new TransactionCloneRequest
            {
                Amount = 123.45M,
                Options = new TransactionOptionsCloneRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> cloneResult = gateway.Transaction.CloneTransaction(transaction.Id, cloneRequest);
            Assert.IsTrue(cloneResult.IsSuccess());
            Transaction cloneTransaction = cloneResult.Target;

            Assert.AreEqual(123.45, cloneTransaction.Amount);
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, cloneTransaction.Status);
        }

        [Test]
        public void CloneTransaction_WithValidationErrors()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Credit(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            TransactionCloneRequest cloneRequest = new TransactionCloneRequest
            {
                Amount = 123.45M,
            };

            Result<Transaction> cloneResult = gateway.Transaction.CloneTransaction(transaction.Id, cloneRequest);
            Assert.IsFalse(cloneResult.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_CANNOT_CLONE_CREDIT, cloneResult.Errors.ForObject("Transaction").OnField("Base")[0].Code);
        }

        [Test]
        public void CardTypeIndicators()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                  Number = CreditCardNumbers.CardTypeIndicators.Unknown,
                  ExpirationDate = "12/2015",
                }
            };

            Result<Transaction> result = gateway.Transaction.Credit(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(transaction.CreditCard.Prepaid, Braintree.CreditCardPrepaid.UNKNOWN);
            Assert.AreEqual(transaction.CreditCard.Debit, Braintree.CreditCardDebit.UNKNOWN);
            Assert.AreEqual(transaction.CreditCard.DurbinRegulated, Braintree.CreditCardDurbinRegulated.UNKNOWN);
            Assert.AreEqual(transaction.CreditCard.Commercial, Braintree.CreditCardCommercial.UNKNOWN);
            Assert.AreEqual(transaction.CreditCard.Healthcare, Braintree.CreditCardHealthcare.UNKNOWN);
            Assert.AreEqual(transaction.CreditCard.Payroll, Braintree.CreditCardPayroll.UNKNOWN);
        }

        [Test]
        public void CreateTransaction_WithPaymentMethodNonce()
        {
          String nonce = TestHelper.GenerateUnlockedNonce(gateway);
          TransactionRequest request = new TransactionRequest
          {
            Amount = SandboxValues.TransactionAmount.AUTHORIZE,
            PaymentMethodNonce = nonce
          };
          Result<Transaction> result = gateway.Transaction.Credit(request);
          Assert.IsTrue(result.IsSuccess());
        }
    }

}
