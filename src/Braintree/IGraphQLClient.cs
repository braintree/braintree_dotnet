using System.Collections.Generic;
using System.Threading.Tasks;

namespace Braintree
{
    public interface IGraphQLClient
    {
        GraphQLResponse Query(string definition);
        GraphQLResponse Query(string definition, Dictionary<string, object> variables);
        Task<GraphQLResponse> QueryAsync(string definition);
        Task<GraphQLResponse> QueryAsync(string definition, Dictionary<string, object> variables);
    }
}
