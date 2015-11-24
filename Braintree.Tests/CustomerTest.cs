using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Braintree.Exceptions;
using Braintree.Test;

namespace Braintree.Tests
{
    [TestFixture]
    public class CustomerTest
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
        [Category("Integration")]
        public void Find_FindsCustomerWithGivenId()
        {
            string id = Guid.NewGuid().ToString();
            var createRequest = new CustomerRequest
            {
                Id = id,
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Some Company",
                Email = "hansolo64@example.com",
                Phone = "312.555.1111",
                Fax = "312.555.1112",
                Website = "www.example.com",
                CreditCard = new CreditCardRequest
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/12",
                    CVV = "123",
                    CardholderName = "Michael Angelo",
                    BillingAddress = new CreditCardAddressRequest()
                    {
                        FirstName = "Mike",
                        LastName = "Smith",
                        Company = "Smith Co.",
                        StreetAddress = "1 W Main St",
                        ExtendedAddress = "Suite 330",
                        Locality = "Chicago",
                        Region = "IL",
                        PostalCode = "60622",
                        CountryName = "United States of America"
                    }
                }
            };

            Customer createdCustomer = gateway.Customer.Create(createRequest).Target;
            Customer customer = gateway.Customer.Find(createdCustomer.Id);
            Assert.AreEqual(id, customer.Id);
            Assert.AreEqual("Michael", customer.FirstName);
            Assert.AreEqual("Angelo", customer.LastName);
            Assert.AreEqual("Some Company", customer.Company);
            Assert.AreEqual("hansolo64@example.com", customer.Email);
            Assert.AreEqual("312.555.1111", customer.Phone);
            Assert.AreEqual("312.555.1112", customer.Fax);
            Assert.AreEqual("www.example.com", customer.Website);
            Assert.AreEqual(DateTime.Now.Year, customer.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, customer.UpdatedAt.Value.Year);
            Assert.AreEqual(1, customer.CreditCards.Length);
            Assert.AreEqual("510510", customer.CreditCards[0].Bin);
            Assert.AreEqual("5100", customer.CreditCards[0].LastFour);
            Assert.AreEqual("05", customer.CreditCards[0].ExpirationMonth);
            Assert.AreEqual("2012", customer.CreditCards[0].ExpirationYear);
            Assert.AreEqual("Michael Angelo", customer.CreditCards[0].CardholderName);
            Assert.IsTrue(Regex.IsMatch(customer.CreditCards[0].UniqueNumberIdentifier, "\\A\\w{32}\\z"));
            Assert.AreEqual(DateTime.Now.Year, customer.CreditCards[0].CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, customer.CreditCards[0].UpdatedAt.Value.Year);
            Assert.AreEqual("Mike", customer.Addresses[0].FirstName);
            Assert.AreEqual("Smith", customer.Addresses[0].LastName);
            Assert.AreEqual("Smith Co.", customer.Addresses[0].Company);
            Assert.AreEqual("1 W Main St", customer.Addresses[0].StreetAddress);
            Assert.AreEqual("Suite 330", customer.Addresses[0].ExtendedAddress);
            Assert.AreEqual("Chicago", customer.Addresses[0].Locality);
            Assert.AreEqual("IL", customer.Addresses[0].Region);
            Assert.AreEqual("60622", customer.Addresses[0].PostalCode);
            Assert.AreEqual("United States of America", customer.Addresses[0].CountryName);
        }

        [Test]
        [Category("Integration")]
        public void Find_IncludesApplePayCardsInPaymentMethods()
        {
            var createRequest = new CustomerRequest
            {
                PaymentMethodNonce = Nonce.ApplePayAmex
            };
            Customer createdCustomer = gateway.Customer.Create(createRequest).Target;
            Customer customer = gateway.Customer.Find(createdCustomer.Id);
            Assert.IsNotNull(customer.ApplePayCards);
            Assert.IsNotNull(customer.PaymentMethods);
            ApplePayCard card = customer.ApplePayCards[0];
            Assert.IsNotNull(card.Token);
            Assert.AreEqual(card, customer.PaymentMethods[0]);
        }

        [Test]
        [Category("Integration")]
        public void Find_IncludesAndroidPayProxyCardsInPaymentMethods()
        {
            var createRequest = new CustomerRequest
            {
                PaymentMethodNonce = Nonce.AndroidPayDiscover
            };
            Customer createdCustomer = gateway.Customer.Create(createRequest).Target;
            Customer customer = gateway.Customer.Find(createdCustomer.Id);
            Assert.IsNotNull(customer.AndroidPayCards);
            Assert.IsNotNull(customer.PaymentMethods);
            AndroidPayCard card = customer.AndroidPayCards[0];
            Assert.IsNotNull(card.Token);
            Assert.IsNotNull(card.GoogleTransactionId);
            Assert.AreEqual(card, customer.PaymentMethods[0]);
        }

        [Test]
        [Category("Integration")]
        public void Find_IncludesAndroidPayNetworkTokensInPaymentMethods()
        {
            var createRequest = new CustomerRequest
            {
                PaymentMethodNonce = Nonce.AndroidPayMasterCard
            };
            Customer createdCustomer = gateway.Customer.Create(createRequest).Target;
            Customer customer = gateway.Customer.Find(createdCustomer.Id);
            Assert.IsNotNull(customer.AndroidPayCards);
            Assert.IsNotNull(customer.PaymentMethods);
            AndroidPayCard card = customer.AndroidPayCards[0];
            Assert.IsNotNull(card.Token);
            Assert.IsNotNull(card.GoogleTransactionId);
            Assert.AreEqual(card, customer.PaymentMethods[0]);
        }

        [Test]
        [Category("Integration")]
        public void Find_IncludesAmexExpressCheckoutCardsInPaymentMethods()
        {
            var createRequest = new CustomerRequest
            {
                PaymentMethodNonce = Nonce.AmexExpressCheckout
            };
            Customer createdCustomer = gateway.Customer.Create(createRequest).Target;
            Customer customer = gateway.Customer.Find(createdCustomer.Id);
            Assert.IsNotNull(customer.AmexExpressCheckoutCards);
            Assert.IsNotNull(customer.PaymentMethods);
            AmexExpressCheckoutCard card = customer.AmexExpressCheckoutCards[0];
            Assert.IsNotNull(card.Token);
            Assert.IsNotNull(card.CardMemberNumber);
            Assert.AreEqual(card, customer.PaymentMethods[0]);
        }

        [Test]
        [Category("Integration")]
        public void Find_IncludesVenmoAccountsInPaymentMethods()
        {
            var createRequest = new CustomerRequest
            {
                PaymentMethodNonce = Nonce.VenmoAccount
            };
            Customer createdCustomer = gateway.Customer.Create(createRequest).Target;
            Customer customer = gateway.Customer.Find(createdCustomer.Id);
            Assert.IsNotNull(customer.VenmoAccounts);
            Assert.IsNotNull(customer.PaymentMethods);
            VenmoAccount account = customer.VenmoAccounts[0];
            Assert.IsNotNull(account.Token);
            Assert.IsNotNull(account.Username);
            Assert.IsNotNull(account.VenmoUserId);
            Assert.AreEqual(account, customer.PaymentMethods[0]);
        }

        [Test]
        [Category("Integration")]
        public void Find_RaisesIfIdIsInvalid()
        {
            try
            {
                gateway.Customer.Find("DOES_NOT_EXIST_999");
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException)
            {
                // expected
            }
        }

        [Test]
        [Category("Unit")]
        public void Find_RaisesIfIdIsBlank()
        {
            try
            {
                gateway.Customer.Find("  ");
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException)
            {
                // expected
            }
        }

        [Test]
        [Category("Integration")]
        public void Create_CanSetCustomFields()
        {
            var customFields = new Dictionary<string, string>();
            customFields.Add("store_me", "a custom value");
            var createRequest = new CustomerRequest()
            {
                CustomFields = customFields
            };

            Customer customer = gateway.Customer.Create(createRequest).Target;
            Assert.AreEqual("a custom value", customer.CustomFields["store_me"]);
        }

        [Test]
        [Category("Integration")]
        public void Create_CreatesCustomerWithSpecifiedValues()
        {
            var createRequest = new CustomerRequest()
            {
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Some Company",
                Email = "hansolo64@example.com",
                Phone = "312.555.1111",
                Fax = "312.555.1112",
                Website = "www.example.com",
                CreditCard = new CreditCardRequest()
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/12",
                    BillingAddress = new CreditCardAddressRequest
                    {
                        CountryName = "Macau",
                        CountryCodeAlpha2 = "MO",
                        CountryCodeAlpha3 = "MAC",
                        CountryCodeNumeric = "446"
                    }
                }
            };

            Customer customer = gateway.Customer.Create(createRequest).Target;
            Assert.AreEqual("Michael", customer.FirstName);
            Assert.AreEqual("Angelo", customer.LastName);
            Assert.AreEqual("Some Company", customer.Company);
            Assert.AreEqual("hansolo64@example.com", customer.Email);
            Assert.AreEqual("312.555.1111", customer.Phone);
            Assert.AreEqual("312.555.1112", customer.Fax);
            Assert.AreEqual("www.example.com", customer.Website);
            Assert.AreEqual(DateTime.Now.Year, customer.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, customer.UpdatedAt.Value.Year);

            Address billingAddress = customer.CreditCards[0].BillingAddress;
            Assert.AreEqual("Macau", billingAddress.CountryName);
            Assert.AreEqual("MO", billingAddress.CountryCodeAlpha2);
            Assert.AreEqual("MAC", billingAddress.CountryCodeAlpha3);
            Assert.AreEqual("446", billingAddress.CountryCodeNumeric);
        }
        
        [Test]
        [Category("Integration")]
        public void Create_withSecurityParams()
        {
            var createRequest = new CustomerRequest()
            {
                CreditCard = new CreditCardRequest()
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/12",
                    CVV = "123",
                    DeviceSessionId = "my_dsid"
                }
            };

            Result<Customer> result = gateway.Customer.Create(createRequest);

            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        [Category("Integration")]
        public void Create_withErrorsOnCountry()
        {
            var createRequest = new CustomerRequest()
            {
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Some Company",
                Email = "hansolo64@example.com",
                Phone = "312.555.1111",
                Fax = "312.555.1112",
                Website = "www.example.com",
                CreditCard = new CreditCardRequest()
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/12",
                    BillingAddress = new CreditCardAddressRequest
                    {
                        CountryName = "zzzzzz",
                        CountryCodeAlpha2 = "zz",
                        CountryCodeAlpha3 = "zzz",
                        CountryCodeNumeric = "000"
                    }
                }
            };

            Result<Customer> result = gateway.Customer.Create(createRequest);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_COUNTRY_NAME_IS_NOT_ACCEPTED,
                result.Errors.ForObject("Customer").ForObject("CreditCard").ForObject("BillingAddress").OnField("CountryName")[0].Code
            );
            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_COUNTRY_CODE_ALPHA2_IS_NOT_ACCEPTED,
                result.Errors.ForObject("Customer").ForObject("CreditCard").ForObject("BillingAddress").OnField("CountryCodeAlpha2")[0].Code
            );
            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_COUNTRY_CODE_ALPHA3_IS_NOT_ACCEPTED,
                result.Errors.ForObject("Customer").ForObject("CreditCard").ForObject("BillingAddress").OnField("CountryCodeAlpha3")[0].Code
            );
            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_COUNTRY_CODE_NUMERIC_IS_NOT_ACCEPTED,
                result.Errors.ForObject("Customer").ForObject("CreditCard").ForObject("BillingAddress").OnField("CountryCodeNumeric")[0].Code
            );
        }

        [Test]
        [Category("Integration")]
        public void Create_CreateCustomerWithCreditCard()
        {
            var createRequest = new CustomerRequest()
            {
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Some Company",
                Email = "hansolo64@example.com",
                Phone = "312.555.1111",
                Fax = "312.555.1112",
                Website = "www.example.com",
                CreditCard = new CreditCardRequest()
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/12",
                    CVV = "123",
                    CardholderName = "Michael Angelo"
                }
            };

            Customer customer = gateway.Customer.Create(createRequest).Target;
            Assert.AreEqual("Michael", customer.FirstName);
            Assert.AreEqual("Angelo", customer.LastName);
            Assert.AreEqual("Some Company", customer.Company);
            Assert.AreEqual("hansolo64@example.com", customer.Email);
            Assert.AreEqual("312.555.1111", customer.Phone);
            Assert.AreEqual("312.555.1112", customer.Fax);
            Assert.AreEqual("www.example.com", customer.Website);
            Assert.AreEqual(DateTime.Now.Year, customer.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, customer.UpdatedAt.Value.Year);
            Assert.AreEqual(1, customer.CreditCards.Length);
            Assert.AreEqual("510510", customer.CreditCards[0].Bin);
            Assert.AreEqual("5100", customer.CreditCards[0].LastFour);
            Assert.AreEqual("05", customer.CreditCards[0].ExpirationMonth);
            Assert.AreEqual("2012", customer.CreditCards[0].ExpirationYear);
            Assert.AreEqual("Michael Angelo", customer.CreditCards[0].CardholderName);
            Assert.AreEqual(DateTime.Now.Year, customer.CreditCards[0].CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, customer.CreditCards[0].UpdatedAt.Value.Year);
        }

        [Test]
        [Category("Integration")]
        public void Create_CreateCustomerUsingAccessToken()
        {
            var createRequest = new CustomerRequest()
            {
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Some Company",
                Email = "hansolo64@example.com",
                Phone = "312.555.1111",
                Fax = "312.555.1112",
                Website = "www.example.com",
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

            Customer customer = gateway.Customer.Create(createRequest).Target;
            Assert.AreEqual("Michael", customer.FirstName);
            Assert.AreEqual("Angelo", customer.LastName);
            Assert.AreEqual("Some Company", customer.Company);
            Assert.AreEqual("hansolo64@example.com", customer.Email);
            Assert.AreEqual("312.555.1111", customer.Phone);
            Assert.AreEqual("312.555.1112", customer.Fax);
            Assert.AreEqual("www.example.com", customer.Website);
            Assert.AreEqual(DateTime.Now.Year, customer.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, customer.UpdatedAt.Value.Year);
        }

        [Test]
        [Category("Integration")]
        public void Create_CreateCustomerWithCreditCardAndBillingAddress()
        {
            var createRequest = new CustomerRequest()
            {
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Some Company",
                Email = "hansolo64@example.com",
                Phone = "312.555.1111",
                Fax = "312.555.1112",
                Website = "www.example.com",
                CreditCard = new CreditCardRequest()
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/12",
                    CVV = "123",
                    CardholderName = "Michael Angelo",
                    BillingAddress = new CreditCardAddressRequest()
                    {
                        FirstName = "Michael",
                        LastName = "Angelo",
                        Company = "Angelo Co.",
                        StreetAddress = "1 E Main St",
                        ExtendedAddress = "Apt 3",
                        Locality = "Chicago",
                        Region = "IL",
                        PostalCode = "60622",
                        CountryName = "United States of America"
                    }
                }
            };

            Customer customer = gateway.Customer.Create(createRequest).Target;
            Assert.AreEqual("Michael", customer.FirstName);
            Assert.AreEqual("Angelo", customer.LastName);
            Assert.AreEqual("Some Company", customer.Company);
            Assert.AreEqual("hansolo64@example.com", customer.Email);
            Assert.AreEqual("312.555.1111", customer.Phone);
            Assert.AreEqual("312.555.1112", customer.Fax);
            Assert.AreEqual("www.example.com", customer.Website);
            Assert.AreEqual(DateTime.Now.Year, customer.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, customer.UpdatedAt.Value.Year);
            Assert.AreEqual(1, customer.CreditCards.Length);
            Assert.AreEqual("510510", customer.CreditCards[0].Bin);
            Assert.AreEqual("5100", customer.CreditCards[0].LastFour);
            Assert.AreEqual("05", customer.CreditCards[0].ExpirationMonth);
            Assert.AreEqual("2012", customer.CreditCards[0].ExpirationYear);
            Assert.AreEqual("Michael Angelo", customer.CreditCards[0].CardholderName);
            Assert.AreEqual(DateTime.Now.Year, customer.CreditCards[0].CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, customer.CreditCards[0].UpdatedAt.Value.Year);
            Assert.AreEqual(customer.Addresses[0].Id, customer.CreditCards[0].BillingAddress.Id);
            Assert.AreEqual("Michael", customer.Addresses[0].FirstName);
            Assert.AreEqual("Angelo", customer.Addresses[0].LastName);
            Assert.AreEqual("Angelo Co.", customer.Addresses[0].Company);
            Assert.AreEqual("1 E Main St", customer.Addresses[0].StreetAddress);
            Assert.AreEqual("Apt 3", customer.Addresses[0].ExtendedAddress);
            Assert.AreEqual("Chicago", customer.Addresses[0].Locality);
            Assert.AreEqual("IL", customer.Addresses[0].Region);
            Assert.AreEqual("60622", customer.Addresses[0].PostalCode);
            Assert.AreEqual("United States of America", customer.Addresses[0].CountryName);
        }

        [Test]
        [Category("Integration")]
        public void Create_WithVenmoSdkPaymentMethodCode()
        {
            var createRequest = new CustomerRequest()
            {
                FirstName = "Michael",
                LastName = "Angelo",
                CreditCard = new CreditCardRequest()
                {
                    VenmoSdkPaymentMethodCode = SandboxValues.VenmoSdk.VISA_PAYMENT_METHOD_CODE
                }
            };

            Customer customer = gateway.Customer.Create(createRequest).Target;
            Assert.AreEqual("Michael", customer.FirstName);
            Assert.AreEqual("Angelo", customer.LastName);
            Assert.AreEqual("411111", customer.CreditCards[0].Bin);
            Assert.AreEqual("1111", customer.CreditCards[0].LastFour);
        }

        [Test]
        [Category("Integration")]
        public void Create_WithVenmoSdkSession()
        {
            var createRequest = new CustomerRequest()
            {
                CreditCard = new CreditCardRequest()
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/12",
                    Options = new CreditCardOptionsRequest() {
                        VenmoSdkSession = SandboxValues.VenmoSdk.SESSION
                    }
                }
            };

            Customer customer = gateway.Customer.Create(createRequest).Target;
            Assert.IsTrue(customer.CreditCards[0].IsVenmoSdk.Value);
        }

        [Test]
        [Category("Integration")]
        public void Create_WithPaymentMethodNonce()
        {
            string nonce = TestHelper.GenerateUnlockedNonce(gateway);
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest{
                CreditCard = new CreditCardRequest{
                    PaymentMethodNonce = nonce
                }
            });
            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        [Category("Integration")]
        public void Create_WithPayPalPaymentMethodNonce()
        {
            string nonce = TestHelper.GenerateFuturePaymentPayPalNonce(gateway);
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest{
                PaymentMethodNonce = nonce
            });
            Assert.IsTrue(result.IsSuccess());
            var customer = result.Target;
            Assert.AreEqual(1, customer.PayPalAccounts.Length);
            Assert.AreEqual(customer.PayPalAccounts[0].Token, customer.DefaultPaymentMethod.Token);
        }

        #pragma warning disable 0618
        [Test]
        [Category("Integration")]
        public void ConfirmTransparentRedirect_CreatesTheCustomer()
        {
            CustomerRequest trParams = new CustomerRequest();

            CustomerRequest request = new CustomerRequest
            {
                FirstName = "John",
                LastName = "Doe"
            };

            string queryString = TestHelper.QueryStringForTR(trParams, request, gateway.Customer.TransparentRedirectURLForCreate(), service);
            Result<Customer> result = gateway.Customer.ConfirmTransparentRedirect(queryString);
            Assert.IsTrue(result.IsSuccess());
            Customer customer = result.Target;
            Assert.AreEqual("John", customer.FirstName);
            Assert.AreEqual("Doe", customer.LastName);
        }
        #pragma warning restore 0618

        #pragma warning disable 0618
        [Test]
        [Category("Integration")]
        public void ConfirmTransparentRedirect_CreatesNestedElementsAndCustomFields()
        {
            CustomerRequest trParams = new CustomerRequest();

            CustomerRequest request = new CustomerRequest
            {
                FirstName = "John",
                LastName = "Doe",
                CreditCard = new CreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    CardholderName = "John Doe",
                    ExpirationDate = "05/10",
                    BillingAddress = new CreditCardAddressRequest
                    {
                        CountryName = "Mexico",
                        CountryCodeAlpha2 = "MX",
                        CountryCodeAlpha3 = "MEX",
                        CountryCodeNumeric = "484"
                    }
                },
                CustomFields = new Dictionary<string, string>
                {
                    { "store_me", "a custom value" }
                }
            };

            string queryString = TestHelper.QueryStringForTR(trParams, request, gateway.Customer.TransparentRedirectURLForCreate(), service);
            Result<Customer> result = gateway.Customer.ConfirmTransparentRedirect(queryString);
            Assert.IsTrue(result.IsSuccess());
            Customer customer = result.Target;
            Assert.AreEqual("John", customer.FirstName);
            Assert.AreEqual("Doe", customer.LastName);
            Assert.AreEqual("John Doe", customer.CreditCards[0].CardholderName);
            Assert.AreEqual("a custom value", customer.CustomFields["store_me"]);

            Address address = customer.CreditCards[0].BillingAddress;
            Assert.AreEqual("Mexico", address.CountryName);
            Assert.AreEqual("MX", address.CountryCodeAlpha2);
            Assert.AreEqual("MEX", address.CountryCodeAlpha3);
            Assert.AreEqual("484", address.CountryCodeNumeric);
        }
        #pragma warning restore 0618

        #pragma warning disable 0618
        [Test]
        [Category("Integration")]
        public void ConfirmTransparentRedirect_UpdatesTheCustomer()
        {
            CustomerRequest createRequest = new CustomerRequest
            {
                FirstName = "Jane",
                LastName = "Deer"
            };
            Customer createdCustomer = gateway.Customer.Create(createRequest).Target;

            CustomerRequest trParams = new CustomerRequest
            {
                CustomerId = createdCustomer.Id
            };

            CustomerRequest request = new CustomerRequest
            {
                FirstName = "John",
                LastName = "Doe"
            };

            string queryString = TestHelper.QueryStringForTR(trParams, request, gateway.Customer.TransparentRedirectURLForUpdate(), service);
            Result<Customer> result = gateway.Customer.ConfirmTransparentRedirect(queryString);
            Assert.IsTrue(result.IsSuccess());
            Customer customer = result.Target;
            Assert.AreEqual("John", customer.FirstName);
            Assert.AreEqual("Doe", customer.LastName);
        }
        #pragma warning restore 0618

        #pragma warning disable 0618
        [Test]
        [Category("Integration")]
        public void Update_UpdatesCustomerAndNestedValuesViaTr()
        {
            var createRequest = new CustomerRequest()
            {
                FirstName = "Old First",
                LastName = "Old Last",
                CreditCard = new CreditCardRequest()
                {
                    Number = "4111111111111111",
                    ExpirationDate = "10/10",
                    BillingAddress = new CreditCardAddressRequest()
                    {
                        PostalCode = "11111"
                    }
                }
            };

            Customer customer = gateway.Customer.Create(createRequest).Target;
            CreditCard creditCard = customer.CreditCards[0];
            Address address = creditCard.BillingAddress;

            var trParams = new CustomerRequest()
            {
                CustomerId = customer.Id,
                FirstName = "New First",
                LastName = "New Last",
                CreditCard = new CreditCardRequest()
                {
                    ExpirationDate = "12/12",
                    Options = new CreditCardOptionsRequest()
                    {
                        UpdateExistingToken = creditCard.Token
                    },
                    BillingAddress = new CreditCardAddressRequest()
                    {
                        PostalCode = "44444",
                        CountryName = "Chad",
                        CountryCodeAlpha2 = "TD",
                        CountryCodeAlpha3 = "TCD",
                        CountryCodeNumeric = "148",
                        Options = new CreditCardAddressOptionsRequest()
                        {
                            UpdateExisting = true
                        }
                    }
                }
            };

            string queryString = TestHelper.QueryStringForTR(trParams, new CustomerRequest(), gateway.Customer.TransparentRedirectURLForUpdate(), service);
            Customer updatedCustomer = gateway.Customer.ConfirmTransparentRedirect(queryString).Target;
            CreditCard updatedCreditCard = gateway.CreditCard.Find(creditCard.Token);

            Address updatedAddress = gateway.Address.Find(customer.Id, address.Id);

            Assert.AreEqual("New First", updatedCustomer.FirstName);
            Assert.AreEqual("New Last", updatedCustomer.LastName);
            Assert.AreEqual("12/2012", updatedCreditCard.ExpirationDate);
            Assert.AreEqual("44444", updatedAddress.PostalCode);
            Assert.AreEqual("Chad", updatedAddress.CountryName);
            Assert.AreEqual("TD", updatedAddress.CountryCodeAlpha2);
            Assert.AreEqual("TCD", updatedAddress.CountryCodeAlpha3);
            Assert.AreEqual("148", updatedAddress.CountryCodeNumeric);
        }
        #pragma warning restore 0618

        [Test]
        [Category("Integration")]
        public void Update_UpdatesCustomerWithNewValues()
        {
            string oldId = Guid.NewGuid().ToString();
            string newId = Guid.NewGuid().ToString();
            var createRequest = new CustomerRequest()
            {
                Id = oldId,
                FirstName = "Old First",
                LastName = "Old Last",
                Company = "Old Company",
                Email = "old@example.com",
                Phone = "312.555.1111 xOld",
                Fax = "312.555.1112 xOld",
                Website = "old.example.com"
            };

            gateway.Customer.Create(createRequest);

            var updateRequest = new CustomerRequest()
            {
                Id = newId,
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Some Company",
                Email = "hansolo64@example.com",
                Phone = "312.555.1111",
                Fax = "312.555.1112",
                Website = "www.example.com"
            };

            Customer updatedCustomer = gateway.Customer.Update(oldId, updateRequest).Target;
            Assert.AreEqual(newId, updatedCustomer.Id);
            Assert.AreEqual("Michael", updatedCustomer.FirstName);
            Assert.AreEqual("Angelo", updatedCustomer.LastName);
            Assert.AreEqual("Some Company", updatedCustomer.Company);
            Assert.AreEqual("hansolo64@example.com", updatedCustomer.Email);
            Assert.AreEqual("312.555.1111", updatedCustomer.Phone);
            Assert.AreEqual("312.555.1112", updatedCustomer.Fax);
            Assert.AreEqual("www.example.com", updatedCustomer.Website);
            Assert.AreEqual(DateTime.Now.Year, updatedCustomer.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, updatedCustomer.UpdatedAt.Value.Year);
        }

        [Test]
        [Category("Integration")]
        public void Update_UpdatesCustomerAndNestedValues()
        {
            var createRequest = new CustomerRequest()
            {
                FirstName = "Old First",
                LastName = "Old Last",
                CreditCard = new CreditCardRequest()
                {
                    Number = "4111111111111111",
                    ExpirationDate = "10/10",
                    BillingAddress = new CreditCardAddressRequest()
                    {
                        PostalCode = "11111"
                    }
                }
            };

            Customer customer = gateway.Customer.Create(createRequest).Target;
            CreditCard creditCard = customer.CreditCards[0];
            Address address = creditCard.BillingAddress;

            var updateRequest = new CustomerRequest()
            {
                FirstName = "New First",
                LastName = "New Last",
                CreditCard = new CreditCardRequest()
                {
                    ExpirationDate = "12/12",
                    Options = new CreditCardOptionsRequest()
                    {
                        UpdateExistingToken = creditCard.Token
                    },
                    BillingAddress = new CreditCardAddressRequest()
                    {
                        PostalCode = "44444",
                        CountryName = "Chad",
                        CountryCodeAlpha2 = "TD",
                        CountryCodeAlpha3 = "TCD",
                        CountryCodeNumeric = "148",
                        Options = new CreditCardAddressOptionsRequest()
                        {
                            UpdateExisting = true
                        }
                    }
                }
            };

            Customer updatedCustomer = gateway.Customer.Update(customer.Id, updateRequest).Target;
            CreditCard updatedCreditCard = gateway.CreditCard.Find(creditCard.Token);
            Address updatedAddress = gateway.Address.Find(customer.Id, address.Id);

            Assert.AreEqual("New First", updatedCustomer.FirstName);
            Assert.AreEqual("New Last", updatedCustomer.LastName);
            Assert.AreEqual("12/2012", updatedCreditCard.ExpirationDate);
            Assert.AreEqual("44444", updatedAddress.PostalCode);
            Assert.AreEqual("Chad", updatedAddress.CountryName);
            Assert.AreEqual("TD", updatedAddress.CountryCodeAlpha2);
            Assert.AreEqual("TCD", updatedAddress.CountryCodeAlpha3);
            Assert.AreEqual("148", updatedAddress.CountryCodeNumeric);
        }

        [Test]
        [Category("Integration")]
        public void Update_AcceptsNestedBillingAddressId()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            AddressRequest addressRequest = new AddressRequest
            {
                FirstName = "John",
                LastName = "Doe"
            };

            Address address = gateway.Address.Create(customer.Id, addressRequest).Target;

            var updateRequest = new CustomerRequest
            {
                CreditCard = new CreditCardRequest
                {
                    Number = "4111111111111111",
                    ExpirationDate = "10/10",
                    BillingAddressId = address.Id
                }
            };

            Customer updatedCustomer = gateway.Customer.Update(customer.Id, updateRequest).Target;
            Address billingAddress = updatedCustomer.CreditCards[0].BillingAddress;
            Assert.AreEqual(address.Id, billingAddress.Id);
            Assert.AreEqual("John", billingAddress.FirstName);
            Assert.AreEqual("Doe", billingAddress.LastName);
        }

        [Test]
        [Category("Integration")]
        public void Update_AcceptsPaymentMethodNonce()
        {
            var create = new CustomerRequest
            {
                CreditCard = new CreditCardRequest
                {
                    Number = "4111111111111111",
                    ExpirationDate = "10/18",
                }
            };
            var customer = gateway.Customer.Create(create).Target;

            var update = new CustomerRequest
            {
                PaymentMethodNonce = Nonce.PayPalFuturePayment
            };
            var updatedCustomer = gateway.Customer.Update(customer.Id, update).Target;

            Assert.AreEqual(1, updatedCustomer.PayPalAccounts.Length);
            Assert.AreEqual(1, updatedCustomer.CreditCards.Length);
            Assert.AreEqual(2, updatedCustomer.PaymentMethods.Length);
        }

        [Test]
        [Category("Integration")]
        public void Delete_DeletesTheCustomer()
        {
            string id = Guid.NewGuid().ToString();
            gateway.Customer.Create(new CustomerRequest() { Id = id });
            Assert.AreEqual(id, gateway.Customer.Find(id).Id);

            Result<Customer> result = gateway.Customer.Delete(id);
            Assert.IsTrue(result.IsSuccess());
            try
            {
                gateway.Customer.Find(id);
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException)
            {
                // expected
            }
        }

        [Test]
        [Category("Integration")]
        public void All() {
            ResourceCollection<Customer> collection = gateway.Customer.All();

            Assert.IsTrue(collection.MaximumCount > 100);
    
            List<string> items = new List<string>();
            foreach (Customer item in collection) {
                items.Add(item.Id);
            }
            HashSet<string> uniqueItems = new HashSet<string>(items);

            Assert.AreEqual(uniqueItems.Count, collection.MaximumCount);
        }

        [Test]
        [Category("Integration")]
        public void Search_FindDuplicateCardsGivenPaymentMethodToken()
        {
            CreditCardRequest creditCard = new CreditCardRequest
            {
                Number = "4111111111111111",
                ExpirationDate = "05/2012"
            };

            CustomerRequest jimRequest = new CustomerRequest
            {
                FirstName = "Jim",
                CreditCard = creditCard
            };

            CustomerRequest joeRequest = new CustomerRequest
            {
                FirstName = "Jim",
                CreditCard = creditCard
            };

            Customer jim = gateway.Customer.Create(jimRequest).Target;
            Customer joe = gateway.Customer.Create(joeRequest).Target;

            CustomerSearchRequest searchRequest = new CustomerSearchRequest().
                PaymentMethodTokenWithDuplicates.Is(jim.CreditCards[0].Token);

            ResourceCollection<Customer> collection = gateway.Customer.Search(searchRequest);

            List<string> customerIds = new List<string>();
            foreach (Customer customer in collection) {
                customerIds.Add(customer.Id);
            }

            Assert.IsTrue(customerIds.Contains(jim.Id));
            Assert.IsTrue(customerIds.Contains(joe.Id));
        }

        [Test]
        [Category("Integration")]
        public void Search_OnAllTextFields()
        {
            string creditCardToken = string.Format("cc{0}", new Random().Next(1000000).ToString());

            CustomerRequest request = new CustomerRequest
            {
                Company = "Braintree",
                Email = "smith@example.com",
                Fax = "5551231234",
                FirstName = "Tom",
                LastName = "Smith",
                Phone = "5551231235",
                Website = "http://example.com",
                CreditCard = new CreditCardRequest
                {
                    CardholderName = "Tim Toole",
                    Number = "4111111111111111",
                    ExpirationDate = "05/2012",
                    Token = creditCardToken,
                    BillingAddress = new CreditCardAddressRequest
                    {
                        Company = "Braintree",
                        CountryName = "United States of America",
                        ExtendedAddress = "Suite 123",
                        FirstName = "Drew",
                        LastName = "Michaelson",
                        Locality = "Chicago",
                        PostalCode = "12345",
                        Region = "IL",
                        StreetAddress = "123 Main St"
                    }
                }
            };

            Customer customer = gateway.Customer.Create(request).Target;
            customer = gateway.Customer.Find(customer.Id);

            CustomerSearchRequest searchRequest = new CustomerSearchRequest().
                Id.Is(customer.Id).
                FirstName.Is("Tom").
                LastName.Is("Smith").
                Company.Is("Braintree").
                Email.Is("smith@example.com").
                Website.Is("http://example.com").
                Fax.Is("5551231234").
                Phone.Is("5551231235").
                AddressFirstName.Is("Drew").
                AddressLastName.Is("Michaelson").
                AddressLocality.Is("Chicago").
                AddressPostalCode.Is("12345").
                AddressRegion.Is("IL").
                AddressCountryName.Is("United States of America").
                AddressStreetAddress.Is("123 Main St").
                AddressExtendedAddress.Is("Suite 123").
                PaymentMethodToken.Is(creditCardToken).
                CardholderName.Is("Tim Toole").
                CreditCardNumber.Is("4111111111111111").
                CreditCardExpirationDate.Is("05/2012");

            ResourceCollection<Customer> collection = gateway.Customer.Search(searchRequest);

            Assert.AreEqual(1, collection.MaximumCount);
            Assert.AreEqual(customer.Id, collection.FirstItem.Id);
        }

        [Test]
        [Category("Integration")]
        public void Search_OnCreatedAt()
        {
            CustomerRequest request = new CustomerRequest();

            Customer customer = gateway.Customer.Create(request).Target;

            DateTime createdAt = customer.CreatedAt.Value;
            DateTime threeHoursEarlier = createdAt.AddHours(-3);
            DateTime oneHourEarlier = createdAt.AddHours(-1);
            DateTime oneHourLater = createdAt.AddHours(1);

            CustomerSearchRequest searchRequest = new CustomerSearchRequest().
                Id.Is(customer.Id).
                CreatedAt.Between(oneHourEarlier, oneHourLater);

            Assert.AreEqual(1, gateway.Customer.Search(searchRequest).MaximumCount);

            searchRequest = new CustomerSearchRequest().
                Id.Is(customer.Id).
                CreatedAt.GreaterThanOrEqualTo(oneHourEarlier);

            Assert.AreEqual(1, gateway.Customer.Search(searchRequest).MaximumCount);

            searchRequest = new CustomerSearchRequest().
                Id.Is(customer.Id).
                CreatedAt.LessThanOrEqualTo(oneHourLater);

            Assert.AreEqual(1, gateway.Customer.Search(searchRequest).MaximumCount);

            searchRequest = new CustomerSearchRequest().
                Id.Is(customer.Id).
                CreatedAt.Between(threeHoursEarlier, oneHourEarlier);

            Assert.AreEqual(0, gateway.Customer.Search(searchRequest).MaximumCount);
        }

        [Test]
        [Category("Integration")]
        public void Search_OnPayPalAccountEmail()
        {
            var request = new CustomerRequest
            {
                PaymentMethodNonce = Nonce.PayPalFuturePayment
            };

            var customer = gateway.Customer.Create(request).Target;

            var search = new CustomerSearchRequest().
                Id.Is(customer.Id).
                PayPalAccountEmail.Is(customer.PayPalAccounts[0].Email);

            Assert.AreEqual(1, gateway.Customer.Search(search).MaximumCount);
        }
    }
}
