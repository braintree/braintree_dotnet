using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class RiskDataRequest: Request
    {
        public string CustomerBrowser { get; set; }
        public string CustomerDeviceId { get; set; }
        public string CustomerIP { get; set; }
        public string CustomerLocationZip { get; set; }
        public int? CustomerTenure { get; set; }

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
            var builder = new RequestBuilder(root);
            builder.AddElement("customer-browser", CustomerBrowser);
            builder.AddElement("customer-device-id", CustomerDeviceId);
            builder.AddElement("customer-ip", CustomerIP);
            builder.AddElement("customer-location-zip", CustomerLocationZip);
            if (CustomerTenure.HasValue) builder.AddElement("customer-tenure", CustomerTenure);

            return builder;
        }
    }
}
