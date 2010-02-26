using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Braintree
{
    /// <summary>
    /// Provides operations for creating, finding, updating, and deleting addresses in the vault
    /// </summary>
    public class AddressGateway
    {
        public virtual Result<Address> Create(String customerId, AddressRequest request)
        {
            XmlNode addressXML = WebServiceGateway.Post("/customers/" + customerId + "/addresses", request);

            return new Result<Address>(new NodeWrapper(addressXML));
        }

        public virtual void Delete(String customerId, String id)
        {
            WebServiceGateway.Delete("/customers/" + customerId + "/addresses/" + id);
        }

        public virtual Address Find(String customerId, String id)
        {
            XmlNode addressXML = WebServiceGateway.Get("/customers/" + customerId + "/addresses/" + id);

            return new Address(new NodeWrapper(addressXML));
        }

        public virtual Result<Address> Update(String customerId, String id, AddressRequest request)
        {
            XmlNode addressXML = WebServiceGateway.Put("/customers/" + customerId + "/addresses/" + id, request);

            return new Result<Address>(new NodeWrapper(addressXML));
        }
    }
}
