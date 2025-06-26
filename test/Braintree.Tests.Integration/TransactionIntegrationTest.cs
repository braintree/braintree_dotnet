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
    public class TransactionIntegrationTest
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

        public void AdvancedKountFraudSetup()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "advanced_fraud_integration_merchant_id",
                PublicKey = "advanced_fraud_integration_public_key",
                PrivateKey = "advanced_fraud_integration_private_key"
            };

            service = new BraintreeService(gateway.Configuration);
        }

        public void FraudProtectionEnterpriseSetup()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "fraud_protection_enterprise_integration_merchant_id",
                PublicKey = "fraud_protection_enterprise_integration_public_key",
                PrivateKey = "fraud_protection_enterprise_integration_private_key"
            };

            service = new BraintreeService(gateway.Configuration);
        }

        public void EffortlessChargebackProtectionSetup()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "fraud_protection_effortless_chargeback_protection_merchant_id",
                PublicKey = "effortless_chargeback_protection_public_key",
                PrivateKey = "effortless_chargeback_protection_private_key"
            };

            service = new BraintreeService(gateway.Configuration);
        }

        public void DuplicateCheckingSetup()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "dup_checking_integration_merchant_id",
                PublicKey = "dup_checking_integration_public_key",
                PrivateKey = "dup_checking_integration_private_key"
            };

            service = new BraintreeService(gateway.Configuration);
        }

        [Test]
        public void Search_OnAchReturnResponsesCreatedAt()
        {
            DateTime oneDayEarlier = DateTime.Now.AddDays(-1);
            DateTime oneDayLater = DateTime.Now.AddDays(1);

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                AchReturnResponsesCreatedAt.
                Between(oneDayEarlier, oneDayLater);
            ResourceCollection<Transaction> collection = gateway.Transaction.Search(searchRequest);
            Assert.AreEqual(4, collection.MaximumCount);
        }

        [Test]
        public void Search_OnReasonCodes()
        {
            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                ReasonCode.
                IncludedIn("R01");
            ResourceCollection<Transaction> collection = gateway.Transaction.Search(searchRequest);
            Assert.AreEqual(2, collection.MaximumCount);

            searchRequest = new TransactionSearchRequest().
                ReasonCode.
                IncludedIn("R01", "R02");
            collection = gateway.Transaction.Search(searchRequest);
            Assert.AreEqual(3, collection.MaximumCount);
        
            searchRequest = new TransactionSearchRequest().
                ReasonCode.
                IncludedIn(TransactionSearchRequest.ACH_ANY_REASON_CODE);
            collection = gateway.Transaction.Search(searchRequest);
            Assert.AreEqual(3, collection.MaximumCount);
        }

        [Test]
        public void Search_OnAllTextFields()
        {
            string creditCardToken = $"cc{new Random().Next(1000000).ToString()}";
            string firstName = $"Tim{new Random().Next(1000000).ToString()}";

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
            gateway.TestTransaction.Settle(transaction.Id);
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
                CustomerId.Is(transaction.CustomerDetails.Id).
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
                ShippingStreetAddress.Is("456 Road").
                User.Is("integration_user_public_id").
                CreditCardUniqueIdentifier.Is(transaction.CreditCard.UniqueNumberIdentifier);

            ResourceCollection<Transaction> collection = gateway.Transaction.Search(searchRequest);

            Assert.AreEqual(1, collection.MaximumCount);
            Assert.AreEqual(transaction.Id, collection.FirstItem.Id);
        }

        [Test]
#if netcore
        public async Task SearchAsync_OnAllTextFields()
#else
        public void SearchAsync_OnAllTextFields()
        {
            Task.Run(async () =>
#endif
        {
            string creditCardToken = $"cc{new Random().Next(1000000).ToString()}";
            string firstName = $"Tim{new Random().Next(1000000).ToString()}";

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

            Result<Transaction> saleResult = await gateway.Transaction.SaleAsync(request);
            Transaction transaction = saleResult.Target;
            gateway.TestTransaction.Settle(transaction.Id);
            transaction = await gateway.Transaction.FindAsync(transaction.Id);

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
                CustomerId.Is(transaction.CustomerDetails.Id).
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
                ShippingStreetAddress.Is("456 Road").
                User.Is("integration_user_public_id").
                CreditCardUniqueIdentifier.Is(transaction.CreditCard.UniqueNumberIdentifier);

            ResourceCollection<Transaction> collection = await gateway.Transaction.SearchAsync(searchRequest);

            Assert.AreEqual(1, collection.MaximumCount);
            Assert.AreEqual(transaction.Id, collection.FirstItem.Id);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Search_OnTextNodeOperators()
        {
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
        public void Search_PaymentInstrumentTypeIsCreditCard()
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
                PaymentInstrumentType.Is("CreditCardDetail");

            ResourceCollection<Transaction> collection = gateway.Transaction.Search(searchRequest);

            Assert.AreEqual(collection.FirstItem.PaymentInstrumentType, PaymentInstrumentType.CREDIT_CARD);
        }

        [Test]
        public void Search_PaymentInstrumentTypeIsEloCreditCard()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.ADYEN_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = "5066991111111118",
                    ExpirationDate = "10/2020",
                    CVV = "737"
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                PaymentInstrumentType.Is("CreditCardDetail");

            ResourceCollection<Transaction> collection = gateway.Transaction.Search(searchRequest);

            Assert.AreEqual(collection.FirstItem.PaymentInstrumentType, PaymentInstrumentType.CREDIT_CARD);
        }

        [Test]
        public void Search_PaymentInstrumentTypeIsPayPal()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.PayPalOneTimePayment
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                PaymentInstrumentType.Is("PayPalDetail");

            ResourceCollection<Transaction> collection = gateway.Transaction.Search(searchRequest);

            Assert.AreEqual(collection.FirstItem.PaymentInstrumentType, PaymentInstrumentType.PAYPAL_ACCOUNT);
        }

        [Test]
        public void Search_PaymentInstrumentTypeIsLocalPayment()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.LocalPayment,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                PaymentInstrumentType.Is("LocalPaymentDetail");

            ResourceCollection<Transaction> collection = gateway.Transaction.Search(searchRequest);

            Assert.AreEqual(collection.FirstItem.PaymentInstrumentType, PaymentInstrumentType.LOCAL_PAYMENT);
        }

        [Test]
        public void Search_PaymentInstrumentTypeIsSepaDirectDebit()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.SepaDirectDebitAccount,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id)
                .PaymentInstrumentType.Is("SEPADebitAccountDetail");

            ResourceCollection<Transaction> collection = gateway.Transaction.Search(searchRequest);

            Assert.AreEqual(collection.FirstItem.PaymentInstrumentType, PaymentInstrumentType.SEPA_DIRECT_DEBIT_ACCOUNT);
        }

        [Test]
        public void Search_SepaDirectDebitPayPalV2OrderId()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.SepaDirectDebitAccount,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            SepaDirectDebitAccountDetails sepaDirectDebitAccountDetails = transaction.SepaDirectDebitAccountDetails;

            TransactionSearchRequest searchRequest = new TransactionSearchRequest()
                .Id.Is(transaction.Id)
                .SepaDebitPayPalV2OrderId.Is(sepaDirectDebitAccountDetails.PayPalV2OrderId);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Search_PaymentInstrumentTypeIsApplePay()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.ApplePayVisa
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                PaymentInstrumentType.Is("ApplePayDetail");

            ResourceCollection<Transaction> collection = gateway.Transaction.Search(searchRequest);

            Assert.AreEqual(collection.FirstItem.PaymentInstrumentType, PaymentInstrumentType.APPLE_PAY_CARD);
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
        public void Search_ForSettlementConfirmedTransaction()
        {
            string TransactionId = "settlement_confirmed_txn";

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(TransactionId);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);
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
            string name = new Random().Next(1000000).ToString();
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

            gateway.TestTransaction.Settle(saleTransaction.Id);
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
            string creditCardToken = $"cc{new Random().Next(1000000).ToString()}";

            TransactionRequest request = new TransactionRequest
            {
                Amount = 100M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.Dispute.CHARGEBACK,
                    ExpirationDate = "05/2012",
                    Token = creditCardToken
                },
            };
            Transaction transaction = gateway.Transaction.Sale(request).Target;

            DateTime disputeDate = transaction.Disputes[0].ReceivedDate.Value;
            DateTime threeDaysEarlier = disputeDate.AddDays(-3);
            DateTime oneDayEarlier = disputeDate.AddDays(-1);
            DateTime oneDayLater = disputeDate.AddDays(1);

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                DisputeDate.Between(oneDayEarlier, oneDayLater);

            for(int i = 0; i < 60; i++) {
                System.Threading.Thread.Sleep(1000);

                if(gateway.Transaction.Search(searchRequest).MaximumCount > 0) {
                    Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);
                    break;
                }
            }

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                DisputeDate.GreaterThanOrEqualTo(oneDayEarlier);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                DisputeDate.LessThanOrEqualTo(oneDayLater);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
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
            gateway.TestTransaction.Settle(transaction.Id);
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
        public void Search_OnStoreIds()
        {
            string TransactionId = "contact_visa_transaction";

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(TransactionId).
                StoreIds.IncludedIn("store-id");

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(TransactionId).
                StoreIds.IncludedIn("invalid-store-id");

            Assert.AreEqual(0, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Search_OnStoreId()
        {
            string TransactionId = "contact_visa_transaction";

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(TransactionId).
                StoreId.Is("store-id");

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);

            searchRequest = new TransactionSearchRequest().
                Id.Is(TransactionId).
                StoreId.Is("invalid-store-id");

            Assert.AreEqual(0, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]        
        public void Search_ReturnsErrorOnTimeout()
        {
            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Amount.Is(-5);
            Assert.Throws<UnexpectedException>(() => gateway.Transaction.Search(searchRequest));
        }

        [Test]      
        public void Search_OnPayPalFields()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.PayPalOneTimePayment
            };
            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(transactionResult.IsSuccess());

            var searchRequest = new TransactionSearchRequest().
                Id.Is(transactionResult.Target.Id).
                PayPalPaymentId.StartsWith("PAY").
                PayPalAuthorizationId.StartsWith("AUTH").
                PayPalPayerEmail.Is("payer@example.com");

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Search_DecimalAmountUsesInvariantCultureFormattingWithToString()
        {
#if netcore
            var originalCulture = CultureInfo.CurrentCulture;
            try {
                CultureInfo.CurrentCulture = new CultureInfo("da-DK");
#else
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            try {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("da-DK");
#endif

                var request = new TransactionRequest
                {
                    Amount = 10.33M,
                    CreditCard = new TransactionCreditCardRequest
                    {
                        Number = SandboxValues.CreditCardNumber.VISA,
                        ExpirationDate = "05/2020"
                    }
                };

                Transaction transaction = gateway.Transaction.Sale(request).Target;

                var searchRequest = new TransactionSearchRequest().
                    Id.Is(transaction.Id).
                    Amount.Is(transaction.Amount);

                ResourceCollection<Transaction> collection = gateway.Transaction.Search(searchRequest);
                Assert.AreEqual(1, collection.MaximumCount);
            } finally {
#if netcore
                CultureInfo.CurrentCulture = originalCulture;
#else
                Thread.CurrentThread.CurrentCulture = originalCulture;
#endif
            }
        }

        [Test]
        [Ignore("Flaky test")]
        public void Search_OnDebitNetwork()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.TransactablePinlessDebitVisa,
                MerchantAccountId = "pinless_debit",
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            Assert.AreEqual(TransactionType.SALE, transaction.Type);
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, transaction.Status);
            Assert.AreEqual(Braintree.TransactionDebitNetwork.STAR,transaction.DebitNetwork);
            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Id.Is(transaction.Id).
                DebitNetwork.Is(Braintree.TransactionDebitNetwork.STAR);

            Assert.AreEqual(1, gateway.Transaction.Search(searchRequest).MaximumCount);
        }

        [Test]
        public void Sale_PinlessEligibleWithProcessDebitAsCreditFlag()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.TransactablePinlessDebitVisa,
                MerchantAccountId = "pinless_debit",
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true,
                    CreditCard = new TransactionOptionsCreditCardRequest{
                        ProcessDebitAsCredit = true
                    }
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            Assert.AreEqual(Braintree.TransactionDebitNetwork.UNRECOGNIZED,transaction.DebitNetwork);
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
            Assert.IsNotNull(transaction.AuthorizationExpiresAt);
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
            Assert.IsNull(transaction.AcquirerReferenceNumber);
        }

        [Test]
        public void Sale_ReturnsSuccessfulScaExemptionResponse()
        {
            var requestedScaExemption = "low_value";
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                ScaExemption = requestedScaExemption,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA_COUNTRY_OF_ISSUANCE_IE,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(requestedScaExemption, transaction.ScaExemptionRequested);
        }

        [Test]
        public void Sale_ReturnsFailureScaExemptionResponse()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                ScaExemption = "invalid_sca_exemption",
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA_COUNTRY_OF_ISSUANCE_IE,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_SCA_EXEMPTION_REQUEST_INVALID, result.Errors.ForObject("Transaction").OnField("ScaExemption")[0].Code);
        }

        [Test]
        public void Sale_ReturnsSuccessfulResponseWithNetworkResponseCodeText()
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
            Assert.AreEqual("XX", transaction.NetworkResponseCode);
            Assert.AreEqual("sample network response text", transaction.NetworkResponseText);
        }

        [Test]
        public void Sale_ReturnsSuccessfulResponseWithMerchantAdviceCodeText()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.DECLINE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.MASTER_CARD,
                    ExpirationDate = "05/2030"
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Transaction transaction = result.Transaction;

            Assert.AreEqual(TransactionStatus.PROCESSOR_DECLINED, transaction.Status);

            Assert.AreEqual("01", transaction.MerchantAdviceCode);
            Assert.AreEqual("New account information available", transaction.MerchantAdviceCodeText);
        }

        [Test]
        public void Sale_WithEloCardType()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.ADYEN_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = "5066991111111118",
                    ExpirationDate = "10/2020",
                    CVV = "737"
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(1000.00, transaction.Amount);
            Assert.AreEqual(TransactionType.SALE, transaction.Type);
            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.Status);
            Assert.IsNotNull(transaction.AuthorizationExpiresAt);
            Assert.AreEqual(DateTime.Now.Year, transaction.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, transaction.UpdatedAt.Value.Year);
            Assert.IsNotNull(transaction.ProcessorAuthorizationCode);
            Assert.AreEqual(TransactionGatewayRejectionReason.UNRECOGNIZED, transaction.GatewayRejectionReason);

            CreditCard creditCard = transaction.CreditCard;
            Assert.AreEqual("506699", creditCard.Bin);
            Assert.AreEqual("1118", creditCard.LastFour);
            Assert.AreEqual("10", creditCard.ExpirationMonth);
            Assert.AreEqual("2020", creditCard.ExpirationYear);
            Assert.AreEqual("10/2020", creditCard.ExpirationDate);
        }


        [Test]
#if netcore
        public async Task SaleAsync_ReturnsSuccessfulResponse()
