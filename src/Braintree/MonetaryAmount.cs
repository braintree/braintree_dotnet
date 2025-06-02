using System.Collections.Generic;

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

        /// <returns>
        /// A dictionary representing the monetary amount, to pass as variables to a GraphQL mutation
        /// </returns>
        public Dictionary<string, object> ToGraphQLVariables()
        {
            var variables = new Dictionary<string, object>();
            variables["value"] = Value;
            if (CurrencyCode != null)
            {
                variables["currencyCode"] = CurrencyCode;
            }

            return variables;
        }
    }
}