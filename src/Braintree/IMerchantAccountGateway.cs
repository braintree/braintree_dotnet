#pragma warning disable 1591

using System.Threading.Tasks;

namespace Braintree
{
    public interface IMerchantAccountGateway
    {
        Result<MerchantAccount> CreateForCurrency(MerchantAccountCreateForCurrencyRequest request);
        Task<Result<MerchantAccount>> CreateForCurrencyAsync(MerchantAccountCreateForCurrencyRequest request);
        MerchantAccount Find(string id);
        Task<MerchantAccount> FindAsync(string id);
        PaginatedCollection<MerchantAccount> All();
    }
}
