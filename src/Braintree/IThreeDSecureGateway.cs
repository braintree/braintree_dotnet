#pragma warning disable 1591

namespace Braintree
{
    public interface IThreeDSecureGateway
    {
        ThreeDSecureLookupResponse Lookup(ThreeDSecureLookupRequest request);
    }
}
