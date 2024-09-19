using NUnit.Framework;
using System;
using System.Linq;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class MerchantIntegrationTest
    {
        private BraintreeGateway gateway;

        [SetUp]
        public void Setup()
        {
            gateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );
        }

        [Test]
        public void Create_ReturnsMerchantAndCredentials()
        {
            ResultImpl<Merchant> result = gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "GBR",
                PaymentMethods = new string[] {"credit_card", "paypal"},
                Scope = "read_write,shared_vault_transactions",
            });

            Assert.IsTrue(result.IsSuccess());
            Assert.IsFalse(string.IsNullOrEmpty(result.Target.Id));
            Assert.AreEqual("name@email.com", result.Target.Email);
            Assert.AreEqual("name@email.com", result.Target.CompanyName);
            Assert.AreEqual("GBR", result.Target.CountryCodeAlpha3);
            Assert.AreEqual("GB", result.Target.CountryCodeAlpha2);
            Assert.AreEqual("826", result.Target.CountryCodeNumeric);
            Assert.AreEqual("United Kingdom", result.Target.CountryName);

            Assert.IsTrue(result.Target.Credentials.AccessToken.StartsWith("access_token$"));
            Assert.IsTrue(result.Target.Credentials.RefreshToken.StartsWith("refresh_token$"));
            Assert.IsTrue(result.Target.Credentials.ExpiresAt > DateTime.Now);
            Assert.AreEqual("bearer", result.Target.Credentials.TokenType);
            Assert.AreEqual("read_write,shared_vault_transactions", result.Target.Credentials.Scope);
        }

        [Test]
        public void Create_FailsWithInvalidPaymentMethods()
        {
            ResultImpl<Merchant> result = gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "GBR",
                PaymentMethods = new string[] {"fake_money"}
            });

            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.MERCHANT_PAYMENT_METHODS_ARE_INVALID,
                result.Errors.ForObject("merchant").OnField("payment-methods")[0].Code
            );
        }

        [Test]
        public void Create_MultiCurrencyMerchant()
        {
            gateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );

            ResultImpl<Merchant> result = gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "GBR",
                PaymentMethods = new string[] {"credit_card", "paypal"},
                CompanyName = "Ziarog LTD",
                Currencies = new string[] {"GBP", "USD"}
            });

            Assert.IsTrue(result.IsSuccess());
            Assert.IsFalse(string.IsNullOrEmpty(result.Target.Id));
            Assert.AreEqual("name@email.com", result.Target.Email);
            Assert.AreEqual("Ziarog LTD", result.Target.CompanyName);
            Assert.AreEqual("GBR", result.Target.CountryCodeAlpha3);
            Assert.AreEqual("GB", result.Target.CountryCodeAlpha2);
            Assert.AreEqual("826", result.Target.CountryCodeNumeric);
            Assert.AreEqual("United Kingdom", result.Target.CountryName);

            Assert.IsTrue(result.Target.Credentials.AccessToken.StartsWith("access_token$"));
            Assert.IsTrue(result.Target.Credentials.RefreshToken.StartsWith("refresh_token$"));
            Assert.IsTrue(result.Target.Credentials.ExpiresAt > DateTime.Now);
            Assert.AreEqual("bearer", result.Target.Credentials.TokenType);

            Assert.AreEqual(2, result.Target.MerchantAccounts.Length);

            var usdMerchantAccount = (from ma in result.Target.MerchantAccounts where ma.Id == "USD" select ma).ToArray()[0];
            Assert.AreEqual("USD", usdMerchantAccount.CurrencyIsoCode);
            Assert.IsFalse(usdMerchantAccount.IsDefault.Value);

            var gbpMerchantAccount = (from ma in result.Target.MerchantAccounts where ma.Id == "GBP" select ma).ToArray()[0];
            Assert.AreEqual("GBP", gbpMerchantAccount.CurrencyIsoCode);
            Assert.IsTrue(gbpMerchantAccount.IsDefault.Value);
        }

        [Test]
        public void Create_PayPalOnlyMultiCurrencyMerchant()
        {
            gateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );

            ResultImpl<Merchant> result = gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "GBR",
                PaymentMethods = new string[] {"paypal"},
                CompanyName = "Ziarog LTD",
                Currencies = new string[] {"GBP", "USD"},
                PayPalAccount = new PayPalOnlyAccountRequest {
                    ClientId = "fake_client_id",
                    ClientSecret = "fake_client_secret"
                }
            });

            Assert.IsTrue(result.IsSuccess());
            Assert.IsFalse(string.IsNullOrEmpty(result.Target.Id));
            Assert.AreEqual("name@email.com", result.Target.Email);
            Assert.AreEqual("Ziarog LTD", result.Target.CompanyName);
            Assert.AreEqual("GBR", result.Target.CountryCodeAlpha3);
            Assert.AreEqual("GB", result.Target.CountryCodeAlpha2);
            Assert.AreEqual("826", result.Target.CountryCodeNumeric);
            Assert.AreEqual("United Kingdom", result.Target.CountryName);

            Assert.IsTrue(result.Target.Credentials.AccessToken.StartsWith("access_token$"));
            Assert.IsTrue(result.Target.Credentials.RefreshToken.StartsWith("refresh_token$"));
            Assert.IsTrue(result.Target.Credentials.ExpiresAt > DateTime.Now);
            Assert.AreEqual("bearer", result.Target.Credentials.TokenType);

            Assert.AreEqual(2, result.Target.MerchantAccounts.Length);

            var usdMerchantAccount = (from ma in result.Target.MerchantAccounts where ma.Id == "USD" select ma).ToArray()[0];
            Assert.AreEqual("USD", usdMerchantAccount.CurrencyIsoCode);
            Assert.IsFalse(usdMerchantAccount.IsDefault.Value);

            var gbpMerchantAccount = (from ma in result.Target.MerchantAccounts where ma.Id == "GBP" select ma).ToArray()[0];
            Assert.AreEqual("GBP", gbpMerchantAccount.CurrencyIsoCode);
            Assert.IsTrue(gbpMerchantAccount.IsDefault.Value);
        }

        [Test]
        public void Create_ReturnsErrorIfInvalidCurrencyPassed()
        {
            gateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );

            ResultImpl<Merchant> result = gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "GBR",
                PaymentMethods = new string[] {"paypal"},
                Currencies = new string[] {"USD", "FAKE"},
                PayPalAccount = new PayPalOnlyAccountRequest {
                    ClientId = "fake_client_id",
                    ClientSecret = "fake_client_secret"
                }
            });

            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.MERCHANT_CURRENCIES_ARE_INVALID,
                result.Errors.ForObject("merchant").OnField("currencies")[0].Code
            );
        }
    }
}
