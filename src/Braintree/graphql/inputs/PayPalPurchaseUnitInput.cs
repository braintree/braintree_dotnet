using System.Collections.Generic;

namespace Braintree.GraphQL
{
    /// <remarks>
    /// <b>Experimental:</b> This class is experimental and may change in future releases.
    /// </remarks>
    /// <summary>
    /// Payee and Amount of the item purchased.
    /// </summary>
    public class PayPalPurchaseUnitInput
    {
        public virtual PayPalPayeeInput Payee { get; protected set; }
        public virtual MonetaryAmountInput Amount { get; protected set; }

        protected PayPalPurchaseUnitInput(MonetaryAmountInput amount)
        {
            Amount = amount;
        }

        /// <returns>
        /// A dictionary representing the input object, to pass as variables to a GraphQL mutation
        /// </returns>
        public Dictionary<string, object> ToGraphQLVariables()
        {
            var variables = new Dictionary<string, object>();
            variables["amount"] = Amount.ToGraphQLVariables();
            if (Payee != null)
            {
                variables["payee"] = Payee.ToGraphQLVariables();
            }
            return variables;
        }

        /// <summary>
        /// Creates a builder for a <see cref="PayPalPurchaseUnitInput"/>.
        /// </summary>
        /// <param name="amount">The monetary amount.</param>
        /// <returns>A <see cref="PayPalPurchaseUnitInputBuilder"/> instance.</returns>
        public static PayPalPurchaseUnitInputBuilder Builder(MonetaryAmountInput amount)
        {
            return new PayPalPurchaseUnitInputBuilder(amount);
        }

        /// <summary>
        /// This class provides a fluent interface for constructing a <see cref="PayPalPurchaseUnitInput"/>.
        /// </summary>
        public class PayPalPurchaseUnitInputBuilder
        {
            private PayPalPurchaseUnitInput PayPalPurchaseUnitInput;

            public PayPalPurchaseUnitInputBuilder(MonetaryAmountInput amount)
            {
                PayPalPurchaseUnitInput = new PayPalPurchaseUnitInput(amount);
            }

            /// <summary>
            /// Sets the PayPal payee.
            /// </summary>
            /// <param name="payee">The PayPal payee.</param>
            /// <returns>The builder instance.</returns>
            public PayPalPurchaseUnitInputBuilder Payee(PayPalPayeeInput payee)
            {
                PayPalPurchaseUnitInput.Payee = payee;
                return this;
            }

            public PayPalPurchaseUnitInput Build()
            {
                return PayPalPurchaseUnitInput;
            }
        }
    }
}
