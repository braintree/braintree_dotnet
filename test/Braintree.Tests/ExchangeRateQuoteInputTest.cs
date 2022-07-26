using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class ExchangeRateQuoteInputTest
    {
        [Test]
        public void TestToGraphQLVariables()
        {
            ExchangeRateQuoteInput input = new ExchangeRateQuoteInput("USD","EUR","10.15","5.00");
            var variable = input.ToGraphQLVariables();

            Assert.AreEqual(variable["baseCurrency"], "USD");
            Assert.AreEqual(variable["quoteCurrency"], "EUR");
            Assert.AreEqual(variable["baseAmount"], "10.15");
            Assert.AreEqual(variable["markup"], "5.00");
        }

        [Test]
        public void TestToGraphQLVariablesWithoutMarkupAndBaseAmount()
        {
            ExchangeRateQuoteInput input = new ExchangeRateQuoteInput("USD", "EUR", null, null);
            var variable = input.ToGraphQLVariables();

            Assert.AreEqual(variable["baseCurrency"], "USD");
            Assert.AreEqual(variable["quoteCurrency"], "EUR");
            Assert.AreEqual(variable["baseAmount"], null);
            Assert.AreEqual(variable["markup"], null);
        }

        [Test]
        public void TestToGraphQLVariablesWithAllEmptyFields()
        {
            ExchangeRateQuoteInput input = new ExchangeRateQuoteInput(null, null, null, null);
            var variable = input.ToGraphQLVariables();

            Assert.AreEqual(variable["baseCurrency"], null);
            Assert.AreEqual(variable["quoteCurrency"], null);
            Assert.AreEqual(variable["baseAmount"], null);
            Assert.AreEqual(variable["markup"], null);
        }
    }
}
