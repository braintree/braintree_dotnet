using System.Xml;

namespace Braintree
{
    public class MerchantGateway : IMerchantGateway
    {
        private readonly BraintreeService service;
        private readonly BraintreeGateway gateway;

        public MerchantGateway(BraintreeGateway gateway)
        {
            this.gateway = gateway;
            service = gateway.Service;
        }

        public ResultImpl<Merchant> Create(MerchantRequest request)
        {
            XmlNode merchantXML = service.Post("/merchants/create_via_api", request);
            return new ResultImpl<Merchant>(new NodeWrapper(merchantXML), gateway);
        }
    }
}
