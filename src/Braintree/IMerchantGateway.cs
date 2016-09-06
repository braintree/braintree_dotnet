#pragma warning disable 1591

namespace Braintree
{
    public interface IMerchantGateway
    {
        ResultImpl<Merchant> Create(MerchantRequest request);
    }
}
