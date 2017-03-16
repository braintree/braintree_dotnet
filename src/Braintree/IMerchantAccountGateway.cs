#pragma warning disable 1591

using System.Threading.Tasks;

namespace Braintree
{
    public interface IMerchantAccountGateway
    {
        Result<MerchantAccount> Create(MerchantAccountRequest request);
        Task<Result<MerchantAccount>> CreateAsync(MerchantAccountRequest request);
        Result<MerchantAccount> CreateForCurrency(MerchantAccountCreateForCurrencyRequest request);
        Task<Result<MerchantAccount>> CreateForCurrencyAsync(MerchantAccountCreateForCurrencyRequest request);
        MerchantAccount Find(string id);
        Task<MerchantAccount> FindAsync(string id);
        Result<MerchantAccount> Update(string id, MerchantAccountRequest request);
        Task<Result<MerchantAccount>> UpdateAsync(string id, MerchantAccountRequest request);
        PaginatedCollection<MerchantAccount> All();
    }
}
