using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var merchantResult = OAuthTestHelper.GetMerchant();
            gateway = new BraintreeGateway(merchantResult.Credentials.AccessToken);

            Result<MerchantAccount> result = gateway.MerchantAccount.CreateForCurrency(new MerchantAccountCreateForCurrencyRequest {
                Currency = "AUD"
            });

            Assert.IsTrue(result.IsSuccess(),
                result.IsSuccess() ? "" : $"Failed to create merchant account: {string.Join(", ", result.Errors.All().Select(e => e.Message))}"
            );
            Assert.AreEqual("AUD", result.Target.CurrencyIsoCode);
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
            var merchantResult = OAuthTestHelper.GetMerchant();
            gateway = new BraintreeGateway(merchantResult.Credentials.AccessToken);

            Result<MerchantAccount> result = await gateway.MerchantAccount.CreateForCurrencyAsync(new MerchantAccountCreateForCurrencyRequest {
                Currency = "NZD"
            });

            Assert.IsTrue(result.IsSuccess(),
                result.IsSuccess() ? "" : $"Failed to create merchant account: {string.Join(", ", result.Errors.All().Select(e => e.Message))}"
            );
            Assert.AreEqual("NZD", result.Target.CurrencyIsoCode);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void CreateForCurrency_HandlesAlreadyExistingMerchantAccountForCurrency()
        {
            var merchantResult = OAuthTestHelper.GetMerchant();
            gateway = new BraintreeGateway(merchantResult.Credentials.AccessToken);

            // Create a merchant account for CAD
            Result<MerchantAccount> createResult = gateway.MerchantAccount.CreateForCurrency(new MerchantAccountCreateForCurrencyRequest {
                Currency = "CAD",
            });

            Assert.IsTrue(createResult.IsSuccess(),
                createResult.IsSuccess() ? "" : $"Failed to create merchant account for CAD: {string.Join(", ", createResult.Errors.All().Select(e => e.Message))}");

            // Try to create another merchant account for the same currency
            Result<MerchantAccount> result = gateway.MerchantAccount.CreateForCurrency(new MerchantAccountCreateForCurrencyRequest {
                Currency = "CAD",
            });

            Assert.IsFalse(result.IsSuccess());
            List<ValidationError> errors = result.Errors.ForObject("merchant").OnField("currency");
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_MERCHANT_ACCOUNT_EXISTS_FOR_CURRENCY, errors[0].Code);
        }

        [Test]
        public void CreateForCurrency_HandlesCurrencyRequirement()
        {
            var merchantResult = OAuthTestHelper.GetMerchant();
            gateway = new BraintreeGateway(merchantResult.Credentials.AccessToken);

            Result<MerchantAccount> result = gateway.MerchantAccount.CreateForCurrency(new MerchantAccountCreateForCurrencyRequest());

            Assert.IsFalse(result.IsSuccess());
            List<ValidationError> errors = result.Errors.ForObject("merchant").OnField("currency");
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_CURRENCY_IS_REQUIRED, errors[0].Code);
        }

        [Test]
        public void CreateForCurrency_HandlesInvalidCurrency()
        {
            var merchantResult = OAuthTestHelper.GetMerchant();
            gateway = new BraintreeGateway(merchantResult.Credentials.AccessToken);

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
            var merchantResult = OAuthTestHelper.GetMerchant();
            gateway = new BraintreeGateway(merchantResult.Credentials.AccessToken);

            string existingMerchantAccountId = null;
            foreach (var ma in merchantResult.MerchantAccounts)
            {
                existingMerchantAccountId = ma.Id;
                break;
            }

            Result<MerchantAccount> result = gateway.MerchantAccount.CreateForCurrency(new MerchantAccountCreateForCurrencyRequest {
                Currency = "GBP",
                Id = existingMerchantAccountId,
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

            string code = OAuthTestHelper.CreateGrant(gateway, "integration_merchant_id", "read_write");

            ResultImpl<OAuthCredentials> accessTokenResult = gateway.OAuth.CreateTokenFromCode(new OAuthCredentialsRequest {
                Code = code,
                Scope = "read_write"
            });

            BraintreeGateway OAuthGateway = new BraintreeGateway(accessTokenResult.Target.AccessToken);

            PaginatedCollection<MerchantAccount> merchantAccountResults = OAuthGateway.MerchantAccount.All();
            var merchantAccounts = new List<MerchantAccount>();
            foreach (var ma in merchantAccountResults)
            {
                merchantAccounts.Add(ma);
            }
            Assert.IsTrue(merchantAccounts.Count > 0);

            MerchantAccount merchantAccount = merchantAccounts[0];
            Assert.AreEqual(MerchantAccountStatus.ACTIVE, merchantAccount.Status);
        }
    }
}
