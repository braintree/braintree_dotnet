using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using Braintree.Exceptions;

namespace Braintree
{
    public class PlanGateway : IPlanGateway
    {
        private readonly BraintreeService service;
        private readonly BraintreeGateway gateway;

        public PlanGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            this.gateway = gateway;
            service = gateway.Service;
        }

        public virtual List<Plan> All()
        {
            var response = new NodeWrapper(service.Get(service.MerchantPath() + "/plans"));

            var plans = new List<Plan>();
            foreach (var node in response.GetList("plan"))
            {
                plans.Add(new Plan(node, gateway));
            }
            return plans;
        }

        public virtual async Task<List<Plan>> AllAsync()
        {
            var response = new NodeWrapper(await service.GetAsync(service.MerchantPath() + "/plans").ConfigureAwait(false));

            var plans = new List<Plan>();
            foreach (var node in response.GetList("plan"))
            {
                plans.Add(new Plan(node, gateway));
            }
            return plans;
        }

        public virtual Result<Plan> Create(PlanRequest request)
        {
            XmlNode planXML = service.Post(service.MerchantPath() + "/plans", request);

            return new ResultImpl<Plan>(new NodeWrapper(planXML), gateway);
        }

        public virtual async Task<Result<Plan>> CreateAsync(PlanRequest request)
        {
            XmlNode planXML = await service.PostAsync(service.MerchantPath() + "/plans", request).ConfigureAwait(false);

            return new ResultImpl<Plan>(new NodeWrapper(planXML), gateway);
        }

        public virtual Plan Find(string id)
        {
            if(id == null || id.Trim().Equals(""))
                throw new NotFoundException();

            XmlNode planXML = service.Get(service.MerchantPath() + "/plans/" + id);

            return new Plan(new NodeWrapper(planXML), gateway);
        }

        public virtual async Task<Plan> FindAsync(string id)
        {
            if(id == null || id.Trim().Equals(""))
                throw new NotFoundException();

            XmlNode planXML = await service.GetAsync(service.MerchantPath() + "/plans/" + id).ConfigureAwait(false);

            return new Plan(new NodeWrapper(planXML), gateway);
        }

        public virtual Result<Plan> Update(string id, PlanRequest request)
        {
            XmlNode planXML = service.Put(service.MerchantPath() + "/plans/" + id, request);

            return new ResultImpl<Plan>(new NodeWrapper(planXML), gateway);
        }

        public virtual async Task<Result<Plan>> UpdateAsync(string id, PlanRequest request)
        {
            XmlNode planXML = await service.PutAsync(service.MerchantPath() + "/plans/" + id, request).ConfigureAwait(false);

            return new ResultImpl<Plan>(new NodeWrapper(planXML), gateway);
        }
    }
}

