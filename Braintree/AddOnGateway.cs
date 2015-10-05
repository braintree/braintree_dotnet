using System;
using System.Collections.Generic;
using System.Xml;

namespace Braintree
{
    public class AddOnGateway : IAddOnGateway
    {
        private readonly BraintreeService Service;

        public AddOnGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            Service = new BraintreeService(gateway.Configuration);
        }

        public virtual List<AddOn> All()
        {
            var response = new NodeWrapper(Service.Get(Service.MerchantPath() + "/add_ons"));

            var addOns = new List<AddOn>();
            foreach (var node in response.GetList("add-on"))
            {
                addOns.Add(new AddOn(node));
            }
            return addOns;
        }
    }
}

