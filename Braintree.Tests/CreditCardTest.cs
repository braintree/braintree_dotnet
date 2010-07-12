using System;
using System.Collections.Generic;
using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class CreditCardTest
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
        public void TransparentRedirectURLForCreate_ReturnsCorrectValue()
        {
            Assert.AreEqual(service.BaseMerchantURL() + "/payment_methods/all/create_via_transparent_redirect_request",
                    gateway.CreditCard.TransparentRedirectURLForCreate());
        }

        [Test]
        public void TransparentRedirectURLForUpdate_ReturnsCorrectValue()
        {
            Assert.AreEqual(service.BaseMerchantURL() + "/payment_methods/all/update_via_transparent_redirect_request",
                    gateway.CreditCard.TransparentRedirectURLForUpdate());
        }

        [Test]
        public void TrData_ReturnsValidTrDataHash()
        {
            String trData = gateway.TrData(new CreditCardRequest(), "http://example.com");
            Assert.IsTrue(TrUtil.IsTrDataValid(trData, service));
        }


        [Test]
        public void Create_CreatesCreditCardForGivenCustomerId()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                CVV = "123",
                CardholderName = "Michael Angelo",
                BillingAddress = new CreditCardAddressRequest
                {
                    FirstName = "John",
                    CountryName = "Chad",
                    CountryCodeAlpha2 = "TD",
                    CountryCodeAlpha3 = "TCD",
                    CountryCodeNumeric = "148"
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(creditCardRequest).Target;

            Assert.AreEqual("510510", creditCard.Bin);
            Assert.AreEqual("5100", creditCard.LastFour);
            Assert.AreEqual("510510******5100", creditCard.MaskedNumber);
            Assert.AreEqual("05", creditCard.ExpirationMonth);
            Assert.AreEqual("2012", creditCard.ExpirationYear);
            Assert.AreEqual("Michael Angelo", creditCard.CardholderName);
            Assert.IsTrue(creditCard.IsDefault.Value);
            Assert.AreEqual(DateTime.Now.Year, creditCard.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, creditCard.UpdatedAt.Value.Year);

            Address billingAddress = creditCard.BillingAddress;
            Assert.AreEqual("Chad", billingAddress.CountryName);
            Assert.AreEqual("TD", billingAddress.CountryCodeAlpha2);
            Assert.AreEqual("TCD", billingAddress.CountryCodeAlpha3);
            Assert.AreEqual("148", billingAddress.CountryCodeNumeric);
        }

        [Test]
        public void ConfirmTransparentRedirectCreate_CreatesTheCreditCard()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            CreditCardRequest trParams = new CreditCardRequest { CustomerId = customer.Id };

            CreditCardRequest request = new CreditCardRequest
            {
                CardholderName = "John Doe",
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                BillingAddress = new CreditCardAddressRequest
                {
                    CountryName = "Greece",
                    CountryCodeAlpha2 = "GR",
                    CountryCodeAlpha3 = "GRC",
                    CountryCodeNumeric = "300"
                }
            };

            String queryString = TestHelper.QueryStringForTR(trParams, request, gateway.CreditCard.TransparentRedirectURLForCreate(), service);
            Result<CreditCard> result = gateway.CreditCard.ConfirmTransparentRedirect(queryString);
            Assert.IsTrue(result.IsSuccess());
            CreditCard card = result.Target;
            Assert.AreEqual("John Doe", card.CardholderName);
            Assert.AreEqual("510510", card.Bin);
            Assert.AreEqual("05", card.ExpirationMonth);
            Assert.AreEqual("2012", card.ExpirationYear);
            Assert.AreEqual("05/2012", card.ExpirationDate);
            Assert.AreEqual("5100", card.LastFour);
            Assert.IsTrue(card.Token != null);

            Address billingAddress = card.BillingAddress;
            Assert.AreEqual("Greece", billingAddress.CountryName);
            Assert.AreEqual("GR", billingAddress.CountryCodeAlpha2);
            Assert.AreEqual("GRC", billingAddress.CountryCodeAlpha3);
            Assert.AreEqual("300", billingAddress.CountryCodeNumeric);
        }

        [Test]
        public void ConfirmTransparentRedirectCreate_CreatesTheCreditCardObservingMakeDefaultInTRParams()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5105105105105100",
                ExpirationDate = "05/12"
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.IsTrue(creditCard.IsDefault.Value);

            CreditCardRequest trParams = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Options = new CreditCardOptionsRequest
                {
                    MakeDefault = true
                }
            };

            String queryString = TestHelper.QueryStringForTR(trParams, request, gateway.CreditCard.TransparentRedirectURLForCreate(), service);

            CreditCard card = gateway.CreditCard.ConfirmTransparentRedirect(queryString).Target;
            Assert.IsTrue(card.IsDefault.Value);
        }

        [Test]
        public void ConfirmTransparentRedirectCreate_CreatesTheCreditCardObservingMakeDefaultInRequest()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                Options = new CreditCardOptionsRequest
                {
                    MakeDefault = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.IsTrue(creditCard.IsDefault.Value);

            CreditCardRequest trParams = new CreditCardRequest
            {
                CustomerId = customer.Id,
            };

            String queryString = TestHelper.QueryStringForTR(trParams, request, gateway.CreditCard.TransparentRedirectURLForCreate(), service);

            CreditCard card = gateway.CreditCard.ConfirmTransparentRedirect(queryString).Target;
            Assert.IsTrue(card.IsDefault.Value);
        }

        [Test]
        public void ConfirmTransparentRedirectCreate_WithErrors()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            CreditCardRequest trParams = new CreditCardRequest { CustomerId = customer.Id };

            CreditCardRequest request = new CreditCardRequest
            {
                CardholderName = "John Doe",
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                BillingAddress = new CreditCardAddressRequest
                {
                    CountryName = "Greece",
                    CountryCodeAlpha2 = "MX"
                }
            };

            String queryString = TestHelper.QueryStringForTR(trParams, request, gateway.CreditCard.TransparentRedirectURLForCreate(), service);
            Result<CreditCard> result = gateway.CreditCard.ConfirmTransparentRedirect(queryString);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_INCONSISTENT_COUNTRY,
                result.Errors.ForObject("credit-card").ForObject("billing-address").OnField("base")[0].Code
            );
        }

        [Test]
        public void Find_FindsCreditCardByToken()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                CVV = "123",
                CardholderName = "Michael Angelo"
            };

            CreditCard originalCreditCard = gateway.CreditCard.Create(creditCardRequest).Target;
            CreditCard creditCard = gateway.CreditCard.Find(originalCreditCard.Token);

            Assert.AreEqual("510510", creditCard.Bin);
            Assert.AreEqual("5100", creditCard.LastFour);
            Assert.AreEqual("05", creditCard.ExpirationMonth);
            Assert.AreEqual("2012", creditCard.ExpirationYear);
            Assert.AreEqual("Michael Angelo", creditCard.CardholderName);
            Assert.AreEqual(DateTime.Now.Year, creditCard.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, creditCard.UpdatedAt.Value.Year);
        }

        [Test]
        public void Find_FindsAssociatedSubscriptions()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                CVV = "123"
            };

            CreditCard originalCreditCard = gateway.CreditCard.Create(creditCardRequest).Target;
            String id = Guid.NewGuid().ToString();
            var subscriptionRequest = new SubscriptionRequest
            {
                Id = id,
                PlanId = "integration_trialless_plan",
                PaymentMethodToken = originalCreditCard.Token,
                Price = 1.00M
            };
            gateway.Subscription.Create(subscriptionRequest);

            CreditCard creditCard = gateway.CreditCard.Find(originalCreditCard.Token);
            Subscription subscription = creditCard.Subscriptions[0];
            Assert.AreEqual(id, subscription.Id);
            Assert.AreEqual("integration_trialless_plan", subscription.PlanId);
            Assert.AreEqual(1.00M, subscription.Price);
        }

        [Test]
        public void Update_UpdatesCreditCardByToken()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardCreateRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                CVV = "123",
                CardholderName = "Michael Angelo"
            };

            CreditCard originalCreditCard = gateway.CreditCard.Create(creditCardCreateRequest).Target;

            var creditCardUpdateRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "4111111111111111",
                ExpirationDate = "12/05",
                CVV = "321",
                CardholderName = "Dave Inchy"
            };

            CreditCard creditCard = gateway.CreditCard.Update(originalCreditCard.Token, creditCardUpdateRequest).Target;

            Assert.AreEqual("411111", creditCard.Bin);
            Assert.AreEqual("1111", creditCard.LastFour);
            Assert.AreEqual("12", creditCard.ExpirationMonth);
            Assert.AreEqual("2005", creditCard.ExpirationYear);
            Assert.AreEqual("Dave Inchy", creditCard.CardholderName);
            Assert.AreEqual(DateTime.Now.Year, creditCard.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, creditCard.UpdatedAt.Value.Year);
        }

        [Test]
        public void Create_SetsDefaultIfSpecified()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var request1 = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                CVV = "123",
                CardholderName = "Michael Angelo"
            };

            var request2 = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                CVV = "123",
                CardholderName = "Michael Angelo",
                Options = new CreditCardOptionsRequest
                {
                    MakeDefault = true
                },
            };

            CreditCard card1 = gateway.CreditCard.Create(request1).Target;
            CreditCard card2 = gateway.CreditCard.Create(request2).Target;

            Assert.IsFalse(gateway.CreditCard.Find(card1.Token).IsDefault.Value);
            Assert.IsTrue(gateway.CreditCard.Find(card2.Token).IsDefault.Value);
        }

        [Test]
        public void Update_UpdatesDefaultIfSpecified()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardCreateRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                CVV = "123",
                CardholderName = "Michael Angelo"
            };

            CreditCard card1 = gateway.CreditCard.Create(creditCardCreateRequest).Target;
            CreditCard card2 = gateway.CreditCard.Create(creditCardCreateRequest).Target;

            Assert.IsTrue(card1.IsDefault.Value);
            Assert.IsFalse(card2.IsDefault.Value);


            var creditCardUpdateRequest = new CreditCardRequest
            {
                Options = new CreditCardOptionsRequest
                {
                    MakeDefault = true
                }
            };

            gateway.CreditCard.Update(card2.Token, creditCardUpdateRequest);

            Assert.IsFalse(gateway.CreditCard.Find(card1.Token).IsDefault.Value);
            Assert.IsTrue(gateway.CreditCard.Find(card2.Token).IsDefault.Value);
        }

        [Test]
        public void Update_CreatesNewBillingAddressByDefault()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                BillingAddress = new CreditCardAddressRequest
                {
                    FirstName = "John"
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;


            var updateRequest = new CreditCardRequest
            {
                BillingAddress = new CreditCardAddressRequest
                {
                    LastName = "Jones",
                    CountryName = "El Salvador",
                    CountryCodeAlpha2 = "SV",
                    CountryCodeAlpha3 = "SLV",
                    CountryCodeNumeric = "222"
                }
            };

            CreditCard updatedCreditCard = gateway.CreditCard.Update(creditCard.Token, updateRequest).Target;

            Assert.IsNull(updatedCreditCard.BillingAddress.FirstName);
            Assert.AreEqual("Jones", updatedCreditCard.BillingAddress.LastName);
            Assert.AreNotEqual(creditCard.BillingAddress.Id, updatedCreditCard.BillingAddress.Id);

            Address billingAddress = updatedCreditCard.BillingAddress;
            Assert.AreEqual("El Salvador", billingAddress.CountryName);
            Assert.AreEqual("SV", billingAddress.CountryCodeAlpha2);
            Assert.AreEqual("SLV", billingAddress.CountryCodeAlpha3);
            Assert.AreEqual("222", billingAddress.CountryCodeNumeric);
        }

        [Test]
        public void Update_UpdatesExistingBillingAddressWhenUpdateExistingIsTrue()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                BillingAddress = new CreditCardAddressRequest
                {
                    FirstName = "John"
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;

            var updateRequest = new CreditCardRequest
            {
                BillingAddress = new CreditCardAddressRequest
                {
                    LastName = "Jones",
                    Options = new CreditCardAddressOptionsRequest
                    {
                        UpdateExisting = true
                    }
                }
            };

            CreditCard updatedCreditCard = gateway.CreditCard.Update(creditCard.Token, updateRequest).Target;

            Assert.AreEqual("John", updatedCreditCard.BillingAddress.FirstName);
            Assert.AreEqual("Jones", updatedCreditCard.BillingAddress.LastName);
            Assert.AreEqual(creditCard.BillingAddress.Id, updatedCreditCard.BillingAddress.Id);
        }

        [Test]
        public void Update_UpdatesExistingBillingAddressWhenUpdateExistingIsTrueViaTransparentRedirect()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                BillingAddress = new CreditCardAddressRequest
                {
                    FirstName = "John"
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;

            CreditCardRequest trParams = new CreditCardRequest
            {
                PaymentMethodToken = creditCard.Token,
                BillingAddress = new CreditCardAddressRequest
                {
                    Options = new CreditCardAddressOptionsRequest
                    {
                        UpdateExisting = true
                    }
                }
            };

            CreditCardRequest updateRequest = new CreditCardRequest
            {
                BillingAddress = new CreditCardAddressRequest
                {
                    LastName = "Jones"
                }
            };

            String queryString = TestHelper.QueryStringForTR(trParams, updateRequest, gateway.CreditCard.TransparentRedirectURLForUpdate(), service);
            CreditCard updatedCreditCard = gateway.CreditCard.ConfirmTransparentRedirect(queryString).Target;

            Assert.AreEqual("John", updatedCreditCard.BillingAddress.FirstName);
            Assert.AreEqual("Jones", updatedCreditCard.BillingAddress.LastName);
            Assert.AreEqual(creditCard.BillingAddress.Id, updatedCreditCard.BillingAddress.Id);
        }

        [Test]
        public void UpdateViaTransparentRedirect()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest createRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                BillingAddress = new CreditCardAddressRequest
                {
                    PostalCode = "44444"
                }
            };
            CreditCard createdCard = gateway.CreditCard.Create(createRequest).Target;

            CreditCardRequest trParams = new CreditCardRequest
            {
                PaymentMethodToken = createdCard.Token
            };

            CreditCardRequest request = new CreditCardRequest
            {
                CardholderName = "Joe Cool"
            };

            String queryString = TestHelper.QueryStringForTR(trParams, request, gateway.CreditCard.TransparentRedirectURLForUpdate(), service);
            Result<CreditCard> result = gateway.CreditCard.ConfirmTransparentRedirect(queryString);
            Assert.IsTrue(result.IsSuccess());
            CreditCard card = result.Target;
            Assert.AreEqual("Joe Cool", card.CardholderName);
            Assert.AreEqual("44444", card.BillingAddress.PostalCode);
        }

        [Test]
        public void Delete_DeletesTheCreditCard()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                CVV = "123",
                CardholderName = "Michael Angelo"
            };

            CreditCard creditCard = gateway.CreditCard.Create(creditCardRequest).Target;

            Assert.AreEqual(creditCard.Token, gateway.CreditCard.Find(creditCard.Token).Token);
            gateway.CreditCard.Delete(creditCard.Token);
            try
            {
                gateway.CreditCard.Find(creditCard.Token);
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException)
            {
                // expected
            }
        }

        [Test]
        public void VerifyValidCreditCard()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                CVV = "123",
                Number = "4111111111111111",
                ExpirationDate = "05/12",
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            Result<CreditCard> result = gateway.CreditCard.Create(request);
            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void VerifyValidCreditCardSpecifyingMerhantAccount()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                CVV = "123",
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true,
                    VerificationMerchantAccountId = MerchantAccount.NON_DEFAULT_MERCHANT_ACCOUNT_ID
                }
            };

            Result<CreditCard> result = gateway.CreditCard.Create(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(MerchantAccount.NON_DEFAULT_MERCHANT_ACCOUNT_ID, result.CreditCardVerification.MerchantAccountId);
        }

        [Test]
        public void VerifyInvalidCreditCard()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                CVV = "123",
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            Result<CreditCard> result = gateway.CreditCard.Create(request);
            Assert.IsFalse(result.IsSuccess());
            CreditCardVerification verification = result.CreditCardVerification;
            Assert.AreEqual("processor_declined", verification.Status);
            Assert.IsNull(verification.GatewayRejectionReason);
        }

        [Test]
        public void GatewayRejectionReason_ExposedOnVerification()
        {
            BraintreeGateway processingRulesGateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "processing_rules_merchant_id",
                PublicKey = "processing_rules_public_key",
                PrivateKey = "processing_rules_private_key"
            };

            Customer customer = processingRulesGateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                CVV = "200",
                Number = "4111111111111111",
                ExpirationDate = "05/12",
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            Result<CreditCard> result = processingRulesGateway.CreditCard.Create(request);
            Assert.IsFalse(result.IsSuccess());
            CreditCardVerification verification = result.CreditCardVerification;

            Assert.AreEqual(TransactionGatewayRejectionReason.CVV, verification.GatewayRejectionReason);
        }

        [Test]
        public void Expired()
        {
            ResourceCollection<CreditCard> collection = gateway.CreditCard.Expired();

            Assert.IsTrue(collection.MaximumCount > 1);

            List<String> cards = new List<String>();
            foreach (CreditCard card in collection) {
                Assert.IsTrue(card.IsExpired.Value);
                cards.Add(card.Token);
            }

            HashSet<String> uniqueCards = new HashSet<String>(cards);
            Assert.AreEqual(uniqueCards.Count, collection.MaximumCount);
        }

        [Test]
        public void ExpiringBetween()
        {
            DateTime beginning = new DateTime(2010, 1, 1);
            DateTime end = new DateTime(2010, 12, 31);

            ResourceCollection<CreditCard> collection = gateway.CreditCard.ExpiringBetween(beginning, end);

            Assert.IsTrue(collection.MaximumCount > 1);

            List<String> cards = new List<String>();
            foreach (CreditCard card in collection) {
                Assert.AreEqual("2010", card.ExpirationYear);
                cards.Add(card.Token);
            }

            HashSet<String> uniqueCards = new HashSet<String>(cards);
            Assert.AreEqual(uniqueCards.Count, collection.MaximumCount);
        }
    }
}
