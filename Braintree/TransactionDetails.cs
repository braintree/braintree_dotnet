using System;

namespace Braintree
{
    public class TransactionDetails
    {
        public string Id { get; protected set; }
        public string Amount { get; protected set; }

        protected internal TransactionDetails(NodeWrapper node)
        {
            Id = node.GetString("id");
            Amount = node.GetString("amount");
        }
    }
}

