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

        public String Id { get; protected set; }

        public TransparentRedirectRequest(String queryString, BraintreeService service)
        {
            queryString = queryString.TrimStart('?');

            Dictionary<String, String> paramMap = new Dictionary<String, String>();
            String[] queryParams = queryString.Split('&');

            foreach (String queryParam in queryParams)
            {
                String[] items = queryParam.Split('=');
                paramMap[items[0]] = items[1];
            }

            String message = null;
            if (paramMap.ContainsKey("bt_message"))
            {
                message = HttpUtility.UrlDecode(paramMap["bt_message"]);
            }

            BraintreeService.ThrowExceptionIfErrorStatusCode((HttpStatusCode)Int32.Parse(paramMap["http_status"]), message);

            if (!TrUtil.IsValidTrQueryString(queryString, service))
            {
                throw new ForgedQueryStringException();
            }

            Id = paramMap["id"];
        }

        public override String ToXml()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(BuildXMLElement("id", Id));
            
            return builder.ToString();
        }
    }
}
