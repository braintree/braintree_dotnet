using System.Collections.Generic;

namespace Braintree
{
    public class DiscountGateway : IDiscountGateway
    {
        private readonly BraintreeService service;

        public DiscountGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            service = new BraintreeService(gateway.Configuration);
        }

        public virtual List<Discount> All()
        {
            var response = new NodeWrapper(service.Get(service.MerchantPath() + "/discounts"));

            var discounts = new List<Discount>();
            foreach (var node in response.GetList("discount"))
            {
                discounts.Add(new Discount(node));
            }
            return discounts;
        }
    }
}

