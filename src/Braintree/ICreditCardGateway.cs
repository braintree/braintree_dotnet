#pragma warning disable 1591

using System;

namespace Braintree
{
    /// <summary>
    /// Provides operations for creating, finding, updating, and deleting credit cards in the vault
    /// </summary>
    public interface ICreditCardGateway
    {
        Result<CreditCard> ConfirmTransparentRedirect(string queryString);
        Result<CreditCard> Create(CreditCardRequest request);
        void Delete(string token);
        ResourceCollection<CreditCard> Expired();
        ResourceCollection<CreditCard> ExpiringBetween(DateTime start, DateTime end);
        CreditCard Find(string token);
        CreditCard FromNonce(string nonce);
        string TransparentRedirectURLForCreate();
        string TransparentRedirectURLForUpdate();
        Result<CreditCard> Update(string token, CreditCardRequest request);
    }
}
