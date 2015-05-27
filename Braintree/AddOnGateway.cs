using System;
using System.Collections.Generic;
using System.Xml;

namespace Braintree
{
    public class AddOnGateway
    {
        private BraintreeService Service;

        public AddOnGateway(BraintreeService service)
        {
            Service = service;
        }

        public virtual List<AddOn> All()
        {
            NodeWrapper response = new NodeWrapper(Service.Get(Service.MerchantPath() + "/add_ons"));

            List<AddOn> addOns = new List<AddOn>();
            foreach (NodeWrapper node in response.GetList("add-on"))
            {
                addOns.Add(new AddOn(node));
            }
            return addOns;
        }
    }
}

