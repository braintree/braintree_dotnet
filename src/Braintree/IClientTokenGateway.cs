#pragma warning disable 1591

using System;
using System.Threading.Tasks;

namespace Braintree
{
    /// <summary>
    /// Generates client tokens, which are used to authenticate requests clients make directly
    ///   on behalf of merchants
    /// </summary>
    public interface IClientTokenGateway
    {
        string Generate(ClientTokenRequest request = null);
        Task<string> GenerateAsync(ClientTokenRequest request = null);
    }
}
