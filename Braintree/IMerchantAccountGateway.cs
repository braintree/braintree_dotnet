using System;

namespace Braintree
{
    public interface IMerchantAccountGateway
    {
        Result<MerchantAccount> Create(MerchantAccountRequest request);
        MerchantAccount Find(string id);
        Result<MerchantAccount> Update(string id, MerchantAccountRequest request);
    }
}