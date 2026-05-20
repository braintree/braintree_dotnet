using Braintree.Exceptions;
using Braintree.TestUtil;
using Braintree.GraphQL;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
#if netcore
using System.Net.Http;
#endif

namespace Braintree.Tests.Integration
{
    [TestFixture]
    [Ignore("unpend when we have a more stable CI")]
    public class LocalPaymentContextGatewayTest
    {
        private BraintreeGateway gateway;

        [SetUp]
        public void Setup()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "pwpp_multi_account_merchant",
                PublicKey = "pwpp_multi_account_merchant_public_key",
                PrivateKey = "pwpp_multi_account_merchant_private_key"
            };
        }

        [Test]
        [Ignore("LocalPaymentContext tests pending")]
        public void Create_CreatesLocalPaymentContext()
        {
            var billingAddress = BillingAddressInput.Builder()
                .StreetAddress("Rua da Liberdade, 79")
                .ExtendedAddress("Apt 2")
                .Locality("Lisbon")
                .PostalCode("1250-140")
                .CountryCode("PT")
                .Build();

            var shippingAddress = ShippingAddressInput.Builder()
                .StreetAddress("Av. da República, 123")
                .Locality("Porto")
                .PostalCode("4000-001")
                .CountryCode("PT")
                .Build();

            var payerInfo = PayerInfoInput.Builder()
                .Email("john.doe@example.com")
                .GivenName("John")
                .Surname("Doe")
                .PhoneNumber("912345678")
                .PhoneCountryCode("351")
                .BillingAddress(billingAddress)
                .ShippingAddress(shippingAddress)
                .Build();

            var input = CreateLocalPaymentContextInput.Builder()
                .Amount(10.00m, "EUR")
                .Type(LocalPaymentType.MBWAY)
                .MerchantAccountId("eur_pwpp_multi_account_merchant_account")
                .ReturnUrl("https://example.com/return")
                .CancelUrl("https://example.com/cancel")
                .PayerInfo(payerInfo)
                .Build();

            var result = gateway.LocalPaymentContext.Create(input);

            if (!result.IsSuccess())
            {
                Console.WriteLine("Create failed with errors:");
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors.DeepAll())
                    {
                        Console.WriteLine($"  - {error.Code}: {error.Message}");
                    }
                }
            }

            Assert.IsTrue(result.IsSuccess(), $"Expected success but got errors: {(result.Errors != null ? string.Join(", ", result.Errors.DeepAll()) : "unknown error")}");
            Assert.IsNotNull(result.Target);
            Assert.IsNotNull(result.Target.Id);
            Assert.IsNotNull(result.Target.LegacyId);
            Assert.AreEqual("MBWAY", result.Target.Type);
            Assert.AreEqual("eur_pwpp_multi_account_merchant_account", result.Target.MerchantAccountId);
            Assert.IsNotNull(result.Target.Amount);
            Assert.AreEqual("10.00", result.Target.Amount.Value.ToString());
            Assert.AreEqual("EUR", result.Target.Amount.CurrencyCode);
        }

        [Test]
        [Ignore("LocalPaymentContext tests pending")]
        public void Create_WithOnlyRequiredFields()
        {
            var payerInfo = PayerInfoInput.Builder()
                .Email("john.doe@example.com")
                .GivenName("John")
                .Surname("Doe")
                .PhoneNumber("912345678")
                .PhoneCountryCode("351")
                .Build();

            var input = CreateLocalPaymentContextInput.Builder()
                .Amount(15.00m, "EUR")
                .Type(LocalPaymentType.MBWAY)
                .MerchantAccountId("eur_pwpp_multi_account_merchant_account")
                .ReturnUrl("https://example.com/return")
                .CancelUrl("https://example.com/cancel")
                .PayerInfo(payerInfo)
                .Build();

            var result = gateway.LocalPaymentContext.Create(input);

            Assert.IsTrue(result.IsSuccess(), $"Expected success but got errors: {(result.Errors != null ? string.Join(", ", result.Errors.DeepAll()) : "unknown error")}");
            Assert.IsNotNull(result.Target);
            Assert.IsNotNull(result.Target.Id);
            Assert.IsNotNull(result.Target.LegacyId);
            Assert.AreEqual("MBWAY", result.Target.Type);
            Assert.AreEqual("eur_pwpp_multi_account_merchant_account", result.Target.MerchantAccountId);
            Assert.IsNotNull(result.Target.Amount);
            Assert.AreEqual("15.00", result.Target.Amount.Value.ToString());
            Assert.AreEqual("EUR", result.Target.Amount.CurrencyCode);
        }

        [Test]
        [Ignore("LocalPaymentContext tests pending")]
