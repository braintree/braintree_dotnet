using System;
using System.Collections.Generic;

namespace Braintree
{
    public interface IDiscountGateway
    {
        List<Discount> All();
    }
}