#else
        public void SaleAsync_ReturnsSuccessfulResponse()
        {
            Task.Run(async () =>
#endif
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

            Result<Transaction> result = await gateway.Transaction.SaleAsync(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(1000.00, transaction.Amount);
            Assert.AreEqual(TransactionType.SALE, transaction.Type);
            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.Status);
            Assert.AreEqual(ProcessorResponseType.APPROVED, transaction.ProcessorResponseType);
            Assert.IsNotNull(transaction.AuthorizationExpiresAt);
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
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        [Ignore("Flaky test. See: https://paypal.slack.com/archives/C03P4S18JBT/p1741207572015819 for more details.")]
        public void Sale_ReturnsSuccessfulResponseWithUsBankAccount()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID,
                PaymentMethodNonce = TestHelper.GenerateValidUsBankAccountNonce(gateway),
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true,
                    StoreInVault = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(1000.00, transaction.Amount);
            Assert.AreEqual(TransactionType.SALE, transaction.Type);
            Assert.AreEqual(TransactionStatus.SETTLEMENT_PENDING, transaction.Status);
            Assert.IsNotNull(transaction.AuthorizationExpiresAt);
            Assert.AreEqual(DateTime.Now.Year, transaction.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, transaction.UpdatedAt.Value.Year);

            UsBankAccountDetails usBankAccountDetails = transaction.UsBankAccountDetails;
            Assert.AreEqual("021000021", usBankAccountDetails.RoutingNumber);
            Assert.AreEqual("0000", usBankAccountDetails.Last4);
            Assert.AreEqual("checking", usBankAccountDetails.AccountType);
            Assert.AreEqual("Dan Schulman", usBankAccountDetails.AccountHolderName);
            Assert.IsTrue(Regex.IsMatch(usBankAccountDetails.BankName, ".*CHASE.*"));
            AchMandate achMandate = usBankAccountDetails.AchMandate;
            Assert.AreEqual("cl mandate text", achMandate.Text);
            Assert.AreEqual("DateTime", achMandate.AcceptedAt.GetType().Name);
        }

        [Test]
        public void Sale_ReturnsSuccessfulResponseWithUsBankAccountAndVaultedToken()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID,
                PaymentMethodNonce = TestHelper.GenerateValidUsBankAccountNonce(gateway),
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true,
                    StoreInVault = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(1000.00, transaction.Amount);
            Assert.AreEqual(TransactionType.SALE, transaction.Type);
            Assert.AreEqual(TransactionStatus.SETTLEMENT_PENDING, transaction.Status);
            Assert.IsNotNull(transaction.AuthorizationExpiresAt);
            Assert.AreEqual(DateTime.Now.Year, transaction.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, transaction.UpdatedAt.Value.Year);

            UsBankAccountDetails usBankAccountDetails = transaction.UsBankAccountDetails;
            Assert.AreEqual("021000021", usBankAccountDetails.RoutingNumber);
            Assert.AreEqual("0000", usBankAccountDetails.Last4);
            Assert.AreEqual("checking", usBankAccountDetails.AccountType);
            Assert.AreEqual("Dan Schulman", usBankAccountDetails.AccountHolderName);
            Assert.IsTrue(Regex.IsMatch(usBankAccountDetails.BankName, ".*CHASE.*"));
            AchMandate achMandate = usBankAccountDetails.AchMandate;
            Assert.AreEqual("cl mandate text", achMandate.Text);
            Assert.AreEqual("DateTime", achMandate.AcceptedAt.GetType().Name);

            request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID,
                PaymentMethodToken = usBankAccountDetails.Token,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true,
                }
            };

            result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            transaction = result.Target;

            Assert.AreEqual(1000.00, transaction.Amount);
            Assert.AreEqual(TransactionType.SALE, transaction.Type);
            Assert.AreEqual(TransactionStatus.SETTLEMENT_PENDING, transaction.Status);
            Assert.IsNotNull(transaction.AuthorizationExpiresAt);
            Assert.AreEqual(DateTime.Now.Year, transaction.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, transaction.UpdatedAt.Value.Year);

            usBankAccountDetails = transaction.UsBankAccountDetails;
            Assert.AreEqual("021000021", usBankAccountDetails.RoutingNumber);
            Assert.AreEqual("0000", usBankAccountDetails.Last4);
            Assert.AreEqual("checking", usBankAccountDetails.AccountType);
            Assert.AreEqual("Dan Schulman", usBankAccountDetails.AccountHolderName);
            Assert.IsTrue(Regex.IsMatch(usBankAccountDetails.BankName, ".*CHASE.*"));
            achMandate = usBankAccountDetails.AchMandate;
            Assert.AreEqual("cl mandate text", achMandate.Text);
            Assert.AreEqual("DateTime", achMandate.AcceptedAt.GetType().Name);
        }

        [Test]
        public void Sale_ReturnsFailureResponseWithInvalidUsBankAccount()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID,
                PaymentMethodNonce = TestHelper.GenerateInvalidUsBankAccountNonce(),
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true,
                    StoreInVault = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_PAYMENT_METHOD_NONCE_UNKNOWN, result.Errors.ForObject("Transaction").OnField("PaymentMethodNonce")[0].Code);
        }

        [Test]
        public void Sale_ReturnsSuccessfulResponseWithPartialSettlement()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.MASTER_CARD,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> authorizationResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(authorizationResult.IsSuccess());
            Transaction authorizedTransaction = authorizationResult.Target;

            Assert.AreEqual(1000.00, authorizedTransaction.Amount);
            Assert.AreEqual(TransactionType.SALE, authorizedTransaction.Type);
            Assert.AreEqual(TransactionStatus.AUTHORIZED, authorizedTransaction.Status);
            Assert.IsNotNull(authorizedTransaction.ProcessorAuthorizationCode);

            var partialSettlementResult1 = gateway.Transaction.SubmitForPartialSettlement(authorizedTransaction.Id, 400);
            var partialSettlementTransaction1 = partialSettlementResult1.Target;
            Assert.AreEqual(400.00, partialSettlementTransaction1.Amount);
            Assert.AreEqual(TransactionType.SALE, partialSettlementTransaction1.Type);
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, partialSettlementTransaction1.Status);
            Assert.AreEqual(authorizedTransaction.Id, partialSettlementTransaction1.AuthorizedTransactionId);

            var refreshedAuthorizedTransaction = gateway.Transaction.Find(authorizedTransaction.Id);
            Assert.AreEqual(TransactionStatus.SETTLEMENT_PENDING, refreshedAuthorizedTransaction.Status);
            var partialSettlementTransactionIds = new string[] { partialSettlementTransaction1.Id };
            Assert.AreEqual(refreshedAuthorizedTransaction.PartialSettlementTransactionIds, partialSettlementTransactionIds);

            var partialSettlementResult2 = gateway.Transaction.SubmitForPartialSettlement(authorizedTransaction.Id, 600);
            var partialSettlementTransaction2 = partialSettlementResult2.Target;
            Assert.AreEqual(600.00, partialSettlementTransaction2.Amount);
            Assert.AreEqual(TransactionType.SALE, partialSettlementTransaction2.Type);
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, partialSettlementTransaction2.Status);

            refreshedAuthorizedTransaction = gateway.Transaction.Find(authorizedTransaction.Id);
            partialSettlementTransactionIds = new string[] { partialSettlementTransaction1.Id, partialSettlementTransaction2.Id };
            CollectionAssert.AreEquivalent(refreshedAuthorizedTransaction.PartialSettlementTransactionIds, partialSettlementTransactionIds);
        }

        public void Sale_ReturnsUnsuccessfulResponseForPartialSettlementWithAPartialSettlementTransaction()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.ApplePayAmex
            };

            Result<Transaction> authorizationResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(authorizationResult.IsSuccess());
            Transaction authorizedTransaction = authorizationResult.Target;

            var partialSettlementResult1 = gateway.Transaction.SubmitForPartialSettlement(authorizedTransaction.Id, 400);
            var partialSettlementTransaction1 = partialSettlementResult1.Target;
            Assert.IsTrue(partialSettlementResult1.IsSuccess());

            var partialSettlementResult2 = gateway.Transaction.SubmitForPartialSettlement(partialSettlementTransaction1.Id, 600);
            Assert.IsFalse(partialSettlementResult2.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_CANNOT_SUBMIT_FOR_PARTIAL_SETTLEMENT, partialSettlementResult2.Errors.ForObject("Transaction").OnField("Base")[0].Code);
        }

        [Test]
        public void Sale_ReturnsUnsuccessfulResponseForPartialSettlementWithUnacceptedPaymentInstrumentType()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.FAKE_VENMO_ACCOUNT_MERCHANT_ACCOUNT_ID,
                PaymentMethodNonce = Nonce.VenmoAccount
            };

            Result<Transaction> authorizationResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(authorizationResult.IsSuccess());
            Transaction authorizedTransaction = authorizationResult.Target;

            var partialSettlementResult = gateway.Transaction.SubmitForPartialSettlement(authorizedTransaction.Id, 400);
            Assert.IsFalse(partialSettlementResult.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_PAYMENT_INSTRUMENT_TYPE_IS_NOT_ACCEPTED, partialSettlementResult.Errors.ForObject("Transaction").OnField("Base")[0].Code);
        }

        [Test]
        [Ignore("unpend when we have a more stable CI")]
        public void Sale_WithSuccessfulAmexRewardsResponse()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.FAKE_AMEX_DIRECT_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.AmexPayWithPoints.SUCCESS,
                    ExpirationDate = "05/2009",
                },
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true,
                    AmexRewards = new TransactionOptionsAmexRewardsRequest
                    {
                        RequestId = "ABC123",
                        Points = "100",
                        CurrencyAmount = "1",
                        CurrencyIsoCode = "USD"
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(1000.00, transaction.Amount);
            Assert.AreEqual(TransactionType.SALE, transaction.Type);
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, transaction.Status);
        }

        [Test]
        [Ignore("unpend when we have a more stable CI")]
        public void Sale_WithAmexRewardsResponseSucceedsEvenIfCardIsIneligible()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.FAKE_AMEX_DIRECT_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.AmexPayWithPoints.INELIGIBLE_CARD,
                    ExpirationDate = "05/2009",
                },
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true,
                    AmexRewards = new TransactionOptionsAmexRewardsRequest
                    {
                        RequestId = "ABC123",
                        Points = "100",
                        CurrencyAmount = "1",
                        CurrencyIsoCode = "USD"
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(1000.00, transaction.Amount);
            Assert.AreEqual(TransactionType.SALE, transaction.Type);
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, transaction.Status);
        }

        [Test]
        [Ignore("unpend when we have a more stable CI")]
        public void Sale_WithAmexRewardsResponseSucceedsEvenIfCardHasInsufficientPoints()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.FAKE_AMEX_DIRECT_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.AmexPayWithPoints.INSUFFICIENT_POINTS,
                    ExpirationDate = "05/2009",
                },
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true,
                    AmexRewards = new TransactionOptionsAmexRewardsRequest
                    {
                        RequestId = "ABC123",
                        Points = "100",
                        CurrencyAmount = "1",
                        CurrencyIsoCode = "USD"
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(1000.00, transaction.Amount);
            Assert.AreEqual(TransactionType.SALE, transaction.Type);
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, transaction.Status);
        }

        [Test]
        public void Sale_ReturnsSuccessfulResponseUsingAccessToken()
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

            BraintreeGateway oauthGateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );
            string code = OAuthTestHelper.CreateGrant(oauthGateway, "integration_merchant_id", "read_write");
            ResultImpl<OAuthCredentials> accessTokenResult = oauthGateway.OAuth.CreateTokenFromCode(new OAuthCredentialsRequest {
                Code = code,
                Scope = "read_write"
            });

            gateway = new BraintreeGateway(accessTokenResult.Target.AccessToken);

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(1000.00, transaction.Amount);
            Assert.AreEqual(TransactionType.SALE, transaction.Type);
            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.Status);
            Assert.IsNotNull(transaction.AuthorizationExpiresAt);
            Assert.AreEqual(DateTime.Now.Year, transaction.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, transaction.UpdatedAt.Value.Year);
            Assert.IsNotNull(transaction.ProcessorAuthorizationCode);
        }

        [Test]
        public void Sale_ReturnsSuccessfulResponseWithRiskData()
        {
            FraudProtectionEnterpriseSetup();
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

            Assert.IsNotNull(transaction.RiskData);
            Assert.IsNotNull(transaction.RiskData.decision);
            Assert.IsNotNull(transaction.RiskData.DecisionReasons);
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
            Assert.IsNotNull(transaction.AuthorizationExpiresAt);
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
        public void Sale_WithExchangeRateQuoteId()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ExchangeRateQuoteId = "DummyExchangeRateQuoteId-DotNet",
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(1000.00, transaction.Amount);
            Assert.AreEqual(TransactionType.SALE, transaction.Type);
            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.Status);
        }


        [Test]
        public void Sale_ErrorWithInvalidExchangeRateQuoteId()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ExchangeRateQuoteId = new string('a', 4010),
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.EXCHANGE_RATE_QUOTE_ID_IS_TOO_LONG, result.Errors.ForObject("Transaction").OnField("ExchangeRateQuoteId")[0].Code);
        }


        [Test]
        public void Sale_WithRiskData()
        {
            AdvancedKountFraudSetup();
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest()
                {
                    Number = "4111111111111111",
                    ExpirationDate = "10/10"
                },
                RiskData = new RiskDataRequest()
                {
                    CustomerBrowser = "IE6",
                    CustomerDeviceId = "customer_device_id_012",
                    CustomerIP = "192.168.0.1",
                    CustomerLocationZip = "91244",
                    CustomerTenure = 20
                },
                DeviceData = "{\"device_session_id\":\"my_dsid\", \"fraud_merchant_id\":\"7\"}"
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;
            Assert.IsNotNull(transaction.RiskData.decision);
            Assert.IsNotNull(transaction.RiskData.fraudServiceProvider);

        }

        [Test]
        public void Sale_FailureResponseWithInvalidRiskData()
        {
            AdvancedKountFraudSetup();
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest()
                {
                    Number = "4111111111111111",
                    ExpirationDate = "10/10"
                },
                RiskData = new RiskDataRequest()
                {
                    CustomerBrowser = "IE6",
                    CustomerDeviceId = "customer_device_id_012",
                    CustomerIP = "192.168.0.1",
                    CustomerLocationZip = "912$4",
                    CustomerTenure = 20
                },
                DeviceData = "{\"device_session_id\":\"my_dsid\", \"fraud_merchant_id\":\"7\"}"
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.RISK_DATA_CUSTOMER_LOCATION_ZIP_INVALID_CHARACTERS,
                result.Errors.ForObject("Transaction").ForObject("RiskData").OnField("CustomerLocationZip")[0].Code
            );
        }

        [Test]
        public void Sale_ReturnsPaymentInstrumentType()
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

            Assert.AreEqual(PaymentInstrumentType.CREDIT_CARD, transaction.PaymentInstrumentType);
        }

        [Test]
        public void Sale_ReturnsPaymentInstrumentTypeForPayPal()
        {
            string nonce = TestHelper.GenerateOneTimePayPalNonce(gateway);
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(PaymentInstrumentType.PAYPAL_ACCOUNT, transaction.PaymentInstrumentType);
        }

        [Test]
        public void Sale_ReturnsDebugIdForPayPal()
        {
            string nonce = TestHelper.GenerateOneTimePayPalNonce(gateway);
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.IsNotNull(transaction.PayPalDetails);
            Assert.IsNotNull(transaction.PayPalDetails.DebugId);
        }

        #pragma warning disable 0618
        [Test]
        public void Sale_WithAllAttributes()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                Channel = "MyShoppingCartProvider",
                OrderId = "123",
                ProductSku = "productsku01",
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
                    Company = "Braintree",
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
                    CountryCodeNumeric = "840",
                    InternationalPhone = new InternationalPhoneRequest
                    {
                        CountryCode = "1",
                        NationalNumber = "3121234567" 
                    }
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
                    PhoneNumber = "122-555-1236",
                    PostalCode = "60103",
                    CountryName = "Mexico",
                    CountryCodeAlpha2 = "MX",
                    CountryCodeAlpha3 = "MEX",
                    CountryCodeNumeric = "484",
                    ShippingMethod = ShippingMethod.ELECTRONIC,
                    InternationalPhone = new InternationalPhoneRequest
                    {
                        CountryCode = "1",
                        NationalNumber = "3121234567" 
                    }
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
            CustomerDetails customer = transaction.CustomerDetails;
            Assert.AreEqual("Dan", customer.FirstName);
            Assert.AreEqual("Smith", customer.LastName);
            Assert.AreEqual("Braintree", customer.Company);
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
            Assert.AreEqual("1", billingAddress.InternationalPhone.CountryCode);
            Assert.AreEqual("3121234567", billingAddress.InternationalPhone.NationalNumber);

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
            Assert.AreEqual("1", shippingAddress.InternationalPhone.CountryCode);
            Assert.AreEqual("3121234567", shippingAddress.InternationalPhone.NationalNumber);
        }
        #pragma warning restore 0618

        [Test]
        public void Sale_WithTransactionSourceAsRecurringFirst()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                TransactionSource = "recurring_first"
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Transaction transaction = result.Target;
            Assert.IsTrue(result.IsSuccess());
            Assert.IsTrue(transaction.Recurring.Value);
        }

        [Test]
        public void Sale_WithTransactionSourceAsRecurring()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                TransactionSource = "recurring"
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Transaction transaction = result.Target;
            Assert.IsTrue(result.IsSuccess());
            Assert.IsTrue(transaction.Recurring.Value);
        }

        [Test]
        public void Sale_WithTransactionSourceAsMerchant()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                TransactionSource = "merchant"
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Transaction transaction = result.Target;
            Assert.IsTrue(result.IsSuccess());
            Assert.IsFalse(transaction.Recurring.Value);
        }

        [Test]
        public void Sale_WithTransactionSourceAsMoto()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                TransactionSource = "moto"
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Transaction transaction = result.Target;
            Assert.IsTrue(result.IsSuccess());
            Assert.IsFalse(transaction.Recurring.Value);
        }

        [Test]
        public void Sale_WithTransactionSourceInvalid()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                TransactionSource = "invalid_value"
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_TRANSACTION_SOURCE_IS_INVALID, result.Errors.ForObject("Transaction").OnField("Transaction-Source")[0].Code);
        }

        [Test]
        public void Sale_WithProductSkuInvalid()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ProductSku = "product$ku!"
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_PRODUCT_SKU_IS_INVALID, result.Errors.ForObject("Transaction").OnField("ProductSku")[0].Code);
        }

        [Test]
        [Obsolete]
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
            string customerId = new Random().Next(1000000).ToString();
            string paymentToken = new Random().Next(1000000).ToString();

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

            CustomerDetails customer = transaction.CustomerDetails;
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
                Email = "hansolo64@example.com",
                Phone = "312.555.1111",
                Fax = "312.555.1112",
                Website = "www.example.com"
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
                Email = "hansolo64@example.com",
                Phone = "312.555.1111",
                Fax = "312.555.1112",
                Website = "www.example.com"
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

            CustomerDetails customer = transaction.CustomerDetails;
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

            CustomerDetails customer = transaction.CustomerDetails;
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

            CustomerDetails customer = transaction.CustomerDetails;
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
        public void Sale_WithThreeDSecureOptionRequired()
        {
            string nonce = TestHelper.GenerateUnlockedNonce(gateway);
            var request = new TransactionRequest
            {
                MerchantAccountId = MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID,
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest()
                {
                    ThreeDSecure = new TransactionOptionsThreeDSecureRequest()
                    {
                        Required = true
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Transaction transaction = result.Transaction;

            Assert.AreEqual(TransactionGatewayRejectionReason.THREE_D_SECURE, transaction.GatewayRejectionReason);
        }

        [Test]
        public void Sale_WithThreeDSecureAuthenticationId()
        {
            var three_d_secure_authentication_id = TestHelper.Create3DSVerification(service, MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID, new ThreeDSecureRequestForTests() {
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationMonth = "05",
                ExpirationYear = "2009"
            });

            var request = new TransactionRequest
            {
                MerchantAccountId = MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID,
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                ThreeDSecureAuthenticationId = three_d_secure_authentication_id,
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
            Assert.AreEqual("authenticate_successful", transaction.ThreeDSecureInfo.Status);
            Assert.IsNotNull(transaction.ThreeDSecureInfo.Enrolled);
            Assert.IsNotNull(transaction.ThreeDSecureInfo.Cavv);
            Assert.IsNotNull(transaction.ThreeDSecureInfo.EciFlag);
            Assert.IsNotNull(transaction.ThreeDSecureInfo.ThreeDSecureVersion);
            Assert.IsNotNull(transaction.ThreeDSecureInfo.Xid);
            Assert.IsTrue(transaction.ThreeDSecureInfo.LiabilityShifted);
            Assert.IsTrue(transaction.ThreeDSecureInfo.LiabilityShiftPossible);
        }

        [Test]
        public void Sale_ErrorPaymentMethodDoesNotMatchWithThreeDSecureAuthenticationId()
        {
            var three_d_secure_authentication_id = TestHelper.Create3DSVerification(service, MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID, new ThreeDSecureRequestForTests() {
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationMonth = "05",
                ExpirationYear = "2009",
            });

            var request = new TransactionRequest
            {
                MerchantAccountId = MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID,
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                ThreeDSecureAuthenticationId = three_d_secure_authentication_id,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.MASTER_CARD,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_THREE_D_SECURE_TRANSACTION_PAYMENT_METHOD_DOES_NOT_MATCH_THREE_D_SECURE_AUTHENTICATION_PAYMENT_METHOD, result.Errors.ForObject("Transaction").OnField("Three-D-Secure-Authentication-Id")[0].Code);
        }

        [Test]
        public void Sale_ErrorWithMismatchThreeDSecureNonceAndThreeDSecureAuthenticationId()
        {
            var three_d_secure_authentication_id = TestHelper.Create3DSVerification(service, MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID, new ThreeDSecureRequestForTests() {
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationMonth = "05",
                ExpirationYear = "2009",
            });

            CreditCardRequest creditCardRequest = new CreditCardRequest
            {
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationMonth = "05",
                ExpirationYear = "2020"
            };
            string nonce = TestHelper.Generate3DSNonce(service, creditCardRequest);

            var request = new TransactionRequest
            {
                MerchantAccountId = MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID,
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce,
                ThreeDSecureAuthenticationId = three_d_secure_authentication_id,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.MASTER_CARD,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_THREE_D_SECURE_AUTHENTICATION_ID_DOES_NOT_MATCH_NONCE_THREE_D_SECURE_AUTHENTICATION, result.Errors.ForObject("Transaction").OnField("Three-D-Secure-Authentication-Id")[0].Code);
        }

        [Test]
        public void Sale_ErrorWithBogusThreeDSecureAuthenticationId()
        {
            string three_d_secure_authentication_id = "foo";

            var request = new TransactionRequest
            {
                MerchantAccountId = MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID,
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                ThreeDSecureAuthenticationId = three_d_secure_authentication_id,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_THREE_D_SECURE_AUTHENTICATION_ID_IS_INVALID, result.Errors.ForObject("Transaction").OnField("Three-D-Secure-Authentication-Id")[0].Code);
        }

        [Test]
        public void Sale_ErrorWithThreeDAuthenticationIdAndThreeDSecurePassThru()
        {
            var three_d_secure_authentication_id = TestHelper.Create3DSVerification(service, MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID, new ThreeDSecureRequestForTests() {
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationMonth = "05",
                ExpirationYear = "2009",
            });

           var request = new TransactionRequest
            {
                MerchantAccountId = MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID,
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                ThreeDSecureAuthenticationId = three_d_secure_authentication_id,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ThreeDSecurePassThru = new ThreeDSecurePassThruRequest
                {
                    EciFlag = "02",
                    Cavv = "some_cavv",
                    Xid = "some_xid",
                    AuthenticationResponse = "authentication_response_value",
                    DirectoryResponse = "Y",
                    CavvAlgorithm = "2",
                    ThreeDSecureVersion = "1.0.2"
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Transaction transaction = result.Target;
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_THREE_D_SECURE_AUTHENTICATION_ID_WITH_THREE_D_SECURE_PASS_THRU_IS_INVALID, result.Errors.ForObject("Transaction").OnField("Three-D-Secure-Authentication-Id")[0].Code);
        }

        // NEXT_MAJOR_VERSION remove this test
        // threeDSecureToken has been deprecated in favor of threeDSecureAuthenticationId
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
            Assert.AreEqual("authenticate_successful", transaction.ThreeDSecureInfo.Status);
            Assert.IsNotNull(transaction.ThreeDSecureInfo.Enrolled);
            Assert.IsNotNull(transaction.ThreeDSecureInfo.Cavv);
            Assert.IsNotNull(transaction.ThreeDSecureInfo.EciFlag);
            Assert.IsNotNull(transaction.ThreeDSecureInfo.ThreeDSecureVersion);
            Assert.IsNotNull(transaction.ThreeDSecureInfo.Xid);
            Assert.IsTrue(transaction.ThreeDSecureInfo.LiabilityShifted);
            Assert.IsTrue(transaction.ThreeDSecureInfo.LiabilityShiftPossible);
        }

        // NEXT_MAJOR_VERSION remove this test
        // threeDSecureToken has been deprecated in favor of threeDSecureAuthenticationId
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

        // NEXT_MAJOR_VERSION replace ThreeDSecureToken with ThreeDSecureAuthenticationId
        // threeDSecureToken has been deprecated in favor of threeDSecureAuthenticationId
        [Test]
        public void Sale_ErrorWithNullThreeDSecureToken()
        {
            string three_d_secure_token = null;

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
        public void Sale_WithThreeDSecurePassThru()
        {
            var request = new TransactionRequest
            {
                MerchantAccountId = MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID,
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ThreeDSecurePassThru = new ThreeDSecurePassThruRequest
                {
                    EciFlag = "02",
                    Cavv = "some_cavv",
                    Xid = "some_xid",
                    AuthenticationResponse = "authentication_response_value",
                    DirectoryResponse = "Y",
                    CavvAlgorithm = "2",
                    ThreeDSecureVersion = "1.0.2"
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Transaction transaction = result.Target;
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.Status);
        }

        [Test]
        public void Sale_WithThreeDSecurePassThruForVersion2()
        {
            var request = new TransactionRequest
            {
                MerchantAccountId = MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID,
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2029",
                },
                ThreeDSecurePassThru = new ThreeDSecurePassThruRequest
                {
                    EciFlag = "02",
                    Cavv = "some_cavv",
                    AuthenticationResponse = "authentication_response_value",
                    DirectoryResponse = "Y",
                    CavvAlgorithm = "2",
                    ThreeDSecureVersion = "2.1.0",
                    DsTransactionId = "ds_transaction_id_value"
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Transaction transaction = result.Target;
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.Status);
        }

        [Test]
        public void Sale_ErrorWithThreeDSecurePassThruWhenMerchantAccountDoesNotSupportCardType()
        {
            var request = new TransactionRequest
            {
                MerchantAccountId = "heartland_ma",
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ThreeDSecurePassThru = new ThreeDSecurePassThruRequest
                {
                    EciFlag = "02",
                    Cavv = "some_cavv",
                    Xid = "some_xid",
                    AuthenticationResponse = "authentication_response_value",
                    DirectoryResponse = "Y",
                    CavvAlgorithm = "2",
                    ThreeDSecureVersion = "1.0.2",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Transaction transaction = result.Target;
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_THREE_D_SECURE_PASS_THRU_MERCHANT_ACCOUNT_DOES_NOT_SUPPORT_CARD_TYPE, result.Errors.ForObject("Transaction").OnField("Merchant-Account-Id")[0].Code);
        }

        [Test]
        public void Sale_ErrorWithMissingThreeDSecurePassThruEciFlag()
        {
            var request = new TransactionRequest
            {
                MerchantAccountId = MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID,
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ThreeDSecurePassThru = new ThreeDSecurePassThruRequest
                {
                    EciFlag = "",
                    Cavv = "some_cavv",
                    Xid = "some_xid",
                    AuthenticationResponse = "authentication_response_value",
                    DirectoryResponse = "Y",
                    CavvAlgorithm = "2",
                    ThreeDSecureVersion = "1.0.2",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Transaction transaction = result.Target;
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_THREE_D_SECURE_PASS_THRU_ECI_FLAG_IS_REQUIRED, result.Errors.ForObject("Transaction").ForObject("Three-D-Secure-Pass-Thru").OnField("Eci-Flag")[0].Code);
        }

        [Test]
        public void Sale_ErrorWithMissingThreeDSecurePassThruCavvOrXid()
        {
            var request = new TransactionRequest
            {
                MerchantAccountId = MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID,
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ThreeDSecurePassThru = new ThreeDSecurePassThruRequest
                {
                    EciFlag = "05",
                    Cavv = "",
                    Xid = "",
                    AuthenticationResponse = "authentication_response_value",
                    DirectoryResponse = "Y",
                    CavvAlgorithm = "2",
                    ThreeDSecureVersion = "1.0.2",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Transaction transaction = result.Target;
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_THREE_D_SECURE_PASS_THRU_CAVV_IS_REQUIRED, result.Errors.ForObject("Transaction").ForObject("Three-D-Secure-Pass-Thru").OnField("Cavv")[0].Code);
        }

        [Test]
        public void Sale_ErrorWithInvalidThreeDSecurePassThruEciFlag()
        {
            var request = new TransactionRequest
            {
                MerchantAccountId = MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID,
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ThreeDSecurePassThru = new ThreeDSecurePassThruRequest
                {
                    EciFlag = "bad_eci_flag",
                    Cavv = "some_cavv",
                    Xid = "some_xid",
                    AuthenticationResponse = "authentication_response_value",
                    DirectoryResponse = "Y",
                    CavvAlgorithm = "2",
                    ThreeDSecureVersion = "1.0.2",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Transaction transaction = result.Target;
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_THREE_D_SECURE_PASS_THRU_ECI_FLAG_IS_INVALID, result.Errors.ForObject("Transaction").ForObject("Three-D-Secure-Pass-Thru").OnField("Eci-Flag")[0].Code);
        }

        [Test]
        public void Sale_ErrorWithInvalidThreeDSecureVersion()
        {
            var request = new TransactionRequest
            {
                MerchantAccountId = MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID,
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ThreeDSecurePassThru = new ThreeDSecurePassThruRequest
                {
                    EciFlag = "02",
                    Cavv = "some_cavv",
                    Xid = "some_xid",
                    AuthenticationResponse = "authentication_response_value",
                    DirectoryResponse = "Y",
                    CavvAlgorithm = "2",
                    ThreeDSecureVersion = "invalid",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Transaction transaction = result.Target;
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_THREE_D_SECURE_PASS_THRU_THREE_D_SECURE_VERSION_IS_INVALID, result.Errors.ForObject("Transaction").ForObject("Three-D-Secure-Pass-Thru").OnField("Three-D-Secure-Version")[0].Code);
        }

        [Test]
        public void Sale_ErrorWithThreeDSecurePassThruWhenDirectoryResponseIsInvalid()
        {
            var request = new TransactionRequest
            {
                // Currently we only validate some of the pass thru fields if processor is Adyen.
                MerchantAccountId = MerchantAccountIDs.ADYEN_MERCHANT_ACCOUNT_ID,
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ThreeDSecurePassThru = new ThreeDSecurePassThruRequest
                {
                    EciFlag = "02",
                    Cavv = "some_cavv",
                    Xid = "some_xid",
                    AuthenticationResponse = "authenticate_successful",
                    DirectoryResponse = "",
                    CavvAlgorithm = "2",
                    ThreeDSecureVersion = "1.0.2",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Transaction transaction = result.Target;
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_THREE_D_SECURE_PASS_THRU_DIRECTORY_RESPONSE_IS_INVALID, result.Errors.ForObject("Transaction").ForObject("Three-D-Secure-Pass-Thru").OnField("Directory-Response")[0].Code);
        }
        [Test]
        public void Sale_ErrorWithThreeDSecureCavvAlgorithmIsInvalid()
        {
            var request = new TransactionRequest
            {
                // Currently we only validate some of the pass thru fields if processor is Adyen.
                MerchantAccountId = MerchantAccountIDs.ADYEN_MERCHANT_ACCOUNT_ID,
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ThreeDSecurePassThru = new ThreeDSecurePassThruRequest
                {
                    EciFlag = "02",
                    Cavv = "some_cavv",
                    Xid = "some_xid",
                    AuthenticationResponse = "authenticate_successful",
                    DirectoryResponse = "Y",
                    CavvAlgorithm = "INVALID",
                    ThreeDSecureVersion = "1.0.2",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Transaction transaction = result.Target;
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_THREE_D_SECURE_PASS_THRU_CAVV_ALGORITHM_IS_INVALID, result.Errors.ForObject("Transaction").ForObject("Three-D-Secure-Pass-Thru").OnField("Cavv-Algorithm")[0].Code);
        }

        [Test]
        public void Sale_WithApplePayNonce()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.ApplePayAmex
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Assert.IsNotNull(result.Target.ApplePayDetails.Bin);
            Assert.IsNotNull(result.Target.ApplePayDetails.Business);
            Assert.IsNotNull(result.Target.ApplePayDetails.CardholderName);
            Assert.IsNotNull(result.Target.ApplePayDetails.CardType);
            Assert.IsNotNull(result.Target.ApplePayDetails.Commercial);
            Assert.IsNotNull(result.Target.ApplePayDetails.Consumer);
            Assert.IsNotNull(result.Target.ApplePayDetails.Corporate);
            Assert.IsNotNull(result.Target.ApplePayDetails.CountryOfIssuance);
            Assert.IsNotNull(result.Target.ApplePayDetails.Debit);
            Assert.IsNotNull(result.Target.ApplePayDetails.DurbinRegulated);
            Assert.IsNotNull(result.Target.ApplePayDetails.ExpirationMonth);
            Assert.IsNotNull(result.Target.ApplePayDetails.ExpirationYear);
            Assert.IsNotNull(result.Target.ApplePayDetails.Healthcare);
            Assert.IsNotNull(result.Target.ApplePayDetails.ImageUrl);
            Assert.IsNotNull(result.Target.ApplePayDetails.IssuingBank);
            Assert.IsNotNull(result.Target.ApplePayDetails.LastFour);
            Assert.IsNotNull(result.Target.ApplePayDetails.PaymentInstrumentName);
            Assert.IsNotNull(result.Target.ApplePayDetails.Payroll);
            Assert.IsNotNull(result.Target.ApplePayDetails.Prepaid);
            Assert.IsNotNull(result.Target.ApplePayDetails.PrepaidReloadable);
            Assert.IsNotNull(result.Target.ApplePayDetails.ProductId);
            Assert.IsNotNull(result.Target.ApplePayDetails.Purchase);
            Assert.IsNotNull(result.Target.ApplePayDetails.SourceDescription);
            Assert.IsNotNull(result.Target.ApplePayDetails);
        }

        [Test]
        public void Sale_WithMetaCheckoutCardNonce()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.MetaCheckoutCard
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Assert.AreEqual(PaymentInstrumentType.META_CHECKOUT_CARD, result.Target.PaymentInstrumentType);
            Assert.IsNotNull(result.Target.MetaCheckoutCardDetails);
            Assert.IsNotNull(result.Target.MetaCheckoutCardDetails.Bin);
            Assert.IsNotNull(result.Target.MetaCheckoutCardDetails.Business);
            Assert.IsNotNull(result.Target.MetaCheckoutCardDetails.CardholderName);
            Assert.IsNotNull(result.Target.MetaCheckoutCardDetails.CardType);
            Assert.IsNotNull(result.Target.MetaCheckoutCardDetails.Commercial);
            Assert.IsNotNull(result.Target.MetaCheckoutCardDetails.Consumer);
            Assert.IsNotNull(result.Target.MetaCheckoutCardDetails.Corporate);
            Assert.IsNotNull(result.Target.MetaCheckoutCardDetails.Debit);
            Assert.IsNotNull(result.Target.MetaCheckoutCardDetails.DurbinRegulated);
            Assert.IsNotNull(result.Target.MetaCheckoutCardDetails.ExpirationMonth);
            Assert.IsNotNull(result.Target.MetaCheckoutCardDetails.ExpirationYear);
            Assert.IsNotNull(result.Target.MetaCheckoutCardDetails.Healthcare);
            Assert.IsNotNull(result.Target.MetaCheckoutCardDetails.ImageUrl);
            Assert.IsNotNull(result.Target.MetaCheckoutCardDetails.LastFour);
            Assert.IsNotNull(result.Target.MetaCheckoutCardDetails.Payroll);
            Assert.IsNotNull(result.Target.MetaCheckoutCardDetails.Prepaid);
            Assert.IsNotNull(result.Target.MetaCheckoutCardDetails.PrepaidReloadable);
            Assert.IsNotNull(result.Target.MetaCheckoutCardDetails.ProductId);
            Assert.IsNotNull(result.Target.MetaCheckoutCardDetails.Purchase);
        }

        [Test]
        public void Sale_WithMetaCheckoutTokenNonce()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.MetaCheckoutToken
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Assert.AreEqual(PaymentInstrumentType.META_CHECKOUT_TOKEN, result.Target.PaymentInstrumentType);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.Bin);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.Business);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.CardholderName);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.CardType);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.Commercial);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.Consumer);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.Corporate);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.Cryptogram);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.Debit);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.DurbinRegulated);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.ECommerceIndicator);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.ExpirationMonth);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.ExpirationYear);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.Healthcare);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.ImageUrl);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.LastFour);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.Payroll);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.Prepaid);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.PrepaidReloadable);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.ProductId);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails.Purchase);
            Assert.IsNotNull(result.Target.MetaCheckoutTokenDetails);
        }

        [Test]
        public void Sale_WithApplePayParams()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                ApplePayCard = new TransactionApplePayCardRequest
                {
                    Number = "4111111111111111",
                    CardholderName = "Dan Schulman",
                    Cryptogram = "AAAAAAAA/COBt84dnIEcwAA3gAAGhgEDoLABAAhAgAABAAAALnNCLw==",
                    ExpirationMonth = "05",
                    ExpirationYear = "10",
                    EciIndicator = "07"
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Assert.IsNotNull(result.Target.ApplePayDetails);
            Assert.AreEqual("Dan Schulman", result.Target.ApplePayDetails.CardholderName);
            Assert.AreEqual("05", result.Target.ApplePayDetails.ExpirationMonth);
            Assert.AreEqual("2010", result.Target.ApplePayDetails.ExpirationYear);
        }

        [Test]
        public void Sale_WithAndroidPayProxyCardNonce()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.AndroidPay
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Assert.IsNotNull(result.Target.AndroidPayDetails);

            Assert.IsInstanceOf(typeof(AndroidPayDetails), result.Target.AndroidPayDetails);
            AndroidPayDetails androidPayDetails = (AndroidPayDetails)result.Target.AndroidPayDetails;

            Assert.IsFalse(androidPayDetails.IsNetworkTokenized);
            Assert.IsNotNull(androidPayDetails.Bin);
            Assert.IsNotNull(androidPayDetails.Business);
            Assert.IsNotNull(androidPayDetails.CardType);
            Assert.IsNotNull(androidPayDetails.Commercial);
            Assert.IsNotNull(androidPayDetails.Consumer);
            Assert.IsNotNull(androidPayDetails.Corporate);
            Assert.IsNotNull(androidPayDetails.CountryOfIssuance);
            Assert.IsNotNull(androidPayDetails.Debit);
            Assert.IsNotNull(androidPayDetails.DurbinRegulated);
            Assert.IsNotNull(androidPayDetails.ExpirationMonth);
            Assert.IsNotNull(androidPayDetails.ExpirationYear);
            Assert.IsNotNull(androidPayDetails.GoogleTransactionId);
            Assert.IsNotNull(androidPayDetails.Healthcare);
            Assert.IsNotNull(androidPayDetails.ImageUrl);
            Assert.IsNotNull(androidPayDetails.IssuingBank);
            Assert.IsNotNull(androidPayDetails.Last4);
            Assert.IsNotNull(androidPayDetails.Payroll);
            Assert.IsNotNull(androidPayDetails.Prepaid);
            Assert.IsNotNull(androidPayDetails.PrepaidReloadable);
            Assert.IsNotNull(androidPayDetails.ProductId);
            Assert.IsNotNull(androidPayDetails.Purchase);
            Assert.IsNotNull(androidPayDetails.SourceCardLast4);
            Assert.IsNotNull(androidPayDetails.SourceCardType);
            Assert.IsNotNull(androidPayDetails.SourceDescription);
            Assert.IsNotNull(androidPayDetails.VirtualCardLast4);
            Assert.IsNotNull(androidPayDetails.VirtualCardType);
            Assert.IsNull(androidPayDetails.Token);
        }

        [Test]
        public void Sale_WithAndroidPayNetworkTokenNonce()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.AndroidPayDiscover
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Assert.IsNotNull(result.Target.AndroidPayDetails);

            Assert.IsInstanceOf(typeof(AndroidPayDetails), result.Target.AndroidPayDetails);
            AndroidPayDetails androidPayDetails = result.Target.AndroidPayDetails;

            Assert.IsFalse(androidPayDetails.IsNetworkTokenized);
            Assert.IsNotNull(androidPayDetails.Bin);
            Assert.IsNotNull(androidPayDetails.Business);
            Assert.IsNotNull(androidPayDetails.CardType);
            Assert.IsNotNull(androidPayDetails.Commercial);
            Assert.IsNotNull(androidPayDetails.Consumer);
            Assert.IsNotNull(androidPayDetails.Corporate);
            Assert.IsNotNull(androidPayDetails.CountryOfIssuance);
            Assert.IsNotNull(androidPayDetails.Debit);
            Assert.IsNotNull(androidPayDetails.DurbinRegulated);
            Assert.IsNotNull(androidPayDetails.ExpirationMonth);
            Assert.IsNotNull(androidPayDetails.ExpirationYear);
            Assert.IsNotNull(androidPayDetails.GoogleTransactionId);
            Assert.IsNotNull(androidPayDetails.Healthcare);
            Assert.IsNotNull(androidPayDetails.ImageUrl);
            Assert.IsNotNull(androidPayDetails.IssuingBank);
            Assert.IsNotNull(androidPayDetails.Last4);
            Assert.IsNotNull(androidPayDetails.Payroll);
            Assert.IsNotNull(androidPayDetails.Prepaid);
            Assert.IsNotNull(androidPayDetails.PrepaidReloadable);
            Assert.IsNotNull(androidPayDetails.ProductId);
            Assert.IsNotNull(androidPayDetails.Purchase);
            Assert.IsNotNull(androidPayDetails.SourceCardLast4);
            Assert.IsNotNull(androidPayDetails.SourceCardType);
            Assert.IsNotNull(androidPayDetails.SourceDescription);
            Assert.IsNotNull(androidPayDetails.VirtualCardLast4);
            Assert.IsNotNull(androidPayDetails.VirtualCardType);
            Assert.IsNull(androidPayDetails.Token);
        }

        [Test]
        public void Sale_WithGooglePayParams()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                AndroidPayCard = new TransactionAndroidPayCardRequest
                {
                    Cryptogram = "AAAAAAAA/COBt84dnIEcwAA3gAAGhgEDoLABAAhAgAABAAAALnNCLw==",
                    EciIndicator = "05",
                    ExpirationMonth = "10",
                    ExpirationYear = "14",
                    GoogleTransactionId = "25469d622c1dd37cb1a403c6d438e850",
                    Number = "4012888888881881",
                    SourceCardLastFour = "1881",
                    SourceCardType = "Visa"
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(PaymentInstrumentType.ANDROID_PAY_CARD, result.Target.PaymentInstrumentType);

            Assert.IsNotNull(result.Target.AndroidPayDetails);

            Assert.IsInstanceOf(typeof(AndroidPayDetails), result.Target.AndroidPayDetails);
            AndroidPayDetails androidPayDetails = result.Target.AndroidPayDetails;

            Assert.AreEqual("Visa", androidPayDetails.SourceCardType);
            Assert.AreEqual("1881", androidPayDetails.SourceCardLast4);
            Assert.AreEqual("10", androidPayDetails.ExpirationMonth);
            Assert.AreEqual("14", androidPayDetails.ExpirationYear);
            Assert.AreEqual("25469d622c1dd37cb1a403c6d438e850", androidPayDetails.GoogleTransactionId);
        }

        [Test]
        public void Sale_WithVenmoAccountNonce()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.FAKE_VENMO_ACCOUNT_MERCHANT_ACCOUNT_ID,
                PaymentMethodNonce = Nonce.VenmoAccount
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Assert.AreEqual(result.Target.PaymentInstrumentType, PaymentInstrumentType.VENMO_ACCOUNT);
            Assert.IsNotNull(result.Target.VenmoAccountDetails);

            VenmoAccountDetails venmoAccountDetails = result.Target.VenmoAccountDetails;

            Assert.IsNull(venmoAccountDetails.Token);
            Assert.IsNotNull(venmoAccountDetails.Username);
            Assert.IsNotNull(venmoAccountDetails.VenmoUserId);
            Assert.IsNotNull(venmoAccountDetails.ImageUrl);
            Assert.IsNotNull(venmoAccountDetails.SourceDescription);
        }

        [Test]
        public void Sale_WithVenmoAccountNonceAndProfileId()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.FAKE_VENMO_ACCOUNT_MERCHANT_ACCOUNT_ID,
                PaymentMethodNonce = Nonce.VenmoAccount,
                Options = new TransactionOptionsRequest
                {
                    Venmo = new TransactionOptionsVenmoRequest
                    {
                        ProfileId = "integration_venmo_merchant_public_id"
                    }
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        [Ignore("Flaky test. See: https://paypal.slack.com/archives/C03P4S18JBT/p1741207572015819 for more details.")]
        public void Sale_WithUsBankAccountNonce()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID,
                PaymentMethodNonce = TestHelper.GenerateValidUsBankAccountNonce(gateway),
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Assert.AreEqual(PaymentInstrumentType.US_BANK_ACCOUNT, result.Target.PaymentInstrumentType);
            Assert.IsNotNull(result.Target.UsBankAccountDetails);

            UsBankAccountDetails usBankAccountDetails = result.Target.UsBankAccountDetails;
            Assert.IsNull(usBankAccountDetails.Token);

            Assert.AreEqual("021000021", usBankAccountDetails.RoutingNumber);
            Assert.AreEqual("0000", usBankAccountDetails.Last4);
            Assert.AreEqual("checking", usBankAccountDetails.AccountType);
            Assert.AreEqual("Dan Schulman", usBankAccountDetails.AccountHolderName);
            Assert.IsTrue(Regex.IsMatch(usBankAccountDetails.BankName, ".*CHASE.*"));
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
            Assert.AreEqual(ProcessorResponseType.SOFT_DECLINED, transaction.ProcessorResponseType);
            Assert.AreEqual("2000", transaction.ProcessorResponseCode);
            Assert.AreEqual("2000 : Do Not Honor", transaction.AdditionalProcessorResponse);
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
        public void Sale_HardDeclined()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.HARD_DECLINE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009"
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Transaction transaction = result.Transaction;

            Assert.AreEqual(2015.00, transaction.Amount);
            Assert.AreEqual(TransactionStatus.PROCESSOR_DECLINED, transaction.Status);
            Assert.AreEqual(ProcessorResponseType.HARD_DECLINED, transaction.ProcessorResponseType);
            Assert.AreEqual("2015", transaction.ProcessorResponseCode);
            Assert.AreEqual("2015 : Transaction Not Allowed", transaction.AdditionalProcessorResponse);
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
        public void Sale_GatewayRejectedForApplicationIncomplete()
        {
            BraintreeGateway oauthGateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );

            ResultImpl<Merchant> merchantResult = oauthGateway.Merchant.Create(new MerchantRequest
            {
                Email = "name@email.com",
                CountryCodeAlpha3 = "GBR",
                PaymentMethods = new string[] { "credit_card", "paypal" }
            });

            gateway = new BraintreeGateway(merchantResult.Target.Credentials.AccessToken);

            var request = new TransactionRequest
            {
                Amount = 4000.00M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2020"
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Transaction transaction = result.Transaction;

            Assert.AreEqual(TransactionGatewayRejectionReason.APPLICATION_INCOMPLETE, transaction.GatewayRejectionReason);
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
        [Ignore("Marking this test case as pending as we revoking category 2, 3 and 4")]
        public void Sale_GatewayRejectedForExcessiveRetry()
        {
            DuplicateCheckingSetup();
            var excessiveRetry = false;
            var counter = 0;
            Result<Braintree.Transaction> result = null;
            Transaction transaction = null;
            while ((excessiveRetry == false) && (counter < 25))
            {
                var request = new TransactionRequest
                {
                    Amount = SandboxValues.TransactionAmount.DECLINE,
                    CreditCard = new TransactionCreditCardRequest
                    {
                        Number = SandboxValues.CreditCardNumber.VISA,
                        ExpirationDate = "05/2017",
                        CVV = "333"
                    }
                };
                result = gateway.Transaction.Sale(request);
                transaction = result.Transaction;
                excessiveRetry = (result.Transaction.Status == TransactionStatus.GATEWAY_REJECTED);
                counter += 1;
            }


            Assert.AreEqual(TransactionGatewayRejectionReason.EXCESSIVE_RETRY, transaction.GatewayRejectionReason);
        }

        [Test]
        public void Sale_GatewayRejectedForFraud()
        {
            AdvancedKountFraudSetup();
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

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Transaction transaction = result.Transaction;

            Assert.AreEqual(TransactionGatewayRejectionReason.FRAUD, transaction.GatewayRejectionReason);
        }

        [Test]
        public void Sale_GatewayRejectedForRiskThresholds()
        {
            AdvancedKountFraudSetup();
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.RISK_THRESHOLD,
                    ExpirationDate = "05/2017",
                    CVV = "333"
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Transaction transaction = result.Transaction;

            Assert.AreEqual(TransactionGatewayRejectionReason.RISK_THRESHOLD, transaction.GatewayRejectionReason);
        }

        [Test]
        public void Sale_GatewayRejectedForTokenIssuance()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.FAKE_VENMO_ACCOUNT_MERCHANT_ACCOUNT_ID,
                PaymentMethodNonce = Nonce.GatewayRejectedTokenIssuance
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Transaction transaction = result.Transaction;

            TestContext.WriteLine(transaction.GatewayRejectionReason);
            Assert.AreEqual(TransactionGatewayRejectionReason.TOKEN_ISSUANCE, transaction.GatewayRejectionReason);
        }

        [Test]
        public void Sale_WithCustomFields()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CustomFields = new Dictionary<string, string>
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
                CustomFields = new Dictionary<string, string>
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
                    CountryCodeNumeric = "000",
                    PhoneNumber = "122-555-1237-123456"
                },
                ShippingAddress = new AddressRequest
                {
                    CountryName = "zzzzz",
                    CountryCodeAlpha2 = "zz",
                    CountryCodeAlpha3 = "zzz",
                    CountryCodeNumeric = "000",
                    PhoneNumber = "122-555-1236-123456"
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
            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_BILLING_PHONE_NUMBER_IS_INVALID,
                result.Errors.ForObject("Transaction").ForObject("Billing").OnField("PhoneNumber")[0].Code
            );

            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_COUNTRY_CODE_ALPHA2_IS_NOT_ACCEPTED,
                result.Errors.ForObject("Transaction").ForObject("Shipping").OnField("CountryCodeAlpha2")[0].Code
            );
            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_COUNTRY_CODE_ALPHA3_IS_NOT_ACCEPTED,
                result.Errors.ForObject("Transaction").ForObject("Shipping").OnField("CountryCodeAlpha3")[0].Code
            );
            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_COUNTRY_CODE_NUMERIC_IS_NOT_ACCEPTED,
                result.Errors.ForObject("Transaction").ForObject("Shipping").OnField("CountryCodeNumeric")[0].Code
            );
            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_COUNTRY_NAME_IS_NOT_ACCEPTED,
                result.Errors.ForObject("Transaction").ForObject("Shipping").OnField("CountryName")[0].Code
            );
            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_SHIPPING_PHONE_NUMBER_IS_INVALID,
                result.Errors.ForObject("Transaction").ForObject("Shipping").OnField("PhoneNumber")[0].Code
            );

            Dictionary<string, string> parameters = result.Parameters;
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
        public void Sale_WithLevel2ValidationsWithZeroTax()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                TaxExempt = true,
                TaxAmount = 0M,
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
            Assert.AreEqual(0.00, transaction.TaxAmount.Value);
            Assert.AreEqual("12345", transaction.PurchaseOrderNumber);
        }

  
        [Test]
        public void Sale_WithLodgingIndustryData()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                Industry = new IndustryRequest
                {
                    IndustryType = TransactionIndustryType.LODGING,
                    IndustryData = new IndustryDataRequest
                    {
                        FolioNumber = "aaa",
                        CheckInDate = "2014-07-07",
                        CheckOutDate = "2014-07-11",
                        RoomRate = 170.00M,
                        RoomTax = 30.00M,
                        NoShow = false,
                        AdvancedDeposit = false,
                        FireSafe = true,
                        PropertyPhone = "1112223345",
                        AdditionalCharges = new IndustryDataAdditionalChargeRequest[]
                        {
                            new IndustryDataAdditionalChargeRequest
                            {
                                AdditionalChargeKind = IndustryDataAdditionalChargeKind.MINI_BAR,
                                Amount = 50.00M
                            },
                            new IndustryDataAdditionalChargeRequest
                            {
                                AdditionalChargeKind = IndustryDataAdditionalChargeKind.OTHER,
                                Amount = 150.00M
                            }
                        }
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void Sale_WithLodgingIndustryDataValidation()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                Industry = new IndustryRequest
                {
                    IndustryType = TransactionIndustryType.LODGING,
                    IndustryData = new IndustryDataRequest
                    {
                        FolioNumber = "aaa",
                        CheckInDate = "2014-07-07",
                        CheckOutDate = "2014-06-06",
                        AdditionalCharges = new IndustryDataAdditionalChargeRequest[]
                        {
                            new IndustryDataAdditionalChargeRequest
                            {
                                AdditionalChargeKind = IndustryDataAdditionalChargeKind.OTHER,
                                Amount = 0M
                            }
                        }
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.INDUSTRY_DATA_LODGING_CHECK_OUT_DATE_MUST_FOLLOW_CHECK_IN_DATE,
                result.Errors.ForObject("Transaction").ForObject("Industry").OnField("CheckOutDate")[0].Code
            );

            Assert.AreEqual(
                ValidationErrorCode.INDUSTRY_DATA_ADDITIONAL_CHARGE_AMOUNT_MUST_BE_GREATER_THAN_ZERO,
                result.Errors.ForObject("Transaction").ForObject("Industry").ForObject("AdditionalCharges").ForObject("index_0").OnField("Amount")[0].Code
            );
        }

        [Test]
        public void Sale_WithTravelCruiseIndustryData()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                Industry = new IndustryRequest
                {
                    IndustryType = TransactionIndustryType.TRAVEL_AND_CRUISE,
                    IndustryData = new IndustryDataRequest
                    {
                        TravelPackage = "flight",
                        DepartureDate = "2014-07-07",
                        LodgingCheckInDate = "2014-07-07",
                        LodgingCheckOutDate = "2014-08-08",
                        LodgingName = "Lodgy Lodge",
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void Sale_WithTravelCruiseIndustryDataValidation()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                Industry = new IndustryRequest
                {
                    IndustryType = TransactionIndustryType.TRAVEL_AND_CRUISE,
                    IndustryData = new IndustryDataRequest
                    {
                        TravelPackage = "foot",
                        DepartureDate = "2014-07-07",
                        LodgingCheckInDate = "2014-07-07",
                        LodgingCheckOutDate = "2014-08-08",
                        LodgingName = "Lodgy Lodge",
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.INDUSTRY_DATA_TRAVEL_CRUISE_TRAVEL_PACKAGE_IS_INVALID,
                result.Errors.ForObject("Transaction").ForObject("Industry").OnField("TravelPackage")[0].Code
            );
        }

        [Test]
        public void Sale_WithTravelFlightIndustryData_ReturnsSuccessfulResponse()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.PayPalOneTimePayment,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                },
                Industry = new IndustryRequest
                {
                    IndustryType = TransactionIndustryType.TRAVEL_AND_FLIGHT,
                    IndustryData = new IndustryDataRequest
                    {
                        PassengerFirstName = "John",
                        PassengerLastName = "Doe",
                        PassengerMiddleInitial = "M",
                        PassengerTitle = "Mr.",
                        IssuedDate = new DateTime(2018, 1, 1),
                        TravelAgencyName = "Expedia",
                        TravelAgencyCode = "12345678",
                        TicketNumber = "ticket-number",
                        IssuingCarrierCode = "AA",
                        CustomerCode = "customer-code",
                        FareAmount = 7000M,
                        FeeAmount = 1000M,
                        TaxAmount = 2000M,
                        RestrictedTicket = false,
                        ArrivalDate = new DateTime(2018, 1, 1),
                        TicketIssuerAddress = "tkt-issuer-address",
                        DateOfBirth = "2012-12-12",
                        CountryCode = "US",
                        Legs = new IndustryDataLegRequest[]
                        {
                            new IndustryDataLegRequest
                            {
                                ConjunctionTicket = "CJ0001",
                                ExchangeTicket = "ET0001",
                                CouponNumber = "1",
                                ServiceClass = "Y",
                                CarrierCode = "AA",
                                FareBasisCode = "W",
                                FlightNumber = "AA100",
                                DepartureDate = new DateTime(2018, 1, 2),
                                DepartureAirportCode = "MDW",
                                DepartureTime = "08:00",
                                ArrivalAirportCode = "ATX",
                                ArrivalTime = "10:00",
                                StopoverPermitted = false,
                                FareAmount = 3500M,
                                FeeAmount = 500M,
                                TaxAmount = 1000M,
                                EndorsementOrRestrictions = "NOT REFUNDABLE",
                            },
                            new IndustryDataLegRequest
                            {
                                ConjunctionTicket = "CJ0002",
                                ExchangeTicket = "ET0002",
                                CouponNumber = "1",
                                ServiceClass = "Y",
                                CarrierCode = "AA",
                                FareBasisCode = "W",
                                FlightNumber = "AA200",
                                DepartureDate = new DateTime(2018, 1, 3),
                                DepartureAirportCode = "ATX",
                                DepartureTime = "12:00",
                                ArrivalAirportCode = "MDW",
                                ArrivalTime = "14:00",
                                StopoverPermitted = false,
                                FareAmount = 3500M,
                                FeeAmount = 500M,
                                TaxAmount = 1000M,
                                EndorsementOrRestrictions = "NOT REFUNDABLE",
                            }
                        }
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void Sale_WithTravelFlightIndustryDataValidation_ReturnsValidationErrorResponse()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.PayPalOneTimePayment,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                },
                Industry = new IndustryRequest
                {
                    IndustryType = TransactionIndustryType.TRAVEL_AND_FLIGHT,
                    IndustryData = new IndustryDataRequest
                    {
                        FareAmount = -1.23M,
                        Legs = new IndustryDataLegRequest[]
                        {
                            new IndustryDataLegRequest
                            {
                                FareAmount = -1.23M,
                            },
                        },
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.INDUSTRY_DATA_TRAVEL_FLIGHT_FARE_AMOUNT_CANNOT_BE_NEGATIVE,
                result.Errors.ForObject("Transaction").ForObject("Industry").OnField("FareAmount")[0].Code
            );
            Assert.AreEqual(
                ValidationErrorCode.INDUSTRY_DATA_LEG_TRAVEL_FLIGHT_FARE_AMOUNT_CANNOT_BE_NEGATIVE,
                result.Errors.ForObject("Transaction").ForObject("Industry").ForObject("Legs").ForObject("index_0").OnField("FareAmount")[0].Code
            );
        }

        [Test]
        public void SubmitForSettlement_WithTravelFlightIndustryDataCreditCard_ReturnsSuccessfulResponse()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.FAKE_FIRST_DATA_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2008"
                }
            };
            Transaction transaction = gateway.Transaction.Sale(request).Target;

            TransactionRequest submitForSettlementRequest = new TransactionRequest
            {
                Industry = new IndustryRequest
                {
                    IndustryType = TransactionIndustryType.TRAVEL_AND_FLIGHT,
                    IndustryData = new IndustryDataRequest
                    {
                        PassengerFirstName = "John",
                        PassengerLastName = "Doe",
                        PassengerMiddleInitial = "M",
                        PassengerTitle = "Mr.",
                        IssuedDate = new DateTime(2018, 1, 1),
                        TravelAgencyName = "Expedia",
                        TravelAgencyCode = "12345678",
                        TicketNumber = "ticket-number",
                        IssuingCarrierCode = "AA",
                        CustomerCode = "customer-code",
                        FareAmount = 7000M,
                        FeeAmount = 1000M,
                        TaxAmount = 2000M,
                        RestrictedTicket = false,
                        ArrivalDate = new DateTime(2018, 1, 1),
                        TicketIssuerAddress = "tkt-issuer-address",
                        DateOfBirth = "2012-12-12",
                        CountryCode = "US",
                        Legs = new IndustryDataLegRequest[]
                        {
                            new IndustryDataLegRequest
                            {
                                ConjunctionTicket = "CJ0001",
                                ExchangeTicket = "ET0001",
                                CouponNumber = "1",
                                ServiceClass = "Y",
                                CarrierCode = "AA",
                                FareBasisCode = "W",
                                FlightNumber = "AA100",
                                DepartureDate = new DateTime(2018, 1, 2),
                                DepartureAirportCode = "MDW",
                                DepartureTime = "08:00",
                                ArrivalAirportCode = "ATX",
                                ArrivalTime = "10:00",
                                StopoverPermitted = false,
                                FareAmount = 3500M,
                                FeeAmount = 500M,
                                TaxAmount = 1000M,
                                EndorsementOrRestrictions = "NOT REFUNDABLE",
                            },
                            new IndustryDataLegRequest
                            {
                                ConjunctionTicket = "CJ0002",
                                ExchangeTicket = "ET0002",
                                CouponNumber = "1",
                                ServiceClass = "Y",
                                CarrierCode = "AA",
                                FareBasisCode = "W",
                                FlightNumber = "AA200",
                                DepartureDate = new DateTime(2018, 1, 3),
                                DepartureAirportCode = "ATX",
                                DepartureTime = "12:00",
                                ArrivalAirportCode = "MDW",
                                ArrivalTime = "14:00",
                                StopoverPermitted = false,
                                FareAmount = 3500M,
                                FeeAmount = 500M,
                                TaxAmount = 1000M,
                                EndorsementOrRestrictions = "NOT REFUNDABLE",
                            }
                        }
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.SubmitForSettlement(transaction.Id, submitForSettlementRequest);

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, result.Target.Status);
        }

        [Test]
        public void Sale_WithAdvancedKountFraudCheckingSkipped()
        {
            AdvancedKountFraudSetup();
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2016",
                },
                Options = new TransactionOptionsRequest
                {
                    SkipAdvancedFraudChecking = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Transaction transaction = result.Target;
            Assert.Null(transaction.RiskData);
        }

        [Test]
        public void Sale_WithProcessingOverrides()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2016",
                },
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true,
                    ProcessingOverrides = new TransactionOptionsProcessingOverridesRequest
                    {
                        CustomerEmail = "dotnetSDK@example.com",
                        CustomerFirstName = "dotnetSDK_test_first_name",
                        CustomerLastName = "dotnetSDK_test_last_name",
                        CustomerTaxIdentifier = "1.2.3.4.5.6"
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Transaction transaction = result.Target;
            Assert.AreEqual(TransactionType.SALE, transaction.Type);
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, transaction.Status);
        }

        [Test]
        public void Sale_WithSkipAvsOptionSet()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2019",
                },
                Options = new TransactionOptionsRequest
                {
                    SkipAvs = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Transaction transaction = result.Target;
            Assert.Null(transaction.AvsErrorResponseCode);
            Assert.AreEqual("B", transaction.AvsStreetAddressResponseCode);
        }

        [Test]
        public void Sale_WithSkipCvvOptionSet()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2019",
                },
                Options = new TransactionOptionsRequest
                {
                    SkipCvv = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Transaction transaction = result.Target;
            Assert.AreEqual("B", transaction.CvvResponseCode);
        }

        [Test]
        public void Sale_DeserializesRetrievalReferenceNumber()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.IsNotNull(transaction.RetrievalReferenceNumber);
        }

        [Test]
        public void Sale_WithVisaReturnsNetworkTransactionIdentifier()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.IsNotNull(transaction.NetworkTransactionId);
        }

        [Test]
        public void Sale_WithAmexReturnsNetworkTransactionIdentifier()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.AMEX,
                    ExpirationDate = "05/2009",
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.IsNotNull(transaction.NetworkTransactionId);
        }

        [Test]
        public void Sale_WithJCBReturnsNetworkTransactionIdentifier()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.JCB,
                    ExpirationDate = "05/2009",
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.IsNotNull(transaction.NetworkTransactionId);
        }

        [Test]
        public void Sale_VisaWithExternalVaultStatus_ReturnsSuccessfulResponse()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ExternalVault = new ExternalVaultRequest
                {
                    Status = "will_vault",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;
            Assert.IsNotNull(transaction.NetworkTransactionId);
        }

        [Test]
        public void Sale_AmexWithExternalVaultStatus_ReturnsSuccessfulResponse()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.AMEX,
                    ExpirationDate = "05/2009",
                },
                ExternalVault = new ExternalVaultRequest
                {
                    Status = "will_vault",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;
            Assert.IsNotNull(transaction.NetworkTransactionId);
        }

        [Test]
        public void Sale_AmexWithNullExternalVaultPreviousNetworkTransactionId_ReturnsSuccessfulResponse()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.AMEX,
                    ExpirationDate = "05/2009",
                },
                ExternalVault = new ExternalVaultRequest
                {
                    Status = "will_vault",
                    PreviousNetworkTransactionId = null,
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;
            Assert.IsNotNull(transaction.NetworkTransactionId);
        }

        [Test]
        public void Sale_WithExternalVaultPreviousNetworkTransactionId_ReturnsSuccessfulResponse()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ExternalVault = new ExternalVaultRequest
                {
                    Status = "vaulted",
                    PreviousNetworkTransactionId = "123456789012345",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;
            Assert.IsNotNull(transaction.NetworkTransactionId);
        }

        [Test]
        public void Sale_WithExternalVaultStatusVaultedWithoutPreviousNetworkTransactionId_ReturnsSuccessfulResponse()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ExternalVault = new ExternalVaultRequest
                {
                    Status = "vaulted",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;
            Assert.IsNotNull(transaction.NetworkTransactionId);
        }

        [Test]
        public void Sale_WithExternalVault_ValidationErrorStatusIsInvalid()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ExternalVault = new ExternalVaultRequest
                {
                    Status = "bad value",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_EXTERNAL_VAULT_STATUS_IS_INVALID,
                result.Errors.ForObject("Transaction").ForObject("ExternalVault").OnField("Status")[0].Code
            );
        }

        [Test]
        public void Sale_WithExternalVault_ValidationErrorStatusWithPreviousNetworkTransactionIdIsInvalid()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ExternalVault = new ExternalVaultRequest
                {
                    Status = "will_vault",
                    PreviousNetworkTransactionId = "123456789012345",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_EXTERNAL_VAULT_STATUS_WITH_PREVIOUS_NETWORK_TRANSACTION_ID_IS_INVALID,
                result.Errors.ForObject("Transaction").ForObject("ExternalVault").OnField("Status")[0].Code
            );
        }

        [Test]
        public void Sale_HiperWithAccountTypeCredit_ReturnsSuccessfulResponse()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.BRAZIL_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.HIPER,
                    ExpirationDate = "05/2009",
                },
                Options = new TransactionOptionsRequest
                {
                    CreditCard = new TransactionOptionsCreditCardRequest
                    {
                        AccountType = "credit"
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;
            Assert.AreEqual("credit", transaction.CreditCard.AccountType);
        }

        [Test]
        public void Sale_HiperWithAccountTypeDebit_ReturnsSuccessfulResponse()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.BRAZIL_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.HIPER,
                    ExpirationDate = "05/2009",
                },
                Options = new TransactionOptionsRequest
                {
                    CreditCard = new TransactionOptionsCreditCardRequest
                    {
                        AccountType = "debit"
                    },
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;
            Assert.AreEqual("debit", transaction.CreditCard.AccountType);
        }

        [Test]
        public void Sale_HiperWithAccountType_ReturnsErrorAccountTypeInvalid()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.BRAZIL_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.HIPER,
                    ExpirationDate = "05/2009",
                },
                Options = new TransactionOptionsRequest
                {
                    CreditCard = new TransactionOptionsCreditCardRequest
                    {
                        AccountType = "ach"
                    }
                }
            };

          Result<Transaction> result = gateway.Transaction.Sale(request);
          Assert.IsFalse(result.IsSuccess());
          Assert.AreEqual(
              ValidationErrorCode.TRANSACTION_OPTIONS_CREDIT_CARD_ACCOUNT_TYPE_IS_INVALID,
              result.Errors.ForObject("Transaction").ForObject("Options").ForObject("CreditCard").OnField("AccountType")[0].Code
          );
        }

        [Test]
        public void Sale_HiperWithAccountType_ReturnsErrorAccountTypeNotSupported()
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
                    CreditCard = new TransactionOptionsCreditCardRequest
                    {
                        AccountType = "credit"
                    }
                }
            };

          Result<Transaction> result = gateway.Transaction.Sale(request);
          Assert.IsFalse(result.IsSuccess());
          Assert.AreEqual(
              result.Errors.ForObject("Transaction").ForObject("Options").ForObject("CreditCard").OnField("AccountType")[0].Code,
              ValidationErrorCode.TRANSACTION_OPTIONS_CREDIT_CARD_ACCOUNT_TYPE_NOT_SUPPORTED
          );
        }

        [Test]
        public void Sale_HiperWithAccountType_ReturnsErrorAccountDebitDoesNotSupportAuths()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.BRAZIL_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.HIPER,
                    ExpirationDate = "05/2009",
                },
                Options = new TransactionOptionsRequest
                {
                    CreditCard = new TransactionOptionsCreditCardRequest
                    {
                        AccountType = "debit"
                    }
                }
            };

          Result<Transaction> result = gateway.Transaction.Sale(request);
          Assert.IsFalse(result.IsSuccess());
          Assert.AreEqual(
              result.Errors.ForObject("Transaction").ForObject("Options").ForObject("CreditCard").OnField("AccountType")[0].Code,
              ValidationErrorCode.TRANSACTION_OPTIONS_CREDIT_CARD_ACCOUNT_TYPE_DEBIT_DOES_NOT_SUPPORT_AUTHS
          );
        }

        [Test]
        public void Sale_ReturnsErrorAmountNotSupportedByProcessor()
        {
            var request = new TransactionRequest
            {
                Amount = 0.20M,
                MerchantAccountId = MerchantAccountIDs.BRAZIL_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.HIPER,
                    ExpirationDate = "05/2009",
                },
            };

          Result<Transaction> result = gateway.Transaction.Sale(request);
          Assert.IsFalse(result.IsSuccess());
          Assert.AreEqual(
              result.Errors.ForObject("Transaction").OnField("Amount")[0].Code,
              ValidationErrorCode.TRANSACTION_AMOUNT_NOT_SUPPORTED_BY_PROCESSOR
          );
        }

     [Test]
     public void Sale_WithLevel3SummaryFields_ReturnsSuccessfulResponse()
     {
         var request = new TransactionRequest
         {
             Amount = SandboxValues.TransactionAmount.AUTHORIZE,
             CreditCard = new TransactionCreditCardRequest
             {
                 Number = SandboxValues.CreditCardNumber.VISA,
                 ExpirationDate = "05/2009",
             },
             ShippingAmount = 1.00M,
             DiscountAmount = 2.00M,
             ShipsFromPostalCode = "12345",
         };

         Result<Transaction> result = gateway.Transaction.Sale(request);
         Assert.IsTrue(result.IsSuccess());
         Transaction transaction = result.Target;

         Assert.AreEqual(1.00, transaction.ShippingAmount);
         Assert.AreEqual(2.00, transaction.DiscountAmount);
         Assert.AreEqual("12345", transaction.ShipsFromPostalCode);
     }

     [Test]
     public void Sale_WithShippingTaxAmount()
     {
         var request = new TransactionRequest
         {
             Amount = SandboxValues.TransactionAmount.AUTHORIZE,
             CreditCard = new TransactionCreditCardRequest
             {
                 Number = SandboxValues.CreditCardNumber.VISA,
                 ExpirationDate = "05/2009",
             },
             ShippingAmount = 1.00M,
             ShippingTaxAmount = 3.00M,
             DiscountAmount = 2.00M,
             ShipsFromPostalCode = "12345",
         };

         Result<Transaction> result = gateway.Transaction.Sale(request);
         Assert.IsTrue(result.IsSuccess());
         Transaction transaction = result.Target;

         Assert.AreEqual(1.00, transaction.ShippingAmount);
         Assert.AreEqual(3.00, transaction.ShippingTaxAmount);
         Assert.AreEqual(2.00, transaction.DiscountAmount);
         Assert.AreEqual("12345", transaction.ShipsFromPostalCode);
     }

      [Test]
      public void Sale_WhenDiscountAmountCannotBeNegative()
      {
          var request = new TransactionRequest
          {
              Amount = SandboxValues.TransactionAmount.AUTHORIZE,
              CreditCard = new TransactionCreditCardRequest
              {
                  Number = SandboxValues.CreditCardNumber.VISA,
                  ExpirationDate = "05/2009",
              },
              DiscountAmount = -2.00M,
          };

          Result<Transaction> result = gateway.Transaction.Sale(request);
          Assert.IsFalse(result.IsSuccess());
          Assert.AreEqual(
              ValidationErrorCode.TRANSACTION_DISCOUNT_AMOUNT_CANNOT_BE_NEGATIVE,
              result.Errors.ForObject("Transaction").OnField("DiscountAmount")[0].Code
          );
      }

        [Test]
        public void Sale_WhenDiscountAmountIsTooLarge()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                DiscountAmount = 2147483647,
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_DISCOUNT_AMOUNT_IS_TOO_LARGE,
                result.Errors.ForObject("Transaction").OnField("DiscountAmount")[0].Code
            );
        }

        [Test]
        public void Sale_WhenShippingAmountCannotBeNegative()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ShippingAmount = -1.00M,
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_SHIPPING_AMOUNT_CANNOT_BE_NEGATIVE,
                result.Errors.ForObject("Transaction").OnField("ShippingAmount")[0].Code
            );
        }

        [Test]
        public void Sale_WhenShippingAmountIsTooLarge()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ShippingAmount = 2147483647,
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_SHIPPING_AMOUNT_IS_TOO_LARGE,
                result.Errors.ForObject("Transaction").OnField("ShippingAmount")[0].Code
            );
        }

        [Test]
        public void Sale_WhenShipsFromPostalCodeIsTooLong()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ShipsFromPostalCode = "12345678901",
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_SHIPS_FROM_POSTAL_CODE_IS_TOO_LONG,
                result.Errors.ForObject("Transaction").OnField("ShipsFromPostalCode")[0].Code
            );
        }

        [Test]
        public void Sale_WhenShipsFromPostalCodeInvalidCharacters()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                ShipsFromPostalCode = "1$345",
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_SHIPS_FROM_POSTAL_CODE_INVALID_CHARACTERS,
                result.Errors.ForObject("Transaction").OnField("ShipsFromPostalCode")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsZero()
        {
            var request = new TransactionRequest
            {
                Amount = 45.15M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Transaction transaction = result.Target;

            List<TransactionLineItem> lineItems = transaction.GetLineItems();
            Assert.AreEqual(0, lineItems.Count);
        }

        [Test]
        public void Sale_WithLineItemsSingleOnlyRequiredFields()
        {
            var request = new TransactionRequest
            {
                Amount = 45.15M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        TotalAmount = 45.15M,
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Transaction transaction = result.Target;

            List<TransactionLineItem> lineItems = transaction.GetLineItems();
            Assert.AreEqual(1, lineItems.Count);

            TransactionLineItem lineItem = lineItems[0];
            Assert.AreEqual(1.0232M, lineItem.Quantity);
            Assert.AreEqual("Name #1", lineItem.Name);
            Assert.AreEqual(TransactionLineItemKind.DEBIT, lineItem.Kind);
            Assert.AreEqual(45.1232M, lineItem.UnitAmount);
            Assert.AreEqual(45.15M, lineItem.TotalAmount);
        }

        [Test]
        public void Sale_WithLineItemsSingleZeroAmounts()
        {
            var request = new TransactionRequest
            {
                Amount = 45.15M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        TotalAmount = 45.15M,
                        DiscountAmount = 0,
                        TaxAmount = 0,
                        UnitTaxAmount = 0,
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Transaction transaction = result.Target;

            List<TransactionLineItem> lineItems = transaction.GetLineItems();
            Assert.AreEqual(1, lineItems.Count);

            TransactionLineItem lineItem = lineItems[0];
            Assert.AreEqual(1.0232M, lineItem.Quantity);
            Assert.AreEqual("Name #1", lineItem.Name);
            Assert.AreEqual(TransactionLineItemKind.DEBIT, lineItem.Kind);
            Assert.AreEqual(45.1232M, lineItem.UnitAmount);
            Assert.AreEqual(45.15M, lineItem.TotalAmount);
            Assert.AreEqual(0, lineItem.DiscountAmount);
            Assert.AreEqual(0, lineItem.TaxAmount);
            Assert.AreEqual(0, lineItem.UnitTaxAmount);
        }

        [Test]
        public void Sale_WithLineItemsSingleQuantities()
        {
            var quantities = new decimal[] { 1, 1.2M, 1.23M, 1.234M, 1.2345M };
            for (var i = 0; i < quantities.Length; i++)
            {
                var request = new TransactionRequest
                {
                    Amount = 45.15M,
                    CreditCard = new TransactionCreditCardRequest
                    {
                        Number = SandboxValues.CreditCardNumber.VISA,
                        ExpirationDate = "05/2009",
                    },
                    LineItems = new TransactionLineItemRequest[]
                    {
                        new TransactionLineItemRequest
                        {
                            Quantity = quantities[i],
                            Name = "Name #1",
                            LineItemKind = TransactionLineItemKind.DEBIT,
                            UnitAmount = 45.1232M,
                            TotalAmount = 45.15M,
                        }
                    }
                };

                Result<Transaction> result = gateway.Transaction.Sale(request);
                Assert.IsTrue(result.IsSuccess());

                Transaction transaction = result.Target;

                List<TransactionLineItem> lineItems = transaction.GetLineItems();
                Assert.AreEqual(1, lineItems.Count);

                TransactionLineItem lineItem = lineItems[0];
                Assert.AreEqual(quantities[i], lineItem.Quantity);
                Assert.AreEqual("Name #1", lineItem.Name);
                Assert.AreEqual(TransactionLineItemKind.DEBIT, lineItem.Kind);
                Assert.AreEqual(45.1232M, lineItem.UnitAmount);
                Assert.AreEqual(45.15M, lineItem.TotalAmount);
            }
        }

        [Test]
        public void Sale_WithLineItemsSingleUnitAmounts()
        {
            var unitAmounts = new decimal[] { 1, 1.2M, 1.23M, 1.234M, 1.2345M };
            for (var i = 0; i < unitAmounts.Length; i++)
            {
                var request = new TransactionRequest
                {
                    Amount = 45.15M,
                    CreditCard = new TransactionCreditCardRequest
                    {
                        Number = SandboxValues.CreditCardNumber.VISA,
                        ExpirationDate = "05/2009",
                    },
                    LineItems = new TransactionLineItemRequest[]
                    {
                        new TransactionLineItemRequest
                        {
                            Quantity = 1.0232M,
                            Name = "Name #1",
                            LineItemKind = TransactionLineItemKind.DEBIT,
                            UnitAmount = unitAmounts[i],
                            TotalAmount = 45.15M,
                        }
                    }
                };

                Result<Transaction> result = gateway.Transaction.Sale(request);
                Assert.IsTrue(result.IsSuccess());

                Transaction transaction = result.Target;

                List<TransactionLineItem> lineItems = transaction.GetLineItems();
                Assert.AreEqual(1, lineItems.Count);

                TransactionLineItem lineItem = lineItems[0];
                Assert.AreEqual(1.0232M, lineItem.Quantity);
                Assert.AreEqual("Name #1", lineItem.Name);
                Assert.AreEqual(TransactionLineItemKind.DEBIT, lineItem.Kind);
                Assert.AreEqual(unitAmounts[i], lineItem.UnitAmount);
                Assert.AreEqual(45.15M, lineItem.TotalAmount);
            }
        }

        [Test]
        public void Sale_WithLineItemsSingle()
        {
            var request = new TransactionRequest
            {
                Amount = 45.15M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        Description = "Description #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitTaxAmount = 1.23M,
                        TaxAmount = 1.33M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                        Url = "https://example.com/products/23434",
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Transaction transaction = result.Target;

            List<TransactionLineItem> lineItems = transaction.GetLineItems();
            Assert.AreEqual(1, lineItems.Count);

            TransactionLineItem lineItem = lineItems[0];
            Assert.AreEqual(1.0232M, lineItem.Quantity);
            Assert.AreEqual("Name #1", lineItem.Name);
            Assert.AreEqual("Description #1", lineItem.Description);
            Assert.AreEqual(TransactionLineItemKind.DEBIT, lineItem.Kind);
            Assert.AreEqual(45.1232M, lineItem.UnitAmount);
            Assert.AreEqual(1.23M, lineItem.UnitTaxAmount);
            Assert.AreEqual(1.33M, lineItem.TaxAmount);
            Assert.AreEqual("gallon", lineItem.UnitOfMeasure);
            Assert.AreEqual(1.02M, lineItem.DiscountAmount);
            Assert.AreEqual(45.15M, lineItem.TotalAmount);
            Assert.AreEqual("23434", lineItem.ProductCode);
            Assert.AreEqual("9SAASSD8724", lineItem.CommodityCode);
            Assert.AreEqual("https://example.com/products/23434", lineItem.Url);
        }

        [Test]
        public void Sale_WithLineItemsSinglePayPal()
        {
            var request = new TransactionRequest
            {
                Amount = 45.15M,
                PaymentMethodNonce = Nonce.PayPalOneTimePayment,
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1,
                        Name = "Name #1",
                        Description = "Description #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.12M,
                        UnitTaxAmount = 1.23M,
                        TaxAmount = 1.33M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                        Url = "https://example.com/products/23434",
                        ImageUrl = "https://google.com/image.png",
                        UpcCode = "042100005264",
                        UpcType = "UPC-A",
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Transaction transaction = result.Target;

            List<TransactionLineItem> lineItems = transaction.GetLineItems();
            Assert.AreEqual(1, lineItems.Count);

            TransactionLineItem lineItem = lineItems[0];
            Assert.AreEqual(1, lineItem.Quantity);
            Assert.AreEqual("Name #1", lineItem.Name);
            Assert.AreEqual("Description #1", lineItem.Description);
            Assert.AreEqual(TransactionLineItemKind.DEBIT, lineItem.Kind);
            Assert.AreEqual(45.12M, lineItem.UnitAmount);
            Assert.AreEqual(1.23M, lineItem.UnitTaxAmount);
            Assert.AreEqual(1.33M, lineItem.TaxAmount);
            Assert.AreEqual("gallon", lineItem.UnitOfMeasure);
            Assert.AreEqual(1.02M, lineItem.DiscountAmount);
            Assert.AreEqual(45.15M, lineItem.TotalAmount);
            Assert.AreEqual("23434", lineItem.ProductCode);
            Assert.AreEqual("9SAASSD8724", lineItem.CommodityCode);
            Assert.AreEqual("https://example.com/products/23434", lineItem.Url);
            Assert.AreEqual("https://google.com/image.png", lineItem.ImageUrl);
            Assert.AreEqual("042100005264", lineItem.UpcCode);
            Assert.AreEqual("UPC-A", lineItem.UpcType);
        }

        [Test]
        public void Sale_WithLineItemsMultiple()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        Description = "Description #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                    new TransactionLineItemRequest
                    {
                        Quantity = 2.02M,
                        Name = "Name #2",
                        Description = "Description #2",
                        LineItemKind = TransactionLineItemKind.CREDIT,
                        UnitAmount = 5,
                        UnitOfMeasure = "gallon",
                        TotalAmount = 45.15M,
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Transaction transaction = result.Target;

            List<TransactionLineItem> lineItems = transaction.GetLineItems();
            Assert.AreEqual(2, lineItems.Count);

            TransactionLineItem lineItem1 = null;
            for (int i = 0; i < lineItems.Count; i++) {
                TransactionLineItem lineItem = lineItems[i];
                if (lineItem.Name.Equals("Name #1")) {
                    lineItem1 = lineItem;
                    break;
                }
            }
            if (lineItem1 == null) {
                Assert.Fail("TransactionLineItem with name \"Name #1\" not returned.");
            }
            Assert.AreEqual(1.0232M, lineItem1.Quantity);
            Assert.AreEqual("Name #1", lineItem1.Name);
            Assert.AreEqual("Description #1", lineItem1.Description);
            Assert.AreEqual(TransactionLineItemKind.DEBIT, lineItem1.Kind);
            Assert.AreEqual(45.1232M, lineItem1.UnitAmount);
            Assert.AreEqual("gallon", lineItem1.UnitOfMeasure);
            Assert.AreEqual(1.02M, lineItem1.DiscountAmount);
            Assert.AreEqual(45.15M, lineItem1.TotalAmount);
            Assert.AreEqual("23434", lineItem1.ProductCode);
            Assert.AreEqual("9SAASSD8724", lineItem1.CommodityCode);

            TransactionLineItem lineItem2 = null;
            for (int i = 0; i < lineItems.Count; i++) {
                TransactionLineItem lineItem = lineItems[i];
                if (lineItem.Name.Equals("Name #2")) {
                    lineItem2 = lineItem;
                    break;
                }
            }
            if (lineItem2 == null) {
                Assert.Fail("TransactionLineItem with name \"Name #2\" not returned.");
            }
            Assert.AreEqual(2.02M, lineItem2.Quantity);
            Assert.AreEqual("Name #2", lineItem2.Name);
            Assert.AreEqual("Description #2", lineItem2.Description);
            Assert.AreEqual(TransactionLineItemKind.CREDIT, lineItem2.Kind);
            Assert.AreEqual(5, lineItem2.UnitAmount);
            Assert.AreEqual("gallon", lineItem2.UnitOfMeasure);
            Assert.AreEqual(45.15M, lineItem2.TotalAmount);
            Assert.AreEqual(null, lineItem2.DiscountAmount);
            Assert.AreEqual(null, lineItem2.ProductCode);
            Assert.AreEqual(null, lineItem2.CommodityCode);
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorCommodityCodeIsTooLong()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #2",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "0123456789123",
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_COMMODITY_CODE_IS_TOO_LONG,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_1").OnField("CommodityCode")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorDescriptionIsTooLong()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #2",
                        Description = "This is a line item description which is far too long. Like, way too long to be practical. We don't like how long this line item description is.",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_DESCRIPTION_IS_TOO_LONG,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_1").OnField("Description")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorDiscountAmountIsTooLarge()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #2",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 2147483648,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_DISCOUNT_AMOUNT_IS_TOO_LARGE,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_1").OnField("DiscountAmount")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorDiscountAmountCannotBeNegative()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #2",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = -2,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_DISCOUNT_AMOUNT_CANNOT_BE_NEGATIVE,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_1").OnField("DiscountAmount")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorKindIsRequired()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #2",
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_KIND_IS_REQUIRED,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_1").OnField("Kind")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorNameIsRequired()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_NAME_IS_REQUIRED,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_1").OnField("Name")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorNameIsTooLong()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "123456789012345678901234567890123456",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_NAME_IS_TOO_LONG,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_1").OnField("Name")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorProductCodeIsTooLong()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #2",
                        LineItemKind = TransactionLineItemKind.CREDIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "123456789012345678901234567890123456",
                        CommodityCode = "9SAASSD8724",
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_PRODUCT_CODE_IS_TOO_LONG,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_1").OnField("ProductCode")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorQuantityIsRequired()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                    new TransactionLineItemRequest
                    {
                        Name = "Name #2",
                        LineItemKind = TransactionLineItemKind.CREDIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_QUANTITY_IS_REQUIRED,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_1").OnField("Quantity")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorQuantityIsTooLarge()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                    new TransactionLineItemRequest
                    {
                        Quantity = 2147483648,
                        Name = "Name #2",
                        LineItemKind = TransactionLineItemKind.CREDIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_QUANTITY_IS_TOO_LARGE,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_1").OnField("Quantity")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorTotalAmountIsRequired()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #2",
                        LineItemKind = TransactionLineItemKind.CREDIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_TOTAL_AMOUNT_IS_REQUIRED,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_1").OnField("TotalAmount")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorTotalAmountIsTooLarge()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #2",
                        LineItemKind = TransactionLineItemKind.CREDIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 2147483648,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_TOTAL_AMOUNT_IS_TOO_LARGE,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_1").OnField("TotalAmount")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorTotalAmountMustBeGreaterThanZero()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #2",
                        LineItemKind = TransactionLineItemKind.CREDIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = -2,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_TOTAL_AMOUNT_MUST_BE_GREATER_THAN_ZERO,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_1").OnField("TotalAmount")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorUnitAmountIsRequired()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #2",
                        LineItemKind = TransactionLineItemKind.CREDIT,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_UNIT_AMOUNT_IS_REQUIRED,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_1").OnField("UnitAmount")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorUnitAmountIsTooLarge()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #2",
                        LineItemKind = TransactionLineItemKind.CREDIT,
                        UnitAmount = 2147483648,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_UNIT_AMOUNT_IS_TOO_LARGE,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_1").OnField("UnitAmount")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorUnitAmountMustBeGreaterThanZero()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #2",
                        LineItemKind = TransactionLineItemKind.CREDIT,
                        UnitAmount = -2,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_UNIT_AMOUNT_MUST_BE_GREATER_THAN_ZERO,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_1").OnField("UnitAmount")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorUnitOfMeasureIsTooLong()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #2",
                        LineItemKind = TransactionLineItemKind.CREDIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "1234567890123",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_UNIT_OF_MEASURE_IS_TOO_LONG,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_1").OnField("UnitOfMeasure")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorUnitTaxAmountIsTooLarge()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.2322M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitTaxAmount = 1.23M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.2322M,
                        Name = "Name #2",
                        LineItemKind = TransactionLineItemKind.CREDIT,
                        UnitAmount = 45.0122M,
                        UnitTaxAmount = 2147483648,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_UNIT_TAX_AMOUNT_IS_TOO_LARGE,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_1").OnField("UnitTaxAmount")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorUnitTaxAmountCannotBeNegative()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.2322M,
                        Name = "Name #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.2322M,
                        Name = "Name #2",
                        LineItemKind = TransactionLineItemKind.CREDIT,
                        UnitAmount = 45.0122M,
                        UnitTaxAmount = -1.23M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_UNIT_TAX_AMOUNT_CANNOT_BE_NEGATIVE,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_1").OnField("UnitTaxAmount")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorTaxAmountIsTooLarge()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.2322M,
                        Name = "Name #2",
                        LineItemKind = TransactionLineItemKind.CREDIT,
                        UnitAmount = 45.0122M,
                        UnitTaxAmount = 1.23M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TaxAmount = 2147483648,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_TAX_AMOUNT_IS_TOO_LARGE,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_0").OnField("TaxAmount")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorTaxAmountCannotBeNegative()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.2322M,
                        Name = "Name #2",
                        LineItemKind = TransactionLineItemKind.CREDIT,
                        UnitAmount = 45.0122M,
                        UnitTaxAmount = 1.23M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TaxAmount = -1.23M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                    },
                },
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_TAX_AMOUNT_CANNOT_BE_NEGATIVE,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_0").OnField("TaxAmount")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorTooManyLineItems()
        {
            var lineItems = new TransactionLineItemRequest[250];
            for (int i = 0; i < 250; i++) {
                lineItems[i] = new TransactionLineItemRequest
                {
                    Quantity = 2.02M,
                    Name = "Line item #" + i,
                    LineItemKind = TransactionLineItemKind.CREDIT,
                    UnitAmount = 5,
                    UnitOfMeasure = "gallon",
                    TotalAmount = 10.1M,
                };
            }

            var request = new TransactionRequest
            {
                Amount = 35.05M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                },
                LineItems = lineItems,
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_TOO_MANY_LINE_ITEMS,
                result.Errors.ForObject("Transaction").OnField("LineItems")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorUpcCodeIsTooLongUpcTypeIsInvalid()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                PaymentMethodNonce = Nonce.PayPalOneTimePayment,
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1,
                        Name = "Name #1",
                        Description = "Description #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.12M,
                        UnitTaxAmount = 1.23M,
                        TaxAmount = 1.33M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                        Url = "https://example.com/products/23434",
                        ImageUrl = "https://google.com/image.png",
                        UpcCode = "THECODEISTOODARNLONG",
                        UpcType = "USB-A",
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_UPC_CODE_IS_TOO_LONG,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_0").OnField("UpcCode")[0].Code
            );
            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_UPC_TYPE_IS_INVALID,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_0").OnField("UpcType")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorUpcCodeIsMissing()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                PaymentMethodNonce = Nonce.PayPalOneTimePayment,
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1,
                        Name = "Name #1",
                        Description = "Description #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.12M,
                        UnitTaxAmount = 1.23M,
                        TaxAmount = 1.33M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                        Url = "https://example.com/products/23434",
                        ImageUrl = "https://google.com/image.png",
                        UpcType = "USB-A",
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_UPC_CODE_IS_MISSING,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_0").OnField("UpcCode")[0].Code
            );
        }

        [Test]
        public void Sale_WithLineItemsValidationErrorUpcTypeIsMissing()
        {
            var request = new TransactionRequest
            {
                Amount = 35.05M,
                PaymentMethodNonce = Nonce.PayPalOneTimePayment,
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1,
                        Name = "Name #1",
                        Description = "Description #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.12M,
                        UnitTaxAmount = 1.23M,
                        TaxAmount = 1.33M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                        Url = "https://example.com/products/23434",
                        ImageUrl = "https://google.com/image.png",
                        UpcCode = "042100005264",
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.TRANSACTION_LINE_ITEM_UPC_TYPE_IS_MISSING,
                result.Errors.ForObject("Transaction").ForObject("LineItems").ForObject("index_0").OnField("UpcType")[0].Code
            );
        }

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
#if netcore
        public async Task CreditAsync_WithValidParams()
#else
        public void CreditAsync_WithValidParams()
        {
            Task.Run(async () =>
#endif
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

            Result<Transaction> result = await gateway.Transaction.CreditAsync(request);
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
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

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
                CustomFields = new Dictionary<string, string>
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

            Dictionary<string, string> parameters = result.Parameters;
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
            Assert.IsNotNull(transaction.GraphQLId);
        }

        [Test]
#if netcore
        public async Task FindAsync_WithAValidTransactionId()
#else
        public void FindAsync_WithAValidTransactionId()
        {
            Task.Run(async () =>
#endif
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

            Result<Transaction> transactionResult = await gateway.Transaction.SaleAsync(request);
            Transaction transaction = transactionResult.Target;

            Transaction foundTransaction = await gateway.Transaction.FindAsync(transaction.Id);

            Assert.AreEqual(transaction.Id, foundTransaction.Id);
            Assert.AreEqual(TransactionStatus.AUTHORIZED, foundTransaction.Status);
            Assert.AreEqual("05/2008", foundTransaction.CreditCard.ExpirationDate);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Find_WithBadId()
        {
            Assert.Throws<NotFoundException>(() => gateway.Transaction.Find("badId"));
        }

        [Test]
        public void Find_ExposesThreeDSecureInfo()
        {
            Transaction transaction = gateway.Transaction.Find("threedsecuredtransaction");

            ThreeDSecureInfo info = transaction.ThreeDSecureInfo;
            Assert.AreEqual("authenticate_successful", info.Status);
            Assert.IsTrue(info.LiabilityShifted);
            Assert.IsTrue(info.LiabilityShiftPossible);
            Assert.IsNotNull(info.Enrolled);
            Assert.IsNotNull(info.Cavv);
            Assert.IsNotNull(info.EciFlag);
            Assert.IsNotNull(info.ThreeDSecureVersion);
            Assert.IsNotNull(info.Xid);
            Assert.IsNotNull(info.ParesStatus);
            Assert.IsInstanceOf(typeof(ThreeDSecureLookupInfo), info.Lookup);
            Assert.IsInstanceOf(typeof(ThreeDSecureAuthenticationInfo), info.Authentication);
        }

        [Test]
        public void Find_ExposesNullThreeDSecureInfoIfBlank()
        {
            Transaction transaction = gateway.Transaction.Find("settledtransaction");

            Assert.IsNull(transaction.ThreeDSecureInfo);
        }

        [Test]
        public void Find_ExposesDisbursementDetails()
        {
            Transaction transaction = gateway.Transaction.Find("deposittransaction");

            Assert.AreEqual(transaction.IsDisbursed(), true);

            DisbursementDetails details = transaction.DisbursementDetails;
            Assert.AreEqual(details.DisbursementDate, DateTime.Parse("2013-04-10"));
            Assert.AreEqual(details.SettlementAmount, decimal.Parse("100.00"));
            Assert.AreEqual(details.SettlementCurrencyIsoCode, "USD");
            Assert.AreEqual(details.SettlementCurrencyExchangeRate, "1");
            Assert.AreEqual(details.FundsHeld, false);
            Assert.AreEqual(details.Success, true);
        }

        [Test]
        public void Find_ExposesAcquirerReferenceNumber()
        {
            Transaction transaction = gateway.Transaction.Find("transactionwithacquirerreferencenumber");

            Assert.AreEqual(transaction.AcquirerReferenceNumber, "123456789 091019");
        }

        [Test]
        public void Find_ExposesDisputes()
        {
            Transaction transaction = gateway.Transaction.Find("disputedtransaction");

            List<Dispute> disputes = transaction.Disputes;
            Dispute dispute = disputes[0];

            Assert.AreEqual(dispute.ReceivedDate, DateTime.Parse("2014-03-01"));
            Assert.AreEqual(dispute.ReplyByDate, DateTime.Parse("2014-03-21"));
            Assert.AreEqual(dispute.Amount, decimal.Parse("250.00"));
            Assert.AreEqual(dispute.CurrencyIsoCode, "USD");
            Assert.AreEqual(dispute.Reason, DisputeReason.FRAUD);
            Assert.AreEqual(dispute.Status, DisputeStatus.WON);
            Assert.AreEqual(dispute.Kind, DisputeKind.CHARGEBACK);
            Assert.AreEqual(dispute.DateOpened, DateTime.Parse("2014-03-01"));
            Assert.AreEqual(dispute.DateWon, DateTime.Parse("2014-03-07"));
        }

        [Test]
        public void Find_ExposesRetrievals()
        {
            Transaction transaction = gateway.Transaction.Find("retrievaltransaction");

            List<Dispute> disputes = transaction.Disputes;
            Dispute dispute = disputes[0];

            Assert.AreEqual(dispute.Amount, decimal.Parse("1000.00"));
            Assert.AreEqual(dispute.CurrencyIsoCode, "USD");
            Assert.AreEqual(dispute.Reason, DisputeReason.RETRIEVAL);
        }

        [Test]
        public void Find_ExposesAuthorizationAdjustmentsApproved()
        {
            Transaction transaction = gateway.Transaction.Find("authadjustmenttransaction");

            List<AuthorizationAdjustment> authorizationAdjustments = transaction.AuthorizationAdjustments;
            AuthorizationAdjustment authorizationAdjustment = authorizationAdjustments[0];

            Assert.AreEqual(authorizationAdjustment.Amount, decimal.Parse("-20.00"));
            Assert.AreEqual(authorizationAdjustment.Success, true);
            Assert.AreEqual(authorizationAdjustment.Timestamp.Value.Year, DateTime.Now.Year);
            Assert.AreEqual(authorizationAdjustment.ProcessorResponseCode, "1000");
            Assert.AreEqual(authorizationAdjustment.ProcessorResponseText, "Approved");
            Assert.AreEqual(ProcessorResponseType.APPROVED, authorizationAdjustment.ProcessorResponseType);
        }

        [Test]
        public void Find_ExposesAuthorizationAdjustmentsSoftDeclined()
        {
            Transaction transaction = gateway.Transaction.Find("authadjustmenttransactionsoftdeclined");

            List<AuthorizationAdjustment> authorizationAdjustments = transaction.AuthorizationAdjustments;
            AuthorizationAdjustment authorizationAdjustment = authorizationAdjustments[0];

            Assert.AreEqual(decimal.Parse("-20.00"), authorizationAdjustment.Amount);
            Assert.AreEqual(false, authorizationAdjustment.Success);
            Assert.AreEqual(DateTime.Now.Year, authorizationAdjustment.Timestamp.Value.Year);
            Assert.AreEqual("3000", authorizationAdjustment.ProcessorResponseCode);
            Assert.AreEqual("Processor Network Unavailable - Try Again", authorizationAdjustment.ProcessorResponseText);
            Assert.AreEqual(ProcessorResponseType.SOFT_DECLINED, authorizationAdjustment.ProcessorResponseType);
        }

        [Test]
        public void Find_ExposesAuthorizationAdjustmentsHardDeclined()
        {
            Transaction transaction = gateway.Transaction.Find("authadjustmenttransactionharddeclined");

            List<AuthorizationAdjustment> authorizationAdjustments = transaction.AuthorizationAdjustments;
            AuthorizationAdjustment authorizationAdjustment = authorizationAdjustments[0];

            Assert.AreEqual(decimal.Parse("-20.00"), authorizationAdjustment.Amount);
            Assert.AreEqual(false, authorizationAdjustment.Success);
            Assert.AreEqual(DateTime.Now.Year, authorizationAdjustment.Timestamp.Value.Year);
            Assert.AreEqual("2015", authorizationAdjustment.ProcessorResponseCode);
            Assert.AreEqual("Transaction Not Allowed", authorizationAdjustment.ProcessorResponseText);
            Assert.AreEqual(ProcessorResponseType.HARD_DECLINED, authorizationAdjustment.ProcessorResponseType);
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
#if netcore
        public async Task VoidAsync_VoidsTheTransaction()
#else
        public void VoidAsync_VoidsTheTransaction()
        {
            Task.Run(async () =>
#endif
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

            Result<Transaction> saleResult = await gateway.Transaction.SaleAsync(request);
            Transaction transaction = saleResult.Target;
            Result<Transaction> result = await gateway.Transaction.VoidAsync(transaction.Id);
            Assert.IsTrue(result.IsSuccess());

            Assert.AreEqual(transaction.Id, result.Target.Id);
            Assert.AreEqual(TransactionStatus.VOIDED, result.Target.Status);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Void_WithBadId()
        {
            Assert.Throws<NotFoundException>(() => gateway.Transaction.Void("badId"));
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
#if netcore
        public async Task SubmitForSettlementAsync_WithoutAmount()
#else
        public void SubmitForSettlementAsync_WithoutAmount()
        {
            Task.Run(async () =>
#endif
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

            Result<Transaction> saleResult = await gateway.Transaction.SaleAsync(request);
            Transaction transaction = saleResult.Target;

            Result<Transaction> result = await gateway.Transaction.SubmitForSettlementAsync(transaction.Id);

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, result.Target.Status);
            Assert.AreEqual(SandboxValues.TransactionAmount.AUTHORIZE, result.Target.Amount);
            Assert.IsNull(result.Message);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

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
            Result<Transaction> result = gateway.Transaction.SubmitForSettlement(transaction.Id, decimal.Parse("50.00"));

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, result.Target.Status);
            Assert.AreEqual(50.00, result.Target.Amount);
        }

        [Test]
#if netcore
        public async Task SubmitForSettlementAsync_WithAmount()
#else
        public void SubmitForSettlementAsync_WithAmount()
        {
            Task.Run(async () =>
#endif
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

            Result<Transaction> saleResult = await gateway.Transaction.SaleAsync(request);
            Transaction transaction = saleResult.Target;
            Result<Transaction> result = await gateway.Transaction.SubmitForSettlementAsync(transaction.Id, decimal.Parse("50.00"));

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, result.Target.Status);
            Assert.AreEqual(50.00, result.Target.Amount);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void SubmitForSettlement_WithOrderId()
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

            TransactionRequest submitForSettlementRequest = new TransactionRequest
            {
                OrderId = "ABC123"
            };

            Result<Transaction> result = gateway.Transaction.SubmitForSettlement(transaction.Id, submitForSettlementRequest);

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, result.Target.Status);
            Assert.AreEqual("ABC123", result.Target.OrderId);
        }

        [Test]
#if netcore
        public async Task SubmitForSettlementAsync_WithOrderId()
#else
        public void SubmitForSettlementAsync_WithOrderId()
        {
            Task.Run(async () =>
#endif
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

            Result<Transaction> saleResult = await gateway.Transaction.SaleAsync(request);
            Transaction transaction = saleResult.Target;

            TransactionRequest submitForSettleRequest = new TransactionRequest
            {
                OrderId = "order-id"
            };

            Result<Transaction> result = await gateway.Transaction.SubmitForSettlementAsync(transaction.Id, submitForSettleRequest);

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, result.Target.Status);
            Assert.AreEqual("order-id", result.Target.OrderId);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void SubmitForSettlement_WithLevel2Data()
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

            TransactionRequest submitForSettlementRequest = new TransactionRequest
            {
                PurchaseOrderNumber = "ABC123",
                TaxAmount = 1.12M,
                TaxExempt = true
            };

            Result<Transaction> result = gateway.Transaction.SubmitForSettlement(transaction.Id, submitForSettlementRequest);

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, result.Target.Status);
        }

        [Test]
        public void SubmitForSettlement_WithLevel3Data()
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

            TransactionRequest submitForSettlementRequest = new TransactionRequest
            {
                PurchaseOrderNumber = "ABC123",
                TaxAmount = 1.12M,
                TaxExempt = true,
                ShippingAmount = 1.00M,
                ShippingTaxAmount = 0.23M,
                DiscountAmount = 2.00M,
                ShipsFromPostalCode = "12345",
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1.0232M,
                        Name = "Name #1",
                        Description = "Description #1",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        UnitAmount = 45.1232M,
                        UnitTaxAmount = 1.23M,
                        TaxAmount = 1.33M,
                        UnitOfMeasure = "gallon",
                        DiscountAmount = 1.02M,
                        TotalAmount = 45.15M,
                        ProductCode = "23434",
                        CommodityCode = "9SAASSD8724",
                        Url = "https://example.com/products/23434",
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.SubmitForSettlement(transaction.Id, submitForSettlementRequest);

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(0.23, result.Target.ShippingTaxAmount);
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, result.Target.Status);
        }


        [Test]
        public void SubmitForSettlement_WithDescriptor()
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

            TransactionRequest submitForSettlementRequest = new TransactionRequest
            {
                Descriptor = new DescriptorRequest
                {
                    Name = "123*123456789012345678",
                    Phone = "3334445555",
                    Url = "ebay.com"
                }
            };

            Result<Transaction> result = gateway.Transaction.SubmitForSettlement(transaction.Id, submitForSettlementRequest);

            Transaction submittedTransaction = result.Target;

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, submittedTransaction.Status);
            Assert.AreEqual("123*123456789012345678", submittedTransaction.Descriptor.Name);
            Assert.AreEqual("3334445555", submittedTransaction.Descriptor.Phone);
            Assert.AreEqual("ebay.com", submittedTransaction.Descriptor.Url);
        }

        [Test]
        public void SubmitForSettlement_WithInvalidParams()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.FAKE_AMEX_DIRECT_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.AmexPayWithPoints.SUCCESS,
                    ExpirationDate = "05/2008"
                },
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            TransactionRequest submitForSettlementRequest = new TransactionRequest
            {
                SharedCustomerId = "invalid",
            };

            Assert.Throws<AuthorizationException>(() => gateway.Transaction.SubmitForSettlement(transaction.Id, submitForSettlementRequest));
        }

        [Test]
        [Ignore("unpend when we have a more stable CI")]
        public void SubmitForSettlement_WithAmexRewardsSucceeds()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.FAKE_AMEX_DIRECT_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.AmexPayWithPoints.SUCCESS,
                    ExpirationDate = "05/2008"
                },
                Options = new TransactionOptionsRequest
                {
                    AmexRewards = new TransactionOptionsAmexRewardsRequest
                    {
                        RequestId = "ABC123",
                        Points = "100",
                        CurrencyAmount = "1",
                        CurrencyIsoCode = "USD"
                    }
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            Result<Transaction> result = gateway.Transaction.SubmitForSettlement(transaction.Id, decimal.Parse("50.00"));

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, result.Target.Status);
            Assert.AreEqual(50.00, result.Target.Amount);
        }

        [Test]
        [Ignore("unpend when we have a more stable CI")]
        public void SubmitForSettlement_WithAmexRewardsSucceedsEvenIfCardIsIneligible()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.FAKE_AMEX_DIRECT_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.AmexPayWithPoints.INELIGIBLE_CARD,
                    ExpirationDate = "05/2008"
                },
                Options = new TransactionOptionsRequest
                {
                    AmexRewards = new TransactionOptionsAmexRewardsRequest
                    {
                        RequestId = "ABC123",
                        Points = "100",
                        CurrencyAmount = "1",
                        CurrencyIsoCode = "USD"
                    }
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            Result<Transaction> result = gateway.Transaction.SubmitForSettlement(transaction.Id, decimal.Parse("50.00"));

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, result.Target.Status);
            Assert.AreEqual(50.00, result.Target.Amount);
        }

        [Test]
        [Ignore("unpend when we have a more stable CI")]
        public void SubmitForSettlement_WithAmexRewardsSucceedsEvenIfCardBalanceIsInsufficient()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.FAKE_AMEX_DIRECT_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.AmexPayWithPoints.INSUFFICIENT_POINTS,
                    ExpirationDate = "05/2008"
                },
                Options = new TransactionOptionsRequest
                {
                    AmexRewards = new TransactionOptionsAmexRewardsRequest
                    {
                        RequestId = "ABC123",
                        Points = "100",
                        CurrencyAmount = "1",
                        CurrencyIsoCode = "USD"
                    }
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            Result<Transaction> result = gateway.Transaction.SubmitForSettlement(transaction.Id, decimal.Parse("50.00"));

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, result.Target.Status);
            Assert.AreEqual(50.00, result.Target.Amount);
        }

        // [Test]
        // public void SubmitForSettlement_TransactionAmountLessThanServiceFeeAmount()
        // {
        //     TransactionRequest request = new TransactionRequest
        //     {
        //         Amount = 100M,
        //         MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_SUB_MERCHANT_ACCOUNT_ID,
        //         CreditCard = new TransactionCreditCardRequest
        //         {
        //             Number = SandboxValues.CreditCardNumber.VISA,
        //             ExpirationDate = "06/2008"
        //         },
        //         ServiceFeeAmount = 50M
        //     };

        //     Transaction transaction = gateway.Transaction.Sale(request).Target;
        //     Result<Transaction> result = gateway.Transaction.SubmitForSettlement(transaction.Id, 25M);

        //     Assert.IsFalse(result.IsSuccess());
        //     Assert.AreEqual(
        //         ValidationErrorCode.TRANSACTION_SETTLEMENT_AMOUNT_IS_LESS_THAN_SERVICE_FEE_AMOUNT,
        //         result.Errors.ForObject("Transaction").OnField("Amount")[0].Code
        //     );
        // }

        [Test]
        public void UpdateDetails()
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

            TransactionRequest updateRequest = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE - 1,
                OrderId = "123",
                Descriptor = new DescriptorRequest
                {
                    Name = "123*123456789012345678",
                    Phone = "3334445555",
                    Url = "ebay.com"
                }
            };

            Result<Transaction> result = gateway.Transaction.UpdateDetails(transaction.Id, updateRequest);

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(SandboxValues.TransactionAmount.AUTHORIZE - 1, result.Target.Amount);
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, result.Target.Status);
            Assert.AreEqual("123", result.Target.OrderId);
            Assert.AreEqual("123*123456789012345678", result.Target.Descriptor.Name);
            Assert.AreEqual("3334445555", result.Target.Descriptor.Phone);
            Assert.AreEqual("ebay.com", result.Target.Descriptor.Url);
        }

        [Test]
        public void UpdateDetails_WithInvalidParams()
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

            TransactionRequest updateRequest = new TransactionRequest
            {
                PurchaseOrderNumber = "111"
            };

            try
            {
                gateway.Transaction.UpdateDetails(transaction.Id, updateRequest);
                Assert.Fail("Expected ServerException.");
            }
            catch (AuthorizationException)
            {
                // expected
            }
        }

        [Test]
        public void UpdateDetails_WithInvalidAmount()
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

            TransactionRequest updateRequest = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE * 10
            };

            Result<Transaction> result = gateway.Transaction.UpdateDetails(transaction.Id, updateRequest);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_SETTLEMENT_AMOUNT_IS_TOO_LARGE, result.Errors.ForObject("Transaction").OnField("Amount")[0].Code);
        }

        [Test]
        public void UpdateDetails_WithInvalidDescriptor()
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

            TransactionRequest updateRequest = new TransactionRequest
            {
                Descriptor = new DescriptorRequest
                {
                    Name = "invalid name",
                    Phone = "invalid phone",
                    Url = "invalid url that is too long to be valid"
                }
            };

            Result<Transaction> result = gateway.Transaction.UpdateDetails(transaction.Id, updateRequest);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.DESCRIPTOR_NAME_FORMAT_IS_INVALID, result.Errors.ForObject("Transaction").ForObject("Descriptor").OnField("Name")[0].Code);
            Assert.AreEqual(ValidationErrorCode.DESCRIPTOR_PHONE_FORMAT_IS_INVALID, result.Errors.ForObject("Transaction").ForObject("Descriptor").OnField("Phone")[0].Code);
            Assert.AreEqual(ValidationErrorCode.DESCRIPTOR_URL_FORMAT_IS_INVALID, result.Errors.ForObject("Transaction").ForObject("Descriptor").OnField("Url")[0].Code);
        }

        [Test]
        public void UpdateDetails_WithInvalidOrderId()
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

            TransactionRequest updateRequest = new TransactionRequest
            {
                OrderId = new string('A', 256)
            };

            Result<Transaction> result = gateway.Transaction.UpdateDetails(transaction.Id, updateRequest);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_ORDER_ID_IS_TOO_LONG, result.Errors.ForObject("Transaction").OnField("OrderId")[0].Code);
        }

        [Test]
        public void UpdateDetails_WithInvalidStatus()
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

            TransactionRequest updateRequest = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE - 1,
                OrderId = "123",
                Descriptor = new DescriptorRequest
                {
                    Name = "123*123456789012345678",
                    Phone = "3334445555",
                    Url = "ebay.com"
                }
            };

            Result<Transaction> result = gateway.Transaction.UpdateDetails(transaction.Id, updateRequest);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_CANNOT_UPDATE_DETAILS_NOT_SUBMITTED_FOR_SETTLEMENT, result.Errors.ForObject("Transaction").OnField("base")[0].Code);
        }

        [Test]
        public void UpdateDetails_WithInvalidProcessor()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.FAKE_AMEX_DIRECT_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.AmexPayWithPoints.SUCCESS,
                    ExpirationDate = "05/2008"
                },
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            TransactionRequest updateRequest = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE - 1,
                OrderId = "123",
                Descriptor = new DescriptorRequest
                {
                    Name = "123*123456789012345678",
                    Phone = "3334445555",
                    Url = "ebay.com"
                }
            };

            Result<Transaction> result = gateway.Transaction.UpdateDetails(transaction.Id, updateRequest);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_PROCESSOR_DOES_NOT_SUPPORT_UPDATING_DETAILS, result.Errors.ForObject("Transaction").OnField("base")[0].Code);
        }

        [Test]
        public void SubmitForPartialSettlement_WithFinalCapture()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                       CreditCard = new TransactionCreditCardRequest
                       {
                           Number = SandboxValues.CreditCardNumber.MASTER_CARD,
                           ExpirationDate = "05/2027",
                       }
            };

            Transaction authorizedTransaction = gateway.Transaction.Sale(request).Target;
            Assert.AreEqual(TransactionStatus.AUTHORIZED, authorizedTransaction.Status);

            var partialSettlementTransaction1 = gateway.Transaction.SubmitForPartialSettlement(authorizedTransaction.Id, 400).Target;
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, partialSettlementTransaction1.Status);
            Assert.AreEqual(authorizedTransaction.Id, partialSettlementTransaction1.AuthorizedTransactionId);

            var refreshedAuthorizedTransaction1 = gateway.Transaction.Find(authorizedTransaction.Id);
            Assert.AreEqual(TransactionStatus.SETTLEMENT_PENDING, refreshedAuthorizedTransaction1.Status);
            var partialSettlementTransactionIds = new string[] { partialSettlementTransaction1.Id };
            Assert.AreEqual(refreshedAuthorizedTransaction1.PartialSettlementTransactionIds, partialSettlementTransactionIds);

            var submitForPartialSettlementRequest = new TransactionRequest
            {
                Amount = decimal.Parse("300.00"),
                FinalCapture = true
            };
            
            var partialSettlementTransaction2 = gateway.Transaction.SubmitForPartialSettlement(authorizedTransaction.Id, submitForPartialSettlementRequest).Target;
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, partialSettlementTransaction2.Status);
            Assert.AreEqual(authorizedTransaction.Id, partialSettlementTransaction2.AuthorizedTransactionId);

            var refreshedAuthorizedTransaction2 = gateway.Transaction.Find(authorizedTransaction.Id);
            Assert.AreEqual(TransactionStatus.SETTLEMENT_PENDING, refreshedAuthorizedTransaction2.Status);
            partialSettlementTransactionIds = new string[] { partialSettlementTransaction1.Id, partialSettlementTransaction2.Id };
            CollectionAssert.AreEquivalent(refreshedAuthorizedTransaction2.PartialSettlementTransactionIds, partialSettlementTransactionIds); 
        }

        [Test]
        public void SubmitForPartialSettlement_WithAmount()
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
            Result<Transaction> result = gateway.Transaction.SubmitForPartialSettlement(transaction.Id, decimal.Parse("50.00"));

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, result.Target.Status);
            Assert.AreEqual(50.00, result.Target.Amount);
        }

        [Test]
        public void SubmitForSettlement_WithTravelFlightIndustryData()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.PayPalOneTimePayment,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = false
                }
            };
            Transaction transaction = gateway.Transaction.Sale(request).Target;

            request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                Industry = new IndustryRequest
                {
                    IndustryType = TransactionIndustryType.TRAVEL_AND_FLIGHT,
                    IndustryData = new IndustryDataRequest
                    {
                        PassengerFirstName = "John",
                        PassengerLastName = "Doe",
                        PassengerMiddleInitial = "M",
                        PassengerTitle = "Mr.",
                        IssuedDate = new DateTime(2018, 1, 1),
                        TravelAgencyName = "Expedia",
                        TravelAgencyCode = "12345678",
                        TicketNumber = "ticket-number",
                        IssuingCarrierCode = "AA",
                        CustomerCode = "customer-code",
                        FareAmount = 7000M,
                        FeeAmount = 1000M,
                        TaxAmount = 2000M,
                        RestrictedTicket = false,
                        ArrivalDate = new DateTime(2018, 1, 1),
                        TicketIssuerAddress = "tkt-issuer-address",
                        DateOfBirth = "2012-12-12",
                        CountryCode = "US",
                        Legs = new IndustryDataLegRequest[]
                        {
                            new IndustryDataLegRequest
                            {
                                ConjunctionTicket = "CJ0001",
                                ExchangeTicket = "ET0001",
                                CouponNumber = "1",
                                ServiceClass = "Y",
                                CarrierCode = "AA",
                                FareBasisCode = "W",
                                FlightNumber = "AA100",
                                DepartureDate = new DateTime(2018, 1, 2),
                                DepartureAirportCode = "MDW",
                                DepartureTime = "08:00",
                                ArrivalAirportCode = "ATX",
                                ArrivalTime = "10:00",
                                StopoverPermitted = false,
                                FareAmount = 3500M,
                                FeeAmount = 500M,
                                TaxAmount = 1000M,
                                EndorsementOrRestrictions = "NOT REFUNDABLE",
                            },
                            new IndustryDataLegRequest
                            {
                                ConjunctionTicket = "CJ0002",
                                ExchangeTicket = "ET0002",
                                CouponNumber = "1",
                                ServiceClass = "Y",
                                CarrierCode = "AA",
                                FareBasisCode = "W",
                                FlightNumber = "AA200",
                                DepartureDate = new DateTime(2018, 1, 3),
                                DepartureAirportCode = "ATX",
                                DepartureTime = "12:00",
                                ArrivalAirportCode = "MDW",
                                ArrivalTime = "14:00",
                                StopoverPermitted = false,
                                FareAmount = 3500M,
                                FeeAmount = 500M,
                                TaxAmount = 1000M,
                                EndorsementOrRestrictions = "NOT REFUNDABLE",
                            }
                        }
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.SubmitForSettlement(transaction.Id, request);
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.SETTLING, result.Target.Status);
            Assert.AreEqual(SandboxValues.TransactionAmount.AUTHORIZE, result.Target.Amount);
        }

        [Test]
        public void SubmitForPartialSettlement_WithTravelFlightIndustryData()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.PayPalOneTimePayment,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = false
                }
            };
            Transaction transaction = gateway.Transaction.Sale(request).Target;

            request = new TransactionRequest
            {
                Amount = decimal.Parse("50.00"),
                Industry = new IndustryRequest
                {
                    IndustryType = TransactionIndustryType.TRAVEL_AND_FLIGHT,
                    IndustryData = new IndustryDataRequest
                    {
                        PassengerFirstName = "John",
                        PassengerLastName = "Doe",
                        PassengerMiddleInitial = "M",
                        PassengerTitle = "Mr.",
                        IssuedDate = new DateTime(2018, 1, 1),
                        TravelAgencyName = "Expedia",
                        TravelAgencyCode = "12345678",
                        TicketNumber = "ticket-number",
                        IssuingCarrierCode = "AA",
                        CustomerCode = "customer-code",
                        FareAmount = 7000M,
                        FeeAmount = 1000M,
                        TaxAmount = 2000M,
                        RestrictedTicket = false,
                        ArrivalDate = new DateTime(2018, 1, 1),
                        TicketIssuerAddress = "tkt-issuer-address",
                        DateOfBirth = "2012-12-12",
                        CountryCode = "US",
                        Legs = new IndustryDataLegRequest[]
                        {
                            new IndustryDataLegRequest
                            {
                                ConjunctionTicket = "CJ0001",
                                ExchangeTicket = "ET0001",
                                CouponNumber = "1",
                                ServiceClass = "Y",
                                CarrierCode = "AA",
                                FareBasisCode = "W",
                                FlightNumber = "AA100",
                                DepartureDate = new DateTime(2018, 1, 2),
                                DepartureAirportCode = "MDW",
                                DepartureTime = "08:00",
                                ArrivalAirportCode = "ATX",
                                ArrivalTime = "10:00",
                                StopoverPermitted = false,
                                FareAmount = 3500M,
                                FeeAmount = 500M,
                                TaxAmount = 1000M,
                                EndorsementOrRestrictions = "NOT REFUNDABLE",
                            },
                            new IndustryDataLegRequest
                            {
                                ConjunctionTicket = "CJ0002",
                                ExchangeTicket = "ET0002",
                                CouponNumber = "1",
                                ServiceClass = "Y",
                                CarrierCode = "AA",
                                FareBasisCode = "W",
                                FlightNumber = "AA200",
                                DepartureDate = new DateTime(2018, 1, 3),
                                DepartureAirportCode = "ATX",
                                DepartureTime = "12:00",
                                ArrivalAirportCode = "MDW",
                                ArrivalTime = "14:00",
                                StopoverPermitted = false,
                                FareAmount = 3500M,
                                FeeAmount = 500M,
                                TaxAmount = 1000M,
                                EndorsementOrRestrictions = "NOT REFUNDABLE",
                            }
                        }
                    }
                }
            };

            Result<Transaction> result = gateway.Transaction.SubmitForPartialSettlement(transaction.Id, request);
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.SETTLING, result.Target.Status);
            Assert.AreEqual(50.00, result.Target.Amount);
        }

        [Test]
        public void SubmitForPartialSettlement_WithOrderId()
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

            TransactionRequest submitForSettlementRequest = new TransactionRequest
            {
                OrderId = "ABC123",
                Amount = decimal.Parse("50.00")
            };

            Result<Transaction> result = gateway.Transaction.SubmitForPartialSettlement(transaction.Id, submitForSettlementRequest);

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, result.Target.Status);
            Assert.AreEqual("ABC123", result.Target.OrderId);
        }

        [Test]
        public void SubmitForPartialSettlement_WithDescriptor()
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

            TransactionRequest submitForSettlementRequest = new TransactionRequest
            {
                Descriptor = new DescriptorRequest
                {
                    Name = "123*123456789012345678",
                    Phone = "3334445555",
                    Url = "ebay.com"
                },
                Amount = decimal.Parse("50.00")
            };

            Result<Transaction> result = gateway.Transaction.SubmitForPartialSettlement(transaction.Id, submitForSettlementRequest);

            Transaction submittedTransaction = result.Target;

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, submittedTransaction.Status);
            Assert.AreEqual("123*123456789012345678", submittedTransaction.Descriptor.Name);
            Assert.AreEqual("3334445555", submittedTransaction.Descriptor.Phone);
            Assert.AreEqual("ebay.com", submittedTransaction.Descriptor.Url);
        }

        [Test]
        public void SubmitForPartialSettlement_WithInvalidParams()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.FAKE_AMEX_DIRECT_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.AmexPayWithPoints.SUCCESS,
                    ExpirationDate = "05/2008"
                },
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;

            TransactionRequest submitForSettlementRequest = new TransactionRequest
            {
                SharedCustomerId = "invalid",
                Amount = decimal.Parse("50.00")
            };

            Assert.Throws<AuthorizationException>(() => gateway.Transaction.SubmitForPartialSettlement(transaction.Id, submitForSettlementRequest));
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

            string transactionId = gateway.Transaction.Sale(request).Target.Id;
            Transaction transaction = gateway.Transaction.SubmitForSettlement(transactionId, decimal.Parse("50.00")).Target;

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
            Assert.Throws<NotFoundException>(() => gateway.Transaction.SubmitForSettlement("badId"));
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
            gateway.TestTransaction.Settle(transaction.Id);

            Result<Transaction> result;
            try
            {
                result = gateway.Transaction.Refund(transaction.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine("Got exception! " + e.Source);
                throw e;
            }

            Assert.IsTrue(result.IsSuccess());
            var refund = result.Target;

            Assert.AreEqual(TransactionType.CREDIT, refund.Type);
            Assert.AreEqual(transaction.Amount, refund.Amount);

            Transaction firstTransaction = gateway.Transaction.Find(transaction.Id);
            Assert.AreEqual(refund.Id, firstTransaction.RefundIds[0]);
            Assert.AreEqual(firstTransaction.Id, refund.RefundedTransactionId);
        }
        #pragma warning restore 0618

        #pragma warning disable 0618
        [Test]
