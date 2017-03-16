#pragma warning disable 1591

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Braintree
{
    public interface IPlanGateway
    {
        List<Plan> All();
        Task<List<Plan>> AllAsync();
    }
}
