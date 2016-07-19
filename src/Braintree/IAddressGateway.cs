#pragma warning disable 1591

using System;

namespace Braintree
{
    /// <summary>
    /// Provides operations for creating, finding, updating, and deleting addresses in the vault
    /// </summary>
    public interface IAddressGateway
    {
        Result<Address> Create(string customerId, AddressRequest request);
        Result<Address> Delete(string customerId, string id);
        Address Find(string customerId, string id);
        Result<Address> Update(string customerId, string id, AddressRequest request);
    }
}
