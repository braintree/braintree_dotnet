using System.Collections.Generic;
using System;

namespace Braintree.GraphQL
{
    /// <remarks>
    /// <b>Experimental:</b> This class is experimental and may change in future releases.
    /// </remarks>
    /// <summary>
    /// Input fields representing an amount with currency.
    /// </summary>
    public class MonetaryAmountInput
    {
        public virtual Decimal Value { get; protected set; }
        public virtual string CurrencyCode { get; protected set; }

        public MonetaryAmountInput(Decimal value, string currencyCode)
        {
            Value = value;
            CurrencyCode = currencyCode;
        }

        /// <returns>
        /// A dictionary representing the input object, to pass as variables to a GraphQL mutation
        /// </returns>
        public Dictionary<string, object> ToGraphQLVariables()
        {
            var variables = new Dictionary<string, object>();
            variables["value"] = Value.ToString();
            variables["currencyCode"] = CurrencyCode;
            return variables;
        }
    }
}
