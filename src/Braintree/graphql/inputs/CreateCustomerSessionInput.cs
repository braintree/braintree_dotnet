using Braintree;
using Braintree.GraphQL;
using System.Collections.Generic;

namespace Braintree.GraphQL
{
    /// <summary>
    /// Represents the input to request the creation of a PayPal customer session.
    /// </summary>
    public class CreateCustomerSessionInput
    {
        public string MerchantAccountId { get; protected set; }
        public string SessionId { get; protected set; }
        public CustomerSessionInput Customer { get; protected set; }
        public string Domain { get; protected set; }

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
            /// Sets the customer session ID.
            /// </summary>
            /// <param name="sessionId">The customer session ID.</param>
            /// <returns>The builder instance.</returns>
            public CreateCustomerSessionInputBuilder SessionId(string sessionId)
            {
                createCustomerSessionInput.SessionId = sessionId;
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
