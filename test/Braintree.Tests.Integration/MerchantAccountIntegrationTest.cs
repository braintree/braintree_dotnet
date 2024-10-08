using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class MerchantAccountIntegrationTest
    {
        private BraintreeGateway gateway;

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
        }

        [Test]
        public void Create_DeprecatedWithoutId()
        {
            var request = deprecatedCreateRequest(null);
            Result<MerchantAccount> result = gateway.MerchantAccount.Create(request);
            Assert.IsTrue(result.IsSuccess());

            MerchantAccount merchantAccount = result.Target;
            Assert.AreEqual(MerchantAccountStatus.PENDING, merchantAccount.Status);
            Assert.AreEqual("sandbox_master_merchant_account", merchantAccount.MasterMerchantAccount.Id);
            Assert.IsTrue(merchantAccount.IsSubMerchant);
            Assert.IsFalse(merchantAccount.MasterMerchantAccount.IsSubMerchant);
            Assert.IsTrue(merchantAccount.Id != null);
        }

        [Test]
        public void Create_DeprecatedWithId()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 10000);
            var subMerchantAccountId = "sub_merchant_account_id_" + randomNumber;
            var request = deprecatedCreateRequest(subMerchantAccountId);
            Result<MerchantAccount> result = gateway.MerchantAccount.Create(request);
            Assert.IsTrue(result.IsSuccess());

            MerchantAccount merchantAccount = result.Target;
            Assert.AreEqual(MerchantAccountStatus.PENDING, merchantAccount.Status);
            Assert.AreEqual("sandbox_master_merchant_account", merchantAccount.MasterMerchantAccount.Id);
            Assert.IsTrue(merchantAccount.IsSubMerchant);
            Assert.IsFalse(merchantAccount.MasterMerchantAccount.IsSubMerchant);
            Assert.AreEqual(subMerchantAccountId, merchantAccount.Id);
        }

        [Test]
        public void Create_HandlesUnsuccessfulResults()
        {
            Result<MerchantAccount> result = gateway.MerchantAccount.Create(new MerchantAccountRequest());
            Assert.IsFalse(result.IsSuccess());
            List<ValidationError> errors = result.Errors.ForObject("merchant-account").OnField("master-merchant-account-id");
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_MASTER_MERCHANT_ACCOUNT_ID_IS_REQUIRED, errors[0].Code);
        }

        [Test]
        public void Create_WithoutId()
        {
            var request = createRequest(null);
            Result<MerchantAccount> result = gateway.MerchantAccount.Create(request);
            Assert.IsTrue(result.IsSuccess());

            MerchantAccount merchantAccount = result.Target;
            Assert.AreEqual(MerchantAccountStatus.PENDING, merchantAccount.Status);
            Assert.AreEqual("sandbox_master_merchant_account", merchantAccount.MasterMerchantAccount.Id);
            Assert.IsTrue(merchantAccount.IsSubMerchant);
            Assert.IsFalse(merchantAccount.MasterMerchantAccount.IsSubMerchant);
            Assert.IsTrue(merchantAccount.Id != null);
        }

        [Test]
#if netcore
        public async Task CreateAsync_WithoutId()
