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
    public class CustomerIntegrationTest
    {
        private BraintreeGateway gateway;
        private BraintreeService service;
        private Random random;

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
            random = new Random();
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

        [Test]
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
            Assert.IsNotNull(customer.GraphQLId);
        }

        [Test]
#if netcore
        public async Task FindAsync_FindsCustomerWithGivenId()
#else
        public void FindAsync_FindsCustomerWithGivenId()
        {
            Task.Run(async () =>
#endif
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

            Result<Customer> customerResult = await gateway.Customer.CreateAsync(createRequest);
            Customer customer = await gateway.Customer.FindAsync(customerResult.Target.Id);
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
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
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
        public void Find_IncludesUsBankAccountInPaymentMethods()
        {
            var createRequest = new CustomerRequest
            {
                PaymentMethodNonce = TestHelper.GenerateValidUsBankAccountNonce(gateway),
                CreditCard = new CreditCardRequest {
                  Options = new CreditCardOptionsRequest {
                    VerificationMerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID
                  }
                }
            };
            Customer createdCustomer = gateway.Customer.Create(createRequest).Target;
            Customer customer = gateway.Customer.Find(createdCustomer.Id);
            Assert.IsNotNull(customer.UsBankAccounts);
            Assert.IsNotNull(customer.PaymentMethods);

            UsBankAccount usBankAccount = customer.UsBankAccounts[0];
            Assert.IsNotNull(usBankAccount.Token);
            Assert.AreEqual(usBankAccount, customer.PaymentMethods[0]);

            Assert.AreEqual(1, customer.PaymentMethods.Length);
            Assert.AreEqual("021000021", usBankAccount.RoutingNumber);
            Assert.AreEqual("0000", usBankAccount.Last4);
            Assert.AreEqual("checking", usBankAccount.AccountType);
            Assert.AreEqual("Dan Schulman", usBankAccount.AccountHolderName);
            Assert.IsTrue(Regex.IsMatch(usBankAccount.BankName, ".*CHASE.*"));
        }

        [Test]
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
        public void Find_RaisesIfIdIsInvalid()
        {
            Assert.Throws<NotFoundException>(() => gateway.Customer.Find("DOES_NOT_EXIST_999"));
        }

        [Test]
        public void Find_FindsCustomerWithAllFilterableAssociationsFilteredOut()
        {
            string id = Guid.NewGuid().ToString();
            var customFields = new Dictionary<string, string>();
            customFields.Add("store_me", "a custom value");

            var createRequest = new CustomerRequest
            {
                Id = id,
                FirstName = "Michael",
                LastName = "Angelo",
                CustomFields = customFields,
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
                        Locality = "Chicago",
                        Region = "IL",
                        PostalCode = "60622",
                        CountryName = "United States of America"
                    }
                }
            };

            Customer createdCustomer = gateway.Customer.Create(createRequest).Target;
            Customer customer = gateway.Customer.Find(createdCustomer.Id, "customernoassociations");

            Assert.AreEqual(0, customer.CreditCards.Length);
            Assert.AreEqual(0, customer.PaymentMethods.Length);
            Assert.AreEqual(0, customer.Addresses.Length);
            Assert.IsEmpty(customer.CustomFields);
        }

        [Test]
        public void Find_FindsCustomerWithNestedFilterableAssociationsFilteredOut()
        {
            string id = Guid.NewGuid().ToString();
            var customFields = new Dictionary<string, string>();
            customFields.Add("store_me", "a custom value");

            var createRequest = new CustomerRequest
            {
                Id = id,
                FirstName = "Michael",
                LastName = "Angelo",
                CustomFields = customFields,
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
                        Locality = "Chicago",
                        Region = "IL",
                        PostalCode = "60622",
                        CountryName = "United States of America"
                    }
                }
            };

            Customer createdCustomer = gateway.Customer.Create(createRequest).Target;
            CreditCard creditCard = createdCustomer.CreditCards[0];
            TestPlan plan = PlanFixture.PLAN_WITHOUT_TRIAL;
            SubscriptionRequest subscriptionRequest = new SubscriptionRequest
            {
                PaymentMethodToken = creditCard.Token,
                PlanId = plan.Id
            };
            Subscription subscription = gateway.Subscription.Create(subscriptionRequest).Target;

            Customer customer = gateway.Customer.Find(createdCustomer.Id, "customertoplevelassociations");

            Assert.AreEqual(1, customer.CreditCards.Length);
            Assert.AreEqual(0, customer.CreditCards[0].Subscriptions.Length);
            Assert.AreEqual(1, customer.PaymentMethods.Length);
            Assert.AreEqual(1, customer.Addresses.Length);
            Assert.AreEqual("a custom value", customer.CustomFields["store_me"]);
        }

        [Test]
        public void Find_RaisesIfAssociationFilterIdIsInvalid()
        {
            string id = Guid.NewGuid().ToString();

            var createRequest = new CustomerRequest
            {
                Id = id,
                FirstName = "Michael",
                LastName = "Angelo",
            };

            Customer createdCustomer = gateway.Customer.Create(createRequest).Target;

            Assert.Throws<NotFoundException>(() => gateway.Customer.Find(createdCustomer.Id, null));
            Assert.Throws<NotFoundException>(() => gateway.Customer.Find(createdCustomer.Id, ""));
        }

        [Test]
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
        public void Create_WithoutCustomerRequestObject()
        {
            Result<Customer> customerResult = gateway.Customer.Create();
            Assert.IsTrue(customerResult.IsSuccess());

            Customer customer = customerResult.Target;
            Assert.IsNotNull(customer.Id);
        }

        [Test]
