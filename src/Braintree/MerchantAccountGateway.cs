using Braintree.Exceptions;
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
            service = new BraintreeService(gateway.Configuration);
        }

        public virtual Result<MerchantAccount> Create(MerchantAccountRequest request)
        {
            XmlNode merchantAccountXML = service.Post(service.MerchantPath() + "/merchant_accounts/create_via_api", request);

            return new ResultImpl<MerchantAccount>(new NodeWrapper(merchantAccountXML), gateway);
        }

        public virtual Result<MerchantAccount> CreateForCurrency(MerchantAccountCreateForCurrencyRequest request)
        {
            XmlNode merchantAccountXML = service.Post(service.MerchantPath() + "/merchant_accounts/create_for_currency", request);

            return new ResultImpl<MerchantAccount>(new NodeWrapper(merchantAccountXML), gateway);
        }

        public virtual Result<MerchantAccount> Update(string id, MerchantAccountRequest request)
        {
            XmlNode merchantAccountXML = service.Put(service.MerchantPath() + "/merchant_accounts/" + id + "/update_via_api", request);

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
    }
}
