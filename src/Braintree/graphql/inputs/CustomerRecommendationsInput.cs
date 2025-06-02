using System.Collections.Generic;
using System.Linq;

namespace Braintree.GraphQL
{
    /// <remarks>
    /// <b>Experimental:</b> This class is experimental and may change in future releases.
    /// </remarks>
    /// <summary>
    /// Represents the input to request PayPal customer session recommendations.
    /// </summary>
    public class CustomerRecommendationsInput
    {
        public virtual string SessionId { get; protected set; }
        public virtual CustomerSessionInput Customer { get; protected set; }
        public virtual List<PayPalPurchaseUnitInput> PurchaseUnits { get; protected set; }
        public virtual string Domain { get; protected set; }
        public virtual string MerchantAccountId { get; protected set; }

        protected CustomerRecommendationsInput()
        {
        }

        /// <returns>
        /// A dictionary representing the input object, to pass as variables to a GraphQL mutation
        /// </returns>
        public Dictionary<string, object> ToGraphQLVariables()
        {
            var variables = new Dictionary<string, object>();

            if (SessionId != null)
            {
                variables.Add("sessionId", SessionId);
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

            if (Domain != null)
            {
                variables.Add("domain", Domain);
            }

            if (MerchantAccountId != null) {
                variables.Add("merchantAccountId", MerchantAccountId);
            }

            return variables;
        }

        /// <summary>
        /// Creates a builder for a <see cref="CustomerRecommendationsInput"/>.
        /// </summary>
        /// <returns>A <see cref="CustomerRecommendationsInputBuilder"/> instance.</returns>
        public static CustomerRecommendationsInputBuilder Builder()
        {
            return new CustomerRecommendationsInputBuilder();
        }

        /// <summary>
        /// This class provides a fluent interface for constructing a <see cref="CustomerRecommendationsInput"/>.
        /// <param name="customer">The customer session input object.</param>
        /// </summary>
        public class CustomerRecommendationsInputBuilder
        {
            private CustomerRecommendationsInput customerRecommendationsInput;

            public CustomerRecommendationsInputBuilder()
            {
                customerRecommendationsInput = new CustomerRecommendationsInput();
            }

            /// <summary>
            /// Sets the customer session ID. If not set, a new session will be created.
            /// </summary>
            /// <param name="sessionId">The customer session ID.</param>
            /// <returns>The builder instance.</returns>
            public CustomerRecommendationsInputBuilder SessionId(string sessionId)
            {
                customerRecommendationsInput.SessionId = sessionId;
                return this;
            }

            /// <summary>
            /// Sets the input object representing customer information relevant to the customer session.
            /// </summary>
            /// <param name="customer">The customer session input object.</param>
            /// <returns>The builder instance.</returns>
            public CustomerRecommendationsInputBuilder Customer(CustomerSessionInput customer)
            {
                customerRecommendationsInput.Customer = customer;
                return this;
            }

            /// <summary>
            /// Sets the Purchase Units for the items purchased.
            /// </summary>
            /// <param name="purchaseUnits">Purchase units.</param>
            /// <returns>The builder instance.</returns>
            public CustomerRecommendationsInputBuilder PurchaseUnits(List<PayPalPurchaseUnitInput> purchaseUnits)
            {
                customerRecommendationsInput.PurchaseUnits = purchaseUnits;
                return this;
            }
            
            /// <summary>
            /// Sets the customer domain.
            /// </summary>
            /// <param name="domain">The customer domain.</param>
            /// <returns>The builder instance.</returns>
            public CustomerRecommendationsInputBuilder Domain(string domain)
            {
                customerRecommendationsInput.Domain = domain;
                return this;
            }

            /// <summary>
            /// Sets the merchant account ID.
            /// </summary>
            /// <param name="merchantAccountId">The merchant account ID.</param>
            /// <returns>The builder instance.</returns>
            public CustomerRecommendationsInputBuilder MerchantAccountId(string merchantAccountId)
            {
                customerRecommendationsInput.MerchantAccountId = merchantAccountId;
                return this;
            }

            public CustomerRecommendationsInput Build()
            {
                return customerRecommendationsInput;
            }
        }
    }
}
