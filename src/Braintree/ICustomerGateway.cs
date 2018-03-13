#pragma warning disable 1591

using System.Threading.Tasks;

namespace Braintree
{
    /// <summary>
    /// Provides operations for creating, finding, updating, and deleting customers in the vault
    /// </summary>
    public interface ICustomerGateway
    {
        ResourceCollection<Customer> All();
        Task<ResourceCollection<Customer>> AllAsync();
        Result<Customer> ConfirmTransparentRedirect(string queryString);
        Result<Customer> Create();
        Task<Result<Customer>> CreateAsync();
        Result<Customer> Create(CustomerRequest request);
        Task<Result<Customer>> CreateAsync(CustomerRequest request);
        Result<Customer> Delete(string Id);
        Task<Result<Customer>> DeleteAsync(string Id);
        Customer Find(string Id);
        Customer Find(string Id, string AssociationFilterId);
        Task<Customer> FindAsync(string Id);
        ResourceCollection<Customer> Search(CustomerSearchRequest query);
        Task<ResourceCollection<Customer>> SearchAsync(CustomerSearchRequest query);
        string TransparentRedirectURLForCreate();
        string TransparentRedirectURLForUpdate();
        Result<Customer> Update(string Id, CustomerRequest request);
        Task<Result<Customer>> UpdateAsync(string Id, CustomerRequest request);
    }
}