#if netcore
        public async Task CreateAsync_WithoutCustomerRequestObject()
#else
        public void CreateAsync_WithoutCustomerRequestObject()
        {
            Task.Run(async () =>
#endif
        {
            Result<Customer> customerResult = await gateway.Customer.CreateAsync();
            Assert.IsTrue(customerResult.IsSuccess());

            Customer customer = customerResult.Target;
            Assert.IsNotNull(customer.Id);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
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
#if netcore
        public async Task CreateAsync_CreatesCustomerWithSpecifiedValues()
#else
        public void CreateAsync_CreatesCustomerWithSpecifiedValues()
        {
            Task.Run(async () =>
#endif
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

            Result<Customer> customerResult = await gateway.Customer.CreateAsync(createRequest);
            Customer customer = customerResult.Target;

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
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        [Obsolete]
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
        public void Create_withDeviceData()
        {
            var createRequest = new CustomerRequest()
            {
                CreditCard = new CreditCardRequest()
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/12",
                    CVV = "123",
                    DeviceData = "{\"device_session_id\":\"my_dsid\", \"fraud_merchant_id\":\"7\"}"
                }
            };

            Result<Customer> result = gateway.Customer.Create(createRequest);

            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void Create_withRiskDataParam()
        {
            var createRequest = new CustomerRequest()
            {
                CreditCard = new CreditCardRequest()
                {
                    Number = "4111111111111111",
                    ExpirationDate = "10/10",
                    Options = new CreditCardOptionsRequest
                    {
                        VerifyCard = true
                    }
                },
                RiskData = new RiskDataRequest()
                {
                    CustomerBrowser = "IE6",
                    CustomerIP = "192.168.0.1"
                }
            };

            Result<Customer> result = gateway.Customer.Create(createRequest);

            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
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
        public void Create_CreateCustomerWithInvalidThreeDSecurePassThruParams() {
            ThreeDSecurePassThruRequest passThruParamsWithoutThreeDSecureVersion = new ThreeDSecurePassThruRequest()
            {
                EciFlag = "05",
                Cavv = "some_cavv",
                Xid = "some_xid",
                AuthenticationResponse = "Y",
                DirectoryResponse = "Y",
                CavvAlgorithm = "2",
                DsTransactionId = "some_ds_transaction_id"
            };
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
                    Number = "4111111111111111",
                    ExpirationDate = "05/25",
                    CVV = "123",
                    CardholderName = "Michael Angelo",
                    ThreeDSecurePassThru = passThruParamsWithoutThreeDSecureVersion,
                    Options = new CreditCardOptionsRequest()
                    {
                        VerifyCard = true
                    }
                }
            };

            Result<Customer> result = gateway.Customer.Create(createRequest);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.VERIFICATION_THREE_D_SECURE_THREE_D_SECURE_VERSION_IS_REQUIRED,
                result.Errors.ForObject("Verification").OnField("ThreeDSecureVersion")[0].Code
            );
        }

        [Test]
        public void Create_CreateCustomerWithThreeDSecurePassThruParams()
        {
            ThreeDSecurePassThruRequest passThruParamsWithThreeDSecureVersion = new ThreeDSecurePassThruRequest()
            {
                EciFlag = "05",
                Cavv = "some_cavv",
                Xid = "some_xid",
                ThreeDSecureVersion = "1.0.2",
                AuthenticationResponse = "Y",
                DirectoryResponse = "Y",
                CavvAlgorithm = "2",
                DsTransactionId = "some_ds_transaction_id"
            };
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
                    Number = "4111111111111111",
                    ExpirationDate = "05/25",
                    CVV = "123",
                    CardholderName = "Michael Angelo",
                    ThreeDSecurePassThru = passThruParamsWithThreeDSecureVersion,
                    Options = new CreditCardOptionsRequest()
                    {
                        VerifyCard = true
                    }
                }
            };

            Result<Customer> result = gateway.Customer.Create(createRequest);
            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
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
        public void Create_CreateCustomerWithCreditCardAndVerificationAmount()
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
                    Number = "5555555555554444",
                    ExpirationDate = "05/12",
                    CVV = "123",
                    CardholderName = "Michael Angelo",
                    Options = new CreditCardOptionsRequest
                    {
                        VerifyCard = true,
                        VerificationAmount = "1.02"
                    }
                }
            };

            Result<Customer> result = gateway.Customer.Create(createRequest);
            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void CreateWithoutSkipAdvancedFraudCheckingIncludesRiskData()
        {
            FraudProtectionEnterpriseSetup();
            CustomerRequest request = new CustomerRequest
            {
                FirstName = "John",
                LastName = "Doe",
                CreditCard = new CreditCardRequest()
                {
                    Number = "4111111111111111",
                    ExpirationDate = "05/12",
                    CVV = "123",
                    CardholderName = "John Doe",
                    Options = new CreditCardOptionsRequest
                    {
                        VerifyCard = true,
                        SkipAdvancedFraudChecking = false
                    },
                }
            };

            Result<Customer> result = gateway.Customer.Create(request);
            Assert.IsTrue(result.IsSuccess());

            CreditCardVerification verification = result.Target.CreditCards[0].Verification;
            Assert.IsNotNull(verification);
            Assert.IsNotNull(verification.RiskData);
        }

        [Test]
        public void CreateWithSkipAdvancedFraudCheckingDoesNotIncludeRiskData()
        {
            FraudProtectionEnterpriseSetup();
            CustomerRequest request = new CustomerRequest
            {
                FirstName = "John",
                LastName = "Doe",
                CreditCard = new CreditCardRequest()
                {
                    Number = "4111111111111111",
                    ExpirationDate = "05/12",
                    CVV = "123",
                    CardholderName = "John Doe",
                    Options = new CreditCardOptionsRequest
                    {
                        VerifyCard = true,
                        SkipAdvancedFraudChecking = true
                    },
                }
            };

            Result<Customer> result = gateway.Customer.Create(request);
            Assert.IsTrue(result.IsSuccess());

            CreditCardVerification verification = result.Target.CreditCards[0].Verification;
            Assert.IsNotNull(verification);
            Assert.IsNull(verification.RiskData);
        }

        [Test]
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
        public void Create_WithThreeDSecureNonce()
        {
            string nonce = TestHelper.GenerateUnlockedNonce(gateway);
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest{
                CreditCard = new CreditCardRequest{
                    PaymentMethodNonce = Nonce.ThreeDSecureVisaFullAuthentication,
                    Options = new CreditCardOptionsRequest()
                    {
                        VerifyCard = true,
                    }
                }
            });
            Assert.IsTrue(result.IsSuccess());
            var customer = result.Target;

            var verification = customer.CreditCards[0].Verification;

            Assert.AreEqual("authenticate_successful", verification.ThreeDSecureInfo.Status);
            Assert.IsNotNull(verification.ThreeDSecureInfo.Enrolled);
            Assert.IsNotNull(verification.ThreeDSecureInfo.Cavv);
            Assert.IsNotNull(verification.ThreeDSecureInfo.EciFlag);
            Assert.IsNotNull(verification.ThreeDSecureInfo.ThreeDSecureVersion);
            Assert.IsNotNull(verification.ThreeDSecureInfo.Xid);
            Assert.IsTrue(verification.ThreeDSecureInfo.LiabilityShifted);
            Assert.IsTrue(verification.ThreeDSecureInfo.LiabilityShiftPossible);
        }

        [Test]
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

        [Test]
        public void Create_WithPayPalOrderPaymentMethodNonce()
        {
            string nonce = TestHelper.GenerateOrderPaymentPayPalNonce(gateway);
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest{
                PaymentMethodNonce = nonce
            });
            Assert.IsTrue(result.IsSuccess());
            var customer = result.Target;
            Assert.AreEqual(1, customer.PayPalAccounts.Length);
            Assert.AreEqual(customer.PayPalAccounts[0].Token, customer.DefaultPaymentMethod.Token);

         }

        [Test]
        public void Create_WithPayPalOrderPaymentMethodNonceAndPayPalOptions()
        {
            string nonce = TestHelper.GenerateOrderPaymentPayPalNonce(gateway);
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest{
                PaymentMethodNonce = nonce,
                Options = new CustomerOptionsRequest {
                    OptionsPayPal = new CustomerOptionsPayPalRequest {
                        PayeeEmail = "payee@example.com",
                        OrderId = "order-id",
                        CustomField = "custom merchant field",
                        Description = "merchant description",
                        Amount = 1.23M,
                    }
                }
            });
            Assert.IsTrue(result.IsSuccess());
            var customer = result.Target;
            Assert.AreEqual(1, customer.PayPalAccounts.Length);
            Assert.AreEqual(customer.PayPalAccounts[0].Token, customer.DefaultPaymentMethod.Token);
         }

        [Test]
        public void Create_WithUsBankAccountPaymentMethodNonce()
        {
            string nonce = TestHelper.GenerateValidUsBankAccountNonce(gateway);
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest{
                PaymentMethodNonce = nonce,
                CreditCard = new CreditCardRequest {
                  Options = new CreditCardOptionsRequest {
                    VerificationMerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID
                  }
                }
            });
            Assert.IsTrue(result.IsSuccess());
            UsBankAccount usBankAccount = result.Target.UsBankAccounts[0];

            Assert.AreEqual(1, result.Target.PaymentMethods.Length);
            Assert.AreEqual("021000021", usBankAccount.RoutingNumber);
            Assert.AreEqual("0000", usBankAccount.Last4);
            Assert.AreEqual("checking", usBankAccount.AccountType);
            Assert.AreEqual("Dan Schulman", usBankAccount.AccountHolderName);
            Assert.IsTrue(Regex.IsMatch(usBankAccount.BankName, ".*CHASE.*"));
        }

        [Test]
        public void Create_WithVisaCheckoutCardPaymentMethodNonce()
        {
            string nonce = Braintree.Test.Nonce.VisaCheckoutMasterCard;
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest{
                PaymentMethodNonce = nonce
            });
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(1, result.Target.PaymentMethods.Length);
            Assert.AreEqual("VisaCheckoutCard", result.Target.PaymentMethods[0].GetType().Name);

            VisaCheckoutCard visaCheckoutCard = result.Target.VisaCheckoutCards[0];
            Assert.AreEqual("abc123", visaCheckoutCard.CallId);
            Assert.IsNotNull(visaCheckoutCard.BillingAddress);
            Assert.IsNotNull(visaCheckoutCard.Bin);
            Assert.IsNotNull(visaCheckoutCard.CardType);
            Assert.IsNotNull(visaCheckoutCard.CardholderName);
            Assert.IsNotNull(visaCheckoutCard.Commercial);
            Assert.IsNotNull(visaCheckoutCard.CountryOfIssuance);
            Assert.IsNotNull(visaCheckoutCard.CreatedAt);
            Assert.IsNotNull(visaCheckoutCard.CustomerId);
            Assert.IsNotNull(visaCheckoutCard.CustomerLocation);
            Assert.IsNotNull(visaCheckoutCard.Debit);
            Assert.IsNotNull(visaCheckoutCard.IsDefault);
            Assert.IsNotNull(visaCheckoutCard.DurbinRegulated);
            Assert.IsNotNull(visaCheckoutCard.ExpirationDate);
            Assert.IsNotNull(visaCheckoutCard.ExpirationMonth);
            Assert.IsNotNull(visaCheckoutCard.ExpirationYear);
            Assert.IsNotNull(visaCheckoutCard.IsExpired);
            Assert.IsNotNull(visaCheckoutCard.Healthcare);
            Assert.IsNotNull(visaCheckoutCard.ImageUrl);
            Assert.IsNotNull(visaCheckoutCard.IssuingBank);
            Assert.IsNotNull(visaCheckoutCard.LastFour);
            Assert.IsNotNull(visaCheckoutCard.MaskedNumber);
            Assert.IsNotNull(visaCheckoutCard.Payroll);
            Assert.IsNotNull(visaCheckoutCard.Prepaid);
            Assert.IsNotNull(visaCheckoutCard.ProductId);
            Assert.IsNotNull(visaCheckoutCard.Subscriptions);
            Assert.IsNotNull(visaCheckoutCard.Token);
            Assert.IsNotNull(visaCheckoutCard.UniqueNumberIdentifier);
            Assert.IsNotNull(visaCheckoutCard.UpdatedAt);
        }

        [Test]
        public void Create_WithAccountTypeCredit()
        {
            var createRequest = new CustomerRequest()
            {
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Some Company",
                CreditCard = new CreditCardRequest()
                {
                    Number = SandboxValues.CreditCardNumber.HIPER,
                    ExpirationDate = "05/12",
                    CVV = "123",
                    Options = new CreditCardOptionsRequest()
                    {
                        VerifyCard = true,
                        VerificationMerchantAccountId = "hiper_brl",
                        VerificationAccountType = "credit",
                    }
                }
            };

            Customer customer = gateway.Customer.Create(createRequest).Target;
            Assert.AreEqual("credit", customer.CreditCards[0].Verification.CreditCard.AccountType);
        }

        [Test]
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
#if netcore
        public async Task UpdateAsync_UpdatesCustomerWithNewValues()
