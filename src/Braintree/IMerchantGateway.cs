#pragma warning disable 1591

using System;

namespace Braintree
{
    // NEXT_MAJOR_VERSION remove this interface
    [Obsolete("IMerchantGateway has been deprecated and will be removed in a future version.")]
    public interface IMerchantGateway
    {
        // NEXT_MAJOR_VERSION remove this method
        // The merchant create endpoint has been disabled
        [Obsolete("Merchant.Create is deprecated and will be removed in a future version.")]
        ResultImpl<Merchant> Create(MerchantRequest request);
    }
}