#if netcore
        public async Task RefundAsync_WithABasicTransaction()
#else
        public void RefundAsync_WithABasicTransaction()
        {
            Task.Run(async () =>
#endif
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

            Result<Transaction> saleResult = await gateway.Transaction.SaleAsync(request);
            Transaction transaction = saleResult.Target;
            gateway.TestTransaction.Settle(transaction.Id);

            Result<Transaction> result;
            try
            {
                result = await gateway.Transaction.RefundAsync(transaction.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine("Got exception! " + e.Source);
                throw e;
            }

            Assert.IsTrue(result.IsSuccess());
            var refund = result.Target;

            Assert.AreEqual(TransactionType.CREDIT, refund.Type);
            Assert.AreEqual(transaction.Amount, refund.Amount);

            Transaction firstTransaction = await gateway.Transaction.FindAsync(transaction.Id);
            Assert.AreEqual(refund.Id, firstTransaction.RefundIds[0]);
            Assert.AreEqual(firstTransaction.Id, refund.RefundedTransactionId);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif
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
            gateway.TestTransaction.Settle(transaction.Id);

            Result<Transaction> result = gateway.Transaction.Refund(transaction.Id, decimal.Parse("500.00"));
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionType.CREDIT, result.Target.Type);
            Assert.AreEqual(decimal.Parse("500.00"), result.Target.Amount);
        }

        [Test]
#if netcore
        public async Task RefundAsync_WithAPartialAmount()
#else
        public void RefundAsync_WithAPartialAmount()
        {
            Task.Run(async () =>
#endif
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

            Result<Transaction> saleResult = await gateway.Transaction.SaleAsync(request);
            Transaction transaction = saleResult.Target;
            await gateway.TestTransaction.SettleAsync(transaction.Id);

            Result<Transaction> result = await gateway.Transaction.RefundAsync(transaction.Id, decimal.Parse("500.00"));
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionType.CREDIT, result.Target.Type);
            Assert.AreEqual(decimal.Parse("500.00"), result.Target.Amount);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Refund_WithSoftDecline()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = decimal.Parse("9000.00"),
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
            gateway.TestTransaction.Settle(transaction.Id);

            Result<Transaction> result = gateway.Transaction.Refund(transaction.Id, decimal.Parse("2046.00"));
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionType.CREDIT, result.Target.Type);
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, result.Target.Status);
            Assert.AreEqual("1005", result.Target.ProcessorResponseCode);
            Assert.AreEqual("Auth Declined but Settlement Captured", result.Target.ProcessorResponseText);
            Assert.AreEqual(ProcessorResponseType.APPROVED, result.Target.ProcessorResponseType);
            Assert.AreEqual("2046 : Declined", result.Target.AdditionalProcessorResponse);
        }

        [Test]