#else
        public void UpdateAsync_UpdatesCustomerWithNewValues()
        {
            Task.Run(async () =>
#endif
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

            await gateway.Customer.CreateAsync(createRequest);

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

            Result<Customer> customerResult = await gateway.Customer.UpdateAsync(oldId, updateRequest);
            Customer updatedCustomer = customerResult.Target;

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
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
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
        public void Update_MakeExistingCreditCardPaymentMethodTheDefaultUsingOptions()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            var creditCardToken1 = GenerateToken();

            var creditCardCreateRequest1 = new PaymentMethodRequest
            {
                CustomerId = customer.Id,
                PaymentMethodNonce = Nonce.TransactableMasterCard,
                Token = creditCardToken1
            };
            PaymentMethod creditCardPaymentMethod1 = gateway.PaymentMethod.Create(creditCardCreateRequest1).Target;

            customer = gateway.Customer.Find(customer.Id);
            Assert.AreEqual(customer.DefaultPaymentMethod.Token, creditCardToken1);

            var creditCardToken2 = GenerateToken();

            var creditCardCreateRequest2 = new PaymentMethodRequest()
              {
                 CustomerId = customer.Id,
                 PaymentMethodNonce = Nonce.TransactableVisa,
                 Token = creditCardToken2
            };

            PaymentMethod creditCardPaymentMethod2 = gateway.PaymentMethod.Create(creditCardCreateRequest2).Target;

            customer = gateway.Customer.Find(customer.Id);
            Assert.AreNotEqual(customer.DefaultPaymentMethod.Token, creditCardToken2);
            Assert.AreEqual(customer.DefaultPaymentMethod.Token, creditCardToken1);

            var updateRequest = new CustomerRequest
            {
                CreditCard = new CreditCardRequest()
                {
                    Options = new CreditCardOptionsRequest()
                    {
                        UpdateExistingToken = creditCardToken2,
                        MakeDefault = true
                    }
                }
            };

            gateway.Customer.Update(customer.Id, updateRequest);
            customer = gateway.Customer.Find(customer.Id);
            Assert.AreEqual(customer.DefaultPaymentMethod.Token, creditCardToken2);
        }

        [Test]
        public void Update_MakeExistingPaymentMethodTheDefaultUsingDefaultPaymentMethodToken()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var paypalToken = GenerateToken();

            var paypalCreateRequest = new PaymentMethodRequest
            {
                CustomerId = customer.Id,
                PaymentMethodNonce = Nonce.PayPalBillingAgreement,
                Token = paypalToken
            };
            PaymentMethod paypalPaymentMethod = gateway.PaymentMethod.Create(paypalCreateRequest).Target;

            customer = gateway.Customer.Find(customer.Id);
            Assert.AreEqual(customer.DefaultPaymentMethod.Token, paypalToken);

            var venmoToken = GenerateToken();

            var venmoCreateRequest = new PaymentMethodRequest()
              {
                 CustomerId = customer.Id,
                 PaymentMethodNonce = Nonce.VenmoAccount,
                 Token = venmoToken
            };

            PaymentMethod venmoPaymentMethod = gateway.PaymentMethod.Create(venmoCreateRequest).Target;

            customer = gateway.Customer.Find(customer.Id);
            Assert.AreNotEqual(customer.DefaultPaymentMethod.Token, venmoToken);
            Assert.AreEqual(customer.DefaultPaymentMethod.Token, paypalToken);

            var updateRequest = new CustomerRequest
            {
                DefaultPaymentMethodToken = venmoToken
            };

            gateway.Customer.Update(customer.Id, updateRequest);
            customer = gateway.Customer.Find(customer.Id);
            Assert.AreEqual(customer.DefaultPaymentMethod.Token, venmoToken);
        }

        private String GenerateToken()
        {
            return $"token{random.Next()}";
        }

        [Test]
        public void Update_UpdatesCustomerWithVerificationAmount()
        {
            var createRequest = new CustomerRequest()
            {
                FirstName = "Joe",
                LastName = "Shmo",
            };
            Customer customer = gateway.Customer.Create(createRequest).Target;

            var updateRequest = new CustomerRequest()
            {
                CreditCard = new CreditCardRequest()
                {
                    Number = "5555555555554444",
                    ExpirationDate = "10/10",
                    Options = new CreditCardOptionsRequest
                    {
                        VerifyCard = true,
                        VerificationAmount = "1.02"
                    }
                }
            };

            Result<Customer> result = gateway.Customer.Create(updateRequest);
            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void Update_UpdateCustomerWithInvalidThreeDSecurePassThruParams()
        {
            var createRequest = new CustomerRequest()
            {
                FirstName = "Joe",
                LastName = "Shmo",
            };
            Customer customer = gateway.Customer.Create(createRequest).Target;
            ThreeDSecurePassThruRequest passThruParamsWithoutThreeDSecureVersion = new ThreeDSecurePassThruRequest()
            {
                EciFlag = "05",
                Cavv = "some_cavv",
                Xid = "some_xid",
                AuthenticationResponse = "Y",
                DirectoryResponse = "Y",
                CavvAlgorithm = "2",
                DsTransactionId = "some_ds_transaction_id"
            };

            var updateRequest = new CustomerRequest()
            {
                CreditCard = new CreditCardRequest()
                {
                    Number = "4111111111111111",
                    ExpirationDate = "05/25",
                    CVV = "123",
                    CardholderName = "Michael Angelo",
                    ThreeDSecurePassThru = passThruParamsWithoutThreeDSecureVersion,
                    Options = new CreditCardOptionsRequest()
                    {
                        VerifyCard = true
                    }
                }
            };

            Result<Customer> result = gateway.Customer.Update(customer.Id, updateRequest);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.VERIFICATION_THREE_D_SECURE_THREE_D_SECURE_VERSION_IS_REQUIRED,
                result.Errors.ForObject("Verification").OnField("ThreeDSecureVersion")[0].Code
            );
        }

        [Test]
        public void Update_UpdateCustomerWithThreeDSecurePassThruParams()
        {
            var createRequest = new CustomerRequest()
            {
                FirstName = "Joe",
                LastName = "Shmo",
            };
            Customer customer = gateway.Customer.Create(createRequest).Target;
            ThreeDSecurePassThruRequest passThruParamsWithThreeDSecureVersion = new ThreeDSecurePassThruRequest()
            {
                EciFlag = "05",
                Cavv = "some_cavv",
                Xid = "some_xid",
                AuthenticationResponse = "Y",
                ThreeDSecureVersion = "1.2.2",
                DirectoryResponse = "Y",
                CavvAlgorithm = "2",
                DsTransactionId = "some_ds_transaction_id"
            };

            var updateRequest = new CustomerRequest()
            {
                CreditCard = new CreditCardRequest()
                {
                    Number = "4111111111111111",
                    ExpirationDate = "05/25",
                    CVV = "123",
                    CardholderName = "Michael Angelo",
                    ThreeDSecurePassThru = passThruParamsWithThreeDSecureVersion,
                    Options = new CreditCardOptionsRequest()
                    {
                        VerifyCard = true
                    }
                }
            };

            Result<Customer> result = gateway.Customer.Update(customer.Id, updateRequest);

            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void UpdateWithoutSkipAdvancedFraudCheckingIncludesRiskData()
        {
            FraudProtectionEnterpriseSetup();
            CustomerRequest createRequest = new CustomerRequest()
            {
                FirstName = "John",
                LastName = "Doe",
            };
            Customer customer = gateway.Customer.Create(createRequest).Target;

            CustomerRequest updateRequest = new CustomerRequest()
            {
                CreditCard = new CreditCardRequest()
                {
                    Number = "4111111111111111",
                    ExpirationDate = "05/12",
                    CVV = "123",
                    CardholderName = "John Doe",
                    Options = new CreditCardOptionsRequest
                    {
                        VerifyCard = true,
                        SkipAdvancedFraudChecking = false
                    }
                }
            };

            Result<Customer> result = gateway.Customer.Update(customer.Id, updateRequest);
            Assert.IsTrue(result.IsSuccess());

            CreditCardVerification verification = result.Target.CreditCards[0].Verification;
            Assert.IsNotNull(verification);
            Assert.IsNotNull(verification.RiskData);
        }

        [Test]
        public void UpdateWithSkipAdvancedFraudCheckingDoesNotIncludeRiskData()
        {
            FraudProtectionEnterpriseSetup();
            CustomerRequest createRequest = new CustomerRequest()
            {
                FirstName = "John",
                LastName = "Doe",
            };
            Customer customer = gateway.Customer.Create(createRequest).Target;

            CustomerRequest updateRequest = new CustomerRequest()
            {
                CreditCard = new CreditCardRequest()
                {
                    Number = "4111111111111111",
                    ExpirationDate = "05/12",
                    CVV = "123",
                    CardholderName = "John Doe",
                    Options = new CreditCardOptionsRequest
                    {
                        VerifyCard = true,
                        SkipAdvancedFraudChecking = true
                    }
                }
            };

            Result<Customer> result = gateway.Customer.Update(customer.Id, updateRequest);
            Assert.IsTrue(result.IsSuccess());

            CreditCardVerification verification = result.Target.CreditCards[0].Verification;
            Assert.IsNotNull(verification);
            Assert.IsNull(verification.RiskData);
        }

        [Test]
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
                PaymentMethodNonce = Nonce.PayPalBillingAgreement
            };
            var updatedCustomer = gateway.Customer.Update(customer.Id, update).Target;

            Assert.AreEqual(1, updatedCustomer.PayPalAccounts.Length);
            Assert.AreEqual(1, updatedCustomer.CreditCards.Length);
            Assert.AreEqual(2, updatedCustomer.PaymentMethods.Length);
        }

        [Test]
        public void Update_AcceptsPayPalOrderPaymentMethodNonceAndPayPalOptions()
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

            string paymentMethodNonce = TestHelper.GenerateOrderPaymentPayPalNonce(gateway);

            var update = new CustomerRequest
            {
                PaymentMethodNonce = paymentMethodNonce,
                Options = new CustomerOptionsRequest {
                    OptionsPayPal = new CustomerOptionsPayPalRequest {
                        PayeeEmail = "payee@example.com",
                        OrderId = "order-id",
                        CustomField = "custom merchant field",
                        Description = "merchant description",
                        Amount = 1.23M,
                    }
                }
            };
            var updatedCustomer = gateway.Customer.Update(customer.Id, update).Target;

            Assert.AreEqual(1, updatedCustomer.PayPalAccounts.Length);
            Assert.AreEqual(1, updatedCustomer.CreditCards.Length);
            Assert.AreEqual(2, updatedCustomer.PaymentMethods.Length);
        }

        [Test]
        public void Delete_DeletesTheCustomer()
        {
            string id = Guid.NewGuid().ToString();
            gateway.Customer.Create(new CustomerRequest() { Id = id });
            Assert.AreEqual(id, gateway.Customer.Find(id).Id);

            Result<Customer> result = gateway.Customer.Delete(id);
            Assert.IsTrue(result.IsSuccess());

            Assert.Throws<NotFoundException>(() => gateway.Customer.Find(id));
        }

        [Test]
