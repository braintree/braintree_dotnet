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
    public class AddressGateway
    {
        private BraintreeService Service;

        protected internal AddressGateway(BraintreeService service)
        {
            Service = service;
        }

        public virtual Result<Address> Create(String customerId, AddressRequest request)
        {
            XmlNode addressXML = Service.Post("/customers/" + customerId + "/addresses", request);

            return new Result<Address>(new NodeWrapper(addressXML), Service);
        }

        public virtual void Delete(String customerId, String id)
        {
            Service.Delete("/customers/" + customerId + "/addresses/" + id);
        }

        public virtual Address Find(String customerId, String id)
        {
            if(customerId == null || customerId.Trim().Equals("") || id == null || id.Trim().Equals(""))
                throw new NotFoundException();

            XmlNode addressXML = Service.Get("/customers/" + customerId + "/addresses/" + id);

            return new Address(new NodeWrapper(addressXML));
        }

        public virtual Result<Address> Update(String customerId, String id, AddressRequest request)
        {
            XmlNode addressXML = Service.Put("/customers/" + customerId + "/addresses/" + id, request);

            return new Result<Address>(new NodeWrapper(addressXML), Service);
        }
    }
}
