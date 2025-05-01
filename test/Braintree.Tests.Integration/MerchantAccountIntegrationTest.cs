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
    }
}
