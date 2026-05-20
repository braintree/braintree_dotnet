#pragma warning disable 1591

using System.Threading.Tasks;
using Braintree.GraphQL;

namespace Braintree
{
    public interface ILocalPaymentContextGateway
    {
        Result<LocalPaymentContext> Create(CreateLocalPaymentContextInput input);
        Task<Result<LocalPaymentContext>> CreateAsync(CreateLocalPaymentContextInput input);
        Result<LocalPaymentContext> Find(string id);
        Task<Result<LocalPaymentContext>> FindAsync(string id);
    }
}
