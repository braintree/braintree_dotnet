using System;

namespace Braintree
{
    public class TransactionDetails
    {
        public virtual string Id { get; protected set; }
        public virtual string Amount { get; protected set; }

        protected internal TransactionDetails(NodeWrapper node)
        {
            Id = node.GetString("id");
            Amount = node.GetString("amount");
        }

        [Obsolete("Mock Use Only")]
        protected internal TransactionDetails() { }
    }
}

