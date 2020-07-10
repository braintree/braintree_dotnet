using Braintree.Exceptions;
using Braintree.Test;
using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class CreditCardIntegrationTest
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

        public void AdvancedFraudSetup()
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
#if netcore
        public async Task CreateAsync_CreatesCreditCardForGivenCustomerId()
#else
        public void CreateAsync_CreatesCreditCardForGivenCustomerId()
        {
            Task.Run(async () =>
#endif
        {
            Result<Customer> customerResult = await gateway.Customer.CreateAsync(new CustomerRequest());
            Customer customer = customerResult.Target;

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

            Result<CreditCard> creditCardResult = await gateway.CreditCard.CreateAsync(creditCardRequest);
            CreditCard creditCard = creditCardResult.Target;

            Assert.AreEqual("510510", creditCard.Bin);
            Assert.AreEqual("5100", creditCard.LastFour);
            Assert.AreEqual("510510******5100", creditCard.MaskedNumber);
            Assert.AreEqual("05", creditCard.ExpirationMonth);
            Assert.AreEqual("2012", creditCard.ExpirationYear);
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
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

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
            Assert.IsFalse(creditCard.IsVenmoSdk.Value);
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
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                Options = new CreditCardOptionsRequest
                {
                    VenmoSdkSession = SandboxValues.VenmoSdk.SESSION
                }
            };

            Result<CreditCard> result = gateway.CreditCard.Create(creditCardRequest);
            CreditCard card = result.Target;

            Assert.IsTrue(result.IsSuccess());
            Assert.IsFalse(card.IsVenmoSdk.Value);
        }

        [Test]
        public void Create_DoesNotAddCardToVenmoSdkWithInvalidSession()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5105105105105100",
                ExpirationDate = "05/12",
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
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                CVV = "123",
                CardholderName = "Michael Angelo",
                BillingAddressId = address.Id
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

            string queryString = TestHelper.QueryStringForTR(trParams, request, gateway.TransparentRedirect.Url, service);
            Result<CreditCard> result = gateway.TransparentRedirect.ConfirmCreditCard(queryString);
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
        #pragma warning restore 0618

        #pragma warning disable 0618
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

            string queryString = TestHelper.QueryStringForTR(trParams, request, gateway.TransparentRedirect.Url, service);

            CreditCard card = gateway.TransparentRedirect.ConfirmCreditCard(queryString).Target;
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

            string queryString = TestHelper.QueryStringForTR(trParams, request, gateway.TransparentRedirect.Url, service);

            CreditCard card = gateway.TransparentRedirect.ConfirmCreditCard(queryString).Target;
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
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                BillingAddress = new CreditCardAddressRequest
                {
                    CountryName = "Greece",
                    CountryCodeAlpha2 = "MX"
                }
            };

            string queryString = TestHelper.QueryStringForTR(trParams, request, gateway.TransparentRedirect.Url, service);
            Result<CreditCard> result = gateway.TransparentRedirect.ConfirmCreditCard(queryString);
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
#if netcore
        public async Task FindAsync_FindsCreditCardByToken()
