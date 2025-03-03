using NUnit.Framework;
using System;

namespace Braintree.Tests
{
    [TestFixture]
    public class PayPalPaymentResourceRequestTest
    {
        [Test]
        public void ToXml_IncludesAllData()
        {
            var request = new PayPalPaymentResourceRequest()
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
                PaymentMethodNonce = "some-nonce",
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
            };

            string xmlString = request.ToXml();

            Assert.IsTrue(xmlString.Contains("<paypal-payment-resource>"));
            Assert.IsTrue(xmlString.Contains("<amount>55.00</amount>"));
            Assert.IsTrue(xmlString.Contains("<amount-breakdown>"));
            Assert.IsTrue(xmlString.Contains("<discount>15.00</discount>"));
            Assert.IsTrue(xmlString.Contains("<handling>0.00</handling>"));
            Assert.IsTrue(xmlString.Contains("<insurance>5.00</insurance>"));
            Assert.IsTrue(xmlString.Contains("<item-total>45.00</item-total>"));
            Assert.IsTrue(xmlString.Contains("<shipping>10.00</shipping>"));
            Assert.IsTrue(xmlString.Contains("<shipping-discount>0.00</shipping-discount>"));
            Assert.IsTrue(xmlString.Contains("<tax-total>10.00</tax-total>"));
            Assert.IsTrue(xmlString.Contains("</amount-breakdown>"));
            Assert.IsTrue(xmlString.Contains("<currency-iso-code>USD</currency-iso-code>"));
            Assert.IsTrue(xmlString.Contains("<custom-field>0437</custom-field>"));
            Assert.IsTrue(xmlString.Contains("<description>My Plan Description</description>"));
            Assert.IsTrue(xmlString.Contains("<line-items type=\"array\">"));
            Assert.IsTrue(xmlString.Contains("<description>Shoes</description>"));
            Assert.IsTrue(xmlString.Contains("<image-url>https://example.com/products/23434/pic.png</image-url>"));
            Assert.IsTrue(xmlString.Contains("<kind>debit</kind>"));
            Assert.IsTrue(xmlString.Contains("<name>Name #1</name>"));
            Assert.IsTrue(xmlString.Contains("<product-code>23434</product-code>"));
            Assert.IsTrue(xmlString.Contains("<quantity>1</quantity>"));
            Assert.IsTrue(xmlString.Contains("<total-amount>45.00</total-amount>"));
            Assert.IsTrue(xmlString.Contains("<unit-amount>45.00</unit-amount>"));
            Assert.IsTrue(xmlString.Contains("<unit-tax-amount>10.00</unit-tax-amount>"));
            Assert.IsTrue(xmlString.Contains("<url>https://example.com/products/23434</url>"));
            Assert.IsTrue(xmlString.Contains("</line-items>"));
            Assert.IsTrue(xmlString.Contains("<order-id>order-123456789</order-id>"));
            Assert.IsTrue(xmlString.Contains("<payee-email>bt_buyer_us@paypal.com</payee-email>"));
            Assert.IsTrue(xmlString.Contains("<payment-method-nonce>some-nonce</payment-method-nonce>"));
            Assert.IsTrue(xmlString.Contains("<shipping>"));
            Assert.IsTrue(xmlString.Contains("<country-code-alpha2>US</country-code-alpha2>"));
            Assert.IsTrue(xmlString.Contains("<country-code-alpha3>USA</country-code-alpha3>"));
            Assert.IsTrue(xmlString.Contains("<country-code-numeric>484</country-code-numeric>"));
            Assert.IsTrue(xmlString.Contains("<country-name>United States of America</country-name>"));
            Assert.IsTrue(xmlString.Contains("<extended-address>Apt. #1</extended-address>"));
            Assert.IsTrue(xmlString.Contains("<first-name>John</first-name>"));
            Assert.IsTrue(xmlString.Contains("<international-phone>"));
            Assert.IsTrue(xmlString.Contains("<country-code>1</country-code>"));
            Assert.IsTrue(xmlString.Contains("<national-number>4081111111</national-number>"));
            Assert.IsTrue(xmlString.Contains("</international-phone>"));
            Assert.IsTrue(xmlString.Contains("<last-name>Doe</last-name>"));
            Assert.IsTrue(xmlString.Contains("<locality>Chicago</locality>"));
            Assert.IsTrue(xmlString.Contains("<postal-code>60618</postal-code>"));
            Assert.IsTrue(xmlString.Contains("<region>IL</region>"));
            Assert.IsTrue(xmlString.Contains("<street-address>123 Division Street</street-address>"));
            Assert.IsTrue(xmlString.Contains("</shipping>"));
            Assert.IsTrue(xmlString.Contains("<shipping-options type=\"array\">"));
            Assert.IsTrue(xmlString.Contains("<amount>10.00</amount>"));
            Assert.IsTrue(xmlString.Contains("<id>option1</id>"));
            Assert.IsTrue(xmlString.Contains("<label>fast</label>"));
            Assert.IsTrue(xmlString.Contains("<selected>true</selected>"));
            Assert.IsTrue(xmlString.Contains("<type>SHIPPING</type>"));
            Assert.IsTrue(xmlString.Contains("</shipping-options>"));
            Assert.IsTrue(xmlString.Contains("</paypal-payment-resource>"));
        }
    }
}


