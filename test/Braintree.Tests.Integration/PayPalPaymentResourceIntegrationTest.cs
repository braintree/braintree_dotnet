using Braintree;
using Braintree.Test;
using Braintree.TestUtil;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Braintree.Tests.Integration
{

    [TestFixture]
    public class PayPalPaymentResourceIntegrationTest
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
        public void canUpdatePaymentResource()
        {
            string nonce = TestHelper.GenerateOrderPaymentPayPalNonce(gateway);

            Result<PaymentMethodNonce> result = gateway.PayPalPaymentResource.Update(new PayPalPaymentResourceRequest()
            {
                Amount = 55,
                AmountBreakdown = new AmountBreakdownRequest
                {
                    Discount = 15,
                    Handling = 0,
                    Insurance = 5,
                    ItemTotal = 45,
                    Shipping = 10,
                    ShippingDiscount = 0,
                    TaxTotal = 10,
                },
                CurrencyIsoCode = "USD",
                CustomField = "0437",
                Description = "My Plan Description",
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Description = "Shoes",
                        ImageUrl = "https://example.com/products/23434/pic.png",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        Name = "Name #1",
                        ProductCode = "23434",
                        Quantity = 1,
                        TotalAmount = 45,
                        UnitAmount = 45,
                        UnitTaxAmount = 10,
                        Url = "https://example.com/products/23434",
                    },
                },
                OrderId = "order-123456789",
                PayeeEmail = "bt_buyer_us@paypal.com",
                PaymentMethodNonce = nonce,
                Shipping = new AddressRequest
                {
                    FirstName = "John",
                    LastName = "Doe",
                    StreetAddress = "123 Division Street",
                    ExtendedAddress = "Apt. #1",
                    Locality = "Chicago",
                    Region = "IL",
                    PostalCode = "60618",
                    CountryName = "United States of America",
                    CountryCodeAlpha2 = "US",
                    CountryCodeAlpha3 = "USA",
                    CountryCodeNumeric = "484",
                    InternationalPhone = new InternationalPhoneRequest
                    {
                        CountryCode = "1",
                        NationalNumber = "4081111111",
                    },
                },
                ShippingOptions = new ShippingOptionRequest[]
                {
                    new ShippingOptionRequest
                    {
                        Amount = 10,
                        Id = "option1",
                        Label = "fast",
                        Selected = true,
                        Type = "SHIPPING",
                    },
                }
            });

            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target.Nonce);

        }

                [Test]
#if netcore
        public async Task UpdateAsync_CanUpdatePaymentResource()
#else
        public void UpdateAsync_CanUpdatePaymentResource()
        {
            Task.Run(async () =>
#endif
        {
            string nonce = TestHelper.GenerateOrderPaymentPayPalNonce(gateway);

            Result<PaymentMethodNonce> result = gateway.PayPalPaymentResource.Update(new PayPalPaymentResourceRequest()
            {
                Amount = 55,
                AmountBreakdown = new AmountBreakdownRequest
                {
                    Discount = 15,
                    Handling = 0,
                    Insurance = 5,
                    ItemTotal = 45,
                    Shipping = 10,
                    ShippingDiscount = 0,
                    TaxTotal = 10,
                },
                CurrencyIsoCode = "USD",
                CustomField = "0437",
                Description = "My Plan Description",
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Description = "Shoes",
                        ImageUrl = "https://example.com/products/23434/pic.png",
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        Name = "Name #1",
                        ProductCode = "23434",
                        Quantity = 1,
                        TotalAmount = 45,
                        UnitAmount = 45,
                        UnitTaxAmount = 10,
                        Url = "https://example.com/products/23434",
                    },
                },
                OrderId = "order-123456789",
                PayeeEmail = "bt_buyer_us@paypal.com",
                PaymentMethodNonce = nonce,
                Shipping = new AddressRequest
                {
                    FirstName = "John",
                    LastName = "Doe",
                    StreetAddress = "123 Division Street",
                    ExtendedAddress = "Apt. #1",
                    Locality = "Chicago",
                    Region = "IL",
                    PostalCode = "60618",
                    CountryName = "United States of America",
                    CountryCodeAlpha2 = "US",
                    CountryCodeAlpha3 = "USA",
                    CountryCodeNumeric = "484",
                    InternationalPhone = new InternationalPhoneRequest
                    {
                        CountryCode = "1",
                        NationalNumber = "4081111111",
                    },
                },
                ShippingOptions = new ShippingOptionRequest[]
                {
                    new ShippingOptionRequest
                    {
                        Amount = 10,
                        Id = "option1",
                        Label = "fast",
                        Selected = true,
                        Type = "SHIPPING",
                    },
                }
            });

            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target.Nonce);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void updatePaymentResourceReturnsValidError()
        {
            string nonce = TestHelper.GenerateOrderPaymentPayPalNonce(gateway);

            Result<PaymentMethodNonce> result = gateway.PayPalPaymentResource.Update(new PayPalPaymentResourceRequest()
            {
                Amount = 55,
                CurrencyIsoCode = "USD",
                CustomField = "0437",
                Description = "My Plan Description",
                OrderId = "order-123456789",
                PayeeEmail = "bt_buyer_us@paypal",
                PaymentMethodNonce = nonce,
                ShippingOptions = new ShippingOptionRequest[]
                {
                    new ShippingOptionRequest
                    {
                        Amount = 10,
                        Id = "option1",
                        Label = "fast",
                        Selected = true,
                        Type = "SHIPPING",
                    },
                }
            });

            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.PAYPAL_PAYMENT_RESOURCE_INVALID_EMAIL,
                result.Errors.DeepAll()[0].Code
            );
        }
    }
}