#else
        public void FindAsync_FindsCreditCardByToken()
        {
            Task.Run(async () =>
#endif
        {
            Result<Customer> customerResult = await gateway.Customer.CreateAsync(new CustomerRequest());
            Customer customer = customerResult.Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                CVV = "123",
                CardholderName = "Michael Angelo"
            };

            Result<CreditCard> originalCreditCardResult = await gateway.CreditCard.CreateAsync(creditCardRequest);
            CreditCard originalCreditCard = originalCreditCardResult.Target;

            CreditCard creditCard = await gateway.CreditCard.FindAsync(originalCreditCard.Token);

            Assert.AreEqual("510510", creditCard.Bin);
            Assert.AreEqual("5100", creditCard.LastFour);
            Assert.AreEqual("05", creditCard.ExpirationMonth);
            Assert.AreEqual("2012", creditCard.ExpirationYear);
            Assert.AreEqual("Michael Angelo", creditCard.CardholderName);
            Assert.AreEqual(DateTime.Now.Year, creditCard.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, creditCard.UpdatedAt.Value.Year);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

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
            string id = Guid.NewGuid().ToString();
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
        public void FromNonce_ExchangesANonceForACreditCard()
        {
          Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
          string nonce = TestHelper.GenerateUnlockedNonce(gateway, "4012888888881881", customer.Id);
          CreditCard card = gateway.CreditCard.FromNonce(nonce);
          Assert.AreEqual("401288******1881", card.MaskedNumber);
        }

        [Test]
#if netcore
        public async Task FromNonceAsync_ExchangesANonceForACreditCard()
#else
        public void FromNonceAsync_ExchangesANonceForACreditCard()
        {
            Task.Run(async () =>
#endif
        {
          Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
          string nonce = TestHelper.GenerateUnlockedNonce(gateway, "4012888888881881", customer.Id);
          CreditCard card = await gateway.CreditCard.FromNonceAsync(nonce);
          Assert.AreEqual("401288******1881", card.MaskedNumber);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void FromNonce_ReturnsErrorWhenProvidedNoncePointingToUnlockedSharedCard()
        {
            string nonce = TestHelper.GenerateUnlockedNonce(gateway);
            Exception exception = null;
            try {
                gateway.CreditCard.FromNonce(nonce);
            } catch (Exception tempException) {
                exception = tempException;
            }

            Assert.IsNotNull(exception);
            StringAssert.Contains("not found", exception.Message);
            Assert.IsInstanceOf(typeof(NotFoundException), exception);
        }

        [Test]
#if netcore
        public async Task FromNonceAsync_ReturnsErrorWhenProvidedNoncePointingToUnlockedSharedCard()
#else
        public void FromNonceAsync_ReturnsErrorWhenProvidedNoncePointingToUnlockedSharedCard()
        {
            Task.Run(async () =>
#endif
        {
            string nonce = TestHelper.GenerateUnlockedNonce(gateway);
            Exception exception = null;
            try {
                await gateway.CreditCard.FromNonceAsync(nonce);
            } catch (Exception tempException) {
                exception = tempException;
            }

            Assert.IsNotNull(exception);
            StringAssert.Contains("not found", exception.Message);
            Assert.IsInstanceOf(typeof(NotFoundException), exception);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void FromNonce_ReturnsErrorWhenProvidedConsumedNonce()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            string nonce = TestHelper.GenerateUnlockedNonce(gateway, "4012888888881881", customer.Id);
            gateway.CreditCard.FromNonce(nonce);

            Exception exception = null;
            try {
                gateway.CreditCard.FromNonce(nonce);
            } catch (Exception tempException) {
                exception = tempException;
            }
            StringAssert.Contains("consumed", exception.Message);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOf(typeof(NotFoundException), exception);
        }

        [Test]
#if netcore
        public async Task FromNonceAsync_ReturnsErrorWhenProvidedConsumedNonce()
#else
        public void FromNonceAsync_ReturnsErrorWhenProvidedConsumedNonce()
        {
            Task.Run(async () =>
#endif
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            string nonce = TestHelper.GenerateUnlockedNonce(gateway, "4012888888881881", customer.Id);
            await gateway.CreditCard.FromNonceAsync(nonce);

            Exception exception = null;
            try {
                await gateway.CreditCard.FromNonceAsync(nonce);
            } catch (Exception tempException) {
                exception = tempException;
            }
            StringAssert.Contains("consumed", exception.Message);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOf(typeof(NotFoundException), exception);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif
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
#if netcore
        public async Task UpdateAsync_UpdatesCreditCardByToken()
#else
        public void UpdateAsync_UpdatesCreditCardByToken()
        {
            Task.Run(async () =>
#endif
        {
            Result<Customer> customerResult = await gateway.Customer.CreateAsync(new CustomerRequest());
            Customer customer = customerResult.Target;

            var creditCardCreateRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                CVV = "123",
                CardholderName = "Michael Angelo"
            };

            Result<CreditCard> originalCreditCardResult = await gateway.CreditCard.CreateAsync(creditCardCreateRequest);
            CreditCard originalCreditCard = originalCreditCardResult.Target;

            var creditCardUpdateRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "4111111111111111",
                ExpirationDate = "12/05",
                CVV = "321",
                CardholderName = "Dave Inchy"
            };

            Result<CreditCard> creditCardUpdateResult = await gateway.CreditCard.UpdateAsync(originalCreditCard.Token, creditCardUpdateRequest);
            CreditCard creditCard = creditCardUpdateResult.Target;

            Assert.AreEqual("411111", creditCard.Bin);
            Assert.AreEqual("1111", creditCard.LastFour);
            Assert.AreEqual("12", creditCard.ExpirationMonth);
            Assert.AreEqual("2005", creditCard.ExpirationYear);
            Assert.AreEqual("Dave Inchy", creditCard.CardholderName);
            Assert.AreEqual(DateTime.Now.Year, creditCard.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, creditCard.UpdatedAt.Value.Year);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

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
        public void Update_CheckDuplicateCreditCard()
        {
            var customer = new CustomerRequest
            {
                CreditCard = new CreditCardRequest {
                    Number = "4111111111111111",
                    ExpirationDate = "05/12",
                }
            };

            var dupCustomer = new CustomerRequest
            {
                CreditCard = new CreditCardRequest {
                    Number = "4111111111111111",
                    ExpirationDate = "05/12",
                    Options = new CreditCardOptionsRequest
                    {
                        FailOnDuplicatePaymentMethod = true
                    }
                }
            };

            var createResult = gateway.Customer.Create(customer);
            Assert.IsTrue(createResult.IsSuccess());
            var updateResult = gateway.Customer.Update(createResult.Target.Id, dupCustomer);
            Assert.IsFalse(updateResult.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.CREDIT_CARD_DUPLICATE_CARD_EXISTS,
                updateResult.Errors.ForObject("Customer").ForObject("CreditCard").OnField("Number")[0].Code
            );
        }

        #pragma warning disable 0618
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

            string queryString = TestHelper.QueryStringForTR(trParams, updateRequest, gateway.TransparentRedirect.Url, service);
            CreditCard updatedCreditCard = gateway.TransparentRedirect.ConfirmCreditCard(queryString).Target;

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

            string queryString = TestHelper.QueryStringForTR(trParams, request, gateway.TransparentRedirect.Url, service);
            Result<CreditCard> result = gateway.TransparentRedirect.ConfirmCreditCard(queryString);
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
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                CVV = "123",
                CardholderName = "Michael Angelo"
            };

            CreditCard creditCard = gateway.CreditCard.Create(creditCardRequest).Target;

            Assert.AreEqual(creditCard.Token, gateway.CreditCard.Find(creditCard.Token).Token);
            gateway.CreditCard.Delete(creditCard.Token);
            Assert.Throws<NotFoundException>(() => gateway.CreditCard.Find(creditCard.Token));
        }

        [Test]
#if netcore
        public async Task DeleteAsync_DeletesTheCreditCard()
#else
        public void DeleteAsync_DeletesTheCreditCard()
        {
            Task.Run(async () =>
#endif
        {
            Result<Customer> customerResult = await gateway.Customer.CreateAsync(new CustomerRequest());
            Customer customer = customerResult.Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                CVV = "123",
                CardholderName = "Michael Angelo"
            };

            Result<CreditCard> creditCardResult = await gateway.CreditCard.CreateAsync(creditCardRequest);
            CreditCard creditCard = creditCardResult.Target;

            Assert.AreEqual(creditCard.Token, gateway.CreditCard.Find(creditCard.Token).Token);
            await gateway.CreditCard.DeleteAsync(creditCard.Token);
            Assert.Throws<NotFoundException>(() => gateway.CreditCard.Find(creditCard.Token));
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

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
                ExpirationDate = "05/12",
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
        public void VerifyValidCreditCardWithVerificationRiskData()
        {
            AdvancedFraudSetup();
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
                },
                DeviceSessionId = "abc123"
            };

            Result<CreditCard> result = gateway.CreditCard.Create(request);
            Assert.IsTrue(result.IsSuccess());

            CreditCard card = result.Target;

            CreditCardVerification verification = card.Verification;
            Assert.IsNotNull(verification);

            Assert.IsNotNull(verification.RiskData);
            Assert.IsNotNull(verification.RiskData.decision);
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
                ExpirationDate = "05/12",
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true,
                    VerificationAmount = "1.02"
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
                    VerificationMerchantAccountId = MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID
                }
            };

            Result<CreditCard> result = gateway.CreditCard.Create(request);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID, result.CreditCardVerification.MerchantAccountId);
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
            Assert.AreEqual(VerificationStatus.PROCESSOR_DECLINED, verification.Status);
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
            DateTime beginning = new DateTime(2010, 1, 1);
            DateTime end = new DateTime(2010, 12, 31);

            ResourceCollection<CreditCard> collection = gateway.CreditCard.ExpiringBetween(beginning, end);

            Assert.IsTrue(collection.MaximumCount > 1);

            List<string> cards = new List<string>();
            foreach (CreditCard card in collection) {
                Assert.AreEqual("2010", card.ExpirationYear);
                cards.Add(card.Token);
            }

            HashSet<string> uniqueCards = new HashSet<string>(cards);
            Assert.AreEqual(uniqueCards.Count, collection.MaximumCount);
        }

        [Test]
#if netcore
        public async Task ExpiringBetweenAsync()
#else
        public void ExpiringBetweenAsync()
        {
            Task.Run(async () =>
#endif
        {
            DateTime beginning = new DateTime(2010, 1, 1);
            DateTime end = new DateTime(2010, 12, 31);

            ResourceCollection<CreditCard> collection = await gateway.CreditCard.ExpiringBetweenAsync(beginning, end);

            Assert.IsTrue(collection.MaximumCount > 1);

            List<string> cards = new List<string>();
            foreach (CreditCard card in collection) {
                Assert.AreEqual("2010", card.ExpirationYear);
                cards.Add(card.Token);
            }

            HashSet<string> uniqueCards = new HashSet<string>(cards);
            Assert.AreEqual(uniqueCards.Count, collection.MaximumCount);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Prepaid()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest request = new CreditCardRequest
            {
                CustomerId = customer.Id,
                CardholderName = "John Doe",
                CVV = "123",
                Number = TestUtil.CreditCardNumbers.CardTypeIndicators.Prepaid,
                ExpirationDate = "05/12",
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.AreEqual(CreditCardPrepaid.YES, creditCard.Prepaid);
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
                Number = TestUtil.CreditCardNumbers.CardTypeIndicators.Commercial,
                ExpirationDate = "05/12",
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.AreEqual(CreditCardCommercial.YES, creditCard.Commercial);
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
                Number = TestUtil.CreditCardNumbers.CardTypeIndicators.Debit,
                ExpirationDate = "05/12",
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.AreEqual(CreditCardDebit.YES, creditCard.Debit);
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
                Number = TestUtil.CreditCardNumbers.CardTypeIndicators.Healthcare,
                ExpirationDate = "05/12",
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.AreEqual(CreditCardHealthcare.YES, creditCard.Healthcare);
            Assert.AreEqual("J3", creditCard.ProductId);
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
                Number = TestUtil.CreditCardNumbers.CardTypeIndicators.Payroll,
                ExpirationDate = "05/12",
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.AreEqual(CreditCardPayroll.YES, creditCard.Payroll);
            Assert.AreEqual("MSA", creditCard.ProductId);
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
                Number = TestUtil.CreditCardNumbers.CardTypeIndicators.DurbinRegulated,
                ExpirationDate = "05/12",
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.AreEqual(CreditCardDurbinRegulated.YES, creditCard.DurbinRegulated);
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
                Number = TestUtil.CreditCardNumbers.CardTypeIndicators.CountryOfIssuance,
                ExpirationDate = "05/12",
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.AreEqual(TestUtil.CreditCardDefaults.CountryOfIssuance, creditCard.CountryOfIssuance);
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
                Number = TestUtil.CreditCardNumbers.CardTypeIndicators.IssuingBank,
                ExpirationDate = "05/12",
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.AreEqual(TestUtil.CreditCardDefaults.IssuingBank, creditCard.IssuingBank);
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
                Number = TestUtil.CreditCardNumbers.CardTypeIndicators.No,
                ExpirationDate = "05/12",
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.AreEqual(CreditCardPrepaid.NO, creditCard.Prepaid);
            Assert.AreEqual(CreditCardCommercial.NO, creditCard.Commercial);
            Assert.AreEqual(CreditCardHealthcare.NO, creditCard.Healthcare);
            Assert.AreEqual(CreditCardDurbinRegulated.NO, creditCard.DurbinRegulated);
            Assert.AreEqual(CreditCardPayroll.NO, creditCard.Payroll);
            Assert.AreEqual(CreditCardDebit.NO, creditCard.Debit);
            Assert.AreEqual("MSB", creditCard.ProductId);
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
                Number = TestUtil.CreditCardNumbers.CardTypeIndicators.Unknown,
                ExpirationDate = "05/12",
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true
                }
            };

            CreditCard creditCard = gateway.CreditCard.Create(request).Target;
            Assert.AreEqual(CreditCardPrepaid.UNKNOWN, creditCard.Prepaid);
            Assert.AreEqual(CreditCardCommercial.UNKNOWN, creditCard.Commercial);
            Assert.AreEqual(CreditCardHealthcare.UNKNOWN, creditCard.Healthcare);
            Assert.AreEqual(CreditCardDurbinRegulated.UNKNOWN, creditCard.DurbinRegulated);
            Assert.AreEqual(CreditCardPayroll.UNKNOWN, creditCard.Payroll);
            Assert.AreEqual(CreditCardDebit.UNKNOWN, creditCard.Debit);
            Assert.AreEqual(CreditCard.CountryOfIssuanceUnknown, creditCard.CountryOfIssuance);
            Assert.AreEqual(CreditCard.IssuingBankUnknown, creditCard.IssuingBank);
            Assert.AreEqual(Braintree.CreditCard.ProductIdUnknown, creditCard.ProductId);
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
        public void CreateWithThreeDSecureNonce()
        {
          Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
          CreditCardRequest request = new CreditCardRequest
          {
            CustomerId = customer.Id,
            PaymentMethodNonce = Nonce.ThreeDSecureVisaFullAuthentication,
            Options = new CreditCardOptionsRequest()
            {
                VerifyCard = true
            },
          };
          Result<CreditCard> result = gateway.CreditCard.Create(request);

          CreditCard card = result.Target;

          CreditCardVerification verification = card.Verification;

          Assert.AreEqual("Y", verification.ThreeDSecureInfo.Enrolled);
          Assert.AreEqual("cavv_value", verification.ThreeDSecureInfo.Cavv);
          Assert.AreEqual("05", verification.ThreeDSecureInfo.EciFlag);
          Assert.AreEqual("authenticate_successful", verification.ThreeDSecureInfo.Status);
          Assert.AreEqual("1.0.2", verification.ThreeDSecureInfo.ThreeDSecureVersion);
          Assert.AreEqual("xid_value", verification.ThreeDSecureInfo.Xid);
          Assert.IsTrue(verification.ThreeDSecureInfo.LiabilityShifted);
          Assert.IsTrue(verification.ThreeDSecureInfo.LiabilityShiftPossible);
        }
        
        [Test]
        public void CreateCreditCardWithInvalidThreeDSecurePassThruParams()
        {
          Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
          ThreeDSecurePassThruRequest passThruRequestWithoutThreeDSecureVersion = new ThreeDSecurePassThruRequest()
          {
              EciFlag = "05",
              Cavv = "some_cavv",
              Xid = "some_xid",
              AuthenticationResponse = "Y",
              DirectoryResponse = "Y",
              CavvAlgorithm = "2",
              DsTransactionId = "some_ds_transaction_id"
          };
          CreditCardRequest request = new CreditCardRequest
          {
            CustomerId = customer.Id,
            PaymentMethodNonce = Nonce.Transactable,
            ThreeDSecurePassThru = passThruRequestWithoutThreeDSecureVersion,
            Options = new CreditCardOptionsRequest()
            {
                VerifyCard = true
            },
          };
 
          Result<CreditCard> result = gateway.CreditCard.Create(request);
          Assert.IsFalse(result.IsSuccess());
          Assert.AreEqual(
            ValidationErrorCode.VERIFICATION_THREE_D_SECURE_THREE_D_SECURE_VERSION_IS_REQUIRED,
            result.Errors.ForObject("Verification").OnField("ThreeDSecureVersion")[0].Code
          );
        }

        [Test]
        public void CreateCreditCardWithThreeDSecurePassThruParams()
        {
          Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
          CreditCardRequest request = new CreditCardRequest
          {
            CustomerId = customer.Id,
            PaymentMethodNonce = Nonce.Transactable,
            ThreeDSecurePassThru = new ThreeDSecurePassThruRequest()
            {
                EciFlag = "05",
                Cavv = "some_cavv",
                Xid = "some_xid",
                ThreeDSecureVersion = "2.2.1",
                AuthenticationResponse = "Y",
                DirectoryResponse = "Y",
                CavvAlgorithm = "2",
                DsTransactionId = "some_ds_transaction_id"
            },
            Options = new CreditCardOptionsRequest()
            {
                VerifyCard = true
            },
          };

          Result<CreditCard> result = gateway.CreditCard.Create(request);
          Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void Create_WithAccountTypeCredit()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = SandboxValues.CreditCardNumber.HIPER,
                ExpirationDate = "05/12",
                CVV = "123",
                Options = new CreditCardOptionsRequest()
                {
                    VerifyCard = true,
                    VerificationMerchantAccountId = MerchantAccountIDs.BRAZIL_MERCHANT_ACCOUNT_ID,
                    VerificationAccountType = "credit",
                },
            };

            CreditCard creditCard = gateway.CreditCard.Create(creditCardRequest).Target;
            Assert.AreEqual("credit", creditCard.Verification.CreditCard.AccountType);
        }

        [Test]
        public void Create_WithAccountTypeDebit()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = SandboxValues.CreditCardNumber.HIPER,
                ExpirationDate = "05/12",
                CVV = "123",
                Options = new CreditCardOptionsRequest()
                {
                    VerifyCard = true,
                    VerificationMerchantAccountId = MerchantAccountIDs.BRAZIL_MERCHANT_ACCOUNT_ID,
                    VerificationAccountType = "debit",
                },
            };

            CreditCard creditCard = gateway.CreditCard.Create(creditCardRequest).Target;
            Assert.AreEqual("debit", creditCard.Verification.CreditCard.AccountType);
        }

        [Test]
        public void Create_WithErrorAccountTypeIsInvalid()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = SandboxValues.CreditCardNumber.HIPER,
                ExpirationDate = "05/12",
                CVV = "123",
                Options = new CreditCardOptionsRequest()
                {
                    VerifyCard = true,
                    VerificationMerchantAccountId = MerchantAccountIDs.BRAZIL_MERCHANT_ACCOUNT_ID,
                    VerificationAccountType = "ach",
                },
            };

            Result<CreditCard> result = gateway.CreditCard.Create(creditCardRequest);
            Assert.IsFalse(result.IsSuccess());
            StringAssert.Contains("Verification account type must be either `credit` or `debit`", result.Message);
            Assert.AreEqual(
                ValidationErrorCode.CREDIT_CARD_OPTIONS_VERIFICATION_ACCOUNT_TYPE_IS_INVALID,
                result.Errors.ForObject("CreditCard").ForObject("Options").OnField("VerificationAccountType")[0].Code
            );
        }

        [Test]
        public void Create_WithErrorAccountTypeNotSupported()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationDate = "05/12",
                CVV = "123",
                Options = new CreditCardOptionsRequest()
                {
                    VerifyCard = true,
                    VerificationAccountType = "credit",
                },
            };

            Result<CreditCard> result = gateway.CreditCard.Create(creditCardRequest);
            Assert.IsFalse(result.IsSuccess());
            StringAssert.Contains("Merchant account does not support verification account type", result.Message);
            Assert.AreEqual(
                ValidationErrorCode.CREDIT_CARD_OPTIONS_VERIFICATION_ACCOUNT_TYPE_NOT_SUPPORTED,
                result.Errors.ForObject("CreditCard").ForObject("Options").OnField("VerificationAccountType")[0].Code
            );
        }
        
        [Test]
        public void UpdateCreditCardWithThreeDSecurePassThruParams()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardCreateRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                PaymentMethodNonce = Nonce.Transactable,
            };
            ThreeDSecurePassThruRequest passThruRequestWithThreeDSecureVersion = new ThreeDSecurePassThruRequest()
            {
                EciFlag = "05",
                Cavv = "some_cavv",
                Xid = "some_xid",
                ThreeDSecureVersion = "2.2.0",
                AuthenticationResponse = "Y",
                DirectoryResponse = "Y",
                CavvAlgorithm = "2",
                DsTransactionId = "some_ds_transaction_id"
            };

            CreditCard originalCreditCard = gateway.CreditCard.Create(creditCardCreateRequest).Target;

            var creditCardUpdateRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                PaymentMethodNonce = Nonce.Transactable,
                ThreeDSecurePassThru = passThruRequestWithThreeDSecureVersion,
                Options = new CreditCardOptionsRequest()
                {
                    VerifyCard = true,
                }
            };

            Result<CreditCard> result = gateway.CreditCard.Update(originalCreditCard.Token, creditCardUpdateRequest);
            Assert.IsTrue(result.IsSuccess());
        }
        
        [Test]
        public void updateCreditCardWithInvalidThreeDSecurePassThruParams()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardCreateRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                PaymentMethodNonce = Nonce.Transactable,
            };
            ThreeDSecurePassThruRequest passThruRequestWithoutThreeDSecureVersion = new ThreeDSecurePassThruRequest()
            {
                EciFlag = "05",
                Cavv = "some_cavv",
                Xid = "some_xid",
                AuthenticationResponse = "Y",
                DirectoryResponse = "Y",
                CavvAlgorithm = "2",
                DsTransactionId = "some_ds_transaction_id"
            };

            CreditCard originalCreditCard = gateway.CreditCard.Create(creditCardCreateRequest).Target;

            var creditCardUpdateRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                PaymentMethodNonce = Nonce.Transactable,
                ThreeDSecurePassThru = passThruRequestWithoutThreeDSecureVersion,
                Options = new CreditCardOptionsRequest()
                {
                    VerifyCard = true,
                },
            };

            Result<CreditCard> result = gateway.CreditCard.Update(originalCreditCard.Token, creditCardUpdateRequest);
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.VERIFICATION_THREE_D_SECURE_THREE_D_SECURE_VERSION_IS_REQUIRED,
                result.Errors.ForObject("Verification").OnField("ThreeDSecureVersion")[0].Code
            );
        }

        [Test]
        public void Update_WithAccountTypeCredit()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardCreateRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = SandboxValues.CreditCardNumber.HIPER,
                ExpirationDate = "05/12",
                CVV = "123",
            };

            CreditCard originalCreditCard = gateway.CreditCard.Create(creditCardCreateRequest).Target;

            var creditCardUpdateRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = SandboxValues.CreditCardNumber.HIPER,
                ExpirationDate = "12/05",
                CVV = "321",
                Options = new CreditCardOptionsRequest()
                {
                    VerifyCard = true,
                    VerificationMerchantAccountId = MerchantAccountIDs.BRAZIL_MERCHANT_ACCOUNT_ID,
                    VerificationAccountType = "credit",
                },
            };

            CreditCard creditCard = gateway.CreditCard.Update(originalCreditCard.Token, creditCardUpdateRequest).Target;
            Assert.AreEqual("credit", creditCard.Verification.CreditCard.AccountType);
        }

        [Test]
        public void Update_WithAccountTypeDebit()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardCreateRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = SandboxValues.CreditCardNumber.HIPER,
                ExpirationDate = "05/12",
                CVV = "123",
            };

            CreditCard originalCreditCard = gateway.CreditCard.Create(creditCardCreateRequest).Target;

            var creditCardUpdateRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = SandboxValues.CreditCardNumber.HIPER,
                ExpirationDate = "12/05",
                CVV = "321",
                Options = new CreditCardOptionsRequest()
                {
                    VerifyCard = true,
                    VerificationMerchantAccountId = MerchantAccountIDs.BRAZIL_MERCHANT_ACCOUNT_ID,
                    VerificationAccountType = "debit",
                },
            };

            CreditCard creditCard = gateway.CreditCard.Update(originalCreditCard.Token, creditCardUpdateRequest).Target;
            Assert.AreEqual("debit", creditCard.Verification.CreditCard.AccountType);
        }
    }
}
