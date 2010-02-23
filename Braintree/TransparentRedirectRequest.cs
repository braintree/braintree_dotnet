using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class TransparentRedirectRequest : Request
    {
        private String ID;

        public TransparentRedirectRequest(String queryString)
        {
            Dictionary<String, String> paramMap = new Dictionary<String, String>();
            String[] queryParams = queryString.Split('&');

            foreach (String queryParam in queryParams)
            {
                String[] items = queryParam.Split('=');
                paramMap[items[0]] = items[1];
            }

            ID = paramMap["id"];
        }

        internal override string ToXml(string rootElement)
        {
            throw new NotImplementedException();
        }

        internal override String ToXml()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(BuildXMLElement("id", ID));
            
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
