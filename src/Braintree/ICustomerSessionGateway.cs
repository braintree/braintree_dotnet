using Braintree;
using Braintree.GraphQL;
namespace Braintree
{
    public interface ICustomerSessionGateway
    {
        Result<string> CreateCustomerSession(CreateCustomerSessionInput input);
        Result<string> UpdateCustomerSession(UpdateCustomerSessionInput input);
        Result<CustomerRecommendationsPayload> GetCustomerRecommendations(CustomerRecommendationsInput input);
    }
}