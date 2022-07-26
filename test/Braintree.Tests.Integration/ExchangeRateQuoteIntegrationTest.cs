using Braintree.Exceptions;
using NUnit.Framework;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class ExchangeRateQuoteIntegrationTest
    {
        public BraintreeGateway GetGateway()
        {
            Configuration configuration = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );
            return new BraintreeGateway(configuration);
        }

        [Test]
        public void ExchangeRateQuoteWithGraphQL()
        {
            BraintreeGateway gateway = GetGateway();

            var input1 = new ExchangeRateQuoteInput("USD", "EUR", "12.19", "12.14");
            var input2 = new ExchangeRateQuoteInput("EUR", "CAD", "15.16", "2.64");
            var request = new ExchangeRateQuoteRequest();
            request.AddExchangeRateQuoteInput(input1);
            request.AddExchangeRateQuoteInput(input2);

            var result = gateway.ExchangeRateQuote.Generate(request);
            Assert.IsTrue(result.IsSuccess);

            var quotes = result.Quotes;
            Assert.AreNotEqual(null, quotes);
            Assert.AreEqual(2, quotes.Count);

            ExchangeRateQuote quote1 = quotes[0];
            Assert.AreEqual("12.19", quote1.BaseAmount.Value.ToString());
            Assert.AreEqual("USD", quote1.BaseAmount.CurrencyCode);
            Assert.AreEqual("12.16", quote1.QuoteAmount.Value.ToString());
            Assert.AreEqual("EUR", quote1.QuoteAmount.CurrencyCode);
            Assert.AreEqual("0.997316360864", quote1.ExchangeRate);
            Assert.AreEqual("0.01", quote1.TradeRate);
            Assert.AreEqual("ZXhjaGFuZ2VyYXRlcXVvdGVfMDEyM0FCQw", quote1.Id);

            var ExpiresAt1 = quote1.ExpiresAt;
            Assert.AreEqual(6, ExpiresAt1.Month);
            Assert.AreEqual(16, ExpiresAt1.Day);
            Assert.AreEqual(2021, ExpiresAt1.Year);
            Assert.AreEqual(2, ExpiresAt1.Hour);
            Assert.AreEqual(0, ExpiresAt1.Minute);
            Assert.AreEqual(0, ExpiresAt1.Second);

            var RefreshesAt1 = quote1.RefreshesAt;
            Assert.AreEqual(6, RefreshesAt1.Month);
            Assert.AreEqual(16, RefreshesAt1.Day);
            Assert.AreEqual(2021, RefreshesAt1.Year);
            Assert.AreEqual(0, RefreshesAt1.Hour);
            Assert.AreEqual(0, RefreshesAt1.Minute);
            Assert.AreEqual(0, RefreshesAt1.Second);

            ExchangeRateQuote quote2 = quotes[1];
            Assert.AreEqual("15.16", quote2.BaseAmount.Value.ToString());
            Assert.AreEqual("EUR", quote2.BaseAmount.CurrencyCode);
            Assert.AreEqual("23.30", quote2.QuoteAmount.Value.ToString());
            Assert.AreEqual("CAD", quote2.QuoteAmount.CurrencyCode);
            Assert.AreEqual("1.536744692129366", quote2.ExchangeRate);
            Assert.AreEqual(null, quote2.TradeRate);
            Assert.AreEqual("ZXhjaGFuZ2VyYXRlcXVvdGVfQUJDMDEyMw", quote2.Id);

            var ExpiresAt2 = quote2.ExpiresAt;
            Assert.AreEqual(6, ExpiresAt2.Month);
            Assert.AreEqual(16, ExpiresAt2.Day);
            Assert.AreEqual(2021, ExpiresAt2.Year);
            Assert.AreEqual(2, ExpiresAt2.Hour);
            Assert.AreEqual(0, ExpiresAt2.Minute);
            Assert.AreEqual(0, ExpiresAt2.Second);

            var RefreshesAt2 = quote2.RefreshesAt;
            Assert.AreEqual(6, RefreshesAt2.Month);
            Assert.AreEqual(16, RefreshesAt2.Day);
            Assert.AreEqual(2021, RefreshesAt2.Year);
            Assert.AreEqual(0, RefreshesAt2.Hour);
            Assert.AreEqual(0, RefreshesAt2.Minute);
            Assert.AreEqual(0, RefreshesAt2.Second);
        }

        [Test]
        public void ExchangeRateQuoteWithGraphQLQuoteCurrencyValidationError()
        {
            BraintreeGateway gateway = GetGateway();

            var input1 = new ExchangeRateQuoteInput("USD", null, "12.19", "12.14");
            var input2 = new ExchangeRateQuoteInput("EUR", "CAD", "15.16", "2.64");
            var request = new ExchangeRateQuoteRequest();
            request.AddExchangeRateQuoteInput(input1);
            request.AddExchangeRateQuoteInput(input2);

            Assert.Throws<UnexpectedException>(() => gateway.ExchangeRateQuote.Generate(request));
        }

        [Test]
        public void ExchangeRateQuoteWithGraphQLBaseCurrencyValidationError()
        {
            BraintreeGateway gateway = GetGateway();

            var input1 = new ExchangeRateQuoteInput("USD", "EUR", "12.19", "12.14");
            var input2 = new ExchangeRateQuoteInput(null, "CAD", "15.16", "2.64");
            var request = new ExchangeRateQuoteRequest();
            request.AddExchangeRateQuoteInput(input1);
            request.AddExchangeRateQuoteInput(input2);

            Assert.Throws<UnexpectedException>(() => gateway.ExchangeRateQuote.Generate(request));
        }

        [Test]
        public void ExchangeRateQuoteWithGraphQLBothBaseCurrencyAndQuoteCurrencyValidationError()
        {
            BraintreeGateway gateway = GetGateway();

            var input1 = new ExchangeRateQuoteInput("USD", "EUR", "12.19", "12.14");
            var input2 = new ExchangeRateQuoteInput(null, null, "15.16", "2.64");
            var request = new ExchangeRateQuoteRequest();
            request.AddExchangeRateQuoteInput(input1);
            request.AddExchangeRateQuoteInput(input2);

            Assert.Throws<UnexpectedException>(() => gateway.ExchangeRateQuote.Generate(request));
        }

        [Test]
        public void ExchangeRateQuoteWithGraphQLWithoutbaseAmount()
        {
            BraintreeGateway gateway = GetGateway();

            var input1 = new ExchangeRateQuoteInput("USD", "EUR", null, null);
            var input2 = new ExchangeRateQuoteInput("EUR", "CAD", null, null);
            var request = new ExchangeRateQuoteRequest();
            request.AddExchangeRateQuoteInput(input1);
            request.AddExchangeRateQuoteInput(input2);

            var result = gateway.ExchangeRateQuote.Generate(request);
            Assert.AreEqual(true, result.IsSuccess);
        }
    }
}
