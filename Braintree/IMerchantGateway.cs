using System;

namespace Braintree
{
    public interface IMerchantGateway
    {
        ResultImpl<Merchant> Create(MerchantRequest request);
    }
}