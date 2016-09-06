#pragma warning disable 1591

namespace Braintree
{
    /// <summary>
    /// Provides operations for creating, finding, updating, and deleting customers in the vault
    /// </summary>
    public interface ICustomerGateway
    {
        ResourceCollection<Customer> All();
        Result<Customer> ConfirmTransparentRedirect(string queryString);
        Result<Customer> Create();
        Result<Customer> Create(CustomerRequest request);
        Result<Customer> Delete(string Id);
        Customer Find(string Id);
        ResourceCollection<Customer> Search(CustomerSearchRequest query);
        string TransparentRedirectURLForCreate();
        string TransparentRedirectURLForUpdate();
        Result<Customer> Update(string Id, CustomerRequest request);
    }
}
