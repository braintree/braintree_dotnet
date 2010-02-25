using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Braintree
{
    public class CustomerGateway
    {
        public Customer Find(String Id)
        {
            XmlNode customerXML = WebServiceGateway.Get("/customers/" + Id);

            return new Customer(new NodeWrapper(customerXML));
        }

        public Result<Customer> Create(CustomerRequest request)
        {
            XmlNode customerXML = WebServiceGateway.Post("/customers", request);

            return new Result<Customer>(new NodeWrapper(customerXML));
        }

        public void Delete(String Id)
        {
            WebServiceGateway.Delete("/customers/" + Id);
        }

        public Result<Customer> Update(String Id, CustomerRequest request)
        {
            XmlNode customerXML = WebServiceGateway.Put("/customers/" + Id, request);

            return new Result<Customer>(new NodeWrapper(customerXML));
        }

        public Result<Customer> ConfirmTransparentRedirect(String queryString)
        {
            TransparentRedirectRequest trRequest = new TransparentRedirectRequest(queryString);
            XmlNode node = WebServiceGateway.Post("/customers/all/confirm_transparent_redirect_request", trRequest);

            return new Result<Customer>(new NodeWrapper(node));
        }

        public String TransparentRedirectURLForCreate()
        {
            return Configuration.BaseMerchantURL() + "/customers/all/create_via_transparent_redirect_request";
        }

        public String TransparentRedirectURLForUpdate()
        {
            return Configuration.BaseMerchantURL() + "/customers/all/update_via_transparent_redirect_request";
        }
    }
}
