#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using Braintree.Exceptions;
using System.Net;

namespace Braintree
{
    public class TransparentRedirectRequest : Request
    {
        private String Id;

        public TransparentRedirectRequest(String queryString)
        {
            queryString = queryString.TrimStart('?');

            Dictionary<String, String> paramMap = new Dictionary<String, String>();
            String[] queryParams = queryString.Split('&');

            foreach (String queryParam in queryParams)
            {
                String[] items = queryParam.Split('=');
                paramMap[items[0]] = items[1];
            }

            WebServiceGateway.ThrowExceptionIfErrorStatusCode((HttpStatusCode)Int32.Parse(paramMap["http_status"]));

            if (!TrUtil.IsValidTrQueryString(queryString))
            {
                throw new ForgedQueryStringException();
            }

            Id = paramMap["id"];
        }

        public override string ToXml(string rootElement)
        {
            throw new NotImplementedException();
        }

        public override String ToXml()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(BuildXMLElement("id", Id));
            
            return builder.ToString();
        }

        public override String ToQueryString(String parent)
        {
            return null;
        }

        public override String ToQueryString()
        {
            return null;
        }
    }
}
