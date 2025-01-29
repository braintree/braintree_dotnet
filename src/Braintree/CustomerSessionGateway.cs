using Braintree.Exceptions;
using Braintree.GraphQL;
using System;
using System.Collections.Generic;

namespace Braintree
{
    /// <summary>
    /// Creates and manages PayPal customer sessions.
    /// </summary>
    public class CustomerSessionGateway : ICustomerSessionGateway
    {
        private const string CreateCustomerSessionMutation =
            @"mutation CreateCustomerSession($input: CreateCustomerSessionInput!) { 
                createCustomerSession(input: $input) { sessionId }
            }";

        private const string UpdateCustomerSessionMutation =
            @"mutation UpdateCustomerSession($input: UpdateCustomerSessionInput!) {
                updateCustomerSession(input: $input) { sessionId }
            }";

        private const string GetCustomerRecommendationsQuery =
            @"query CustomerRecommendations($input: CustomerRecommendationsInput!) {
                customerRecommendations(input: $input) {
                    isInPayPalNetwork
                    recommendations {
                        ... on PaymentRecommendations {
                            paymentOptions {
                                paymentOption
                                recommendedPriority
                            }
                        }
                    }
                }
            }";

        private readonly IGraphQLClient graphQLClient;

        public CustomerSessionGateway(IGraphQLClient graphQLClient)
        {
            this.graphQLClient = graphQLClient;
        }

        /// <summary>
        /// Creates a PayPal customer session.
        /// </summary>
        /// <example>
        /// For example
        /// <code>
        /// var phoneInput = PhoneInput.Builder()
        /// .CountryPhoneCode("1")
        /// .PhoneNumber("555555555")
        /// .Build();
        /// 
        /// var customer = CustomerSessionInput.Builder()
        /// .Email("test@example.com")
        /// .Phone(phoneInput)
        /// .DeviceFingerprintId("12345")
        /// .PaypalAppInstalled(true)
        /// .VenmoAppInstalled(true)
        /// .Build();
        /// 
        /// var input = CreateCustomerSessionInput.Builder()
        /// .Customer(customer)
        /// .Build();
        ///
        /// var result = PwppGateway().CustomerSession.CreateCustomerSession(input);
        ///
        /// if (result.IsSuccess())
        /// {
        ///     var sessionId = result.Target;
        /// }
        /// </code>
        /// </example>
        /// <param name="input">The input object representing the customer session creation request.</param>
        /// <returns>A <see cref="Result{T}"/> containing the session ID if successful, or validation errors otherwise.</returns>
        /// <exception cref="UnexpectedException">Thrown if the response from the GraphQL server is unexpected.</exception>
        public Result<string> CreateCustomerSession(CreateCustomerSessionInput input)
        {
            var variables = new Dictionary<string, object>
            {
                { "input", input.ToGraphQLVariables() },
            };
            return executeMutation(CreateCustomerSessionMutation, variables, "createCustomerSession");
        }

        /// <summary>
        /// Updates a PayPal customer session.
        /// </summary>
        /// <example>
        /// For example
        /// <code>
        /// var phoneInput = PhoneInput.Builder()
        /// .CountryPhoneCode("1")
        /// .PhoneNumber("555555555")
        /// .Build();
        /// 
        /// var customer = CustomerSessionInput.Builder()
        /// .Email("test@example.com")
        /// .Phone(phoneInput)
        /// .DeviceFingerprintId("12345")
        /// .PaypalAppInstalled(true)
        /// .VenmoAppInstalled(true)
        /// .Build();
        /// 
        /// var input = UpdateCustomerSessionInput.Builder(sessionId)
        /// .Customer(customer)
        /// .Build();
        ///
        /// var result = PwppGateway().CustomerSession.UpdateCustomerSession(input);
        ///
        /// if (result.IsSuccess())
        /// {
        ///     var sessionId = result.Target;
        /// }
        /// </code>
        /// </example>
        /// <param name="input">The input object representing the customer session update request.</param>
        /// <returns>A <see cref="Result{T}"/> containing the session ID if successful, or validation errors otherwise.</returns>
        /// <exception cref="UnexpectedException">Thrown if the response from the GraphQL server is unexpected.</exception>
        public Result<string> UpdateCustomerSession(UpdateCustomerSessionInput input)
        {
            var variables = new Dictionary<string, object>
            {
                { "input", input.ToGraphQLVariables() },
            };
            return executeMutation(UpdateCustomerSessionMutation, variables, "updateCustomerSession");
        }

        /// <summary>
        /// Retrieves PayPal customer session recommendations.
        /// </summary>
        /// <example>
        /// For example
        /// <code>
        /// var input = CustomerRecommendationsInput.Builder(sessionId,  new List<Recommendations> { Recommendations.PAYMENT_RECOMMENDATIONS })
        /// .Customer(customer)
        /// .Build();
        ///
        /// var result = PwppGateway().CustomerSession.GetCustomerRecommendations(input);
        ///
        /// if (result.IsSuccess())
        /// {
        ///     var payload = result.Target;
        ///     var paymentRecommendations = payload.Recommendations.PaymentRecommendations;
        /// }
        /// </code>
        /// </example>
        /// /// <param name="input">The input object representing the customer recommendations request.</param>
        /// <returns>A <see cref="Result{T}"/> containing the customer recommendations payload if successful, or validation errors otherwise.</returns>
        /// <exception cref="UnexpectedException">Thrown if the response from the GraphQL server is unexpected.</exception>
        public Result<CustomerRecommendationsPayload> GetCustomerRecommendations(CustomerRecommendationsInput input)
        {
            var variables = new Dictionary<string, object>
            {
                { "input", input.ToGraphQLVariables() },
            };
            var response = graphQLClient.Query(GetCustomerRecommendationsQuery, variables);
            if (response.errors != null)
            {
                return new ResultImpl<CustomerRecommendationsPayload>(response.GetValidationErrors());
            }
            var payload = new CustomerRecommendationsPayload(response.data);
            return new ResultImpl<CustomerRecommendationsPayload>(payload);
        }

        private ResultImpl<string> executeMutation(
            string queryString,
            Dictionary<string, object> variables,
            string operationName
        )
        {
            var response = graphQLClient.Query(queryString, variables);
            if (response.errors != null)
            {
                return new ResultImpl<string>(response.GetValidationErrors());
            }

            return getSessionIdResultFromResponse(response, operationName);
        }

        private ResultImpl<string> getSessionIdResultFromResponse(
            GraphQLResponse response,
            string key
        )
        {
            var data = response.data;
            if (
                data.TryGetValue(key, out var resultObj)
                && resultObj is Dictionary<string, object> result
            )
            {
                if (!result.ContainsKey("sessionId"))
                {
                    throw new UnexpectedException();
                }
                var sessionId = result["sessionId"].ToString();
                return new ResultImpl<string>(sessionId);
            }
            else
            {
                throw new UnexpectedException();
            }
        }
    }
}
