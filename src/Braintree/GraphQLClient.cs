using System.Collections.Generic;
using System.Threading.Tasks;

namespace Braintree
{
    public class GraphQLClient
    {
        private readonly BraintreeGraphQLService service;

        public GraphQLClient(Configuration configuration) {
            service = new BraintreeGraphQLService(configuration);
        }

        public GraphQLResponse Query(string definition)
        {
            return Query(definition, null);
        }

        public GraphQLResponse Query(string definition, Dictionary<string, object> variables)
        {
            return service.QueryGraphQL(definition, variables);
        }

        public async Task<GraphQLResponse> QueryAsync(string definition)
        {
            return await QueryAsync(definition, null).ConfigureAwait(false);
        }

        public async Task<GraphQLResponse> QueryAsync(string definition, Dictionary<string, object> variables)
        {
            return await service.QueryGraphQLAsync(definition, variables).ConfigureAwait(false);
        }
    }
}
