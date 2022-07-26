using System.Collections.Generic;
namespace Braintree
{
    public class ExchangeRateQuoteRequest
    {
        public virtual List<ExchangeRateQuoteInput> Quotes { get; protected set; }

        public ExchangeRateQuoteRequest()
        {
            Quotes = new List<ExchangeRateQuoteInput>();
        }

        public void AddExchangeRateQuoteInput(ExchangeRateQuoteInput exchangeRateQuoteInput)
        {
            Quotes.Add(exchangeRateQuoteInput);
        }

        public virtual Dictionary<string,object> ToGraphQLVariables()
        {
            var Variables = new Dictionary<string,object>();
            var Input = new Dictionary<string,object>();
            var QuotesList = new List<Dictionary<string, object>>();

            foreach (var quote in this.Quotes) {
                QuotesList.Add(quote.ToGraphQLVariables());
            }
            Input.Add("quotes", QuotesList);
            Variables.Add("exchangeRateQuoteRequest", Input);
            return Variables;
        }
    }
}