#pragma warning disable 1591

using System;
using System.Xml;

namespace Braintree
{
    public class TransparentRedirectGateway
    {
        public const String CREATE_CUSTOMER = "create_customer";
        public const String UPDATE_CUSTOMER = "update_customer";
        public const String CREATE_PAYMENT_METHOD = "create_payment_method";
        public const String UPDATE_PAYMENT_METHOD = "update_payment_method";
        public const String CREATE_TRANSACTION = "create_transaction";

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

        public virtual Result<Customer> ConfirmCustomer(String queryString)
        {
            TransparentRedirectRequest trRequest = new TransparentRedirectRequest(queryString);
            XmlNode node = WebServiceGateway.Post("/transparent_redirect_requests/" + trRequest.Id + "/confirm", trRequest);

            return new Result<Customer>(new NodeWrapper(node));
        }
    }
}
