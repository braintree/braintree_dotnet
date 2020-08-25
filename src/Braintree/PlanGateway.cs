using System.Collections.Generic;
using System.Threading.Tasks;

namespace Braintree
{
    public class PlanGateway : IPlanGateway
    {
        private readonly BraintreeService service;

        public PlanGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            service = gateway.Service;
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

        public virtual async Task<List<Plan>> AllAsync()
        {
            var response = new NodeWrapper(await service.GetAsync(service.MerchantPath() + "/plans").ConfigureAwait(false));

            var plans = new List<Plan>();
            foreach (var node in response.GetList("plan"))
            {
                plans.Add(new Plan(node));
            }
            return plans;
        }
    }
}

