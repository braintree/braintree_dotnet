using System;
using System.Xml;
using System.Security.Cryptography;
using System.Text;

namespace Braintree
{
    public class MerchantGateway : IMerchantGateway
    {
        private readonly BraintreeService service;
        private readonly BraintreeGateway gateway;

        public MerchantGateway(BraintreeGateway gateway)
        {
            this.gateway = gateway;
            service = new BraintreeService(gateway.Configuration);
        }

        public ResultImpl<Merchant> Create(MerchantRequest request)
        {
            XmlNode merchantXML = service.Post("/merchants/create_via_api", request);
            return new ResultImpl<Merchant>(new NodeWrapper(merchantXML), gateway);
        }
    }
}
