using System.Collections.Generic;
using System.Linq;

namespace Braintree.GraphQL
{
    /// <summary>
    /// Represents the input to request PayPal customer session recommendations.
    /// </summary>
    public class CustomerRecommendationsInput
    {
        public virtual string SessionId { get; protected set; }
        public virtual List<Recommendations> Recommendations { get; protected set; }
        public virtual string MerchantAccountId { get; protected set; }
        public virtual CustomerSessionInput Customer { get; protected set; }

        protected CustomerRecommendationsInput(string sessionId, List<Recommendations> recommendations)
        {
            SessionId = sessionId;
            Recommendations = recommendations;
        }

        /// <returns>
        /// A dictionary representing the input object, to pass as variables to a GraphQL mutation
        /// </returns>
        public Dictionary<string, object> ToGraphQLVariables()
        {
            var variables = new Dictionary<string, object>();

            var recommendationsList = new List<string>();
            foreach (var recommendation in Recommendations)
            {
                recommendationsList.Add(recommendation.ToString());
            }

            if (MerchantAccountId != null) {
                variables.Add("merchantAccountId", MerchantAccountId);
            }
            variables.Add("sessionId", SessionId);
            variables.Add("recommendations", recommendationsList);

            if (Customer != null)
            {
                variables.Add("customer", Customer.ToGraphQLVariables());
            }

            return variables;
        }

        /// <summary>
        /// Creates a builder for a <see cref="CustomerRecommendationsInput"/>.
        /// </summary>
        /// <returns>A <see cref="CustomerRecommendationsInputBuilder"/> instance.</returns>
        public static CustomerRecommendationsInputBuilder Builder(
            string sessionId,
            List<Recommendations> recommendations
        )
        {
            return new CustomerRecommendationsInputBuilder(sessionId, recommendations);
        }

        /// <summary>
        /// This class provides a fluent interface for constructing a <see cref="CustomerRecommendationsInput"/>.
        /// </summary>
        public class CustomerRecommendationsInputBuilder
        {
            private CustomerRecommendationsInput customerRecommendationsInput;

            public CustomerRecommendationsInputBuilder(string sessionId, List<Recommendations> recommendations)
            {
                customerRecommendationsInput = new CustomerRecommendationsInput(sessionId, recommendations);
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

            public CustomerRecommendationsInput Build()
            {
                return customerRecommendationsInput;
            }
        }
    }
}
