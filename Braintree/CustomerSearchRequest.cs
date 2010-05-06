#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class CustomerSearchRequest : SearchRequest
    {
        public MultipleValueNode<CustomerSearchRequest> Ids
        {
            get
            {
                return new MultipleValueNode<CustomerSearchRequest>("ids", this);
            }
        }
    }
}
