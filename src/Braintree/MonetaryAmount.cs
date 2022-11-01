namespace Braintree
{
    public class MonetaryAmount
    {
        public virtual Decimal Value {get; protected set;}
        public virtual string CurrencyCode {get; protected set;}
        protected internal MonetaryAmount(Decimal value, string currencyCode)
        {
            Value = value;
            CurrencyCode = currencyCode;
        }
    }
}