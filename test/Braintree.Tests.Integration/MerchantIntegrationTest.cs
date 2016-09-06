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
                CountryCodeAlpha3 = "USA",
                PaymentMethods = new string[] {"credit_card", "paypal"},
                Scope = "read_write,shared_vault_transactions",
            });

            Assert.IsTrue(result.IsSuccess());
            Assert.IsFalse(string.IsNullOrEmpty(result.Target.Id));
            Assert.AreEqual("name@email.com", result.Target.Email);
            Assert.AreEqual("name@email.com", result.Target.CompanyName);
            Assert.AreEqual("USA", result.Target.CountryCodeAlpha3);
            Assert.AreEqual("US", result.Target.CountryCodeAlpha2);
            Assert.AreEqual("840", result.Target.CountryCodeNumeric);
            Assert.AreEqual("United States of America", result.Target.CountryName);

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
                CountryCodeAlpha3 = "USA",
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
                "client_id$development$signup_client_id",
                "client_secret$development$signup_client_secret"
            );

            ResultImpl<Merchant> result = gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "USA",
                PaymentMethods = new string[] {"paypal"},
                CompanyName = "Ziarog LTD",
                Currencies = new string[] {"GBP", "USD"},
                PayPalAccount = new PayPalOnlyAccountRequest {
                    ClientId = "paypal_client_id",
                    ClientSecret = "paypal_client_secret"
                }
            });

            Assert.IsTrue(result.IsSuccess());
            Assert.IsFalse(string.IsNullOrEmpty(result.Target.Id));
            Assert.AreEqual("name@email.com", result.Target.Email);
            Assert.AreEqual("Ziarog LTD", result.Target.CompanyName);
            Assert.AreEqual("USA", result.Target.CountryCodeAlpha3);
            Assert.AreEqual("US", result.Target.CountryCodeAlpha2);
            Assert.AreEqual("840", result.Target.CountryCodeNumeric);
            Assert.AreEqual("United States of America", result.Target.CountryName);

            Assert.IsTrue(result.Target.Credentials.AccessToken.StartsWith("access_token$"));
            Assert.IsTrue(result.Target.Credentials.RefreshToken.StartsWith("refresh_token$"));
            Assert.IsTrue(result.Target.Credentials.ExpiresAt > DateTime.Now);
            Assert.AreEqual("bearer", result.Target.Credentials.TokenType);

            Assert.AreEqual(2, result.Target.MerchantAccounts.Length);

            var usdMerchantAccount = (from ma in result.Target.MerchantAccounts where ma.Id == "USD" select ma).ToArray()[0];
            Assert.AreEqual("USD", usdMerchantAccount.CurrencyIsoCode);
            Assert.IsTrue(usdMerchantAccount.IsDefault.Value);

            var gbpMerchantAccount = (from ma in result.Target.MerchantAccounts where ma.Id == "GBP" select ma).ToArray()[0];
            Assert.AreEqual("GBP", gbpMerchantAccount.CurrencyIsoCode);
            Assert.IsFalse(gbpMerchantAccount.IsDefault.Value);
        }

        [Test]
        public void Create_AllowsCreationOfNonUSMerchantIfOnboardingApplicationIsInternal()
        {
            gateway = new BraintreeGateway(
                "client_id$development$signup_client_id",
                "client_secret$development$signup_client_secret"
            );

            ResultImpl<Merchant> result = gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "JPN",
                PaymentMethods = new string[] {"paypal"},
                PayPalAccount = new PayPalOnlyAccountRequest {
                    ClientId = "paypal_client_id",
                    ClientSecret = "paypal_client_secret"
                }
            });

            Assert.IsTrue(result.IsSuccess());
            Assert.IsFalse(string.IsNullOrEmpty(result.Target.Id));
            Assert.AreEqual("name@email.com", result.Target.Email);
            Assert.AreEqual("name@email.com", result.Target.CompanyName);
            Assert.AreEqual("JPN", result.Target.CountryCodeAlpha3);
            Assert.AreEqual("JP", result.Target.CountryCodeAlpha2);
            Assert.AreEqual("392", result.Target.CountryCodeNumeric);
            Assert.AreEqual("Japan", result.Target.CountryName);

            Assert.IsTrue(result.Target.Credentials.AccessToken.StartsWith("access_token$"));
            Assert.IsTrue(result.Target.Credentials.RefreshToken.StartsWith("refresh_token$"));
            Assert.IsTrue(result.Target.Credentials.ExpiresAt > DateTime.Now);
            Assert.AreEqual("bearer", result.Target.Credentials.TokenType);

            Assert.AreEqual(1, result.Target.MerchantAccounts.Length);

            var merchantAccount = result.Target.MerchantAccounts[0];
            Assert.AreEqual("JPY", merchantAccount.CurrencyIsoCode);
            Assert.IsTrue(merchantAccount.IsDefault.Value);
        }

        [Test]
        public void Create_DefaultsToUSDForNonUSMerchantIfOnboardingApplicationIsInternalAndCountryCurrencyNotSupported()
        {
            gateway = new BraintreeGateway(
                "client_id$development$signup_client_id",
                "client_secret$development$signup_client_secret"
            );

            ResultImpl<Merchant> result = gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "YEM",
                PaymentMethods = new string[] {"paypal"},
                PayPalAccount = new PayPalOnlyAccountRequest {
                    ClientId = "paypal_client_id",
                    ClientSecret = "paypal_client_secret"
                }
            });

            Assert.IsTrue(result.IsSuccess());
            Assert.IsFalse(string.IsNullOrEmpty(result.Target.Id));
            Assert.AreEqual("name@email.com", result.Target.Email);
            Assert.AreEqual("name@email.com", result.Target.CompanyName);
            Assert.AreEqual("YEM", result.Target.CountryCodeAlpha3);
            Assert.AreEqual("YE", result.Target.CountryCodeAlpha2);
            Assert.AreEqual("887", result.Target.CountryCodeNumeric);
            Assert.AreEqual("Yemen", result.Target.CountryName);

            Assert.IsTrue(result.Target.Credentials.AccessToken.StartsWith("access_token$"));
            Assert.IsTrue(result.Target.Credentials.RefreshToken.StartsWith("refresh_token$"));
            Assert.IsTrue(result.Target.Credentials.ExpiresAt > DateTime.Now);
            Assert.AreEqual("bearer", result.Target.Credentials.TokenType);

            Assert.AreEqual(1, result.Target.MerchantAccounts.Length);

            var merchantAccount = result.Target.MerchantAccounts[0];
            Assert.AreEqual("USD", merchantAccount.CurrencyIsoCode);
            Assert.IsTrue(merchantAccount.IsDefault.Value);
        }

        [Test]
        public void Create_IgnoresMultiCurrencyIfOnboardingApplicationIsNotInternal()
        {
            ResultImpl<Merchant> result = gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "USA",
                PaymentMethods = new string[] {"paypal"},
                Currencies = new string[] {"GBP", "USD"},
                PayPalAccount = new PayPalOnlyAccountRequest {
                    ClientId = "paypal_client_id",
                    ClientSecret = "paypal_client_secret"
                }
            });

            Assert.IsTrue(result.IsSuccess());
            Assert.IsFalse(string.IsNullOrEmpty(result.Target.Id));
            Assert.AreEqual("name@email.com", result.Target.Email);
            Assert.AreEqual("name@email.com", result.Target.CompanyName);
            Assert.AreEqual("USA", result.Target.CountryCodeAlpha3);
            Assert.AreEqual("US", result.Target.CountryCodeAlpha2);
            Assert.AreEqual("840", result.Target.CountryCodeNumeric);
            Assert.AreEqual("United States of America", result.Target.CountryName);

            Assert.IsTrue(result.Target.Credentials.AccessToken.StartsWith("access_token$"));
            Assert.IsTrue(result.Target.Credentials.RefreshToken.StartsWith("refresh_token$"));
            Assert.IsTrue(result.Target.Credentials.ExpiresAt > DateTime.Now);
            Assert.AreEqual("bearer", result.Target.Credentials.TokenType);

            Assert.AreEqual(1, result.Target.MerchantAccounts.Length);
        }

        [Test]
        public void Create_ReturnsErrorIfValidPaymentMethodOtherThanPayPalPassedForMultiCurrency()
        {
            gateway = new BraintreeGateway(
                "client_id$development$signup_client_id",
                "client_secret$development$signup_client_secret"
            );

            ResultImpl<Merchant> result = gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "USA",
                PaymentMethods = new string[] {"credit_card"},
                Currencies = new string[] {"GBP", "USD"},
                PayPalAccount = new PayPalOnlyAccountRequest {
                    ClientId = "paypal_client_id",
                    ClientSecret = "paypal_client_secret"
                }
            });

            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.MERCHANT_PAYMENT_METHODS_ARE_NOT_ALLOWED,
                result.Errors.ForObject("merchant").OnField("payment-methods")[0].Code
            );
        }

        [Test]
        public void Create_ReturnsErrorIfInvalidCurrencyPassed()
        {
            gateway = new BraintreeGateway(
                "client_id$development$signup_client_id",
                "client_secret$development$signup_client_secret"
            );

            ResultImpl<Merchant> result = gateway.Merchant.Create(new MerchantRequest {
                Email = "name@email.com",
                CountryCodeAlpha3 = "USA",
                PaymentMethods = new string[] {"paypal"},
                Currencies = new string[] {"GBP", "FAKE"},
                PayPalAccount = new PayPalOnlyAccountRequest {
                    ClientId = "paypal_client_id",
                    ClientSecret = "paypal_client_secret"
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
