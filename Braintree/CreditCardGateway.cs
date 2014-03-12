#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Braintree.Exceptions;


namespace Braintree
{
    /// <summary>
    /// Provides operations for creating, finding, updating, and deleting credit cards in the vault
    /// </summary>
    public class CreditCardGateway
    {
        private BraintreeService Service;

        protected internal CreditCardGateway(BraintreeService service)
        {
            Service = service;
        }

        [Obsolete("Use gateway.TransparentRedirect.Url")]
        public virtual String TransparentRedirectURLForCreate()
        {
            return Service.BaseMerchantURL() + "/payment_methods/all/create_via_transparent_redirect_request";
        }

        [Obsolete("Use gateway.TransparentRedirect.Url")]
        public virtual String TransparentRedirectURLForUpdate()
        {
            return Service.BaseMerchantURL() + "/payment_methods/all/update_via_transparent_redirect_request";
        }

        public virtual Result<CreditCard> Create(CreditCardRequest request)
        {
            XmlNode creditCardXML = Service.Post("/payment_methods", request);

            return new Result<CreditCard>(new NodeWrapper(creditCardXML), Service);
        }

        [Obsolete("Use gateway.TransparentRedirect.Confirm()")]
        public virtual Result<CreditCard> ConfirmTransparentRedirect(String queryString)
        {
            TransparentRedirectRequest trRequest = new TransparentRedirectRequest(queryString, Service);
            XmlNode creditCardXML = Service.Post("/payment_methods/all/confirm_transparent_redirect_request", trRequest);

            return new Result<CreditCard>(new NodeWrapper(creditCardXML), Service);
        }

        public virtual ResourceCollection<CreditCard> Expired()
        {
            NodeWrapper response = new NodeWrapper(Service.Post("/payment_methods/all/expired_ids"));

            return new ResourceCollection<CreditCard>(response, delegate(String[] ids) {
                IdsSearchRequest query = new IdsSearchRequest().
                    Ids.IncludedIn(ids);

                NodeWrapper fetchResponse = new NodeWrapper(Service.Post("/payment_methods/all/expired", query));

                List<CreditCard> creditCards = new List<CreditCard>();
                foreach (NodeWrapper node in fetchResponse.GetList("credit-card"))
                {
                    creditCards.Add(new CreditCard(node, Service));
                }
                return creditCards;
            });
        }

        public virtual ResourceCollection<CreditCard> ExpiringBetween(DateTime start, DateTime end)
        {
            String queryString = String.Format("start={0:MMyyyy}&end={1:MMyyyy}", start, end);

            NodeWrapper response = new NodeWrapper(Service.Post("/payment_methods/all/expiring_ids?" + queryString));

            return new ResourceCollection<CreditCard>(response, delegate(String[] ids) {
                IdsSearchRequest query = new IdsSearchRequest().
                    Ids.IncludedIn(ids);

                NodeWrapper fetchResponse = new NodeWrapper(Service.Post("/payment_methods/all/expiring?" + queryString, query));

                List<CreditCard> creditCards = new List<CreditCard>();
                foreach (NodeWrapper node in fetchResponse.GetList("credit-card"))
                {
                    creditCards.Add(new CreditCard(node, Service));
                }
                return creditCards;
            });
        }

        public virtual CreditCard Find(String token)
        {
            if(token == null || token.Trim().Equals(""))
                throw new NotFoundException();

            XmlNode creditCardXML = Service.Get("/payment_methods/" + token);

            return new CreditCard(new NodeWrapper(creditCardXML), Service);
        }

        public virtual CreditCard FromNonce(String nonce)
        {
            if(nonce == null || nonce.Trim().Equals(""))
                throw new NotFoundException();

            try {
                XmlNode creditCardXML = Service.Get("/payment_methods/from_nonce/" + nonce);
                return new CreditCard(new NodeWrapper(creditCardXML), Service);
            } catch (NotFoundException) {
                throw new NotFoundException("Payment method with nonce " + nonce + " locked, consumed or not found");
            }
        }

        public virtual void Delete(String token)
        {
            Service.Delete("/payment_methods/" + token);
        }

        public virtual Result<CreditCard> Update(String token, CreditCardRequest request)
        {
            XmlNode creditCardXML = Service.Put("/payment_methods/" + token, request);

            return new Result<CreditCard>(new NodeWrapper(creditCardXML), Service);
        }
    }
}
