#pragma warning disable 1591

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Braintree
{
    public interface IDiscountGateway
    {
        List<Discount> All();
        Task<List<Discount>> AllAsync();
    }
}
