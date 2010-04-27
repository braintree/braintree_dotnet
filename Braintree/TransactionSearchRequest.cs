#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class TransactionSearchRequest : SearchRequest
    {
        public TransactionSearchRequest() : base()
        {
        }

        public virtual TextNode<TransactionSearchRequest> BillingCompany()
        {
            return new TextNode<TransactionSearchRequest>("billing-company", this);
        }

        public virtual TextNode<TransactionSearchRequest> Id()
        {
            return new TextNode<TransactionSearchRequest>("id", this);
        }
    }
}
