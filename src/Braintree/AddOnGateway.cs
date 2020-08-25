using System.Collections.Generic;
using System.Threading.Tasks;

namespace Braintree
{
    public class AddOnGateway : IAddOnGateway
    {
        private readonly BraintreeService Service;

        public AddOnGateway(IBraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            Service = gateway.Service;
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

        public virtual async Task<List<AddOn>> AllAsync()
        {
            var response = new NodeWrapper(await Service.GetAsync(Service.MerchantPath() + "/add_ons").ConfigureAwait(false));

            var addOns = new List<AddOn>();
            foreach (var node in response.GetList("add-on"))
            {
                addOns.Add(new AddOn(node));
            }
            return addOns;
        }
    }
}