#if netcore
        public async Task CreateAsync_CreatesLocalPaymentContext()
#else
        public void CreateAsync_CreatesLocalPaymentContext()
        {
            Task.Run(async() =>
#endif
        {
            var billingAddress = BillingAddressInput.Builder()
                .StreetAddress("Rua da Liberdade, 79")
                .ExtendedAddress("Apt 2")
                .Locality("Lisbon")
                .PostalCode("1250-140")
                .CountryCode("PT")
                .Build();

            var payerInfo = PayerInfoInput.Builder()
                .Email("john.doe@example.com")
                .GivenName("John")
                .Surname("Doe")
                .PhoneNumber("912345678")
                .PhoneCountryCode("351")
                .BillingAddress(billingAddress)
                .Build();

            var input = CreateLocalPaymentContextInput.Builder()
                .Amount(10.00m, "EUR")
                .Type(LocalPaymentType.MBWAY)
                .MerchantAccountId("eur_pwpp_multi_account_merchant_account")
                .ReturnUrl("https://example.com/return")
                .CancelUrl("https://example.com/cancel")
                .PayerInfo(payerInfo)
                .Build();

            var result = await gateway.LocalPaymentContext.CreateAsync(input);

            if (!result.IsSuccess())
            {
                Console.WriteLine("CreateAsync failed with errors:");
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors.DeepAll())
                    {
                        Console.WriteLine($"  - {error.Code}: {error.Message}");
                    }
                }
            }

            Assert.IsTrue(result.IsSuccess(), $"Expected success but got errors: {(result.Errors != null ? string.Join(", ", result.Errors.DeepAll()) : "unknown error")}");
            Assert.IsNotNull(result.Target);
            Assert.IsNotNull(result.Target.Id);
            Assert.IsNotNull(result.Target.LegacyId);
            Assert.AreEqual("MBWAY", result.Target.Type);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        [Ignore("LocalPaymentContext tests pending")]
        public void Create_WithMBWAY()
        {
            var billingAddress = BillingAddressInput.Builder()
                .StreetAddress("Rua da Liberdade, 79")
                .Locality("Lisbon")
                .PostalCode("1250-140")
                .CountryCode("PT")
                .Build();

            var payerInfo = PayerInfoInput.Builder()
                .Email("john.doe@example.com")
                .GivenName("John")
                .Surname("Doe")
                .PhoneNumber("912345678")
                .PhoneCountryCode("351")
                .BillingAddress(billingAddress)
                .Build();

            var input = CreateLocalPaymentContextInput.Builder()
                .Amount(10.00m, "EUR")
                .Type(LocalPaymentType.MBWAY)
                .MerchantAccountId("eur_pwpp_multi_account_merchant_account")
                .ReturnUrl("https://example.com/return")
                .CancelUrl("https://example.com/cancel")
                .PayerInfo(payerInfo)
                .Build();

            var result = gateway.LocalPaymentContext.Create(input);

            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target);
            Assert.AreEqual("MBWAY", result.Target.Type);
        }

        [Test]
        [Ignore("LocalPaymentContext tests pending")]
        public void Create_WithCrypto()
        {
            var payerInfo = PayerInfoInput.Builder()
                .Email("john.doe@example.com")
                .GivenName("John")
                .Surname("Doe")
                .Build();

            var input = CreateLocalPaymentContextInput.Builder()
                .Amount(25.00m, "USD")
                .Type(LocalPaymentType.CRYPTO)
                .MerchantAccountId("usd_pwpp_multi_account_merchant_account")
                .ReturnUrl("https://example.com/return")
                .CancelUrl("https://example.com/cancel")
                .PayerInfo(payerInfo)
                .Build();

            var result = gateway.LocalPaymentContext.Create(input);

            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target);
            Assert.AreEqual("CRYPTO", result.Target.Type);
        }

        [Test]
        [Ignore("LocalPaymentContext tests pending")]
        public void Find_FindsLocalPaymentContext()
        {
            var billingAddress = BillingAddressInput.Builder()
                .StreetAddress("Rua da Liberdade, 79")
                .Locality("Lisbon")
                .PostalCode("1250-140")
                .CountryCode("PT")
                .Build();

            var payerInfo = PayerInfoInput.Builder()
                .Email("john.doe@example.com")
                .GivenName("John")
                .Surname("Doe")
                .PhoneNumber("912345678")
                .PhoneCountryCode("351")
                .BillingAddress(billingAddress)
                .Build();

            var input = CreateLocalPaymentContextInput.Builder()
                .Amount(10.00m, "EUR")
                .Type(LocalPaymentType.MBWAY)
                .MerchantAccountId("eur_pwpp_multi_account_merchant_account")
                .ReturnUrl("https://example.com/return")
                .CancelUrl("https://example.com/cancel")
                .PayerInfo(payerInfo)
                .Build();

            var createResult = gateway.LocalPaymentContext.Create(input);
            Assert.IsTrue(createResult.IsSuccess());

            var findResult = gateway.LocalPaymentContext.Find(createResult.Target.Id);

            Assert.IsTrue(findResult.IsSuccess());
            Assert.IsNotNull(findResult.Target);
            Assert.AreEqual(createResult.Target.Id, findResult.Target.Id);
            Assert.AreEqual("MBWAY", findResult.Target.Type);
            Assert.AreEqual("eur_pwpp_multi_account_merchant_account", findResult.Target.MerchantAccountId);
            Assert.IsNotNull(findResult.Target.Amount);
            Assert.AreEqual("10.00", findResult.Target.Amount.Value.ToString());
            Assert.AreEqual("EUR", findResult.Target.Amount.CurrencyCode);
        }

        [Test]
        [Ignore("LocalPaymentContext tests pending")]
