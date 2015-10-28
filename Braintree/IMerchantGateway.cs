#pragma warning diable 1591

using System;

namespace Braintree
{
    public interface IMerchantGateway
    {
        ResultImpl<Merchant> Create(MerchantRequest request);
    }
}
