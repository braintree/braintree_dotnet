#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    [Serializable]
    public class TransactionOptionsCloneRequest : Request
    {
        public Boolean? SubmitForSettlement { get; set; }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        public override String ToQueryString(String root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            return new RequestBuilder(root).AddElement("submit-for-settlement", SubmitForSettlement);
        }
    }
}
