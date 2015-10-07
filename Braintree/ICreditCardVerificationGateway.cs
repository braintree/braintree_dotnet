#pragma warning disable 1591

using System;

namespace Braintree
{
    /// <summary>
    /// Provides operations for finding verifications
    /// </summary>
    public interface ICreditCardVerificationGateway
    {
        CreditCardVerification Find(string Id);
        ResourceCollection<CreditCardVerification> Search(CreditCardVerificationSearchRequest query);
    }
}