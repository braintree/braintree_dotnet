#pragma warning disable 1591

using Braintree.Exceptions;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Braintree
{
    public class TransparentRedirectRequest : Request
    {
        public string Id { get; protected set; }

        public TransparentRedirectRequest(string queryString, BraintreeService service)
        {
            queryString = queryString.TrimStart('?');

            var paramMap = new Dictionary<string, string>();
            string[] queryParams = queryString.Split('&');

            foreach (var queryParam in queryParams)
            {
                var items = queryParam.Split('=');
                paramMap[items[0]] = items[1];
            }

            string message = null;
            if (paramMap.ContainsKey("bt_message"))
            {
                message = WebUtility.UrlDecode(paramMap["bt_message"]);
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
            var builder = new StringBuilder();
            builder.Append(BuildXMLElement("id", Id));
            
            return builder.ToString();
        }
    }
}
