using System;
using System.Collections.Generic;

namespace Braintree
{
    public class PlanGateway
    {
        private BraintreeService service;

        public PlanGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            service = new BraintreeService(gateway.Configuration);
        }

        public virtual List<Plan> All()
        {
            var response = new NodeWrapper(service.Get(service.MerchantPath() + "/plans"));

            var plans = new List<Plan>();
            foreach (var node in response.GetList("plan"))
            {
                plans.Add(new Plan(node));
            }
            return plans;
        }
    }
}