#if netcore
        public async Task FindAsync_FindsLocalPaymentContext()
#else
        public void FindAsync_FindsLocalPaymentContext()
        {
            Task.Run(async() =>
#endif
        {
            var billingAddress = BillingAddressInput.Builder()
                .StreetAddress("Rua da Liberdade, 79")
                .Locality("Lisbon")
                .PostalCode("1250-140")
                .CountryCode("PT")
                .Build();

            var payerInfo = PayerInfoInput.Builder()
                .Email("john.doe@example.com")
                .GivenName("John")
                .Surname("Doe")
                .PhoneNumber("912345678")
                .PhoneCountryCode("351")
                .BillingAddress(billingAddress)
                .Build();

            var input = CreateLocalPaymentContextInput.Builder()
                .Amount(10.00m, "EUR")
                .Type(LocalPaymentType.MBWAY)
                .MerchantAccountId("eur_pwpp_multi_account_merchant_account")
                .ReturnUrl("https://example.com/return")
                .CancelUrl("https://example.com/cancel")
                .PayerInfo(payerInfo)
                .Build();

            var createResult = await gateway.LocalPaymentContext.CreateAsync(input);
            Assert.IsTrue(createResult.IsSuccess());

            var findResult = await gateway.LocalPaymentContext.FindAsync(createResult.Target.Id);

            Assert.IsTrue(findResult.IsSuccess());
            Assert.IsNotNull(findResult.Target);
            Assert.AreEqual(createResult.Target.Id, findResult.Target.Id);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Find_ThrowsNotFoundExceptionForNonexistentId()
        {
            Assert.Throws<NotFoundException>(() => gateway.LocalPaymentContext.Find("nonexistent_id"));
        }

        [Test]
        public void Create_HandlesValidationErrors()
        {
            var input = CreateLocalPaymentContextInput.Builder()
                .Amount(10.00m)
                .Type(LocalPaymentType.MBWAY)
                .Build();

            var result = gateway.LocalPaymentContext.Create(input);

            Assert.IsFalse(result.IsSuccess());
            Assert.IsNotNull(result.Errors);
        }
    }
}
