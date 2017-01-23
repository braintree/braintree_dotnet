#pragma warning disable 1591

namespace Braintree
{
    public interface IMerchantAccountGateway
    {
        Result<MerchantAccount> Create(MerchantAccountRequest request);
        Result<MerchantAccount> CreateForCurrency(MerchantAccountCreateForCurrencyRequest request);
        MerchantAccount Find(string id);
        Result<MerchantAccount> Update(string id, MerchantAccountRequest request);
        PaginatedCollection<MerchantAccount> All();
    }
}
