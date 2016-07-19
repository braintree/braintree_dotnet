using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class RiskDataRequest: Request
    {
        public string CustomerBrowser { get; set; }
        public string CustomerIP { get; set; }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
            AddElement("customer-browser", CustomerBrowser).
            AddElement("customer-ip", CustomerIP);
        }
    }
}
