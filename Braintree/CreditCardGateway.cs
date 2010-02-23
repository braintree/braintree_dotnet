using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Braintree
{
    public class CreditCardGateway
    {
        public String TransparentRedirectURLForCreate()
        {
            return Configuration.BaseMerchantURL() + "/payment_methods/all/create_via_transparent_redirect_request";
        }

        public String TransparentRedirectURLForUpdate()
        {
            return Configuration.BaseMerchantURL() + "/payment_methods/all/update_via_transparent_redirect_request";
        }

        public Result<CreditCard> Create(CreditCardRequest request)
        {
            XmlNode creditCardXML = WebServiceGateway.Post("/payment_methods", request);

            return new Result<CreditCard>(new NodeWrapper(creditCardXML));
        }

        public Result<CreditCard> ConfirmTransparentRedirect(String queryString)
        {
            TransparentRedirectRequest trRequest = new TransparentRedirectRequest(queryString);
            XmlNode creditCardXML = WebServiceGateway.Post("/payment_methods/all/confirm_transparent_redirect_request", trRequest);

            return new Result<CreditCard>(new NodeWrapper(creditCardXML));
        }

        public Result<CreditCard> Find(String token)
        {
            XmlNode creditCardXML = WebServiceGateway.Get("/payment_methods/" + token);

            return new Result<CreditCard>(new NodeWrapper(creditCardXML));
        }

        public void Delete(String token)
        {
            WebServiceGateway.Delete("/payment_methods/" + token);
        }

        public Result<CreditCard> Update(String token, CreditCardRequest request)
        {
            XmlNode creditCardXML = WebServiceGateway.Put("/payment_methods/" + token, request);

            return new Result<CreditCard>(new NodeWrapper(creditCardXML));
        }
    }
}
