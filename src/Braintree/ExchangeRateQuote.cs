using System;
namespace Braintree
{
    public class ExchangeRateQuote
    {
        public virtual string Id { get; protected set; }
        public virtual string TradeRate { get; protected set; }
        public virtual DateTime RefreshesAt { get; protected set; }
        public virtual DateTime ExpiresAt { get; protected set; }
        public virtual string ExchangeRate { get; protected set; }
        public virtual MonetaryAmount BaseAmount { get; protected set; }
        public virtual MonetaryAmount QuoteAmount { get; protected set; }

        protected internal ExchangeRateQuote(string id, string tradeRate, DateTime refreshesAt, DateTime expiresAt, string exchangeRate, MonetaryAmount baseAmount, MonetaryAmount quoteAmount)
        {
            Id = id;
            TradeRate = tradeRate;
            RefreshesAt = refreshesAt;
            ExpiresAt = expiresAt;
            ExchangeRate = exchangeRate;
            BaseAmount = baseAmount;
            QuoteAmount = quoteAmount;
        }
    }
}