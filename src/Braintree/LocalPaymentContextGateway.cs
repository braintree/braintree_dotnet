#pragma warning disable 1591

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Braintree.Exceptions;
using Braintree.GraphQL;

namespace Braintree
{
    /// <summary>
    /// Creates and manages local payment contexts.
    /// </summary>
    public class LocalPaymentContextGateway : ILocalPaymentContextGateway
    {
        private readonly BraintreeService service;
        private readonly IGraphQLClient graphQLClient;

        private const string CREATE_LOCAL_PAYMENT_CONTEXT = @"
            mutation CreateLocalPaymentContext($input: CreateLocalPaymentContextInput!) {
                createLocalPaymentContext(input: $input) {
                    paymentContext {
                        id
                        legacyId
                        type
                        paymentId
                        approvalUrl
                        merchantAccountId
                        orderId
                        createdAt
                        transactedAt
                        approvedAt
                        amount {
                            value
                            currencyCode
                        }
                    }
                }
            }";

        private const string FIND_LOCAL_PAYMENT_CONTEXT = @"
            query Node($id: ID!) {
                node(id: $id) {
                    ... on LocalPaymentContext {
                        id
                        legacyId
                        type
                        amount {
                            value
                            currencyIsoCode
                        }
                        approvalUrl
                        merchantAccountId
                        transactedAt
                        approvedAt
                        createdAt
                        updatedAt
                        expiredAt
                        paymentId
                        orderId
                    }
                }
            }";

        public LocalPaymentContextGateway(BraintreeService service, IGraphQLClient graphQLClient)
        {
            this.service = service;
            this.graphQLClient = graphQLClient;
        }

        public virtual Result<LocalPaymentContext> Create(CreateLocalPaymentContextInput input)
        {
            try
            {
                var variables = new Dictionary<string, object>();
                variables["input"] = input.ToGraphQLVariables();

                var response = graphQLClient.Query(CREATE_LOCAL_PAYMENT_CONTEXT, variables);

                if (response.errors != null)
                {
                    return new ResultImpl<LocalPaymentContext>(response.GetValidationErrors());
                }

                var data = response.data;
                var result = data["createLocalPaymentContext"] as Dictionary<string, object>;
                var paymentContextData = result["paymentContext"] as Dictionary<string, object>;

                var paymentContext = new LocalPaymentContext(paymentContextData);
                return new ResultImpl<LocalPaymentContext>(paymentContext);
            }
            catch (Exception e)
            {
                var errors = new ValidationErrors();
                errors.AddError("base", new ValidationError("base", "0", "Unexpected error: " + e.Message));
                return new ResultImpl<LocalPaymentContext>(errors);
            }
        }

        public virtual async Task<Result<LocalPaymentContext>> CreateAsync(CreateLocalPaymentContextInput input)
        {
            try
            {
                var variables = new Dictionary<string, object>();
                variables["input"] = input.ToGraphQLVariables();

                var response = await graphQLClient.QueryAsync(CREATE_LOCAL_PAYMENT_CONTEXT, variables).ConfigureAwait(false);
                
                if (response.errors != null)
                {
                    return new ResultImpl<LocalPaymentContext>(response.GetValidationErrors());
                }

                var data = response.data;
                var result = data["createLocalPaymentContext"] as Dictionary<string, object>;
                var paymentContextData = result["paymentContext"] as Dictionary<string, object>;

                var paymentContext = new LocalPaymentContext(paymentContextData);
                return new ResultImpl<LocalPaymentContext>(paymentContext);
            }
            catch (Exception e)
            {
                var errors = new ValidationErrors();
                errors.AddError("base", new ValidationError("base", "0", "Unexpected error: " + e.Message));
                return new ResultImpl<LocalPaymentContext>(errors);
            }
        }

        public virtual Result<LocalPaymentContext> Find(string id)
        {
            try
            {
                var variables = new Dictionary<string, object>();
                variables["id"] = id;

                var response = graphQLClient.Query(FIND_LOCAL_PAYMENT_CONTEXT, variables);
                
                if (response.errors != null)
                {
                    return new ResultImpl<LocalPaymentContext>(response.GetValidationErrors());
                }

                var data = response.data;
                var nodeData = data["node"] as Dictionary<string, object>;

                if (nodeData == null)
                {
                    throw new NotFoundException("Local payment context not found");
                }

                var paymentContext = new LocalPaymentContext(nodeData);
                return new ResultImpl<LocalPaymentContext>(paymentContext);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception e)
            {
                var errors = new ValidationErrors();
                errors.AddError("base", new ValidationError("base", "0", "Unexpected error: " + e.Message));
                return new ResultImpl<LocalPaymentContext>(errors);
            }
        }

        public virtual async Task<Result<LocalPaymentContext>> FindAsync(string id)
        {
            try
            {
                var variables = new Dictionary<string, object>();
                variables["id"] = id;

                var response = await graphQLClient.QueryAsync(FIND_LOCAL_PAYMENT_CONTEXT, variables).ConfigureAwait(false);
                
                if (response.errors != null)
                {
                    return new ResultImpl<LocalPaymentContext>(response.GetValidationErrors());
                }

                var data = response.data;
                var nodeData = data["node"] as Dictionary<string, object>;

                if (nodeData == null)
                {
                    throw new NotFoundException("Local payment context not found");
                }

                var paymentContext = new LocalPaymentContext(nodeData);
                return new ResultImpl<LocalPaymentContext>(paymentContext);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception e)
            {
                var errors = new ValidationErrors();
                errors.AddError("base", new ValidationError("base", "0", "Unexpected error: " + e.Message));
                return new ResultImpl<LocalPaymentContext>(errors);
            }
        }
    }
}
