#pragma warning disable 1591

using System.Xml;

namespace Braintree
{
    public class TransparentRedirectGateway : ITransparentRedirectGateway
    {
        public const string CREATE_CUSTOMER = "create_customer";
        public const string UPDATE_CUSTOMER = "update_customer";
        public const string CREATE_PAYMENT_METHOD = "create_payment_method";
        public const string UPDATE_PAYMENT_METHOD = "update_payment_method";
        public const string CREATE_TRANSACTION = "create_transaction";

        public string Url
        {
            get { return service.BaseMerchantURL() + "/transparent_redirect_requests"; }
        }

        private readonly BraintreeService service;
        private readonly BraintreeGateway gateway;

        protected internal TransparentRedirectGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            this.gateway = gateway;
            service = new BraintreeService(gateway.Configuration);
        }

        public string BuildTrData(Request request, string redirectURL)
        {
            return TrUtil.BuildTrData(request, redirectURL, service);
        }

        public virtual Result<Transaction> ConfirmTransaction(string queryString)
        {
            var trRequest = new TransparentRedirectRequest(queryString, service);
            XmlNode node = service.Post(service.MerchantPath() + "/transparent_redirect_requests/" + trRequest.Id + "/confirm", trRequest);

            return new ResultImpl<Transaction>(new NodeWrapper(node), gateway);
        }

        public virtual Result<Customer> ConfirmCustomer(string queryString)
        {
            var trRequest = new TransparentRedirectRequest(queryString, service);
            XmlNode node = service.Post(service.MerchantPath() + "/transparent_redirect_requests/" + trRequest.Id + "/confirm", trRequest);

            return new ResultImpl<Customer>(new NodeWrapper(node), gateway);
        }

        public virtual Result<CreditCard> ConfirmCreditCard(string queryString)
        {
            var trRequest = new TransparentRedirectRequest(queryString, service);
            XmlNode node = service.Post(service.MerchantPath() + "/transparent_redirect_requests/" + trRequest.Id + "/confirm", trRequest);

            return new ResultImpl<CreditCard>(new NodeWrapper(node), gateway);
        }
    }
}