#if netcore
        public async Task RefundAsync_WithSoftDecline()
#else
        public void RefundAsync_WithSoftDecline()
        {
            Task.Run(async () =>
#endif
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = decimal.Parse("9000.00"),
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

            Result<Transaction> saleResult = await gateway.Transaction.SaleAsync(request);
            Transaction transaction = saleResult.Target;
            await gateway.TestTransaction.SettleAsync(transaction.Id);

            Result<Transaction> result = await gateway.Transaction.RefundAsync(transaction.Id, decimal.Parse("2046.00"));
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionType.CREDIT, result.Target.Type);
            Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, result.Target.Status);
            Assert.AreEqual("1005", result.Target.ProcessorResponseCode);
            Assert.AreEqual("Auth Declined but Settlement Captured", result.Target.ProcessorResponseText);
            Assert.AreEqual(ProcessorResponseType.APPROVED, result.Target.ProcessorResponseType);
            Assert.AreEqual("2046 : Declined", result.Target.AdditionalProcessorResponse);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Refund_WithHardDecline()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = decimal.Parse("9000.00"),
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
            gateway.TestTransaction.Settle(transaction.Id);

            Result<Transaction> result = gateway.Transaction.Refund(transaction.Id, decimal.Parse("2004.00"));
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(TransactionType.CREDIT, result.Transaction.Type);
            Assert.AreEqual(TransactionStatus.PROCESSOR_DECLINED, result.Transaction.Status);
            Assert.AreEqual("2004", result.Transaction.ProcessorResponseCode);
            Assert.AreEqual("Expired Card", result.Transaction.ProcessorResponseText);
            Assert.AreEqual(ProcessorResponseType.HARD_DECLINED, result.Transaction.ProcessorResponseType);
            Assert.AreEqual("2004 : Expired Card", result.Transaction.AdditionalProcessorResponse);
        }

        [Test]
