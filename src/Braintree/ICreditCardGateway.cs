#pragma warning disable 1591

using System;
using System.Threading.Tasks;

namespace Braintree
{
    /// <summary>
    /// Provides operations for creating, finding, updating, and deleting credit cards in the vault
    /// </summary>
    public interface ICreditCardGateway
    {
        Result<CreditCard> ConfirmTransparentRedirect(string queryString);
        Result<CreditCard> Create(CreditCardRequest request);
        Task<Result<CreditCard>> CreateAsync(CreditCardRequest request);
        void Delete(string token);
        Task DeleteAsync(string token);
        ResourceCollection<CreditCard> Expired();
        ResourceCollection<CreditCard> ExpiringBetween(DateTime start, DateTime end);
        Task<ResourceCollection<CreditCard>> ExpiringBetweenAsync(DateTime start, DateTime end);
        CreditCard Find(string token);
        Task<CreditCard> FindAsync(string token);
        CreditCard FromNonce(string nonce);
        string TransparentRedirectURLForCreate();
        string TransparentRedirectURLForUpdate();
        Result<CreditCard> Update(string token, CreditCardRequest request);
        Task<Result<CreditCard>> UpdateAsync(string token, CreditCardRequest request);
    }
}
