using System.Collections.Generic;

namespace Braintree
{
    public class ExchangeRateQuoteInput
    {
        public virtual string BaseCurrency {get; protected set;}
        public virtual string QuoteCurrency {get; protected set; }
        public virtual string BaseAmount {get; protected set; }
        public virtual string Markup {get; protected set; }

        public ExchangeRateQuoteInput(string baseCurrency, string quoteCurrency, string baseAmount, string markup)
        {
            BaseCurrency = baseCurrency;
            QuoteCurrency = quoteCurrency;
            BaseAmount = baseAmount;
            Markup = markup;
        }

        public virtual Dictionary<string, object> ToGraphQLVariables() 
        {
            var Variables = new Dictionary<string, object>();
            Variables.Add("baseCurrency", BaseCurrency);
            Variables.Add("quoteCurrency", QuoteCurrency);
            Variables.Add("baseAmount", BaseAmount);
            Variables.Add("markup", Markup);
            return Variables;
        }
    }
}