#if netcore
        public async Task RefundAsync_WithHardDecline()
#else
        public void RefundAsync_WithHardDecline()
        {
            Task.Run(async () =>
#endif
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = decimal.Parse("9000.00"),
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

            Result<Transaction> saleResult = await gateway.Transaction.SaleAsync(request);
            Transaction transaction = saleResult.Target;
            await gateway.TestTransaction.SettleAsync(transaction.Id);

            Result<Transaction> result = await gateway.Transaction.RefundAsync(transaction.Id, decimal.Parse("2004.00"));
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(TransactionType.CREDIT, result.Transaction.Type);
            Assert.AreEqual(TransactionStatus.PROCESSOR_DECLINED, result.Transaction.Status);
            Assert.AreEqual("2004", result.Transaction.ProcessorResponseCode);
            Assert.AreEqual("Expired Card", result.Transaction.ProcessorResponseText);
            Assert.AreEqual(ProcessorResponseType.HARD_DECLINED, result.Transaction.ProcessorResponseType);
            Assert.AreEqual("2004 : Expired Card", result.Transaction.AdditionalProcessorResponse);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif


        [Test]
        public void Refund_WithOrderId()
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
            gateway.TestTransaction.Settle(transaction.Id);

            TransactionRefundRequest refundRequest = new TransactionRefundRequest()
            {
                OrderId = "1234567"
            };

            Result<Transaction> result = gateway.Transaction.Refund(transaction.Id, refundRequest);
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionType.CREDIT, result.Target.Type);
            Assert.AreEqual("1234567", result.Target.OrderId);
        }

        [Test]
