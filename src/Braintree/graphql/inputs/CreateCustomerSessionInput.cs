using Braintree;
using Braintree.GraphQL;
using System.Collections.Generic;

namespace Braintree.GraphQL
{
    /// <remarks>
    /// <b>Experimental:</b> This class is experimental and may change in future releases.
    /// </remarks>
    /// <summary>
    /// Represents the input to request the creation of a PayPal customer session.
    /// </summary>
    public class CreateCustomerSessionInput
    {
        public virtual string SessionId { get; protected set; }
        public virtual string MerchantAccountId { get; protected set; }
        public virtual CustomerSessionInput Customer { get; protected set; }
        public virtual List<PayPalPurchaseUnitInput> PurchaseUnits { get; protected set; }
        public virtual string Domain { get; protected set; }

        protected CreateCustomerSessionInput() { }

        /// <returns>
        /// A dictionary representing the input object, to pass as variables to a GraphQL mutation
        /// </returns>
        public Dictionary<string, object> ToGraphQLVariables()
        {
            var variables = new Dictionary<string, object>();
            
            if (MerchantAccountId != null) {
                variables.Add("merchantAccountId", MerchantAccountId);
            }

            if (SessionId != null) {
                variables.Add("sessionId", SessionId);
            }

            if (Domain != null) {
                variables.Add("domain", Domain);
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
        /// Creates a builder for a <see cref="CreateCustomerSessionInput"/>.
        /// </summary>
        /// <returns>A <see cref="CreateCustomerSessionInputBuilder"/> instance.</returns>
        public static CreateCustomerSessionInputBuilder Builder()
        {
            return new CreateCustomerSessionInputBuilder();
        }

        /// <summary>
        /// This class provides a fluent interface for constructing a <see cref="CreateCustomerSessionInput"/>.
        /// </summary>
        public class CreateCustomerSessionInputBuilder
        {
            private CreateCustomerSessionInput createCustomerSessionInput;

            public CreateCustomerSessionInputBuilder()
            {
                createCustomerSessionInput = new CreateCustomerSessionInput();
            }

            /// <summary>
            /// Sets the customer session ID. If not set, a new session will be created.
            /// </summary>
            /// <param name="sessionId">The customer session ID.</param>
            /// <returns>The builder instance.</returns>
            public CreateCustomerSessionInputBuilder SessionId(string sessionId)
            {
                createCustomerSessionInput.SessionId = sessionId;
                return this;
            }

            /// <summary>
            /// Sets the merchant account ID.
            /// </summary>
            /// <param name="merchantAccountId">The merchant account ID.</param>
            /// <returns>The builder instance.</returns>
            public CreateCustomerSessionInputBuilder MerchantAccountId(string merchantAccountId)
            {
                createCustomerSessionInput.MerchantAccountId = merchantAccountId;
                return this;
            }

            /// <summary>
            /// Sets the input object representing customer information relevant to the customer session.
            /// </summary>
            /// <param name="customer">The customer session input object.</param>
            /// <returns>The builder instance.</returns>
            public CreateCustomerSessionInputBuilder Customer(CustomerSessionInput customer)
            {
                createCustomerSessionInput.Customer = customer;
                return this;
            }

            /// <summary>
            /// Sets the Purchase Units for the items purchased.
            /// </summary>
            /// <param name="purchaseUnits">Purchase units.</param>
            /// <returns>The builder instance.</returns>
            public CreateCustomerSessionInputBuilder PurchaseUnits(List<PayPalPurchaseUnitInput> purchaseUnits)
            {
                createCustomerSessionInput.PurchaseUnits = purchaseUnits;
                return this;
            }

            /// <summary>
            /// Sets the customer domain.
            /// </summary>
            /// <param name="domain">The customer domain.</param>
            /// <returns>The builder instance.</returns>
            public CreateCustomerSessionInputBuilder Domain(string domain)
            {
                createCustomerSessionInput.Domain = domain;
                return this;
            }

            public CreateCustomerSessionInput Build()
            {
                return createCustomerSessionInput;
            }
        }
    }
}
