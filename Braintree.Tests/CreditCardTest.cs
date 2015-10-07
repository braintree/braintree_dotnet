using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using NUnit.Framework;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class CreditCardTest
    {
        private BraintreeGateway gateway;
        private BraintreeService service;

        static readonly string EXP_MONTH_YEAR = "5/22";
        static readonly string EXP_MONTH = "05";
        static readonly string EXP_YEAR = "2022";
        static CreditCardTest()
        {
            EXP_MONTH = DateTime.Today.Month.ToString();
            EXP_YEAR = (DateTime.Today.Year + 5).ToString();
            EXP_MONTH_YEAR = EXP_MONTH + "/" + EXP_YEAR;
        }

        [SetUp]
        public void Setup()
        {
            gateway = new BraintreeGateway();
            service = new BraintreeService(gateway.Configuration);
        }

        #pragma warning disable 0618
        [Test]
        public void TransparentRedirectURLForCreate_ReturnsCorrectValue()
        {
            Assert.AreEqual(service.BaseMerchantURL() + "/payment_methods/all/create_via_transparent_redirect_request",
                    gateway.CreditCard.TransparentRedirectURLForCreate());
        }
        #pragma warning restore 0618

        #pragma warning disable 0618
        [Test]
        public void TransparentRedirectURLForUpdate_ReturnsCorrectValue()
        {
            Assert.AreEqual(service.BaseMerchantURL() + "/payment_methods/all/update_via_transparent_redirect_request",
                    gateway.CreditCard.TransparentRedirectURLForUpdate());
        }
        #pragma warning restore 0618

        [Test]
        public void TrData_ReturnsValidTrDataHash()
        {
            string trData = gateway.TrData(new CreditCardRequest(), "http://example.com");
            Assert.IsTrue(TrUtil.IsTrDataValid(trData, service));
        }


        [Test]
        public void Create_CreatesCreditCardForGivenCustomerId()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
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

            Assert.AreEqual("555555", creditCard.Bin);
            Assert.AreEqual("4444", creditCard.LastFour);
            Assert.AreEqual("555555******4444", creditCard.MaskedNumber);
            Assert.AreEqual(EXP_MONTH, creditCard.ExpirationMonth);
            Assert.AreEqual(EXP_YEAR, creditCard.ExpirationYear);
            Assert.AreEqual("Michael Angelo", creditCard.CardholderName);
            Assert.IsTrue(creditCard.IsDefault.Value);
            Assert.IsFalse(creditCard.IsVenmoSdk.Value);
            Assert.AreEqual(DateTime.Now.Year, creditCard.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, creditCard.UpdatedAt.Value.Year);
            Assert.IsNotNull(creditCard.ImageUrl);

            Address billingAddress = creditCard.BillingAddress;
            Assert.AreEqual("Chad", billingAddress.CountryName);
            Assert.AreEqual("TD", billingAddress.CountryCodeAlpha2);
            Assert.AreEqual("TCD", billingAddress.CountryCodeAlpha3);
            Assert.AreEqual("148", billingAddress.CountryCodeNumeric);
            Assert.IsTrue(Regex.IsMatch(creditCard.UniqueNumberIdentifier, "\\A\\w{32}\\z"));
        }

        [Test]
        public void Create_CreatesCreditCardWithAVenmoSdkPaymentMethodCode()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                VenmoSdkPaymentMethodCode = SandboxValues.VenmoSdk.VISA_PAYMENT_METHOD_CODE,
            };

            Result<CreditCard> result = gateway.CreditCard.Create(creditCardRequest);
            Assert.IsTrue(result.IsSuccess());

            CreditCard creditCard = result.Target;
            Assert.AreEqual("1111", creditCard.LastFour);
            Assert.IsTrue(creditCard.IsVenmoSdk.Value);
        }

        [Test]
        public void Create_CreatesCreditCardWithSecurityParams()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                DeviceSessionId = "abc123",
                FraudMerchantId = "7",
                CardholderName = "John Doe",
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
                BillingAddress = new CreditCardAddressRequest
                {
                    CountryName = "Greece",
                    CountryCodeAlpha2 = "GR",
                    CountryCodeAlpha3 = "GRC",
                    CountryCodeNumeric = "300"
                }
            };

            Result<CreditCard> result = gateway.CreditCard.Create(creditCardRequest);
            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void Create_CreatesCreditCardWithDeviceData()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                DeviceData = "{\"device_session_id\":\"my_dsid\", \"fraud_merchant_id\":\"7\"}",
                CardholderName = "John Doe",
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
                BillingAddress = new CreditCardAddressRequest
                {
                    CountryName = "Greece",
                    CountryCodeAlpha2 = "GR",
                    CountryCodeAlpha3 = "GRC",
                    CountryCodeNumeric = "300"
                }
            };

            Result<CreditCard> result = gateway.CreditCard.Create(creditCardRequest);
            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void Create_FailsToCreateCreditCardWithInvalidVenmoSdkPaymentMethodCode()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                VenmoSdkPaymentMethodCode = SandboxValues.VenmoSdk.INVALID_PAYMENT_METHOD_CODE,
            };

            Result<CreditCard> result = gateway.CreditCard.Create(creditCardRequest);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual("Invalid VenmoSDK payment method code", result.Message);
            Assert.AreEqual(
                ValidationErrorCode.CREDIT_CARD_INVALID_VENMO_SDK_PAYMENT_METHOD_CODE,
                result.Errors.ForObject("CreditCard").OnField("VenmoSdkPaymentMethodCode")[0].Code
            );
        }

        [Test]
        public void Create_AddsCardToVenmoSdkWithValidSession()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
                Options = new CreditCardOptionsRequest
                {
                    VenmoSdkSession = SandboxValues.VenmoSdk.SESSION
                }
            };

            Result<CreditCard> result = gateway.CreditCard.Create(creditCardRequest);
            CreditCard card = result.Target;

            Assert.IsTrue(result.IsSuccess());
            Assert.IsTrue(card.IsVenmoSdk.Value);
        }

        [Test]
        public void Create_DoesNotAddCardToVenmoSdkWithInvalidSession()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
                Options = new CreditCardOptionsRequest
                {
                    VenmoSdkSession = SandboxValues.VenmoSdk.INVALID_SESSION
                }
            };

            Result<CreditCard> result = gateway.CreditCard.Create(creditCardRequest);
            CreditCard card = result.Target;

            Assert.IsTrue(result.IsSuccess());
            Assert.IsFalse(card.IsVenmoSdk.Value);
        }


        [Test]
        public void Create_AcceptsBillingAddressId()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            AddressRequest addressRequest = new AddressRequest
            {
                FirstName = "John",
                CountryName = "Chad",
                CountryCodeAlpha2 = "TD",
                CountryCodeAlpha3 = "TCD",
                CountryCodeNumeric = "148"
            };

            Address address = gateway.Address.Create(customer.Id, addressRequest).Target;
            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
                CVV = "123",
                CardholderName = "Michael Angelo",
                BillingAddressId = address.Id,
            };

            CreditCard creditCard = gateway.CreditCard.Create(creditCardRequest).Target;
            Address billingAddress = creditCard.BillingAddress;
            Assert.AreEqual(address.Id, billingAddress.Id);
            Assert.AreEqual("Chad", billingAddress.CountryName);
            Assert.AreEqual("TD", billingAddress.CountryCodeAlpha2);
            Assert.AreEqual("TCD", billingAddress.CountryCodeAlpha3);
            Assert.AreEqual("148", billingAddress.CountryCodeNumeric);
        }

        #pragma warning disable 0618
        [Test]
        public void ConfirmTransparentRedirectCreate_CreatesTheCreditCard()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            CreditCardRequest trParams = new CreditCardRequest { CustomerId = customer.Id };
            CreditCardRequest request = new CreditCardRequest
            {
                CardholderName = "John Doe",
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
                BillingAddress = new CreditCardAddressRequest
                {
                    CountryName = "Greece",
                    CountryCodeAlpha2 = "GR",
                    CountryCodeAlpha3 = "GRC",
                    CountryCodeNumeric = "300"
                }
            };

            string queryString = TestHelper.QueryStringForTR(trParams, request, gateway.CreditCard.TransparentRedirectURLForCreate(), service);
            Result<CreditCard> result = gateway.CreditCard.ConfirmTransparentRedirect(queryString);
            Assert.IsTrue(result.IsSuccess());
            CreditCard card = result.Target;
            Assert.AreEqual("John Doe", card.CardholderName);
            Assert.AreEqual("555555", card.Bin);
            Assert.AreEqual(EXP_MONTH, card.ExpirationMonth);
            Assert.AreEqual(EXP_YEAR, card.ExpirationYear);
            Assert.AreEqual(EXP_MONTH_YEAR, card.ExpirationDate);
            Assert.AreEqual("4444", card.LastFour);
            Assert.IsTrue(card.Token != null);

            Address billingAddress = card.BillingAddress;
            Assert.AreEqual("Greece", billingAddress.CountryName);
            Assert.AreEqual("GR", billingAddress.CountryCodeAlpha2);
            Assert.AreEqual("GRC", billingAddress.CountryCodeAlpha3);
            Assert.AreEqual("300", billingAddress.CountryCodeNumeric);
        }
        #pragma warning restore 0618

        #pragma warning disable 0618
        [Test]
        public void ConfirmTransparentRedirectCreate_CreatesTheCreditCardObservingMakeDefaultInTRParams()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR
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

            string queryString = TestHelper.QueryStringForTR(trParams, request, gateway.CreditCard.TransparentRedirectURLForCreate(), service);

            CreditCard card = gateway.CreditCard.ConfirmTransparentRedirect(queryString).Target;
            Assert.IsTrue(card.IsDefault.Value);
        }
        #pragma warning restore 0618

        #pragma warning disable 0618
        [Test]
        public void ConfirmTransparentRedirectCreate_CreatesTheCreditCardObservingMakeDefaultInRequest()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
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

            string queryString = TestHelper.QueryStringForTR(trParams, request, gateway.CreditCard.TransparentRedirectURLForCreate(), service);

            CreditCard card = gateway.CreditCard.ConfirmTransparentRedirect(queryString).Target;
            Assert.IsTrue(card.IsDefault.Value);
        }
        #pragma warning restore 0618

        #pragma warning disable 0618
        [Test]
        public void ConfirmTransparentRedirectCreate_WithErrors()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            CreditCardRequest trParams = new CreditCardRequest { CustomerId = customer.Id };

            CreditCardRequest request = new CreditCardRequest
            {
                CardholderName = "John Doe",
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
                BillingAddress = new CreditCardAddressRequest
                {
                    CountryName = "Greece",
                    CountryCodeAlpha2 = "MX"
                }
            };

            string queryString = TestHelper.QueryStringForTR(trParams, request, gateway.CreditCard.TransparentRedirectURLForCreate(), service);
            Result<CreditCard> result = gateway.CreditCard.ConfirmTransparentRedirect(queryString);
            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_INCONSISTENT_COUNTRY,
                result.Errors.ForObject("CreditCard").ForObject("BillingAddress").OnField("Base")[0].Code
            );
        }
        #pragma warning restore 0618

        [Test]
        public void Find_FindsCreditCardByToken()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
                CVV = "123",
                CardholderName = "Michael Angelo"
            };

            CreditCard originalCreditCard = gateway.CreditCard.Create(creditCardRequest).Target;
            CreditCard creditCard = gateway.CreditCard.Find(originalCreditCard.Token);

            Assert.AreEqual("555555", creditCard.Bin);
            Assert.AreEqual("4444", creditCard.LastFour);
            Assert.AreEqual(EXP_MONTH, creditCard.ExpirationMonth);
            Assert.AreEqual(EXP_YEAR, creditCard.ExpirationYear);
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
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
                CVV = "123"
            };

            CreditCard originalCreditCard = gateway.CreditCard.Create(creditCardRequest).Target;
            string id = Guid.NewGuid().ToString();
            var subscriptionRequest = new SubscriptionRequest
            {
                Id = id,
                PlanId = "integration_trialless_plan",
                PaymentMethodToken = originalCreditCard.Token,
                Price = 1.00M
            };
            var resp = gateway.Subscription.Create(subscriptionRequest);
            Assert.IsNotNull(resp);
            if (!resp.IsSuccess())
                Assert.Inconclusive(resp.Message);
            else
            {
                CreditCard creditCard = gateway.CreditCard.Find(originalCreditCard.Token);
                Assert.IsNotNull(creditCard);
                Assert.IsNotEmpty(creditCard.Subscriptions);
                Subscription subscription = creditCard.Subscriptions[0];
                Assert.IsNotNull(subscription);
                Assert.AreEqual(id, subscription.Id);
                Assert.AreEqual("integration_trialless_plan", subscription.PlanId);
                Assert.AreEqual(1.00M, subscription.Price);
            }
        }

        [Test]
        public void Find_FindsErrorsOutOnWhitespaceIds()
        {
            try {
                gateway.CreditCard.Find(" ");
                Assert.Fail("Should throw NotFoundException");
            } catch (NotFoundException) {}
        }

        [Test]
        public void FromNonce_ExchangesANonceForACreditCard()
        {
          Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
          string nonce = TestHelper.GenerateUnlockedNonce(gateway, "4012888888881881", customer.Id);
          CreditCard card = gateway.CreditCard.FromNonce(nonce);
          Assert.AreEqual("401288******1881", card.MaskedNumber);
        }

        [Test]
        public void FromNonce_ReturnsErrorWhenProvidedNoncePointingToUnlockedSharedCard()
        {
            string nonce = TestHelper.GenerateUnlockedNonce(gateway);
            try {
                gateway.CreditCard.FromNonce(nonce);
                Assert.Fail("Should throw NotFoundException");
            } catch (NotFoundException e) {
                StringAssert.Contains("not found", e.Message);
            }
        }

        [Test]
        public void FromNonce_ReturnsErrorWhenProvidedConsumedNonce()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            string nonce = TestHelper.GenerateUnlockedNonce(gateway, "4012888888881881", customer.Id);
            gateway.CreditCard.FromNonce(nonce);
            try {
                gateway.CreditCard.FromNonce(nonce);
                Assert.Fail("Should throw NotFoundException");
            } catch (NotFoundException e) {
                StringAssert.Contains("consumed", e.Message);
            }
        }


        [Test]
        public void Update_UpdatesCreditCardByToken()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardCreateRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
                CVV = "123",
                CardholderName = "Michael Angelo"
            };

            CreditCard originalCreditCard = gateway.CreditCard.Create(creditCardCreateRequest).Target;

            var creditCardUpdateRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "4111111111111111",
                ExpirationDate = "12/25",
                CVV = "321",
                CardholderName = "Dave Inchy"
            };

            CreditCard creditCard = gateway.CreditCard.Update(originalCreditCard.Token, creditCardUpdateRequest).Target;

            Assert.AreEqual("411111", creditCard.Bin);
            Assert.AreEqual("1111", creditCard.LastFour);
            Assert.AreEqual("12", creditCard.ExpirationMonth);
            Assert.AreEqual("2025", creditCard.ExpirationYear);
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
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
                CVV = "123",
                CardholderName = "Michael Angelo"
            };

            var request2 = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
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
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
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
            Assert.IsNotNull(customer);
            var request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
                CVV = "123",
                BillingAddress = new CreditCardAddressRequest
                {
                    FirstName = "John",
                    PostalCode = "90025",
                    StreetAddress = "123 fake st",
                }
            };

            var resp = gateway.CreditCard.Create(request);
            Assert.IsNotNull(resp);
            CreditCard creditCard = resp.Target;
            Assert.IsNotNull(creditCard, resp.Message);

            var updateRequest = new CreditCardRequest
            {
                CVV = "123",
                BillingAddress = new CreditCardAddressRequest
                {
                    LastName = "Jones",
                    CountryName = "El Salvador",
                    CountryCodeAlpha2 = "SV",
                    CountryCodeAlpha3 = "SLV",
                    CountryCodeNumeric = "222",
                    PostalCode = "90025",
                    StreetAddress = "123 fake st",
                }
            };

            var uresp = gateway.CreditCard.Update(creditCard.Token, updateRequest);
            Assert.IsNotNull(uresp);
            CreditCard updatedCreditCard = uresp.Target;
            Assert.IsNotNull(updatedCreditCard, uresp.Message);

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
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
                CVV = "123",
                BillingAddress = new CreditCardAddressRequest
                {
                    FirstName = "John",
                    PostalCode = "90025",
                    StreetAddress = "123 fake st",
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;

            var updateRequest = new CreditCardRequest
            {
                CVV = "123",
                BillingAddress = new CreditCardAddressRequest
                {
                    LastName = "Jones",
                    PostalCode = "90025",
                    StreetAddress = "123 fake st",
                    Options = new CreditCardAddressOptionsRequest
                    {
                        UpdateExisting = true
                    }
                }
            };

            var uresp = gateway.CreditCard.Update(creditCard.Token, updateRequest);
            Assert.IsNotNull(uresp);
            CreditCard updatedCreditCard = uresp.Target;
            Assert.IsNotNull(updatedCreditCard, uresp.Message);

            Assert.AreEqual("John", updatedCreditCard.BillingAddress.FirstName);
            Assert.AreEqual("Jones", updatedCreditCard.BillingAddress.LastName);
            Assert.AreEqual(creditCard.BillingAddress.Id, updatedCreditCard.BillingAddress.Id);
        }

        #pragma warning disable 0618
        [Test]
        public void Update_UpdatesExistingBillingAddressWhenUpdateExistingIsTrueViaTransparentRedirect()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            var request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
                CVV = "123",
                BillingAddress = new CreditCardAddressRequest
                {
                    FirstName = "John",
                    PostalCode = "90025",
                    StreetAddress = "123 fake st",
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
                    LastName = "Jones",
                    PostalCode = "90025",
                    StreetAddress = "123 fake st",
                }
            };
            string queryString = TestHelper.QueryStringForTR(trParams, updateRequest, gateway.CreditCard.TransparentRedirectURLForUpdate(), service);
            var resp = gateway.CreditCard.ConfirmTransparentRedirect(queryString);
            Assert.IsNotNull(resp);
            CreditCard updatedCreditCard = resp.Target;
            Assert.IsNotNull(updatedCreditCard, resp.Message);

            Assert.AreEqual("John", updatedCreditCard.BillingAddress.FirstName);
            Assert.AreEqual("Jones", updatedCreditCard.BillingAddress.LastName);
            Assert.AreEqual(creditCard.BillingAddress.Id, updatedCreditCard.BillingAddress.Id);
        }
        #pragma warning restore 0618

        #pragma warning disable 0618
        [Test]
        public void UpdateViaTransparentRedirect()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest createRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
                CVV = "123",
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

            string queryString = TestHelper.QueryStringForTR(trParams, request, gateway.CreditCard.TransparentRedirectURLForUpdate(), service);
            Result<CreditCard> result = gateway.CreditCard.ConfirmTransparentRedirect(queryString);
            Assert.IsTrue(result.IsSuccess());
            CreditCard card = result.Target;
            Assert.AreEqual("Joe Cool", card.CardholderName);
            Assert.AreEqual("44444", card.BillingAddress.PostalCode);
        }
        #pragma warning restore 0618

        [Test]
        public void Delete_DeletesTheCreditCard()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
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
        public void CheckDuplicateCreditCard()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                CVV = "123",
                Number = "4111111111111111",
                ExpirationDate = EXP_MONTH_YEAR,
                Options = new CreditCardOptionsRequest
                {
                    FailOnDuplicatePaymentMethod = true
                }
            };

            gateway.CreditCard.Create(request);
            Result<CreditCard> result = gateway.CreditCard.Create(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.CREDIT_CARD_DUPLICATE_CARD_EXISTS,
                result.Errors.ForObject("CreditCard").OnField("Number")[0].Code
            );
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
                ExpirationDate = EXP_MONTH_YEAR,
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            Result<CreditCard> result = gateway.CreditCard.Create(request);
            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void VerifyValidCreditCardWithVerificationRiskData()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                CVV = "123",
                Number = "4111111111111111",
                ExpirationDate = EXP_MONTH_YEAR,
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            Result<CreditCard> result = gateway.CreditCard.Create(request);
            Assert.IsTrue(result.IsSuccess());

            CreditCard card = result.Target;

            CreditCardVerification verification = card.Verification;
            Assert.IsNotNull(verification);

            Assert.IsNotNull(verification.RiskData);
        }

        [Test]
        public void VerifyValidCreditCardWithVerificationAmount()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                CVV = "123",
                Number = "4111111111111111",
                ExpirationDate = EXP_MONTH_YEAR,
                BillingAddress = new CreditCardAddressRequest
                {
                    StreetAddress = "123 fake st",
                    PostalCode = "90025",
                },
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true,
                    VerificationAmount = "1.02"
                }
            };

            Result<CreditCard> result = gateway.CreditCard.Create(request);
            Assert.IsTrue(result.IsSuccess(), result.Message);
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
                Number = "5555555555554444",
                ExpirationDate = EXP_MONTH_YEAR,
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true,
                    VerificationMerchantAccountId = MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID,
                }
            };

            Result<CreditCard> result = gateway.CreditCard.Create(request);
            if (result.IsSuccess())
            {
                Assert.IsNotNull(result.CreditCardVerification, result.Message);
                Assert.AreEqual(MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID, result.CreditCardVerification.MerchantAccountId);
            }
            else
                Assert.Inconclusive(result.Message);
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
                ExpirationDate = EXP_MONTH_YEAR,
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            Result<CreditCard> result = gateway.CreditCard.Create(request);
            Assert.IsFalse(result.IsSuccess());
            CreditCardVerification verification = result.CreditCardVerification;
            Assert.AreEqual(VerificationStatus.PROCESSOR_DECLINED, verification.Status);
            Assert.IsNull(verification.GatewayRejectionReason);
        }

        [Test]
        public void GatewayRejectionReason_ExposedOnVerification()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                CVV = "200",
                Number = "4111111111111111",
                ExpirationDate = EXP_MONTH_YEAR,
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                },
            };

            Result<CreditCard> result = gateway.CreditCard.Create(request);
            Assert.IsFalse(result.IsSuccess());
            CreditCardVerification verification = result.CreditCardVerification;

            Assert.AreEqual(TransactionGatewayRejectionReason.CVV, verification.GatewayRejectionReason);
        }

        [Test]
        public void Expired()
        {
            ResourceCollection<CreditCard> collection = gateway.CreditCard.Expired();

            Assert.IsTrue(collection.MaximumCount > 1);

            List<string> cards = new List<string>();
            foreach (CreditCard card in collection) {
                Assert.IsTrue(card.IsExpired.Value);
                cards.Add(card.Token);
            }

            HashSet<string> uniqueCards = new HashSet<string>(cards);
            Assert.AreEqual(uniqueCards.Count, collection.MaximumCount);
        }

        [Test]
        public void ExpiringBetween()
        {
            int year = DateTime.Today.Year;
            DateTime beginning = new DateTime(year, 1, 1);
            DateTime end = new DateTime(year, 12, 31);

            ResourceCollection<CreditCard> collection = gateway.CreditCard.ExpiringBetween(beginning, end);
            Assert.IsTrue(collection.MaximumCount > 1);

            List<string> cards = new List<string>();
            foreach (CreditCard card in collection) {
                Assert.AreEqual(year.ToString(), card.ExpirationYear);
                cards.Add(card.Token);
            }

            HashSet<string> uniqueCards = new HashSet<string>(cards);
            Assert.AreEqual(uniqueCards.Count, collection.MaximumCount);
        }

        [Test]
        public void Prepaid()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                CVV = "123",
                Number = Braintree.Tests.CreditCardNumbers.CardTypeIndicators.Prepaid,
                ExpirationDate = EXP_MONTH_YEAR,
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.AreEqual(Braintree.CreditCardPrepaid.YES, creditCard.Prepaid);
        }

        [Test]
        public void Commercial()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                CVV = "123",
                Number = Braintree.Tests.CreditCardNumbers.CardTypeIndicators.Commercial,
                ExpirationDate = EXP_MONTH_YEAR,
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.AreEqual(Braintree.CreditCardCommercial.YES, creditCard.Commercial);
        }

        [Test]
        public void Debit()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                CVV = "123",
                Number = Braintree.Tests.CreditCardNumbers.CardTypeIndicators.Debit,
                ExpirationDate = EXP_MONTH_YEAR,
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.AreEqual(Braintree.CreditCardDebit.YES, creditCard.Debit);
        }

        [Test]
        public void Healthcare()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                CVV = "123",
                Number = Braintree.Tests.CreditCardNumbers.CardTypeIndicators.Healthcare,
                ExpirationDate = EXP_MONTH_YEAR,
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.AreEqual(Braintree.CreditCardHealthcare.YES, creditCard.Healthcare);
        }

        [Test]
        public void Payroll()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                CVV = "123",
                Number = Braintree.Tests.CreditCardNumbers.CardTypeIndicators.Payroll,
                ExpirationDate = EXP_MONTH_YEAR,
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.AreEqual(Braintree.CreditCardPayroll.YES, creditCard.Payroll);
        }

        [Test]
        public void DurbinRegulated()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                CVV = "123",
                Number = Braintree.Tests.CreditCardNumbers.CardTypeIndicators.DurbinRegulated,
                ExpirationDate = EXP_MONTH_YEAR,
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.AreEqual(Braintree.CreditCardDurbinRegulated.YES, creditCard.DurbinRegulated);
        }

        [Test]
        public void CountryOfIssuance()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                CVV = "123",
                Number = Braintree.Tests.CreditCardNumbers.CardTypeIndicators.CountryOfIssuance,
                ExpirationDate = EXP_MONTH_YEAR,
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.AreEqual(Braintree.Tests.CreditCardDefaults.CountryOfIssuance, creditCard.CountryOfIssuance);
        }

        [Test]
        public void IssuingBank()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                CVV = "123",
                Number = Braintree.Tests.CreditCardNumbers.CardTypeIndicators.IssuingBank,
                ExpirationDate = EXP_MONTH_YEAR,
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.AreEqual(Braintree.Tests.CreditCardDefaults.IssuingBank, creditCard.IssuingBank);
        }

        [Test]
        public void NegativeCardTypeIndicators()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                CVV = "123",
                Number = Braintree.Tests.CreditCardNumbers.CardTypeIndicators.No,
                ExpirationDate = EXP_MONTH_YEAR,
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.AreEqual(Braintree.CreditCardPrepaid.NO, creditCard.Prepaid);
            Assert.AreEqual(Braintree.CreditCardCommercial.NO, creditCard.Commercial);
            Assert.AreEqual(Braintree.CreditCardHealthcare.NO, creditCard.Healthcare);
            Assert.AreEqual(Braintree.CreditCardDurbinRegulated.NO, creditCard.DurbinRegulated);
            Assert.AreEqual(Braintree.CreditCardPayroll.NO, creditCard.Payroll);
            Assert.AreEqual(Braintree.CreditCardDebit.NO, creditCard.Debit);
        }

        [Test]
        public void MissingCardTypeIndicators()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                CVV = "123",
                Number = Braintree.Tests.CreditCardNumbers.CardTypeIndicators.Unknown,
                ExpirationDate = EXP_MONTH_YEAR,
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.AreEqual(Braintree.CreditCardPrepaid.UNKNOWN, creditCard.Prepaid);
            Assert.AreEqual(Braintree.CreditCardCommercial.UNKNOWN, creditCard.Commercial);
            Assert.AreEqual(Braintree.CreditCardHealthcare.UNKNOWN, creditCard.Healthcare);
            Assert.AreEqual(Braintree.CreditCardDurbinRegulated.UNKNOWN, creditCard.DurbinRegulated);
            Assert.AreEqual(Braintree.CreditCardPayroll.UNKNOWN, creditCard.Payroll);
            Assert.AreEqual(Braintree.CreditCardDebit.UNKNOWN, creditCard.Debit);
            Assert.AreEqual(Braintree.CreditCard.CountryOfIssuanceUnknown, creditCard.CountryOfIssuance);
            Assert.AreEqual(Braintree.CreditCard.IssuingBankUnknown, creditCard.IssuingBank);
        }

        [Test]
        public void CreateWithPaymentMethodNonce()
        {
          string nonce = TestHelper.GenerateUnlockedNonce(gateway);
          Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
          CreditCardRequest request = new CreditCardRequest
          {
            CustomerId = customer.Id,
            CardholderName = "John Doe",
            PaymentMethodNonce = nonce
          };
          Result<CreditCard> result = gateway.CreditCard.Create(request);
          Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void VerificationIsLatestVerification()
        {
            string xml = "<credit-card>"
                          + "<verifications>"
                          + "    <verification>"
                          + "        <created-at type=\"datetime\">2014-11-20T17:27:15Z</created-at>"
                          + "        <id>123</id>"
                          + "    </verification>"
                          + "    <verification>"
                          + "        <created-at type=\"datetime\">2014-11-20T17:27:18Z</created-at>"
                          + "        <id>932</id>"
                          + "    </verification>"
                          + "    <verification>"
                          + "        <created-at type=\"datetime\">2014-11-20T17:27:17Z</created-at>"
                          + "        <id>456</id>"
                          + "    </verification>"
                          + "</verifications>"
                        + "</credit-card>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);

            var result = new ResultImpl<CreditCard>(node, gateway);

            Assert.AreEqual("932", result.Target.Verification.Id);
        }
    }
}
