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

        public virtual Customer Find(string Id)
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

        public virtual void Delete(string Id)
        {
            Service.Delete(Service.MerchantPath() + "/customers/" + Id);
        }

        public virtual Result<Customer> Update(string Id, CustomerRequest request)
        {
            XmlNode customerXML = Service.Put(Service.MerchantPath() + "/customers/" + Id, request);

            return new ResultImpl<Customer>(new NodeWrapper(customerXML), Gateway);
        }

        [Obsolete("Use gateway.TransparentRedirect.Confirm()")]
        public virtual Result<Customer> ConfirmTransparentRedirect(string queryString)
        {
            var trRequest = new TransparentRedirectRequest(queryString, Service);
            XmlNode node = Service.Post(Service.MerchantPath() + "/customers/all/confirm_transparent_redirect_request", trRequest);

            return new ResultImpl<Customer>(new NodeWrapper(node), Gateway);
        }

        [Obsolete("Use gateway.TransparentRedirect.Url")]
        public virtual string TransparentRedirectURLForCreate()
        {
            return Service.BaseMerchantURL() + "/customers/all/create_via_transparent_redirect_request";
        }

        [Obsolete("Use gateway.TransparentRedirect.Url")]
        public virtual string TransparentRedirectURLForUpdate()
        {
            return Service.BaseMerchantURL() + "/customers/all/update_via_transparent_redirect_request";
        }

        public virtual ResourceCollection<Customer> All()
        {
            var response = new NodeWrapper(Service.Post(Service.MerchantPath() + "/customers/advanced_search_ids"));
            var query = new CustomerSearchRequest();

            return new ResourceCollection<Customer>(response, delegate(string[] ids) {
                return FetchCustomers(query, ids);
            });
        }

        public virtual ResourceCollection<Customer> Search(CustomerSearchRequest query)
        {
            var response = new NodeWrapper(Service.Post(Service.MerchantPath() + "/customers/advanced_search_ids", query));

            return new ResourceCollection<Customer>(response, delegate(string[] ids) {
                return FetchCustomers(query, ids);
            });
        }

        private List<Customer> FetchCustomers(CustomerSearchRequest query, string[] ids)
        {
            query.Ids.IncludedIn(ids);

            var response = new NodeWrapper(Service.Post(Service.MerchantPath() + "/customers/advanced_search", query));

            var customers = new List<Customer>();
            foreach (var node in response.GetList("customer"))
            {
                customers.Add(new Customer(node, Gateway));
            }
            return customers;
        }
    }
}
