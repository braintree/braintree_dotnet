#pragma warning disable 1591

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Braintree
{
    public interface IAddOnGateway
    {
        List<AddOn> All();
        Task<List<AddOn>> AllAsync();
    }
}
