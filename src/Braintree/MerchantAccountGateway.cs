using Braintree.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace Braintree
{   
    public class MerchantAccountGateway : IMerchantAccountGateway
    {
        private readonly BraintreeService service;
        private readonly BraintreeGateway gateway;

        protected internal MerchantAccountGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            this.gateway = gateway;
            service = gateway.Service;
        }

        public virtual Result<MerchantAccount> CreateForCurrency(MerchantAccountCreateForCurrencyRequest request)
        {
            XmlNode merchantAccountXML = service.Post(service.MerchantPath() + "/merchant_accounts/create_for_currency", request);

            return new ResultImpl<MerchantAccount>(new NodeWrapper(merchantAccountXML), gateway);
        }

        public virtual async Task<Result<MerchantAccount>> CreateForCurrencyAsync(MerchantAccountCreateForCurrencyRequest request)
        {
            XmlNode merchantAccountXML = await service.PostAsync(service.MerchantPath() + "/merchant_accounts/create_for_currency", request).ConfigureAwait(false);

            return new ResultImpl<MerchantAccount>(new NodeWrapper(merchantAccountXML), gateway);
        }

        public virtual MerchantAccount Find(string id)
        {
            if (id == null || id.Trim().Equals(""))
            {
                throw new NotFoundException();
            }

            XmlNode merchantAccountXML = service.Get(service.MerchantPath() + "/merchant_accounts/" + id);

            return new MerchantAccount(new NodeWrapper(merchantAccountXML));
        }

        public virtual async Task<MerchantAccount> FindAsync(string id)
        {
            if (id == null || id.Trim().Equals(""))
            {
                throw new NotFoundException();
            }

            XmlNode merchantAccountXML = await service.GetAsync(service.MerchantPath() + "/merchant_accounts/" + id).ConfigureAwait(false);

            return new MerchantAccount(new NodeWrapper(merchantAccountXML));
        }

        public virtual PaginatedCollection<MerchantAccount> All()
        {
            return new PaginatedCollection<MerchantAccount>(FetchMerchantAccounts);
        }

        private PaginatedResult<MerchantAccount> FetchMerchantAccounts(int page)
        {
            XmlNode merchantAccountXML = service.Get(service.MerchantPath() + "/merchant_accounts?page=" + page);
            var nodeWrapper = new NodeWrapper(merchantAccountXML);

            var totalItems = nodeWrapper.GetInteger("total-items").Value;
            var pageSize = nodeWrapper.GetInteger("page-size").Value;
            var merchantAccounts = new List<MerchantAccount>();
            foreach (var node in nodeWrapper.GetList("merchant-account"))
            {
                merchantAccounts.Add(new MerchantAccount(node));
            }

            return new PaginatedResult<MerchantAccount>(totalItems, pageSize, merchantAccounts);
        }
    }
}
