#pragma warning disable 1591

using System;
using System.Xml;

namespace Braintree
{
    public class TransparentRedirectGateway
    {
        public String Url
        {
            get { return Configuration.BaseMerchantURL() + "/transparent_redirect_requests"; }
        }

        public TransparentRedirectGateway()
        {
        }

        public virtual Result<Transaction> ConfirmTransaction(String queryString)
        {
            TransparentRedirectRequest trRequest = new TransparentRedirectRequest(queryString);
            XmlNode node = WebServiceGateway.Post("/transparent_redirect_requests/" + trRequest.Id + "/confirm", trRequest);

            return new Result<Transaction>(new NodeWrapper(node));
        }
    }
}
