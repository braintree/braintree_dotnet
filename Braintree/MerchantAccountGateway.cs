using System;
using System.Collections.Generic;
using System.Xml;
using Braintree.Exceptions;

namespace Braintree
{
    public class MerchantAccountGateway
    {
        private static readonly String CREATE_URL = "/merchant_accounts/create_via_api";
        private BraintreeService Service;

        protected internal MerchantAccountGateway(BraintreeService service)
        {
            Service = service;
        }

        public virtual Result<MerchantAccount> Create(MerchantAccountRequest request)
        {
            XmlNode merchantAccountXML = Service.Post(CREATE_URL, request);

            return new Result<MerchantAccount>(new NodeWrapper(merchantAccountXML), Service);
        }

        public virtual Result<MerchantAccount> Update(String id, MerchantAccountRequest request)
        {
            XmlNode merchantAccountXML = Service.Put("/merchant_accounts/" + id + "/update_via_api", request);

            return new Result<MerchantAccount>(new NodeWrapper(merchantAccountXML), Service);
        }

        public virtual MerchantAccount Find(String id)
        {
            if(id == null || id.Trim().Equals(""))
                throw new NotFoundException();

            XmlNode merchantAccountXML = Service.Get("/merchant_accounts/" + id);

            return new MerchantAccount(new NodeWrapper(merchantAccountXML));
        }
    }
}