#if netcore
        public async Task DeleteAsync_DeletesTheCustomer()
#else
        public void DeleteAsync_DeletesTheCustomer()
        {
            Task.Run(async () =>
#endif
        {
            string id = Guid.NewGuid().ToString();
            await gateway.Customer.CreateAsync(new CustomerRequest() { Id = id });
            Assert.AreEqual(id, gateway.Customer.Find(id).Id);

            Result<Customer> result = await gateway.Customer.DeleteAsync(id);
            Assert.IsTrue(result.IsSuccess());

            Assert.Throws<NotFoundException>(() => gateway.Customer.Find(id));
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
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
#if netcore
        public async Task AllAsync()
#else
        public void AllAsync()
        {
            Task.Run(async () =>
#endif
        {
            ResourceCollection<Customer> collection = await gateway.Customer.AllAsync();

            Assert.IsTrue(collection.MaximumCount > 100);

            List<string> items = new List<string>();
            foreach (Customer item in collection) {
                items.Add(item.Id);
            }
            HashSet<string> uniqueItems = new HashSet<string>(items);

            Assert.AreEqual(uniqueItems.Count, collection.MaximumCount);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
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
        public void Search_OnAllTextFields()
        {
            string creditCardToken = $"cc{new Random().Next(1000000).ToString()}";

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
#if netcore
        public async Task SearchAsync_OnAllTextFields()
#else
        public void SearchAsync_OnAllTextFields()
        {
            Task.Run(async () =>
#endif
        {
            string creditCardToken = $"cc{new Random().Next(1000000).ToString()}";

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

            Result<Customer> customerResult = await gateway.Customer.CreateAsync(request);
            Customer customer = await gateway.Customer.FindAsync(customerResult.Target.Id);

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

            ResourceCollection<Customer> collection = await gateway.Customer.SearchAsync(searchRequest);

            Assert.AreEqual(1, collection.MaximumCount);
            Assert.AreEqual(customer.Id, collection.FirstItem.Id);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
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
        public void Search_OnPayPalAccountEmail()
        {
            var request = new CustomerRequest
            {
                PaymentMethodNonce = Nonce.PayPalBillingAgreement
            };

            var customer = gateway.Customer.Create(request).Target;

            var search = new CustomerSearchRequest().
                Id.Is(customer.Id).
                PayPalAccountEmail.Is(customer.PayPalAccounts[0].Email);

            Assert.AreEqual(1, gateway.Customer.Search(search).MaximumCount);
        }

        [Test]
        public void Create_WithMerchantCurrencyOption()
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
                    Options = new CreditCardOptionsRequest()
                    {
                        VerificationCurrencyIsoCode = "USD"
                    },
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

            Assert.AreEqual(1, customer.CreditCards.Length);
            Assert.AreEqual("510510", customer.CreditCards[0].Bin);
            Assert.AreEqual("5100", customer.CreditCards[0].LastFour);
            Assert.AreEqual("05", customer.CreditCards[0].ExpirationMonth);
            Assert.AreEqual("2012", customer.CreditCards[0].ExpirationYear);

            Address billingAddress = customer.CreditCards[0].BillingAddress;
            Assert.AreEqual("Macau", billingAddress.CountryName);
            Assert.AreEqual("MO", billingAddress.CountryCodeAlpha2);
            Assert.AreEqual("MAC", billingAddress.CountryCodeAlpha3);
            Assert.AreEqual("446", billingAddress.CountryCodeNumeric);
        }

        [Test]
        public void Create_WithInvalidMerchantCurrencyOption()
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
                    Options = new CreditCardOptionsRequest()
                    {
                        VerificationCurrencyIsoCode = "GBP"
                    },
                    BillingAddress = new CreditCardAddressRequest
                    {
                        CountryName = "Macau",
                        CountryCodeAlpha2 = "MO",
                        CountryCodeAlpha3 = "MAC",
                        CountryCodeNumeric = "446"
                    }
                }
            };

            Result<Customer> customer = gateway.Customer.Create(createRequest);
            Assert.IsFalse(customer.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.CREDIT_CARD_OPTIONS_VERIFICATION_INVALID_PRESENTMENT_CURRENCY,
                customer.Errors.DeepAll()[0].Code
            );
        }

        [Test]
        public void Update_CustomerBySupplyingMerchantCurrencyOption()
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
                Website = "www.example.com",
                CreditCard = new CreditCardRequest()
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/12",
                    Options = new CreditCardOptionsRequest()
                    {
                        VerificationCurrencyIsoCode = "USD"
                    }
                }
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
            Assert.AreEqual(1, updatedCustomer.CreditCards.Length);
            Assert.AreEqual("510510", updatedCustomer.CreditCards[0].Bin);
            Assert.AreEqual("5100", updatedCustomer.CreditCards[0].LastFour);
            Assert.AreEqual("05", updatedCustomer.CreditCards[0].ExpirationMonth);
            Assert.AreEqual("2012", updatedCustomer.CreditCards[0].ExpirationYear);
        }

        [Test]
        public void Update_CustomerBySupplyingInvalidMerchantCurrencyOption()
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
                Website = "old.example.com",
                CreditCard = new CreditCardRequest()
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/12",
                }
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
                Website = "www.example.com",
                CreditCard = new CreditCardRequest()
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/11",
                    Options = new CreditCardOptionsRequest()
                    {
                        VerificationCurrencyIsoCode = "GBP"
                    }
                }
            };

            Result<Customer> updatedCustomer = gateway.Customer.Update(oldId, updateRequest);
            Assert.IsFalse(updatedCustomer.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.CREDIT_CARD_OPTIONS_VERIFICATION_INVALID_PRESENTMENT_CURRENCY,
                updatedCustomer.Errors.DeepAll()[0].Code
            );
        }

        [Test]
        public void Create_WithNonceAndMerchantCurrencyOption()
        {
            string nonce = TestHelper.GenerateUnlockedNonce(gateway);
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest
            {
                CreditCard = new CreditCardRequest
                {
                    PaymentMethodNonce = nonce,
                    Options = new CreditCardOptionsRequest()
                    {
                        VerificationCurrencyIsoCode = "USD"
                    }
                }
            });
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual(1, result.Target.CreditCards.Length);
        }


        [Test]
        public void Create_WithNonceAndInvalidMerchantCurrencyOption()
        {
            string nonce = TestHelper.GenerateUnlockedNonce(gateway);
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest
            {
                CreditCard = new CreditCardRequest
                {
                    PaymentMethodNonce = nonce,
                    Options = new CreditCardOptionsRequest()
                    {
                        VerificationCurrencyIsoCode = "GBP"
                    }
                }
            });
            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
               ValidationErrorCode.CREDIT_CARD_OPTIONS_VERIFICATION_INVALID_PRESENTMENT_CURRENCY,
               result.Errors.DeepAll()[0].Code
           );
        }

        [Test]
        public void Update_CustomerWithNonceAndMerchantCurrencyOption()
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

            string nonce = TestHelper.GenerateUnlockedNonce(gateway);

            var updateRequest = new CustomerRequest()
            {
                Id = newId,
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Some Company",
                Email = "hansolo64@example.com",
                Phone = "312.555.1111",
                Fax = "312.555.1112",
                Website = "www.example.com",
                CreditCard = new CreditCardRequest()
                {
                    PaymentMethodNonce = nonce,
                    Options = new CreditCardOptionsRequest()
                    {
                        VerificationCurrencyIsoCode = "USD"
                    }
                }
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
            Assert.AreEqual(1, updatedCustomer.CreditCards.Length);
        }

        [Test]
        public void Update_CustomerWithNonceAndInvalidMerchantCurrencyOption()
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

            string nonce = TestHelper.GenerateUnlockedNonce(gateway);

            var updateRequest = new CustomerRequest()
            {
                Id = newId,
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Some Company",
                Email = "hansolo64@example.com",
                Phone = "312.555.1111",
                Fax = "312.555.1112",
                Website = "www.example.com",
                CreditCard = new CreditCardRequest()
                {
                    PaymentMethodNonce = nonce,
                    Options = new CreditCardOptionsRequest()
                    {
                        VerificationCurrencyIsoCode = "GBP"
                    }
                }
            };

            Result<Customer> updatedCustomer = gateway.Customer.Update(oldId, updateRequest);
            Assert.IsFalse(updatedCustomer.IsSuccess());
            Assert.AreEqual(
               ValidationErrorCode.CREDIT_CARD_OPTIONS_VERIFICATION_INVALID_PRESENTMENT_CURRENCY,
               updatedCustomer.Errors.DeepAll()[0].Code
           );
        }

        [Test]
        public void Create_CustomerWithTaxIdentifiers()
        {
            var createRequest = new CustomerRequest()
            {
                TaxIdentifiers = new TaxIdentifierRequest[]
                {
                    new TaxIdentifierRequest
                    {
                        CountryCode = "US",
                        Identifier = "123"
                    },
                    new TaxIdentifierRequest
                    {
                        CountryCode = "CL",
                        Identifier = "456"
                    },
                }
            };

            Result<Customer> result = gateway.Customer.Create(createRequest);
            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void Update_CustomerWithTaxIdentifiers()
        {
            var createRequest = new CustomerRequest()
            {
                TaxIdentifiers = new TaxIdentifierRequest[]
                {
                    new TaxIdentifierRequest
                    {
                        CountryCode = "US",
                        Identifier = "123"
                    },
                    new TaxIdentifierRequest
                    {
                        CountryCode = "CL",
                        Identifier = "456"
                    },
                }
            };

            Result<Customer> createResult = gateway.Customer.Create(createRequest);
            Assert.IsTrue(createResult.IsSuccess());

            var id = createResult.Target.Id;

            var updateRequest = new CustomerRequest()
            {
                TaxIdentifiers = new TaxIdentifierRequest[]
                {
                    new TaxIdentifierRequest
                    {
                        CountryCode = "CL",
                        Identifier = "789"
                    },
                }
            };

            Result<Customer> updateResult = gateway.Customer.Update(id, updateRequest);
            Assert.IsTrue(updateResult.IsSuccess());
        }

        [Test]
        public void Create_WithPaymentMethodNonceAndThreeDSecureAuthenticationId()
        {
            string nonce = TestHelper.GenerateUnlockedNonce(
                gateway,
                SandboxValues.CreditCardNumber.VISA,
                null
            );

            string three_d_secure_authentication_id = TestHelper.Create3DSVerification(service, MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID, new ThreeDSecureRequestForTests() {
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationMonth = "05",
                ExpirationYear = "2009"
            });

            CustomerRequest createRequest = new CustomerRequest()
            {
                PaymentMethodNonce = nonce,
                ThreeDSecureAuthenticationId = three_d_secure_authentication_id,
                CreditCard = new CreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationMonth = "05",
                    ExpirationYear = "2009",
                    Options = new CreditCardOptionsRequest
                    {
                        VerifyCard = true,
                        VerificationMerchantAccountId = MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID
                    }
                }
            };

            Result<Customer> result = gateway.Customer.Create(createRequest);
            Assert.IsTrue(result.IsSuccess());

            Customer customer = result.Target;

            CreditCard creditCard = customer.CreditCards[0];
            CreditCardVerification verification = creditCard.Verification;
            ThreeDSecureInfo threeDSecureInfo = verification.ThreeDSecureInfo;

            Assert.AreEqual("authenticate_successful", threeDSecureInfo.Status);
            Assert.IsNotNull(threeDSecureInfo.Enrolled);
            Assert.IsNotNull(threeDSecureInfo.Cavv);
            Assert.IsNotNull(threeDSecureInfo.EciFlag);
            Assert.IsNotNull(threeDSecureInfo.ThreeDSecureVersion);
            Assert.IsNotNull(threeDSecureInfo.Xid);
            Assert.IsTrue(threeDSecureInfo.LiabilityShifted);
            Assert.IsTrue(threeDSecureInfo.LiabilityShiftPossible);
        }
    }
}
