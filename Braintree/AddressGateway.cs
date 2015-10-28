#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Braintree.Exceptions;

namespace Braintree
{
    /// <summary>
    /// Provides operations for creating, finding, updating, and deleting addresses in the vault
    /// </summary>
    public class AddressGateway : IAddressGateway
    {
        private readonly BraintreeService Service;
        private readonly BraintreeGateway Gateway;

        protected internal AddressGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            Gateway = gateway;
            Service = new BraintreeService(gateway.Configuration);
        }

        public virtual Result<Address> Create(string customerId, AddressRequest request)
        {
            XmlNode addressXML = Service.Post(Service.MerchantPath() + "/customers/" + customerId + "/addresses", request);

            return new ResultImpl<Address>(new NodeWrapper(addressXML), Gateway);
        }

        public virtual Result<Address> Delete(string customerId, string id)
        {
            XmlNode addressXML = Service.Delete(Service.MerchantPath() + "/customers/" + customerId + "/addresses/" + id);

            return new ResultImpl<Address>(new NodeWrapper(addressXML), Gateway);
        }

        public virtual Address Find(string customerId, string id)
        {
            if(customerId == null || customerId.Trim().Equals("") || id == null || id.Trim().Equals(""))
                throw new NotFoundException();

            XmlNode addressXML = Service.Get(Service.MerchantPath() + "/customers/" + customerId + "/addresses/" + id);

            return new Address(new NodeWrapper(addressXML));
        }

        public virtual Result<Address> Update(string customerId, string id, AddressRequest request)
        {
            XmlNode addressXML = Service.Put(Service.MerchantPath() + "/customers/" + customerId + "/addresses/" + id, request);

            return new ResultImpl<Address>(new NodeWrapper(addressXML), Gateway);
        }
    }
}
