#pragma warning disable 1591

using System.Collections.Generic;

namespace Braintree
{
    public interface IAddOnGateway
    {
        List<AddOn> All();
    }
}
