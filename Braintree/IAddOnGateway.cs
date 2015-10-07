using System.Collections.Generic;

namespace Braintree
{
    public interface IAddOnGateway
    {
        List<AddOn> All();
    }
}