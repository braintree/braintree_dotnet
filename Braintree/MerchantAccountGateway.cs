using System;
using System.Collections.Generic;
using System.Xml;

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
    }
}
