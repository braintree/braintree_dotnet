#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Braintree
{
    /// <summary>
    /// Provides operations for creating, finding, updating, and deleting credit cards in the vault
    /// </summary>
    public class CreditCardGateway
    {
        public virtual String TransparentRedirectURLForCreate()
        {
            return Configuration.BaseMerchantURL() + "/payment_methods/all/create_via_transparent_redirect_request";
        }

        public virtual String TransparentRedirectURLForUpdate()
        {
            return Configuration.BaseMerchantURL() + "/payment_methods/all/update_via_transparent_redirect_request";
        }

        public virtual Result<CreditCard> Create(CreditCardRequest request)
        {
            XmlNode creditCardXML = WebServiceGateway.Post("/payment_methods", request);

            return new Result<CreditCard>(new NodeWrapper(creditCardXML));
        }

        public virtual Result<CreditCard> ConfirmTransparentRedirect(String queryString)
        {
            TransparentRedirectRequest trRequest = new TransparentRedirectRequest(queryString);
            XmlNode creditCardXML = WebServiceGateway.Post("/payment_methods/all/confirm_transparent_redirect_request", trRequest);

            return new Result<CreditCard>(new NodeWrapper(creditCardXML));
        }

        public virtual ResourceCollection<CreditCard> Expired()
        {
            NodeWrapper response = new NodeWrapper(WebServiceGateway.Post("/payment_methods/all/expired_ids"));

            return new ResourceCollection<CreditCard>(response, delegate(String[] ids) {
                CustomerSearchRequest query = new CustomerSearchRequest().
                    Ids.IncludedIn(ids);

                NodeWrapper fetchResponse = new NodeWrapper(WebServiceGateway.Post("/payment_methods/all/expired", query));

                List<CreditCard> creditCards = new List<CreditCard>();
                foreach (NodeWrapper node in fetchResponse.GetList("credit-card"))
                {
                    creditCards.Add(new CreditCard(node));
                }
                return creditCards;
            });
        }

        public virtual CreditCard Find(String token)
        {
            XmlNode creditCardXML = WebServiceGateway.Get("/payment_methods/" + token);

            return new CreditCard(new NodeWrapper(creditCardXML));
        }

        public virtual void Delete(String token)
        {
            WebServiceGateway.Delete("/payment_methods/" + token);
        }

        public virtual Result<CreditCard> Update(String token, CreditCardRequest request)
        {
            XmlNode creditCardXML = WebServiceGateway.Put("/payment_methods/" + token, request);

            return new Result<CreditCard>(new NodeWrapper(creditCardXML));
        }
    }
}
