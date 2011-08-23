using System;
using System.Collections.Generic;
using System.Xml;

namespace Braintree
{
    public class DiscountGateway
    {
        private BraintreeService Service;

        public DiscountGateway(BraintreeService service)
        {
            Service = service;
        }

        public virtual List<Discount> All()
        {
            NodeWrapper response = new NodeWrapper(Service.Get("/discounts"));

            List<Discount> discounts = new List<Discount>();
            foreach (NodeWrapper node in response.GetList("modification"))
            {
                discounts.Add(new Discount(node));
            }
            return discounts;
        }
    }
}

