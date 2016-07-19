#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class IdsSearchRequest : SearchRequest
    {
        public MultipleValueNode<IdsSearchRequest, string> Ids
        {
            get
            {
                return new MultipleValueNode<IdsSearchRequest, string>("ids", this);
            }
        }
    }
}