#if netcore
        public async Task RefundAsync_WithOrderId()
#else
        public void RefundAsync_WithOrderId()
        {
            Task.Run(async () =>
#endif
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

            Result<Transaction> saleResult = await gateway.Transaction.SaleAsync(request);
            Transaction transaction = saleResult.Target;
            await gateway.TestTransaction.SettleAsync(transaction.Id);

            TransactionRefundRequest refundRequest = new TransactionRefundRequest()
            {
                OrderId = "1234567"
            };

            Result<Transaction> result = await gateway.Transaction.RefundAsync(transaction.Id, refundRequest);
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionType.CREDIT, result.Target.Type);
            Assert.AreEqual("1234567", result.Target.OrderId);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Refund_WithMerchantAccountId()
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
            gateway.TestTransaction.Settle(transaction.Id);

            TransactionRefundRequest refundRequest = new TransactionRefundRequest()
            {
                MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID
            };

            Result<Transaction> result = gateway.Transaction.Refund(transaction.Id, refundRequest);
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionType.CREDIT, result.Target.Type);
            Assert.AreEqual(MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID, result.Target.MerchantAccountId);
        }

        [Test]
#if netcore
        public async Task RefundAsync_WithMerchantAccountId()
#else
        public void RefundAsync_WithMerchantAccountId()
        {
            Task.Run(async () =>
#endif
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

            Result<Transaction> saleResult = await gateway.Transaction.SaleAsync(request);
            Transaction transaction = saleResult.Target;
            await gateway.TestTransaction.SettleAsync(transaction.Id);

            TransactionRefundRequest refundRequest = new TransactionRefundRequest()
            {
                MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID
            };

            Result<Transaction> result = await gateway.Transaction.RefundAsync(transaction.Id, refundRequest);
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionType.CREDIT, result.Target.Type);
            Assert.AreEqual(MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID, result.Target.MerchantAccountId);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Refund_WithAmountOrderId()
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
            gateway.TestTransaction.Settle(transaction.Id);

            TransactionRefundRequest refundRequest = new TransactionRefundRequest()
            {
                Amount = 500M,
                OrderId = "1234567"
            };

            Result<Transaction> result = gateway.Transaction.Refund(transaction.Id, refundRequest);
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(TransactionType.CREDIT, result.Target.Type);
            Assert.AreEqual("1234567", result.Target.OrderId);
            Assert.AreEqual(500M, result.Target.Amount);
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
            gateway.TestTransaction.Settle(transaction.Id);

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
            catch (Exception e)
            {
                Console.WriteLine("Got exception! " + e.Source);
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

            Assert.AreEqual("Dan", cloneTransaction.CustomerDetails.FirstName);
            Assert.AreEqual("Carl", cloneTransaction.BillingAddress.FirstName);
            Assert.AreEqual("Andrew", cloneTransaction.ShippingAddress.FirstName);

        }

        [Test]
#if netcore
        public async Task CloneTransactionAsync()
#else
        public void CloneTransactionAsync()
        {
            Task.Run(async () =>
#endif
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

            Result<Transaction> result = await gateway.Transaction.SaleAsync(request);
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

            Result<Transaction> cloneResult = await gateway.Transaction.CloneTransactionAsync(transaction.Id, cloneRequest);
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

            Assert.AreEqual("Dan", cloneTransaction.CustomerDetails.FirstName);
            Assert.AreEqual("Carl", cloneTransaction.BillingAddress.FirstName);
            Assert.AreEqual("Andrew", cloneTransaction.ShippingAddress.FirstName);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

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
                    Number = TestUtil.CreditCardNumbers.CardTypeIndicators.Unknown,
                    ExpirationDate = "12/2015",
                }
            };

            Result<Transaction> result = gateway.Transaction.Credit(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(transaction.CreditCard.Business, Braintree.CreditCardBusiness.UNKNOWN);
            Assert.AreEqual(transaction.CreditCard.Commercial, Braintree.CreditCardCommercial.UNKNOWN);
            Assert.AreEqual(transaction.CreditCard.Consumer, Braintree.CreditCardConsumer.UNKNOWN);
            Assert.AreEqual(transaction.CreditCard.Corporate, Braintree.CreditCardCorporate.UNKNOWN);
            Assert.AreEqual(transaction.CreditCard.Debit, Braintree.CreditCardDebit.UNKNOWN);
            Assert.AreEqual(transaction.CreditCard.DurbinRegulated, Braintree.CreditCardDurbinRegulated.UNKNOWN);
            Assert.AreEqual(transaction.CreditCard.Healthcare, Braintree.CreditCardHealthcare.UNKNOWN);
            Assert.AreEqual(transaction.CreditCard.Payroll, Braintree.CreditCardPayroll.UNKNOWN);
            Assert.AreEqual(transaction.CreditCard.Prepaid, Braintree.CreditCardPrepaid.UNKNOWN);
            Assert.AreEqual(transaction.CreditCard.PrepaidReloadable, Braintree.CreditCardPrepaidReloadable.UNKNOWN);
            Assert.AreEqual(transaction.CreditCard.Purchase, Braintree.CreditCardPurchase.UNKNOWN);
        }

        [Test]
        public void CreateTransaction_WithPaymentMethodNonce()
        {
            string nonce = TestHelper.GenerateUnlockedNonce(gateway);
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce
            };
            Result<Transaction> result = gateway.Transaction.Credit(request);
            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void CreateTransaction_WithLocalPaymentNonce()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.LocalPayment,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target.LocalPaymentDetails.PaymentId);
            Assert.IsNotNull(result.Target.LocalPaymentDetails.PayerId);
            Assert.IsNotNull(result.Target.LocalPaymentDetails.FundingSource);
            Assert.IsNotNull(result.Target.LocalPaymentDetails.CaptureId);
            Assert.IsNotNull(result.Target.LocalPaymentDetails.DebugId);
            Assert.IsNotNull(result.Target.LocalPaymentDetails.TransactionFeeAmount);
            Assert.IsNotNull(result.Target.LocalPaymentDetails.TransactionFeeCurrencyIsoCode);
        }

        [Test]
        public void RefundLocalPaymentTransaction()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = Nonce.LocalPayment,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            result = gateway.Transaction.Refund(result.Target.Id);
            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target.LocalPaymentDetails.PaymentId);
            Assert.IsNotNull(result.Target.LocalPaymentDetails.PayerId);
            Assert.IsNotNull(result.Target.LocalPaymentDetails.FundingSource);
            Assert.IsNotNull(result.Target.LocalPaymentDetails.RefundId);
            Assert.IsNotNull(result.Target.LocalPaymentDetails.DebugId);
            Assert.IsNotNull(result.Target.LocalPaymentDetails.RefundFromTransactionFeeAmount);
            Assert.IsNotNull(result.Target.LocalPaymentDetails.RefundFromTransactionFeeCurrencyIsoCode);
        }

        [Test]
        public void CreateTransaction_WithLocalPaymentWebhookContent()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PayPalAccount = new TransactionPayPalRequest()
                {
                    PaymentId = "PAY-1234",
                    PayerId = "PAYER-123"
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual("PAY-1234", result.Target.PayPalDetails.PaymentId);
            Assert.AreEqual("PAYER-123", result.Target.PayPalDetails.PayerId);
            Assert.IsNotNull(result.Target.PayPalDetails.DebugId);
        }

        [Test]
        public void CreateTransaction_WithPayeeId()
        {
            string nonce = TestHelper.GenerateOneTimePayPalNonce(gateway);
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce,
                PayPalAccount = new TransactionPayPalRequest()
                {
                    PayeeId = "fake-payee-id"
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target.PayPalDetails.PayerEmail);
            Assert.IsNotNull(result.Target.PayPalDetails.PaymentId);
            Assert.IsNotNull(result.Target.PayPalDetails.AuthorizationId);
            Assert.IsNotNull(result.Target.PayPalDetails.ImageUrl);
            Assert.AreEqual("fake-payee-id", result.Target.PayPalDetails.PayeeId);
            Assert.IsNull(result.Target.PayPalDetails.Token);
            Assert.IsNotNull(result.Target.PayPalDetails.DebugId);
        }

        [Test]
        public void CreateTransaction_WithPayeeIdInOptionsParams()
        {
            string nonce = TestHelper.GenerateOneTimePayPalNonce(gateway);
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce,
                PayPalAccount = new TransactionPayPalRequest()
                {
                },
                Options = new TransactionOptionsRequest()
                {
                    PayeeId = "fake-payee-id"
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target.PayPalDetails.PayerEmail);
            Assert.IsNotNull(result.Target.PayPalDetails.PaymentId);
            Assert.IsNotNull(result.Target.PayPalDetails.AuthorizationId);
            Assert.IsNotNull(result.Target.PayPalDetails.ImageUrl);
            Assert.AreEqual("fake-payee-id", result.Target.PayPalDetails.PayeeId);
            Assert.IsNull(result.Target.PayPalDetails.Token);
            Assert.IsNotNull(result.Target.PayPalDetails.DebugId);
        }

        [Test]
        public void CreateTransaction_WithPayeeIdInOptionsPaypal()
        {
            var nonce = TestHelper.GenerateOneTimePayPalNonce(gateway);
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest()
                {
                    PayPal = new TransactionOptionsPayPalRequest()
                    {
                        PayeeId = "fake-payee-id"
                    }
                }
            };

            var result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target.PayPalDetails.PayerEmail);
            Assert.IsNotNull(result.Target.PayPalDetails.PaymentId);
            Assert.IsNotNull(result.Target.PayPalDetails.AuthorizationId);
            Assert.IsNotNull(result.Target.PayPalDetails.ImageUrl);
            Assert.AreEqual("fake-payee-id", result.Target.PayPalDetails.PayeeId);
            Assert.IsNull(result.Target.PayPalDetails.Token);
            Assert.IsNotNull(result.Target.PayPalDetails.DebugId);
        }

        [Test]
        public void CreateTransaction_WithPayeeEmail()
        {
            string nonce = TestHelper.GenerateOneTimePayPalNonce(gateway);
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce,
                PayPalAccount = new TransactionPayPalRequest()
                {
                    PayeeEmail = "foo@example.com"
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target.PayPalDetails.PayerEmail);
            Assert.IsNotNull(result.Target.PayPalDetails.PaymentId);
            Assert.IsNotNull(result.Target.PayPalDetails.AuthorizationId);
            Assert.IsNotNull(result.Target.PayPalDetails.ImageUrl);
            Assert.AreEqual("foo@example.com", result.Target.PayPalDetails.PayeeEmail);
            Assert.IsNull(result.Target.PayPalDetails.Token);
            Assert.IsNotNull(result.Target.PayPalDetails.DebugId);
        }

        [Test]
        public void CreateTransaction_WithPayeeEmailInOptionsParams()
        {
            string nonce = TestHelper.GenerateOneTimePayPalNonce(gateway);
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce,
                PayPalAccount = new TransactionPayPalRequest()
                {
                },
                Options = new TransactionOptionsRequest()
                {
                    PayeeEmail = "foo@example.com"
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target.PayPalDetails.PayerEmail);
            Assert.IsNotNull(result.Target.PayPalDetails.PaymentId);
            Assert.IsNotNull(result.Target.PayPalDetails.AuthorizationId);
            Assert.IsNotNull(result.Target.PayPalDetails.ImageUrl);
            Assert.AreEqual("foo@example.com", result.Target.PayPalDetails.PayeeEmail);
            Assert.IsNull(result.Target.PayPalDetails.Token);
            Assert.IsNotNull(result.Target.PayPalDetails.DebugId);
        }

        [Test]
        public void CreateTransaction_WithPayeeEmailInOptionsPaypal()
        {
            var nonce = TestHelper.GenerateOneTimePayPalNonce(gateway);
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest()
                {
                    PayPal = new TransactionOptionsPayPalRequest()
                    {
                        PayeeEmail = "bar@example.com"
                    }
                }
            };

            var result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target.PayPalDetails.PayerEmail);
            Assert.IsNotNull(result.Target.PayPalDetails.PaymentId);
            Assert.IsNotNull(result.Target.PayPalDetails.AuthorizationId);
            Assert.IsNotNull(result.Target.PayPalDetails.ImageUrl);
            Assert.AreEqual("bar@example.com", result.Target.PayPalDetails.PayeeEmail);
            Assert.IsNull(result.Target.PayPalDetails.Token);
            Assert.IsNotNull(result.Target.PayPalDetails.DebugId);
        }

        [Test]
        public void CreateTransaction_WithPayPalCustomField()
        {
            var nonce = TestHelper.GenerateOneTimePayPalNonce(gateway);
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest()
                {
                    PayPal = new TransactionOptionsPayPalRequest()
                    {
                        CustomField = "custom field stuff"
                    }
                }
            };

            var result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target.PayPalDetails.PayerEmail);
            Assert.IsNotNull(result.Target.PayPalDetails.PaymentId);
            Assert.IsNotNull(result.Target.PayPalDetails.AuthorizationId);
            Assert.IsNotNull(result.Target.PayPalDetails.ImageUrl);
            Assert.AreEqual("custom field stuff", result.Target.PayPalDetails.CustomField);
            Assert.IsNull(result.Target.PayPalDetails.Token);
            Assert.IsNotNull(result.Target.PayPalDetails.DebugId);
        }

        [Test]
        public void CreateTransaction_WithPayPalSupplementaryData()
        {
            var nonce = TestHelper.GenerateOneTimePayPalNonce(gateway);
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest()
                {
                    PayPal = new TransactionOptionsPayPalRequest()
                    {
                        SupplementaryData = new Dictionary<string, string>
                        {
                            { "key1", "value1" },
                            { "key2", "value2" }
                        }
                    }
                }
            };

            // note - supplementary data is not returned in response
            var result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void CreateTransaction_WithPayPalDescription()
        {
            var nonce = TestHelper.GenerateOneTimePayPalNonce(gateway);
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest()
                {
                    PayPal = new TransactionOptionsPayPalRequest()
                    {
                        Description = "Product Description"
                    }
                }
            };

            var result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual("Product Description", result.Target.PayPalDetails.Description);
        }

        [Test]
        public void CreateTransaction_WithOneTimePayPalNonce()
        {
            string nonce = TestHelper.GenerateOneTimePayPalNonce(gateway);
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target.PayPalDetails.PayerEmail);
            Assert.IsNotNull(result.Target.PayPalDetails.PaymentId);
            Assert.IsNotNull(result.Target.PayPalDetails.AuthorizationId);
            Assert.IsNotNull(result.Target.PayPalDetails.ImageUrl);
            Assert.IsNull(result.Target.PayPalDetails.Token);
            Assert.IsNotNull(result.Target.PayPalDetails.DebugId);
        }

        [Test]
        public void CreateTransaction_WithOneTimePayPalNonceAndAttemptToVault()
        {
            string nonce = TestHelper.GenerateOneTimePayPalNonce(gateway);
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest
                {
                    StoreInVault = true
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target.PayPalDetails.PayerEmail);
            Assert.IsNotNull(result.Target.PayPalDetails.PaymentId);
            Assert.IsNotNull(result.Target.PayPalDetails.AuthorizationId);
            Assert.IsNull(result.Target.PayPalDetails.Token);
            Assert.IsNotNull(result.Target.PayPalDetails.DebugId);
        }

        [Test]
        public void CreateTransaction_WithFuturePayPalNonceAndAttemptToVault()
        {
            string nonce = TestHelper.GenerateFuturePaymentPayPalNonce(gateway);
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest
                {
                    StoreInVault = true
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target.PayPalDetails.PayerEmail);
            Assert.IsNotNull(result.Target.PayPalDetails.PaymentId);
            Assert.IsNotNull(result.Target.PayPalDetails.AuthorizationId);
            Assert.IsNotNull(result.Target.PayPalDetails.Token);
            Assert.IsNotNull(result.Target.PayPalDetails.DebugId);
        }

        [Test]
        public void CreateTransaction_WithPayPalBillingAgreementNonceAndAttemptToVault()
        {
            string nonce = Nonce.PayPalBillingAgreement;
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest
                {
                    StoreInVaultOnSuccess = true
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target.PayPalDetails.PayerEmail);
            Assert.IsNotNull(result.Target.PayPalDetails.PaymentId);
            Assert.IsNotNull(result.Target.PayPalDetails.AuthorizationId);
            Assert.IsNotNull(result.Target.PayPalDetails.Token);
            Assert.IsNotNull(result.Target.PayPalDetails.DebugId);
            Assert.IsNotNull(result.Target.PayPalDetails.BillingAgreementId);
        }

        [Test]
        public void Void_PayPalTransaction()
        {
            string nonce = TestHelper.GenerateFuturePaymentPayPalNonce(gateway);
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Result<Transaction> voidResult = gateway.Transaction.Void(result.Target.Id);
            Assert.IsTrue(voidResult.IsSuccess());
        }

        [Test]
        public void SubmitForSettlement_PayPalTransaction()
        {
            string nonce = TestHelper.GenerateFuturePaymentPayPalNonce(gateway);
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Result<Transaction> settlementResult = gateway.Transaction.SubmitForSettlement(result.Target.Id);
            Assert.IsTrue(settlementResult.IsSuccess());
            Assert.AreEqual(TransactionStatus.SETTLING, settlementResult.Target.Status);
        }

        [Test]
        public void Refund_PayPalTransaction()
        {
            string nonce = TestHelper.GenerateFuturePaymentPayPalNonce(gateway);
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            var id = result.Target.Id;

            Result<Transaction> refundResult = gateway.Transaction.Refund(id);
            Assert.IsTrue(refundResult.IsSuccess());
        }

        [Test]
        public void PayPalTransactionsReturnRequiredFields()
        {
            Transaction transaction = gateway.Transaction.Find("settledtransaction");

            Assert.IsNotNull(transaction.PayPalDetails.DebugId);
            Assert.IsNotNull(transaction.PayPalDetails.PayerEmail);
            Assert.IsNotNull(transaction.PayPalDetails.AuthorizationId);
            Assert.IsNotNull(transaction.PayPalDetails.PayerId);
            Assert.IsNotNull(transaction.PayPalDetails.PayerFirstName);
            Assert.IsNotNull(transaction.PayPalDetails.PayerLastName);
            Assert.IsNotNull(transaction.PayPalDetails.PayerStatus);
            Assert.IsNotNull(transaction.PayPalDetails.SellerProtectionStatus);
            Assert.IsNotNull(transaction.PayPalDetails.CaptureId);
            Assert.IsNotNull(transaction.PayPalDetails.RefundId);
            Assert.IsNotNull(transaction.PayPalDetails.TransactionFeeAmount);
            Assert.IsNotNull(transaction.PayPalDetails.TransactionFeeCurrencyIsoCode);
            Assert.IsNotNull(transaction.PayPalDetails.RefundFromTransactionFeeAmount);
            Assert.IsNotNull(transaction.PayPalDetails.RefundFromTransactionFeeCurrencyIsoCode);
        }

        [Test]
        public void SharedVaultWithPaymentMethodToken() {
            var sharerGateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_public_id",
                PublicKey = "oauth_app_partner_user_public_key",
                PrivateKey = "oauth_app_partner_user_private_key"
            };
            var customerRequest = new CustomerRequest
            {CreditCard = new CreditCardRequest
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/19",
                    BillingAddress = new CreditCardAddressRequest()
                    {
                        PostalCode = "94107"
                }
            }};

            Customer customer = sharerGateway.Customer.Create(customerRequest).Target;
            CreditCard card = customer.CreditCards[0];
            Address billingAddress = card.BillingAddress;
            Address shippingAddress = customer.Addresses[0];

            BraintreeGateway oauthGateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );
            string code = OAuthTestHelper.CreateGrant(oauthGateway, "integration_merchant_id", "shared_vault_transactions");
            ResultImpl<OAuthCredentials> accessTokenResult = oauthGateway.OAuth.CreateTokenFromCode(new OAuthCredentialsRequest {
                Code = code,
                Scope = "shared_vault_transactions"
            });

            gateway = new BraintreeGateway(accessTokenResult.Target.AccessToken);
            var request = new TransactionRequest {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                SharedPaymentMethodToken = card.Token,
                SharedCustomerId = customer.Id,
                SharedShippingAddressId = shippingAddress.Id,
                SharedBillingAddressId = billingAddress.Id
            };
            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(transactionResult.IsSuccess());
        }

        [Test]
        public void SharedVaultWithPaymentMethodNonce() {
            var sharerGateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_public_id",
                PublicKey = "oauth_app_partner_user_public_key",
                PrivateKey = "oauth_app_partner_user_private_key"
            };
            var customerRequest = new CustomerRequest
            {CreditCard = new CreditCardRequest
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/19",
                    BillingAddress = new CreditCardAddressRequest()
                    {
                        PostalCode = "94107"
                }
            }};

            Customer customer = sharerGateway.Customer.Create(customerRequest).Target;
            CreditCard card = customer.CreditCards[0];
            Address billingAddress = card.BillingAddress;
            Address shippingAddress = customer.Addresses[0];

            BraintreeGateway oauthGateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );
            string code = OAuthTestHelper.CreateGrant(oauthGateway, "integration_merchant_id", "shared_vault_transactions");
            ResultImpl<OAuthCredentials> accessTokenResult = oauthGateway.OAuth.CreateTokenFromCode(new OAuthCredentialsRequest {
                Code = code,
                Scope = "shared_vault_transactions"
            });

            string sharedNonce = sharerGateway.PaymentMethodNonce.Create(card.Token).Target.Nonce;

            gateway = new BraintreeGateway(accessTokenResult.Target.AccessToken);
            var request = new TransactionRequest {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                SharedPaymentMethodNonce = sharedNonce,
                SharedCustomerId = customer.Id,
                SharedShippingAddressId = shippingAddress.Id,
                SharedBillingAddressId = billingAddress.Id
            };
            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(transactionResult.IsSuccess());

            Transaction transaction = transactionResult.Target;
            Assert.AreEqual(transaction.FacilitatedDetails.MerchantId, "integration_merchant_id");
            Assert.AreEqual(transaction.FacilitatedDetails.MerchantName, "14ladders");
            Assert.AreEqual(transaction.FacilitatedDetails.PaymentMethodNonce, null);
            Assert.AreEqual(transaction.FacilitatorDetails.OauthApplicationClientId, "client_id$development$integration_client_id");
            Assert.AreEqual(transaction.FacilitatorDetails.OauthApplicationName, "PseudoShop");
        }


        [Test]
        public void PaymentMethodGrantIncludeBillingPostalCode() {
            var partnerMerchantGateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_public_id",
                PublicKey = "oauth_app_partner_user_public_key",
                PrivateKey = "oauth_app_partner_user_private_key"
            };
            var customerRequest = new CustomerRequest
            {
                CreditCard = new CreditCardRequest
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/19",
                    BillingAddress = new CreditCardAddressRequest()
                    {
                        PostalCode = "94107"
                    }
                }
            };

            Customer customer = partnerMerchantGateway.Customer.Create(customerRequest).Target;
            CreditCard creditCard = customer.CreditCards[0];
            var token = creditCard.Token;

            BraintreeGateway oauthGateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );
            string code = OAuthTestHelper.CreateGrant(oauthGateway, "integration_merchant_id", "grant_payment_method");
            ResultImpl<OAuthCredentials> accessTokenResult = oauthGateway.OAuth.CreateTokenFromCode(new OAuthCredentialsRequest {
                Code = code,
                Scope = "grant_payment_method"
            });

            BraintreeGateway accessTokenGateway = new BraintreeGateway(accessTokenResult.Target.AccessToken);
            PaymentMethodGrantRequest optionsRequest = new PaymentMethodGrantRequest()
            {
                AllowVaulting = false,
                IncludeBillingPostalCode = true
            };

            Result<PaymentMethodNonce> grantResult = accessTokenGateway.PaymentMethod.Grant(token, optionsRequest);
            var request = new TransactionRequest {
                Amount = 100M,
                PaymentMethodNonce = grantResult.Target.Nonce
            };

            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(transactionResult.IsSuccess());
            Assert.AreEqual(transactionResult.Target.BillingAddress.PostalCode, "94107");
        }

        [Test]
        public void PaymentMethodGrantIncludesFacilitatedInformation() {
            var partnerMerchantGateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_public_id",
                PublicKey = "oauth_app_partner_user_public_key",
                PrivateKey = "oauth_app_partner_user_private_key"
            };
            var customerRequest = new CustomerRequest
            {
                CreditCard = new CreditCardRequest
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/19"
                }
            };

            Customer customer = partnerMerchantGateway.Customer.Create(customerRequest).Target;
            CreditCard creditCard = customer.CreditCards[0];
            var token = creditCard.Token;

            BraintreeGateway oauthGateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );
            string code = OAuthTestHelper.CreateGrant(oauthGateway, "integration_merchant_id", "grant_payment_method");
            ResultImpl<OAuthCredentials> accessTokenResult = oauthGateway.OAuth.CreateTokenFromCode(new OAuthCredentialsRequest {
                Code = code,
                Scope = "grant_payment_method"
            });

            BraintreeGateway accessTokenGateway = new BraintreeGateway(accessTokenResult.Target.AccessToken);
            PaymentMethodGrantRequest optionsRequest = new PaymentMethodGrantRequest()
            {
                AllowVaulting = false
            };

            Result<PaymentMethodNonce> grantResult = accessTokenGateway.PaymentMethod.Grant(token, optionsRequest);
            var request = new TransactionRequest {
                Amount = 100M,
                PaymentMethodNonce = grantResult.Target.Nonce
            };

            var transactionResult = gateway.Transaction.Sale(request);
            Transaction transaction = transactionResult.Target;
            Assert.IsTrue(transactionResult.IsSuccess());
            Assert.AreEqual(transaction.FacilitatedDetails.MerchantId, "integration_merchant_id");
            Assert.AreEqual(transaction.FacilitatedDetails.MerchantName, "14ladders");
            Assert.AreEqual(transaction.FacilitatedDetails.PaymentMethodNonce, grantResult.Target.Nonce);
            Assert.AreEqual(transaction.FacilitatorDetails.OauthApplicationClientId, "client_id$development$integration_client_id");
            Assert.AreEqual(transaction.FacilitatorDetails.OauthApplicationName, "PseudoShop");
            Assert.AreEqual(transaction.FacilitatorDetails.SourcePaymentMethodToken, token);
        }
        [Test]
        public void PayPalHereParseAttributesForAuthCapture() {
            Transaction transaction = gateway.Transaction.Find("paypal_here_auth_capture_id");
            Assert.AreEqual(transaction.PaymentInstrumentType, PaymentInstrumentType.PAYPAL_HERE);

            Assert.IsNotNull(transaction.PayPalHereDetails);
            Assert.IsNotNull(transaction.PayPalHereDetails.AuthorizationId);
            Assert.IsNotNull(transaction.PayPalHereDetails.CaptureId);
            Assert.IsNotNull(transaction.PayPalHereDetails.InvoiceId);
            Assert.IsNotNull(transaction.PayPalHereDetails.Last4);
            Assert.IsNotNull(transaction.PayPalHereDetails.PaymentType);
            Assert.IsNotNull(transaction.PayPalHereDetails.TransactionFeeAmount);
            Assert.IsNotNull(transaction.PayPalHereDetails.TransactionFeeCurrencyIsoCode);
            Assert.IsNotNull(transaction.PayPalHereDetails.TransactionInitiationDate);
            Assert.IsNotNull(transaction.PayPalHereDetails.TransactionUpdatedDate);
        }

        [Test]
        public void PayPalHereParseAttributesForSale() {
            Transaction transaction = gateway.Transaction.Find("paypal_here_sale_id");

            Assert.IsNotNull(transaction.PayPalHereDetails);
            Assert.IsNotNull(transaction.PayPalHereDetails.PaymentId);
        }

        [Test]
        public void PayPalHereParseAttributesForRefund() {
            Transaction transaction = gateway.Transaction.Find("paypal_here_refund_id");

            Assert.IsNotNull(transaction.PayPalHereDetails);
            Assert.IsNotNull(transaction.PayPalHereDetails.RefundId);
        }

        [Test]
        public void Sale_NetworkTokenizedTransaction() {
            var request = new TransactionRequest {
                Amount = 100,
                PaymentMethodToken = "network_tokenized_credit_card"
            };

            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(transactionResult.IsSuccess());
            Transaction transaction = transactionResult.Target;
            Assert.IsTrue(transaction.ProcessedWithNetworkToken);
            Assert.IsFalse(transaction.Retried);
        }
 
        [Test]
        public void Sale_Retried_NetworkTokenizedTransaction() {
            var request = new TransactionRequest {
                Amount = 2000,
                PaymentMethodToken = "network_tokenized_credit_card",
                MerchantAccountId = "ma_transaction_multiple_retries"
            };

            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsFalse(transactionResult.IsSuccess());
            Assert.IsTrue(transactionResult.Transaction.Retried);
            Assert.IsNotNull(transactionResult.Transaction.RetryIds);
            Assert.IsTrue(transactionResult.Transaction.RetryIds.Count > 1);
            Assert.IsNull(transactionResult.Transaction.RetriedTransactionId);
            Assert.IsTrue(transactionResult.Transaction.ProcessedWithNetworkToken);

            var retry_transaction_id_1 = transactionResult.Transaction.RetryIds[0];
            var retry_transaction_id_2 = transactionResult.Transaction.RetryIds[1];

            var searchRequest = new TransactionSearchRequest().
                Id.Is(retry_transaction_id_1).
                Amount.Is(2000);

            ResourceCollection<Transaction> collection = gateway.Transaction.Search(searchRequest);
            Assert.AreEqual(1, collection.MaximumCount);
            Assert.IsNotNull(collection.FirstItem.RetriedTransactionId);
            Assert.IsEmpty(collection.FirstItem.RetryIds);
            Assert.AreEqual(collection.FirstItem.RetriedTransactionId, transactionResult.Transaction.Id);

            var searchRequest_2 = new TransactionSearchRequest().
                Id.Is(retry_transaction_id_2).
                Amount.Is(2000);

            ResourceCollection<Transaction> collection_2 = gateway.Transaction.Search(searchRequest_2);
            Assert.AreEqual(1, collection_2.MaximumCount);
            Assert.IsNotNull(collection_2.FirstItem.RetriedTransactionId);
            Assert.IsEmpty(collection_2.FirstItem.RetryIds);
            Assert.AreEqual(collection_2.FirstItem.RetriedTransactionId, transactionResult.Transaction.Id);
        }

        [Test]
        public void Sale_NotAvailableForRetry()
        {
            var request = new TransactionRequest
            {
                Amount = 100,
                MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "06/2009",
                }
            };

            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(transactionResult.IsSuccess());
            Transaction transaction = transactionResult.Target;
            Assert.IsFalse(transaction.Retried);
        }

        [Test]
        public void Sale_NonNetworkTokenizedTransaction() {
            var request = new TransactionRequest {
                Amount = 100,
                PaymentMethodNonce = Nonce.TransactableVisa
            };

            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(transactionResult.IsSuccess());
            Transaction transaction = transactionResult.Target;
            Assert.IsFalse(transaction.ProcessedWithNetworkToken);
        }

        [Test]
        public void Sale_WithMerchantCurrency()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CurrencyIsoCode = "USD",
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
            Assert.IsNotNull(transaction.AuthorizationExpiresAt);
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
            Assert.IsNull(transaction.AcquirerReferenceNumber);
        }

        [Test]
        public void Sale_CreateErrorTaxAmountIsRequiredForAibSwedish()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.AIB_SWE_MA,
                CurrencyIsoCode = "SEK",
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2030",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
               ValidationErrorCode.TRANSACTION_TAX_AMOUNT_IS_REQUIRED_FOR_AIB_SWEDISH,
               result.Errors.DeepAll()[0].Code
           );
        }

        [Test]
        public void Sale_WithPaymentMethodTokenAndCreateErrorTaxAmountIsRequiredForAibSwedish()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CVV = "123",
                Number = "5105105105105100",
                ExpirationDate = "05/2030"
            };

            CreditCard creditCard = gateway.CreditCard.Create(creditCardRequest).Target;

            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = MerchantAccountIDs.AIB_SWE_MA,
                PaymentMethodToken = creditCard.Token,
                CurrencyIsoCode = "SEK"
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
               ValidationErrorCode.TRANSACTION_TAX_AMOUNT_IS_REQUIRED_FOR_AIB_SWEDISH,
               result.Errors.DeepAll()[0].Code
           );
        }

        [Test]
        public void Sale_WithInvalidMerchantCurrency()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CurrencyIsoCode = "GBP",
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
               ValidationErrorCode.TRANSACTION_INVALID_PRESENTMENT_CURRENCY,
               result.Errors.DeepAll()[0].Code
           );
        }

        [Test]
        public void Sale_WithNonceAndMerchantCurrency()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CurrencyIsoCode = "USD",
                PaymentMethodNonce = TestHelper.GenerateUnlockedNonce(gateway)
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void Sale_WithNonceAndInvalidMerchantCurrency()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CurrencyIsoCode = "GBP",
                PaymentMethodNonce = TestHelper.GenerateUnlockedNonce(gateway)
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
               ValidationErrorCode.TRANSACTION_INVALID_PRESENTMENT_CURRENCY,
               result.Errors.DeepAll()[0].Code
           );
        }

        [Test]
        public void Sale_WithPaymentMethodTokenAndMerchantCurrency()
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
                PaymentMethodToken = creditCard.Token,
                CurrencyIsoCode = "USD"
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(creditCard.Token, transaction.CreditCard.Token);
            Assert.AreEqual("510510", transaction.CreditCard.Bin);
            Assert.AreEqual("05/2012", transaction.CreditCard.ExpirationDate);
        }

        [Test]
        public void Sale_WithPaymentMethodTokenAndInvalidMerchantCurrency()
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
                PaymentMethodToken = creditCard.Token,
                CurrencyIsoCode = "GBP"
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
               ValidationErrorCode.TRANSACTION_INVALID_PRESENTMENT_CURRENCY,
               result.Errors.DeepAll()[0].Code
           );
        }

        [Test]
        public void Sale_TransactionWithInstallmentCount() {
            var request = new TransactionRequest {
                Amount = 100,
                CreditCard = new TransactionCreditCardRequest
                {
                   ExpirationDate = "05/2012",
                   Number = SandboxValues.CreditCardNumber.VISA
                },
                MerchantAccountId = MerchantAccountIDs.CARD_PROCESSOR_BRAZIL_MERCHANT_ACCOUNT_ID,
                InstallmentRequest = new InstallmentRequest {
                   Count = "4"
                } 
           };

           var transactionResult = gateway.Transaction.Sale(request);
           Assert.IsTrue(transactionResult.IsSuccess());
           Transaction transaction = transactionResult.Target;
           Assert.AreEqual(4, transaction.InstallmentCount);
        }

        [Test]
        public void Sale_TransactionWithInstallmentAdjustments() {
            var request = new TransactionRequest {
                Amount = 100.00M,
                       CreditCard = new TransactionCreditCardRequest
                       {
                           ExpirationDate = "05/2012",
                           Number = SandboxValues.CreditCardNumber.VISA
                       },
                       MerchantAccountId = MerchantAccountIDs.CARD_PROCESSOR_BRAZIL_MERCHANT_ACCOUNT_ID,
                       InstallmentRequest = new InstallmentRequest {
                           Count = "4"
                       }, 
                       Options = new TransactionOptionsRequest
                       {
                           SubmitForSettlement = true
                       }
            };

            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(transactionResult.IsSuccess());
            Transaction transaction = transactionResult.Target;
            gateway.TestTransaction.Settle(transaction.Id);

            Transaction refund = gateway.Transaction.Refund(transaction.Id, decimal.Parse("20.00")).Target;
            for (int i = 0; i < transaction.Installments.Count; i++)
            {
                Assert.AreEqual($"{transaction.Id}_INST_{i+1}", transaction.Installments[i].Id);
                Assert.AreEqual(25.00M, transaction.Installments[i].Amount);
            }

            for (int i = 0; i < refund.RefundedInstallments.Count; i++)
            {
                Assert.AreEqual(-5.00M, refund.RefundedInstallments[i].Adjustments[0].Amount);
                Assert.AreEqual(Kind.REFUND, refund.RefundedInstallments[i].Adjustments[0].Kind);
            }
        }

        [Test]
        public void Successful_ManualKeyEntryTransaction() {
            var request = new TransactionRequest {
                Amount = 100,
                CreditCard = new TransactionCreditCardRequest
                {
                    PaymentReaderCardDetails = new PaymentReaderCardDetailsRequest
                    {
                        EncryptedCardData = "8F34DFB312DC79C24FD5320622F3E11682D79E6B0C0FD881",
                        KeySerialNumber = "FFFFFF02000572A00005",
                    }
                },
            };

            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(transactionResult.IsSuccess());
        }

        [Test]
        public void Unsucccessful_ManualKeyEntryTransaction() {
            var request = new TransactionRequest {
                Amount = 100,
                CreditCard = new TransactionCreditCardRequest
                {
                    PaymentReaderCardDetails = new PaymentReaderCardDetailsRequest
                    {
                        EncryptedCardData = "invalid",
                        KeySerialNumber = "invalid",
                    }
                },
            };

            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsFalse(transactionResult.IsSuccess());
            var transactionError = transactionResult.Errors.ForObject("Transaction").OnField("merchantAccountId")[0];
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_PAYMENT_INSTRUMENT_NOT_SUPPORTED_BY_MERCHANT_ACCOUNT, transactionError.Code);
        }

        [Test]
        public void Successful_ExternalNetworkTokenTransaction() {
            var request = new TransactionRequest {
                Amount = 100,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/2012",
                    NetworkTokenizationAttributes = new NetworkTokenizationAttributesRequest
                    {
                        Cryptogram = "8F34DFB312DC79C24FD5320622F3E11682D79E6B0C0FD881",
                        EcommerceIndicator = "05",
                        TokenRequestorId = "1234567"
                    }
                },
            };

            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(transactionResult.IsSuccess());
            Transaction transaction = transactionResult.Target;

            Assert.IsTrue(transaction.ProcessedWithNetworkToken);
            Assert.IsTrue(transaction.NetworkToken.IsNetworkTokenized);
        }

        [Test]
        public void Unsuccessful_ExternalNetworkTokenTransaction() {
            var request = new TransactionRequest {
                Amount = 100,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/2012",
                    NetworkTokenizationAttributes = new NetworkTokenizationAttributesRequest
                    {
                        EcommerceIndicator = "05",
                        TokenRequestorId = "1234567"
                    }
                },
            };

            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsFalse(transactionResult.IsSuccess());
            var transactionError = transactionResult.Errors.ForObject("Transaction").ForObject("CreditCard").OnField("NetworkTokenizationAttributes")[0];
            Assert.AreEqual(
                ValidationErrorCode.CREDIT_CARD_NETWORK_TOKENIZATION_ATTRIBUTE_CRYPTOGRAM_IS_REQUIRED,
                transactionError.Code
            );
        }

        [Test]
        public void Successful_AdjustAuthorization() {
            var request = new TransactionRequest {
                MerchantAccountId = MerchantAccountIDs.FAKE_FIRST_DATA_MERCHANT_ACCOUNT_ID,
                Amount = 75.50M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/2012",
                },
            };

            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(transactionResult.IsSuccess());
            Transaction transaction = transactionResult.Target;

            var adjustAuthorizedResult = gateway.Transaction.AdjustAuthorization(transaction.Id, decimal.Parse("85.50"));

            Assert.IsTrue(adjustAuthorizedResult.IsSuccess());
            Transaction adjustAuthorizedTransaction = adjustAuthorizedResult.Target;
            Assert.AreEqual(85.50M, adjustAuthorizedTransaction.Amount);
        }

        [Test]
