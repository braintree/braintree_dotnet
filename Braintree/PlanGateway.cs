using System;
using System.Collections.Generic;

namespace Braintree
{
    public class PlanGateway
    {
        private BraintreeService Service;

        public PlanGateway(BraintreeService service)
        {
            Service = service;
        }

        public virtual List<Plan> All()
        {
            NodeWrapper response = new NodeWrapper(Service.Get("/plans"));

            List<Plan> plans = new List<Plan>();
            foreach (NodeWrapper node in response.GetList("plan"))
            {
                plans.Add(new Plan(node));
            }
            return plans;
        }
    }
}

