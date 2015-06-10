using System;
using System.Collections.Generic;
using System.Xml;

namespace Braintree
{
    public class DiscountGateway
    {
        private BraintreeService Service;

        public DiscountGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            Service = new BraintreeService(gateway.Configuration);
        }

        public virtual List<Discount> All()
        {
            var response = new NodeWrapper(Service.Get(Service.MerchantPath() + "/discounts"));

            var discounts = new List<Discount>();
            foreach (var node in response.GetList("discount"))
            {
                discounts.Add(new Discount(node));
            }
            return discounts;
        }
    }
}

