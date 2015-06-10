#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Braintree.Exceptions;
using System.Net;

namespace Braintree
{
    public class TransparentRedirectRequest : Request
    {

        public string Id { get; protected set; }

        public TransparentRedirectRequest(string queryString, BraintreeService service)
        {
            queryString = queryString.TrimStart('?');

            Dictionary<string, string> paramMap = new Dictionary<string, string>();
            string[] queryParams = queryString.Split('&');

            foreach (string queryParam in queryParams)
            {
                string[] items = queryParam.Split('=');
                paramMap[items[0]] = items[1];
            }

            string message = null;
            if (paramMap.ContainsKey("bt_message"))
            {
                message = HttpUtility.UrlDecode(paramMap["bt_message"]);
            }

            BraintreeService.ThrowExceptionIfErrorStatusCode((HttpStatusCode)int.Parse(paramMap["http_status"]), message);

            if (!TrUtil.IsValidTrQueryString(queryString, service))
            {
                throw new ForgedQueryStringException();
            }

            Id = paramMap["id"];
        }

        public override string ToXml()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(BuildXMLElement("id", Id));
            
            return builder.ToString();
        }
    }
}