#if netcore
        public async Task Successful_AdjustAuthorizationeAsync()
#else
        public void Successful_AdjustAuthorizationAsync()
        {
            Task.Run(async () =>
#endif
        {
            var request = new TransactionRequest {
                MerchantAccountId = MerchantAccountIDs.FAKE_FIRST_DATA_MERCHANT_ACCOUNT_ID,
                Amount = 75.50M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/2012",
                },
            };

            var transactionResult = await gateway.Transaction.SaleAsync(request);
            Assert.IsTrue(transactionResult.IsSuccess());
            Transaction transaction = transactionResult.Target;

            var adjustAuthorizedResult = await gateway.Transaction.AdjustAuthorizationAsync(transaction.Id, decimal.Parse("85.50"));

            Assert.IsTrue(adjustAuthorizedResult.IsSuccess());
            Transaction adjustAuthorizedTransaction = adjustAuthorizedResult.Target;
            Assert.AreEqual(85.50M, adjustAuthorizedTransaction.Amount);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void AdjustAuthorization_OnProcessorDoesNotSupport() {
            var request = new TransactionRequest {
                MerchantAccountId = MerchantAccountIDs.DEFAULT_MERCHANT_ACCOUNT_ID,
                Amount = 75.50M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "06/2009",
                },
            };

            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(transactionResult.IsSuccess());
            Transaction transaction = transactionResult.Target;

            var adjustAuthorizedResult = gateway.Transaction.AdjustAuthorization(transaction.Id, decimal.Parse("85.50"));

            Assert.IsFalse(adjustAuthorizedResult.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.PROCESSOR_DOES_NOT_SUPPORT_AUTH_ADJUSTMENT, adjustAuthorizedResult.Errors.ForObject("Transaction").OnField("base")[0].Code);
        }

        [Test]
        public void AdjustAuthorization_OnSubmittedAmountIsSameAsAuthorizedAmount() {
            var request = new TransactionRequest {
                MerchantAccountId = MerchantAccountIDs.FAKE_FIRST_DATA_MERCHANT_ACCOUNT_ID,
                Amount = 75.50M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/2012",
                },
            };

            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(transactionResult.IsSuccess());
            Transaction transaction = transactionResult.Target;

            var adjustAuthorizedResult = gateway.Transaction.AdjustAuthorization(transaction.Id, decimal.Parse("75.50"));

            Assert.IsFalse(adjustAuthorizedResult.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.NO_NET_AMOUNT_TO_PERFORM_AUTH_ADJUSTMENT, adjustAuthorizedResult.Errors.ForObject("AuthorizationAdjustment").OnField("Base")[0].Code);
        }

        [Test]
        public void AdjustAuthorization_OnTransactionAuthorizationTypeIsUndefinedOrFinal() {
            var request = new TransactionRequest {
                MerchantAccountId = MerchantAccountIDs.FAKE_FIRST_DATA_MERCHANT_ACCOUNT_ID,
                Amount = 75.50M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/2012",
                },
                TransactionSource = "recurring",
            };

            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(transactionResult.IsSuccess());
            Transaction transaction = transactionResult.Target;

            var adjustAuthorizedResult = gateway.Transaction.AdjustAuthorization(transaction.Id, decimal.Parse("85.50"));

            Assert.IsFalse(adjustAuthorizedResult.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.TRANSACTION_IS_NOT_ELIGIBLE_FOR_ADJUSTMENT, adjustAuthorizedResult.Errors.ForObject("Transaction").OnField("Base")[0].Code);
        }

        [Test]
        public void Sale_ReturnsSuccessfulResponseWithForeignRetailerWhenSetAsTrueInTheRequest()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2030"
                },
                ForeignRetailer = true
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.Status);
            Assert.IsTrue(transaction.ForeignRetailer);
        }

        [Test]
        public void Sale_ReturnsSuccessfulResponseWithoutForeignRetailerWhenSetAsFalseInTheRequest()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2030"
                },
                ForeignRetailer = false
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.Status);
            Assert.IsNull(transaction.ForeignRetailer);
        }

        [Test]
        public void Sale_ReturnsSuccessfulResponseWithoutForeignRetailerWhenNotSetInTheRequest()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2030"
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.Status);
            Assert.IsNull(transaction.ForeignRetailer);
        }
       
        [Test]
        public void ContactDetails_ReturnedFromTransaction()
        {
            string nonce = TestHelper.GenerateOneTimePayPalNonce(gateway);
            TransactionRequest request = new TransactionRequest
            {
                Amount = 10.0M,
                PaymentMethodNonce = nonce,
                PayPalAccount = new TransactionPayPalRequest()
                {
                },
                Options = new TransactionOptionsRequest()
                {},
            
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target.PayPalDetails.RecipientEmail);
            Assert.IsNotNull(result.Target.PayPalDetails.RecipientPhone);
            Assert.IsNotNull(result.Target.PayPalDetails.PaymentId);
            Assert.IsNotNull(result.Target.PayPalDetails.AuthorizationId);
            Assert.IsNotNull(result.Target.PayPalDetails.ImageUrl);
            Assert.AreEqual("test@paypal.com", result.Target.PayPalDetails.RecipientEmail);
            Assert.IsNull(result.Target.PayPalDetails.Token);
            Assert.IsNotNull(result.Target.PayPalDetails.DebugId);
        }
    }
}
