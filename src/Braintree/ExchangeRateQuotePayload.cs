using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Braintree
{
    public class ExchangeRateQuotePayload
    {
        public virtual List<ExchangeRateQuote> Quotes {get; protected set;}
        public virtual bool IsSuccess {get; protected set;}
        public virtual IList<GraphQLError> Errors {get; protected set;}

        protected internal ExchangeRateQuotePayload(GraphQLResponse rawResponse)
        {
            Quotes = new List<ExchangeRateQuote>();
            if (rawResponse.errors != null)
            {
                IsSuccess = false;
                Errors = rawResponse.errors;
            }
            else
            {
                IsSuccess = true;
                var data = rawResponse.data;
                var generateExchangeRateQuote = (Dictionary<string, object>) data["generateExchangeRateQuote"];

                var quotesLinq = (JArray)generateExchangeRateQuote["quotes"];
                var quotes = quotesLinq.ToList();
             
                foreach(JObject jObject in quotes)
                {
                    var QuoteObj = jObject.ToObject<Dictionary<string, object>>();
                    var baseAmountJObj = (JObject) QuoteObj["baseAmount"];
                    var baseAmountObj = baseAmountJObj.ToObject<Dictionary<string, object>>();
                    var quoteAmountJObj = (JObject) QuoteObj["quoteAmount"];
                    var quoteAmountObj = quoteAmountJObj.ToObject<Dictionary<string, object>>();

                    var baseValue = Convert.ToDecimal((string)baseAmountObj["value"]);
                    var baseCurrencyCode = (string)baseAmountObj["currencyCode"];
                    var baseAmount = new MonetaryAmount(baseValue, baseCurrencyCode);

                    var quoteValue = Convert.ToDecimal((string)quoteAmountObj["value"]);
                    var quoteCurrencyCode = (string)quoteAmountObj["currencyCode"];
                    var quoteAmount = new MonetaryAmount(quoteValue, quoteCurrencyCode);

                    var exchangeRate = (string)QuoteObj["exchangeRate"];
                    var id = (string)QuoteObj["id"];
                    var tradeRate = (string)QuoteObj["tradeRate"];
                    var expiresAt = (DateTime) QuoteObj["expiresAt"];
                    var refreshesAt = (DateTime) QuoteObj["refreshesAt"];
                    var quote = new ExchangeRateQuote(id, tradeRate,refreshesAt, expiresAt, exchangeRate, baseAmount,quoteAmount);
                    Quotes.Add(quote);
                }
            }
        }
    }
}