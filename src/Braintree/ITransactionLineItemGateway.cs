#pragma warning disable 1591

using System.Threading.Tasks;
using System.Collections.Generic;

namespace Braintree
{
    /// <summary>
    /// Provides operations for interacting with transaction line items
    /// </summary>
    public interface ITransactionLineItemGateway
    {
        List<TransactionLineItem> FindAll(string id);
    }
}
