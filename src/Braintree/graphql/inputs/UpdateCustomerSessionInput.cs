using System.Collections.Generic;

namespace Braintree.GraphQL
{
    /// <remarks>
    /// <b>Experimental:</b> This class is experimental and may change in future releases.
    /// </remarks>
    /// <summary>
    /// Represents the input to request an update to a PayPal customer session.
    /// </summary>
    public class UpdateCustomerSessionInput
    {
        public virtual string MerchantAccountId { get; set; }
        public virtual string SessionId { get; protected set; }
        public virtual CustomerSessionInput Customer { get; set; }
        public virtual List<PayPalPurchaseUnitInput> PurchaseUnits { get; protected set; }

        protected UpdateCustomerSessionInput(string sessionId)
        {
            SessionId = sessionId;
        }

        /// <returns>
        /// A dictionary representing the input object, to pass as variables to a GraphQL mutation
        /// </returns>
        public Dictionary<string, object> ToGraphQLVariables()
        {
            var variables = new Dictionary<string, object>
            {
                { "sessionId", SessionId },
            };
            if (MerchantAccountId != null) {
                variables.Add("merchantAccountId", MerchantAccountId);
            }
            if (Customer != null)
            {
                variables.Add("customer", Customer.ToGraphQLVariables());
            }
            if (PurchaseUnits != null) {
                var purchaseUnits = new List<Dictionary<string, object>>();
                foreach (var purchaseUnit in PurchaseUnits)
                {
                    purchaseUnits.Add(purchaseUnit.ToGraphQLVariables());
                }
                variables.Add("purchaseUnits", purchaseUnits);
            }
            return variables;
        }

        /// <summary>
        /// Creates a builder for a <see cref="UpdateCustomerSessionInput"/>.
        /// </summary>
        /// <returns>A <see cref="UpdateCustomerSessionInputBuilder"/> instance.</returns>
        public static UpdateCustomerSessionInputBuilder Builder(string sessionId)
        {
            return new UpdateCustomerSessionInputBuilder(sessionId);
        }

        /// <summary>
        /// This class provides a fluent interface for constructing a <see cref="CreateCustomerSessionInput"/>.
        /// </summary>
        public class UpdateCustomerSessionInputBuilder
        {
            private UpdateCustomerSessionInput updateCustomerSessionInput;

            public UpdateCustomerSessionInputBuilder(string sessionId)
            {
                updateCustomerSessionInput = new UpdateCustomerSessionInput(sessionId);
            }

            /// <summary>
            /// Sets the merchant account ID.
            /// </summary>
            /// <param name="merchantAccountId">The merchant account ID.</param>
            /// <returns>The builder instance.</returns>
            public UpdateCustomerSessionInputBuilder MerchantAccountId(string merchantAccountId)
            {
                updateCustomerSessionInput.MerchantAccountId = merchantAccountId;
                return this;
            }

            /// <summary>
            /// Sets the input object representing customer information relevant to the customer session.
            /// </summary>
            /// <param name="customer">The customer session input object.</param>
            /// <returns>The builder instance.</returns>
            public UpdateCustomerSessionInputBuilder Customer(CustomerSessionInput customer)
            {
                updateCustomerSessionInput.Customer = customer;
                return this;
            }

            /// <summary>
            /// Sets the Purchase Units for the items purchased.
            /// </summary>
            /// <param name="purchaseUnits">Purchase units.</param>
            /// <returns>The builder instance.</returns>
            public UpdateCustomerSessionInputBuilder PurchaseUnits(List<PayPalPurchaseUnitInput> purchaseUnits)
            {
                updateCustomerSessionInput.PurchaseUnits = purchaseUnits;
                return this;
            }

            public UpdateCustomerSessionInput Build()
            {
                return updateCustomerSessionInput;
            }
        }
    }
}