#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Xml;
using Braintree.Exceptions;

namespace Braintree
{
    /// <summary>
    /// Provides operations for creating, finding, updating, and deleting customers in the vault
    /// </summary>
    public class CustomerGateway
    {
        private BraintreeService Service;
        private BraintreeGateway Gateway;

        protected internal CustomerGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            Gateway = gateway;
            Service = new BraintreeService(gateway.Configuration);
        }

        public virtual Customer Find(String Id)
        {
            if(Id == null || Id.Trim().Equals(""))
                throw new NotFoundException();

            XmlNode customerXML = Service.Get(Service.MerchantPath() + "/customers/" + Id);

            return new Customer(new NodeWrapper(customerXML), Gateway);
        }

        public virtual Result<Customer> Create()
        {
            return Create(new CustomerRequest());
        }

        public virtual Result<Customer> Create(CustomerRequest request)
        {
            XmlNode customerXML = Service.Post(Service.MerchantPath() + "/customers", request);

            return new ResultImpl<Customer>(new NodeWrapper(customerXML), Gateway);
        }

        public virtual void Delete(String Id)
        {
            Service.Delete(Service.MerchantPath() + "/customers/" + Id);
        }

        public virtual Result<Customer> Update(String Id, CustomerRequest request)
        {
            XmlNode customerXML = Service.Put(Service.MerchantPath() + "/customers/" + Id, request);

            return new ResultImpl<Customer>(new NodeWrapper(customerXML), Gateway);
        }

        [Obsolete("Use gateway.TransparentRedirect.Confirm()")]
        public virtual Result<Customer> ConfirmTransparentRedirect(String queryString)
        {
            TransparentRedirectRequest trRequest = new TransparentRedirectRequest(queryString, Service);
            XmlNode node = Service.Post(Service.MerchantPath() + "/customers/all/confirm_transparent_redirect_request", trRequest);

            return new ResultImpl<Customer>(new NodeWrapper(node), Gateway);
        }

        [Obsolete("Use gateway.TransparentRedirect.Url")]
        public virtual String TransparentRedirectURLForCreate()
        {
            return Service.BaseMerchantURL() + "/customers/all/create_via_transparent_redirect_request";
        }

        [Obsolete("Use gateway.TransparentRedirect.Url")]
        public virtual String TransparentRedirectURLForUpdate()
        {
            return Service.BaseMerchantURL() + "/customers/all/update_via_transparent_redirect_request";
        }

        public virtual ResourceCollection<Customer> All()
        {
            NodeWrapper response = new NodeWrapper(Service.Post(Service.MerchantPath() + "/customers/advanced_search_ids"));
            CustomerSearchRequest query = new CustomerSearchRequest();

            return new ResourceCollection<Customer>(response, delegate(String[] ids) {
                return FetchCustomers(query, ids);
            });
        }

        public virtual ResourceCollection<Customer> Search(CustomerSearchRequest query)
        {
            NodeWrapper response = new NodeWrapper(Service.Post(Service.MerchantPath() + "/customers/advanced_search_ids", query));

            return new ResourceCollection<Customer>(response, delegate(String[] ids) {
                return FetchCustomers(query, ids);
            });
        }

        private List<Customer> FetchCustomers(CustomerSearchRequest query, String[] ids)
        {
            query.Ids.IncludedIn(ids);

            NodeWrapper response = new NodeWrapper(Service.Post(Service.MerchantPath() + "/customers/advanced_search", query));

            List<Customer> customers = new List<Customer>();
            foreach (NodeWrapper node in response.GetList("customer"))
            {
                customers.Add(new Customer(node, Gateway));
            }
            return customers;
        }
    }
}
