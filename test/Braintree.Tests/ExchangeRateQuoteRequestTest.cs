using NUnit.Framework;
using System.Collections.Generic;

namespace Braintree.Tests
{
    [TestFixture]
    public class ExchangeRateQuoteRequestTest
    {
        [Test]
        public void TestToGraphQLVariables()
        {
            var input1 = new ExchangeRateQuoteInput("USD", "EUR", "5.00", "3.00");
            var input2 = new ExchangeRateQuoteInput("EUR", "CAD", "15.00", "2.64");
            var request = new ExchangeRateQuoteRequest();
            request.AddExchangeRateQuoteInput(input1);
            request.AddExchangeRateQuoteInput(input2);

            var variable = request.ToGraphQLVariables();
            var requestMap = (Dictionary<string, object>)variable["exchangeRateQuoteRequest"];

            Assert.AreNotEqual(null, requestMap);

            var quotes = (List<Dictionary<string,object>>) requestMap["quotes"];
            Assert.AreNotEqual(null, quotes);
            Assert.AreEqual(2, quotes.Count);

            var quote1 = quotes[0];
            var quote2 = quotes[1];
            Assert.AreEqual("USD", quote1["baseCurrency"]);
            Assert.AreEqual("EUR", quote1["quoteCurrency"]);
            Assert.AreEqual("5.00", quote1["baseAmount"]);
            Assert.AreEqual("3.00", quote1["markup"]);
            Assert.AreEqual("EUR", quote2["baseCurrency"]);
            Assert.AreEqual("CAD", quote2["quoteCurrency"]);
            Assert.AreEqual("15.00", quote2["baseAmount"]);
            Assert.AreEqual("2.64", quote2["markup"]);
        }

        [Test]
        public void TestToGraphQLVariablesWithMissingFields()
        {
            var input1 = new ExchangeRateQuoteInput("USD", "EUR", "5.00", null);
            var input2 = new ExchangeRateQuoteInput("EUR", "CAD", null, null);
            var request = new ExchangeRateQuoteRequest();
            request.AddExchangeRateQuoteInput(input1);
            request.AddExchangeRateQuoteInput(input2);

            var variable = request.ToGraphQLVariables();
            var requestMap = (Dictionary<string, object>)variable["exchangeRateQuoteRequest"];

            Assert.AreNotEqual(null, requestMap);

            var quotes = (List<Dictionary<string, object>>)requestMap["quotes"];
            Assert.AreNotEqual(null, quotes);
            Assert.AreEqual(2, quotes.Count);

            var quote1 = quotes[0];
            var quote2 = quotes[1];
            Assert.AreEqual("USD", quote1["baseCurrency"]);
            Assert.AreEqual("EUR", quote1["quoteCurrency"]);
            Assert.AreEqual("5.00", quote1["baseAmount"]);
            Assert.AreEqual(null, quote1["markup"]);
            Assert.AreEqual("EUR", quote2["baseCurrency"]);
            Assert.AreEqual("CAD", quote2["quoteCurrency"]);
            Assert.AreEqual(null, quote2["baseAmount"]);
            Assert.AreEqual(null, quote2["markup"]);
        }
    }
}
