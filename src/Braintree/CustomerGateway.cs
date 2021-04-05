#pragma warning disable 1591

using Braintree.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace Braintree
{
    /// <summary>
    /// Provides operations for creating, finding, updating, and deleting customers in the vault
    /// </summary>
    public class CustomerGateway : ICustomerGateway
    {
        private readonly BraintreeService service;
        private readonly IBraintreeGateway gateway;

        protected internal CustomerGateway(IBraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            this.gateway = gateway;
            service = gateway.Service;
        }

        public virtual Customer Find(string Id)
        {
            if(Id == null || Id.Trim().Equals(""))
                throw new NotFoundException();

            XmlNode customerXML = service.Get(service.MerchantPath() + "/customers/" + Id);

            return new Customer(new NodeWrapper(customerXML), gateway);
        }

        public virtual Customer Find(string Id, string AssociationFilterId)
        {
            if(Id == null || Id.Trim().Equals(""))
                throw new NotFoundException();

            if(AssociationFilterId == null || AssociationFilterId.Trim().Equals(""))
                throw new NotFoundException();

            string queryParams = "?association_filter_id=" + AssociationFilterId;

            XmlNode customerXML = service.Get(service.MerchantPath() + "/customers/" + Id + queryParams);

            return new Customer(new NodeWrapper(customerXML), gateway);
        }

        public virtual async Task<Customer> FindAsync(string Id)
        {
            if(Id == null || Id.Trim().Equals(""))
                throw new NotFoundException();

            XmlNode customerXML = await service.GetAsync(service.MerchantPath() + "/customers/" + Id).ConfigureAwait(false);

            return new Customer(new NodeWrapper(customerXML), gateway);
        }

        public virtual Result<Customer> Create()
        {
            return Create(new CustomerRequest());
        }

        public virtual Task<Result<Customer>> CreateAsync()
        {
            return CreateAsync(new CustomerRequest());
        }

        public virtual Result<Customer> Create(CustomerRequest request)
        {
            XmlNode customerXML = service.Post(service.MerchantPath() + "/customers", request);

            return new ResultImpl<Customer>(new NodeWrapper(customerXML), gateway);
        }

        public virtual async Task<Result<Customer>> CreateAsync(CustomerRequest request)
        {
            XmlNode customerXML = await service.PostAsync(service.MerchantPath() + "/customers", request).ConfigureAwait(false);

            return new ResultImpl<Customer>(new NodeWrapper(customerXML), gateway);
        }

        public virtual Result<Customer> Delete(string Id)
        {
            XmlNode customerXML = service.Delete(service.MerchantPath() + "/customers/" + Id);

            return new ResultImpl<Customer>(new NodeWrapper(customerXML), gateway);
        }

        public virtual async Task<Result<Customer>> DeleteAsync(string Id)
        {
            XmlNode customerXML = await service.DeleteAsync(service.MerchantPath() + "/customers/" + Id).ConfigureAwait(false);

            return new ResultImpl<Customer>(new NodeWrapper(customerXML), gateway);
        }

        public virtual Result<Customer> Update(string Id, CustomerRequest request)
        {
            XmlNode customerXML = service.Put(service.MerchantPath() + "/customers/" + Id, request);

            return new ResultImpl<Customer>(new NodeWrapper(customerXML), gateway);
        }

        public virtual async Task<Result<Customer>> UpdateAsync(string Id, CustomerRequest request)
        {
            XmlNode customerXML = await service.PutAsync(service.MerchantPath() + "/customers/" + Id, request).ConfigureAwait(false);

            return new ResultImpl<Customer>(new NodeWrapper(customerXML), gateway);
        }

        public virtual ResourceCollection<Customer> All()
        {
            var response = new NodeWrapper(service.Post(service.MerchantPath() + "/customers/advanced_search_ids"));
            var query = new CustomerSearchRequest();

            return new ResourceCollection<Customer>(response, ids => FetchCustomers(query, ids));
        }

        public virtual async Task<ResourceCollection<Customer>> AllAsync()
        {
            var response = new NodeWrapper(await service.PostAsync(service.MerchantPath() + "/customers/advanced_search_ids").ConfigureAwait(false));
            var query = new CustomerSearchRequest();

            return new ResourceCollection<Customer>(response, ids => FetchCustomers(query, ids));
        }

        public virtual ResourceCollection<Customer> Search(CustomerSearchRequest query)
        {
            var response = new NodeWrapper(service.Post(service.MerchantPath() + "/customers/advanced_search_ids", query));

            return new ResourceCollection<Customer>(response, ids => FetchCustomers(query, ids));
        }

        public virtual async Task<ResourceCollection<Customer>> SearchAsync(CustomerSearchRequest query)
        {
            var response = new NodeWrapper(await service.PostAsync(service.MerchantPath() + "/customers/advanced_search_ids", query).ConfigureAwait(false));

            return new ResourceCollection<Customer>(response, ids => FetchCustomers(query, ids));
        }

        private List<Customer> FetchCustomers(CustomerSearchRequest query, string[] ids)
        {
            query.Ids.IncludedIn(ids);

            var response = new NodeWrapper(service.Post(service.MerchantPath() + "/customers/advanced_search", query));

            var customers = new List<Customer>();
            foreach (var node in response.GetList("customer"))
            {
                customers.Add(new Customer(node, gateway));
            }
            return customers;
        }
    }
}
