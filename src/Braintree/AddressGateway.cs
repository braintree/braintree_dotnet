#pragma warning disable 1591

using Braintree.Exceptions;
using System.Threading.Tasks;
using System.Xml;

namespace Braintree
{
    /// <summary>
    /// Provides operations for creating, finding, updating, and deleting addresses in the vault
    /// </summary>
    public class AddressGateway : IAddressGateway
    {
        private readonly BraintreeService Service;
        private readonly IBraintreeGateway Gateway;

        protected internal AddressGateway(IBraintreeGateway gateway)
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

        public virtual async Task<Result<Address>> CreateAsync(string customerId, AddressRequest request)
        {
            XmlNode addressXML = await Service.PostAsync(Service.MerchantPath() + "/customers/" + customerId + "/addresses", request).ConfigureAwait(false);

            return new ResultImpl<Address>(new NodeWrapper(addressXML), Gateway);
        }

        public virtual Result<Address> Delete(string customerId, string id)
        {
            XmlNode addressXML = Service.Delete(Service.MerchantPath() + "/customers/" + customerId + "/addresses/" + id);

            return new ResultImpl<Address>(new NodeWrapper(addressXML), Gateway);
        }

        public virtual async Task<Result<Address>> DeleteAsync(string customerId, string id)
        {
            XmlNode addressXML = await Service.DeleteAsync(Service.MerchantPath() + "/customers/" + customerId + "/addresses/" + id).ConfigureAwait(false);

            return new ResultImpl<Address>(new NodeWrapper(addressXML), Gateway);
        }

        public virtual Address Find(string customerId, string id)
        {
            if(customerId == null || customerId.Trim().Equals("") || id == null || id.Trim().Equals(""))
            {
                throw new NotFoundException();
            }

            XmlNode addressXML = Service.Get(Service.MerchantPath() + "/customers/" + customerId + "/addresses/" + id);

            return new Address(new NodeWrapper(addressXML));
        }

        public virtual async Task<Address> FindAsync(string customerId, string id)
        {
            if(customerId == null || customerId.Trim().Equals("") || id == null || id.Trim().Equals(""))
            {
                throw new NotFoundException();
            }

            XmlNode addressXML = await Service.GetAsync(Service.MerchantPath() + "/customers/" + customerId + "/addresses/" + id).ConfigureAwait(false);

            return new Address(new NodeWrapper(addressXML));
        }


        public virtual Result<Address> Update(string customerId, string id, AddressRequest request)
        {
            XmlNode addressXML = Service.Put(Service.MerchantPath() + "/customers/" + customerId + "/addresses/" + id, request);

            return new ResultImpl<Address>(new NodeWrapper(addressXML), Gateway);
        }

        public virtual async Task<Result<Address>> UpdateAsync(string customerId, string id, AddressRequest request)
        {
            XmlNode addressXML = await Service.PutAsync(Service.MerchantPath() + "/customers/" + customerId + "/addresses/" + id, request).ConfigureAwait(false);

            return new ResultImpl<Address>(new NodeWrapper(addressXML), Gateway);
        }
    }
}