#else
        public void CreateAsync_WithoutId()
        {
            Task.Run(async () =>
#endif
        {
            var request = createRequest(null);
            Result<MerchantAccount> result = await gateway.MerchantAccount.CreateAsync(request);
            Assert.IsTrue(result.IsSuccess());

            MerchantAccount merchantAccount = result.Target;
            Assert.AreEqual(MerchantAccountStatus.PENDING, merchantAccount.Status);
            Assert.AreEqual("sandbox_master_merchant_account", merchantAccount.MasterMerchantAccount.Id);
            Assert.IsTrue(merchantAccount.IsSubMerchant);
            Assert.IsFalse(merchantAccount.MasterMerchantAccount.IsSubMerchant);
            Assert.IsTrue(merchantAccount.Id != null);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Create_WithId()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 10000);
            var subMerchantAccountId = "sub_merchant_account_id_" + randomNumber;
            var request = createRequest(subMerchantAccountId);
            Result<MerchantAccount> result = gateway.MerchantAccount.Create(request);
            Assert.IsTrue(result.IsSuccess());

            MerchantAccount merchantAccount = result.Target;
            Assert.AreEqual(MerchantAccountStatus.PENDING, merchantAccount.Status);
            Assert.AreEqual("sandbox_master_merchant_account", merchantAccount.MasterMerchantAccount.Id);
            Assert.IsTrue(merchantAccount.IsSubMerchant);
            Assert.IsFalse(merchantAccount.MasterMerchantAccount.IsSubMerchant);
            Assert.AreEqual(subMerchantAccountId, merchantAccount.Id);
        }

        [Test]
        public void Create_AcceptsBankFundingDestination()
        {
            var request = createRequest(null);
            request.Funding.Destination = FundingDestination.BANK;
            Result<MerchantAccount> result = gateway.MerchantAccount.Create(request);
            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void Create_AcceptsMobilePhoneFundingDestination()
        {
            var request = createRequest(null);
            request.Funding.Destination = FundingDestination.MOBILE_PHONE;
            Result<MerchantAccount> result = gateway.MerchantAccount.Create(request);
            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void Create_AcceptsEmailFundingDestination()
        {
            var request = createRequest(null);
            request.Funding.Destination = FundingDestination.EMAIL;
            Result<MerchantAccount> result = gateway.MerchantAccount.Create(request);
            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void Update_UpdatesAllFields()
        {
            var updateRequest = createRequest(null);
            updateRequest.TosAccepted = null;
            updateRequest.MasterMerchantAccountId = null;
            Result<MerchantAccount> updateResult = gateway.MerchantAccount.Update("sandbox_sub_merchant_account", updateRequest);
            Assert.IsTrue(updateResult.IsSuccess());
            MerchantAccount merchantAccount = updateResult.Target;
            Assert.AreEqual("Job", merchantAccount.IndividualDetails.FirstName);
            Assert.AreEqual("Leoggs", merchantAccount.IndividualDetails.LastName);
            Assert.AreEqual("job@leoggs.com", merchantAccount.IndividualDetails.Email);
            Assert.AreEqual("5555551212", merchantAccount.IndividualDetails.Phone);
            Assert.AreEqual("1235", merchantAccount.IndividualDetails.SsnLastFour);
            Assert.AreEqual("193 Credibility St.", merchantAccount.IndividualDetails.Address.StreetAddress);
            Assert.AreEqual("60611", merchantAccount.IndividualDetails.Address.PostalCode);
            Assert.AreEqual("Avondale", merchantAccount.IndividualDetails.Address.Locality);
            Assert.AreEqual("IN", merchantAccount.IndividualDetails.Address.Region);
            Assert.AreEqual("1985-09-10", merchantAccount.IndividualDetails.DateOfBirth);
            Assert.AreEqual("coaterie.com", merchantAccount.BusinessDetails.LegalName);
            Assert.AreEqual("Coaterie", merchantAccount.BusinessDetails.DbaName);
            Assert.AreEqual("123456780", merchantAccount.BusinessDetails.TaxId);
            Assert.AreEqual("135 Credibility St.", merchantAccount.BusinessDetails.Address.StreetAddress);
            Assert.AreEqual("60602", merchantAccount.BusinessDetails.Address.PostalCode);
            Assert.AreEqual("Gary", merchantAccount.BusinessDetails.Address.Locality);
            Assert.AreEqual("OH", merchantAccount.BusinessDetails.Address.Region);
            Assert.AreEqual(FundingDestination.EMAIL, merchantAccount.FundingDetails.Destination);
            Assert.AreEqual("joe+funding@bloggs.com", merchantAccount.FundingDetails.Email);
            Assert.AreEqual("3125551212", merchantAccount.FundingDetails.MobilePhone);
            Assert.AreEqual("122100024", merchantAccount.FundingDetails.RoutingNumber);
            Assert.AreEqual("8798", merchantAccount.FundingDetails.AccountNumberLast4);
            Assert.AreEqual("Job Leoggs OH", merchantAccount.FundingDetails.Descriptor);
        }

        [Test]
#if netcore
        public async Task UpdateAsync_UpdatesAllFields()
#else
        public void UpdateAsync_UpdatesAllFields()
        {
            Task.Run(async () =>
#endif
        {
            var request = createRequest(null);
            var updateRequest = createRequest(null);
            updateRequest.TosAccepted = null;
            updateRequest.MasterMerchantAccountId = null;
            Result<MerchantAccount> updateResult = await gateway.MerchantAccount.UpdateAsync("sandbox_sub_merchant_account", updateRequest);
            Assert.IsTrue(updateResult.IsSuccess());
            MerchantAccount merchantAccount = updateResult.Target;
            Assert.AreEqual("Job", merchantAccount.IndividualDetails.FirstName);
            Assert.AreEqual("Leoggs", merchantAccount.IndividualDetails.LastName);
            Assert.AreEqual("job@leoggs.com", merchantAccount.IndividualDetails.Email);
            Assert.AreEqual("5555551212", merchantAccount.IndividualDetails.Phone);
            Assert.AreEqual("1235", merchantAccount.IndividualDetails.SsnLastFour);
            Assert.AreEqual("193 Credibility St.", merchantAccount.IndividualDetails.Address.StreetAddress);
            Assert.AreEqual("60611", merchantAccount.IndividualDetails.Address.PostalCode);
            Assert.AreEqual("Avondale", merchantAccount.IndividualDetails.Address.Locality);
            Assert.AreEqual("IN", merchantAccount.IndividualDetails.Address.Region);
            Assert.AreEqual("1985-09-10", merchantAccount.IndividualDetails.DateOfBirth);
            Assert.AreEqual("coaterie.com", merchantAccount.BusinessDetails.LegalName);
            Assert.AreEqual("Coaterie", merchantAccount.BusinessDetails.DbaName);
            Assert.AreEqual("123456780", merchantAccount.BusinessDetails.TaxId);
            Assert.AreEqual("135 Credibility St.", merchantAccount.BusinessDetails.Address.StreetAddress);
            Assert.AreEqual("60602", merchantAccount.BusinessDetails.Address.PostalCode);
            Assert.AreEqual("Gary", merchantAccount.BusinessDetails.Address.Locality);
            Assert.AreEqual("OH", merchantAccount.BusinessDetails.Address.Region);
            Assert.AreEqual(FundingDestination.EMAIL, merchantAccount.FundingDetails.Destination);
            Assert.AreEqual("joe+funding@bloggs.com", merchantAccount.FundingDetails.Email);
            Assert.AreEqual("3125551212", merchantAccount.FundingDetails.MobilePhone);
            Assert.AreEqual("122100024", merchantAccount.FundingDetails.RoutingNumber);
            Assert.AreEqual("8798", merchantAccount.FundingDetails.AccountNumberLast4);
            Assert.AreEqual("Job Leoggs OH", merchantAccount.FundingDetails.Descriptor);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Create_HandlesRequiredValidationErrors()
        {
            var request = new MerchantAccountRequest
            {
                TosAccepted = true,
                MasterMerchantAccountId = "sandbox_master_merchant_account"
            };
            Result<MerchantAccount> result = gateway.MerchantAccount.Create(request);
            Assert.IsFalse(result.IsSuccess());
            ValidationErrors errors = result.Errors.ForObject("merchant-account");
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_INDIVIDUAL_FIRST_NAME_IS_REQUIRED,
                errors.ForObject("individual").OnField("first-name")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_INDIVIDUAL_LAST_NAME_IS_REQUIRED,
                errors.ForObject("individual").OnField("last-name")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_INDIVIDUAL_DATE_OF_BIRTH_IS_REQUIRED,
                errors.ForObject("individual").OnField("date-of-birth")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_INDIVIDUAL_EMAIL_IS_REQUIRED,
                errors.ForObject("individual").OnField("email")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_INDIVIDUAL_ADDRESS_STREET_ADDRESS_IS_REQUIRED,
                errors.ForObject("individual").ForObject("address").OnField("street-address")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_INDIVIDUAL_ADDRESS_LOCALITY_IS_REQUIRED,
                errors.ForObject("individual").ForObject("address").OnField("locality")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_INDIVIDUAL_ADDRESS_POSTAL_CODE_IS_REQUIRED,
                errors.ForObject("individual").ForObject("address").OnField("postal-code")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_INDIVIDUAL_ADDRESS_REGION_IS_REQUIRED,
                errors.ForObject("individual").ForObject("address").OnField("region")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_FUNDING_DESTINATION_IS_REQUIRED,
                errors.ForObject("funding").OnField("destination")[0].Code);
        }

        [Test]
        public void Update_HandlesInvalidValidationErrors()
        {
            var request = new MerchantAccountRequest
            {
                Individual = new IndividualRequest
                {
                    FirstName = "<>",
                    LastName = "<>",
                    Email = "bad",
                    Phone = "999",
                    Address = new AddressRequest
                    {
                        StreetAddress = "nope",
                        PostalCode = "1",
                        Region = "QQ",
                    },
                    DateOfBirth = "hah",
                    Ssn = "12345",
                },
                Business = new BusinessRequest
                {
                    LegalName = "``{}",
                    DbaName = "{}``",
                    TaxId = "bad",
                    Address = new AddressRequest
                    {
                        StreetAddress = "nope",
                        PostalCode = "1",
                        Region = "QQ",
                    },
                },
                Funding = new FundingRequest
                {
                    Destination = FundingDestination.UNRECOGNIZED,
                    Email = "BILLFOLD",
                    MobilePhone = "TRIFOLD",
                    RoutingNumber = "LEATHER",
                    AccountNumber = "BACK POCKET",
                },
                TosAccepted = true,
                MasterMerchantAccountId = "sandbox_master_merchant_account"
            };
            Result<MerchantAccount> result = gateway.MerchantAccount.Update("sandbox_sub_merchant_account", request);
            Assert.IsFalse(result.IsSuccess());
            ValidationErrors errors = result.Errors.ForObject("merchant-account");
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_INDIVIDUAL_FIRST_NAME_IS_INVALID,
                errors.ForObject("individual").OnField("first-name")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_INDIVIDUAL_LAST_NAME_IS_INVALID,
                errors.ForObject("individual").OnField("last-name")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_INDIVIDUAL_DATE_OF_BIRTH_IS_INVALID,
                errors.ForObject("individual").OnField("date-of-birth")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_INDIVIDUAL_PHONE_IS_INVALID,
                errors.ForObject("individual").OnField("phone")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_INDIVIDUAL_SSN_IS_INVALID,
                errors.ForObject("individual").OnField("ssn")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_INDIVIDUAL_EMAIL_IS_INVALID,
                errors.ForObject("individual").OnField("email")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_INDIVIDUAL_ADDRESS_STREET_ADDRESS_IS_INVALID,
                errors.ForObject("individual").ForObject("address").OnField("street-address")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_INDIVIDUAL_ADDRESS_POSTAL_CODE_IS_INVALID,
                errors.ForObject("individual").ForObject("address").OnField("postal-code")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_INDIVIDUAL_ADDRESS_REGION_IS_INVALID,
                errors.ForObject("individual").ForObject("address").OnField("region")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_BUSINESS_DBA_NAME_IS_INVALID,
                errors.ForObject("business").OnField("dba-name")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_BUSINESS_LEGAL_NAME_IS_INVALID,
                errors.ForObject("business").OnField("legal-name")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_BUSINESS_TAX_ID_IS_INVALID,
                errors.ForObject("business").OnField("tax-id")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_BUSINESS_ADDRESS_STREET_ADDRESS_IS_INVALID,
                errors.ForObject("business").ForObject("address").OnField("street-address")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_BUSINESS_ADDRESS_POSTAL_CODE_IS_INVALID,
                errors.ForObject("business").ForObject("address").OnField("postal-code")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_BUSINESS_ADDRESS_REGION_IS_INVALID,
                errors.ForObject("business").ForObject("address").OnField("region")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_FUNDING_DESTINATION_IS_INVALID,
                errors.ForObject("funding").OnField("destination")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_FUNDING_ACCOUNT_NUMBER_IS_INVALID,
                errors.ForObject("funding").OnField("account-number")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_FUNDING_ROUTING_NUMBER_IS_INVALID,
                errors.ForObject("funding").OnField("routing-number")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_FUNDING_EMAIL_IS_INVALID,
                errors.ForObject("funding").OnField("email")[0].Code);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_FUNDING_MOBILE_PHONE_IS_INVALID,
                errors.ForObject("funding").OnField("mobile-phone")[0].Code);
        }

        [Test]
        public void Find()
        {
            MerchantAccount merchantAccount = gateway.MerchantAccount.Create(createRequest(null)).Target;
            MerchantAccount foundMerchantAccount = gateway.MerchantAccount.Find(merchantAccount.Id);
            Assert.AreEqual(merchantAccount.Id, foundMerchantAccount.Id);
        }

        [Test]
#if netcore
        public async Task FindAsync()
#else
        public void FindAsync()
        {
            Task.Run(async () =>
#endif
        {
            Result<MerchantAccount> result = await gateway.MerchantAccount.CreateAsync(createRequest(null));
            MerchantAccount merchantAccount = result.Target;
            MerchantAccount foundMerchantAccount = await gateway.MerchantAccount.FindAsync(merchantAccount.Id);
            Assert.AreEqual(merchantAccount.Id, foundMerchantAccount.Id);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void RetrievesCurrencyIsoCode()
        {
            MerchantAccount foundMerchantAccount = gateway.MerchantAccount.Find("sandbox_master_merchant_account");
            Assert.AreEqual("USD", foundMerchantAccount.CurrencyIsoCode);
        }

        [Test]
        public void CreateForCurrency()
        {
            gateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );

            ResultImpl<Merchant> merchantResult = gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "GBR",
                PaymentMethods = new string[] {"credit_card", "paypal"},
                CompanyName = "Ziarog LTD"
            });

            Assert.IsTrue(merchantResult.IsSuccess());

            gateway = new BraintreeGateway(merchantResult.Target.Credentials.AccessToken);
            Result<MerchantAccount> result = gateway.MerchantAccount.CreateForCurrency(new MerchantAccountCreateForCurrencyRequest {
                Currency = "USD",
                Id = "testId",
            });

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual("testId", result.Target.Id);
            Assert.AreEqual("USD", result.Target.CurrencyIsoCode);
        }

        [Test]
#if netcore
        public async Task CreateForCurrencyAsync()
#else
        public void CreateForCurrencyAsync()
        {
            Task.Run(async () =>
#endif
        {
            gateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );

            ResultImpl<Merchant> merchantResult = gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "GBR",
                PaymentMethods = new string[] {"credit_card", "paypal"},
                CompanyName = "Ziarog LTD"
            });

            Assert.IsTrue(merchantResult.IsSuccess());

            gateway = new BraintreeGateway(merchantResult.Target.Credentials.AccessToken);
            Result<MerchantAccount> result = await gateway.MerchantAccount.CreateForCurrencyAsync(new MerchantAccountCreateForCurrencyRequest {
                Currency = "USD",
                Id = "testId",
            });

            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual("testId", result.Target.Id);
            Assert.AreEqual("USD", result.Target.CurrencyIsoCode);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void CreateForCurrency_HandlesAlreadyExistingMerchantAccountForCurrency()
        {
            gateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );

            ResultImpl<Merchant> merchantResult = gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "GBR",
                PaymentMethods = new string[] {"credit_card", "paypal"},
                CompanyName = "Ziarog LTD"
            });

            Assert.IsTrue(merchantResult.IsSuccess());

            gateway = new BraintreeGateway(merchantResult.Target.Credentials.AccessToken);
            Result<MerchantAccount> result = gateway.MerchantAccount.CreateForCurrency(new MerchantAccountCreateForCurrencyRequest {
                Currency = "GBP",
            });

            Assert.IsFalse(result.IsSuccess());
            List<ValidationError> errors = result.Errors.ForObject("merchant").OnField("currency");
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_MERCHANT_ACCOUNT_EXISTS_FOR_CURRENCY, errors[0].Code);
        }

        [Test]
        public void CreateForCurrency_HandlesCurrencyRequirement()
        {
            gateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );

            ResultImpl<Merchant> merchantResult = gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "GBR",
                PaymentMethods = new string[] {"credit_card", "paypal"},
                CompanyName = "Ziarog LTD"
            });

            Assert.IsTrue(merchantResult.IsSuccess());

            gateway = new BraintreeGateway(merchantResult.Target.Credentials.AccessToken);
            Result<MerchantAccount> result = gateway.MerchantAccount.CreateForCurrency(new MerchantAccountCreateForCurrencyRequest());

            Assert.IsFalse(result.IsSuccess());
            List<ValidationError> errors = result.Errors.ForObject("merchant").OnField("currency");
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_CURRENCY_IS_REQUIRED, errors[0].Code);
        }

        [Test]
        public void CreateForCurrency_HandlesInvalidCurrency()
        {
            gateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );

            ResultImpl<Merchant> merchantResult = gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "GBR",
                PaymentMethods = new string[] {"credit_card", "paypal"},
                CompanyName = "Ziarog LTD"
            });

            Assert.IsTrue(merchantResult.IsSuccess());

            gateway = new BraintreeGateway(merchantResult.Target.Credentials.AccessToken);
            Result<MerchantAccount> result = gateway.MerchantAccount.CreateForCurrency(new MerchantAccountCreateForCurrencyRequest {
                Currency = "badCurrency",
            });

            Assert.IsFalse(result.IsSuccess());
            List<ValidationError> errors = result.Errors.ForObject("merchant").OnField("currency");
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_CURRENCY_IS_INVALID, errors[0].Code);
        }

        [Test]
        public void CreateForCurrency_HandlesExistingMerchantAccountForId()
        {
            gateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );

            ResultImpl<Merchant> merchantResult = gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "GBR",
                PaymentMethods = new string[] {"credit_card", "paypal"},
                CompanyName = "Ziarog LTD"
            });

            Assert.IsTrue(merchantResult.IsSuccess());

            gateway = new BraintreeGateway(merchantResult.Target.Credentials.AccessToken);
            Result<MerchantAccount> result = gateway.MerchantAccount.CreateForCurrency(new MerchantAccountCreateForCurrencyRequest {
                Currency = "GBP",
                Id = merchantResult.Target.MerchantAccounts[0].Id,
            });

            Assert.IsFalse(result.IsSuccess());
            List<ValidationError> errors = result.Errors.ForObject("merchant").OnField("id");
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_MERCHANT_ACCOUNT_EXISTS_FOR_ID, errors[0].Code);
        }

        [Test]
        public void All_ReturnsAllMerchantAccounts()
        {
            gateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );

            var code = OAuthTestHelper.CreateGrant(gateway, "integration_merchant_id", "read_write");
            ResultImpl<OAuthCredentials> accessTokenResult = gateway.OAuth.CreateTokenFromCode(new OAuthCredentialsRequest
            {
                Code = code,
                Scope = "read_write"
            });

            BraintreeGateway OAuthGateway = new BraintreeGateway(accessTokenResult.Target.AccessToken);

            var merchantAccountResults = OAuthGateway.MerchantAccount.All();

            var merchantAccounts = new List<MerchantAccount>();
            foreach (var merchantAccount in merchantAccountResults)
            {
                merchantAccounts.Add(merchantAccount);
            }
            Assert.IsTrue(merchantAccounts.Count > 20);
        }

        [Test]
        public void All_ReturnsMerchantAccountWithCorrectAttributes()
        {
            gateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );

            ResultImpl<Merchant> result = gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "GBR",
                PaymentMethods = new string[] {"credit_card", "paypal"},
                Scope = "read_write,shared_vault_transactions",
            });

            BraintreeGateway OAuthGateway = new BraintreeGateway(result.Target.Credentials.AccessToken);

            PaginatedCollection<MerchantAccount> merchantAccountResults = OAuthGateway.MerchantAccount.All();
            var merchantAccounts = new List<MerchantAccount>();
            foreach (var ma in merchantAccountResults)
            {
                merchantAccounts.Add(ma);
            }
            Assert.AreEqual(1, merchantAccounts.Count);

            MerchantAccount merchantAccount = merchantAccounts[0];
            Assert.AreEqual("GBP", merchantAccount.CurrencyIsoCode);
            Assert.AreEqual(MerchantAccountStatus.ACTIVE, merchantAccount.Status);
            Assert.IsTrue(merchantAccount.IsDefault);
        }
        // this is part of Marketplace and shouldn't be removed unless we're removing all Marketplace code
        private MerchantAccountRequest deprecatedCreateRequest(string id)
        {
            return new MerchantAccountRequest
            {
                ApplicantDetails = new ApplicantDetailsRequest
                {
                    CompanyName = "coattree.com",
                    FirstName = "Joe",
                    LastName = "Bloggs",
                    Email = "joe@bloggs.com",
                    Phone = "555-555-5555",
                    Address = new AddressRequest
                    {
                        StreetAddress = "123 Credibility St.",
                        PostalCode = "60606",
                        Locality = "Chicago",
                        Region = "IL",
                    },
                    DateOfBirth = "10/9/1980",
                    Ssn = "123-00-1234",
                    TaxId = "123456789",
                    RoutingNumber = "122100024",
                    AccountNumber = "43759348798"
                },
                TosAccepted = true,
                Id = id,
                MasterMerchantAccountId = "sandbox_master_merchant_account"
            };
        }

        private MerchantAccountRequest createRequest(string id)
        {
            return new MerchantAccountRequest
            {
                Individual = new IndividualRequest
                {
                    FirstName = "Job",
                    LastName = "Leoggs",
                    Email = "job@leoggs.com",
                    Phone = "555-555-1212",
                    Address = new AddressRequest
                    {
                        StreetAddress = "193 Credibility St.",
                        PostalCode = "60611",
                        Locality = "Avondale",
                        Region = "IN",
                    },
                    DateOfBirth = "10/9/1985",
                    Ssn = "123-00-1235",
                },
                Business = new BusinessRequest
                {
                    LegalName = "coaterie.com",
                    DbaName = "Coaterie",
                    TaxId = "123456780",
                    Address = new AddressRequest
                    {
                        StreetAddress = "135 Credibility St.",
                        PostalCode = "60602",
                        Locality = "Gary",
                        Region = "OH",
                    },
                },
                Funding = new FundingRequest
                {
                    Destination = FundingDestination.EMAIL,
                    Email = "joe+funding@bloggs.com",
                    MobilePhone = "3125551212",
                    RoutingNumber = "122100024",
                    AccountNumber = "43759348798",
                    Descriptor = "Job Leoggs OH",
                },
                TosAccepted = true,
                Id = id,
                MasterMerchantAccountId = "sandbox_master_merchant_account"
            };
        }
    }
}
