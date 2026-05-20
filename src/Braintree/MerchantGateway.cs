using System;
using System.Xml;

namespace Braintree
{
    // NEXT_MAJOR_VERSION remove this class
    [Obsolete("MerchantGateway has been deprecated and will be removed in a future version.")]
    public class MerchantGateway : IMerchantGateway
    {
        private readonly BraintreeService service;
        private readonly BraintreeGateway gateway;

        public MerchantGateway(BraintreeGateway gateway)
        {
            this.gateway = gateway;
            service = gateway.Service;
        }

        // NEXT_MAJOR_VERSION remove this method
        // The merchant create endpoint has been disabled
        [Obsolete("Merchant.Create is deprecated and will be removed in a future version.")]
        public ResultImpl<Merchant> Create(MerchantRequest request)
        {
            XmlNode merchantXML = service.Post("/merchants/create_via_api", request);
            return new ResultImpl<Merchant>(new NodeWrapper(merchantXML), gateway);
        }
    }
}
