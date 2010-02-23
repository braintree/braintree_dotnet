using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Braintree
{
    public class AddressGateway
    {
        public Result<Address> Create(String customerID, AddressRequest request)
        {
            XmlNode addressXML = WebServiceGateway.Post("/customers/" + customerID + "/addresses", request);

            return new Result<Address>(new NodeWrapper(addressXML));
        }

        public void Delete(String customerID, String id)
        {
            WebServiceGateway.Delete("/customers/" + customerID + "/addresses/" + id);
        }

        public Result<Address> Find(String customerID, String id)
        {
            XmlNode addressXML = WebServiceGateway.Get("/customers/" + customerID + "/addresses/" + id);

            return new Result<Address>(new NodeWrapper(addressXML));
        }

        public Result<Address> Update(String customerID, String id, AddressRequest request)
        {
            XmlNode addressXML = WebServiceGateway.Put("/customers/" + customerID + "/addresses/" + id, request);

            return new Result<Address>(new NodeWrapper(addressXML));
        }
    }
}
