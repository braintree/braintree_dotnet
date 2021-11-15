#pragma warning disable 1591

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Braintree
{
    public interface IPlanGateway
    {
        List<Plan> All();
        Task<List<Plan>> AllAsync();
        Result<Plan> Create(PlanRequest request);
        Task<Result<Plan>> CreateAsync(PlanRequest request);
        Plan Find(string id);
        Task<Plan> FindAsync(string id);
        Result<Plan> Update(string id, PlanRequest request);
        Task<Result<Plan>> UpdateAsync(string id, PlanRequest request);
    }
}
