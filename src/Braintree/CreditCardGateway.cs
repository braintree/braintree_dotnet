#pragma warning disable 1591

using Braintree.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;


namespace Braintree
{
    /// <summary>
    /// Provides operations for creating, finding, updating, and deleting credit cards in the vault
    /// </summary>
    public class CreditCardGateway : ICreditCardGateway
    {
        private readonly BraintreeService service;
        private readonly IBraintreeGateway gateway;

        protected internal CreditCardGateway(IBraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            this.gateway = gateway;
            service = new BraintreeService(gateway.Configuration);
        }

        public virtual Result<CreditCard> Create(CreditCardRequest request)
        {
            XmlNode creditCardXML = service.Post(service.MerchantPath() + "/payment_methods", request);

            return new ResultImpl<CreditCard>(new NodeWrapper(creditCardXML), gateway);
        }

        public virtual async Task<Result<CreditCard>> CreateAsync(CreditCardRequest request)
        {
            XmlNode creditCardXML = await service.PostAsync(service.MerchantPath() + "/payment_methods", request).ConfigureAwait(false);

            return new ResultImpl<CreditCard>(new NodeWrapper(creditCardXML), gateway);
        }

        public virtual ResourceCollection<CreditCard> Expired()
        {
            var response = new NodeWrapper(service.Post(service.MerchantPath() + "/payment_methods/all/expired_ids"));

            return new ResourceCollection<CreditCard>(response, delegate(string[] ids) {
                var query = new IdsSearchRequest().
                    Ids.IncludedIn(ids);

                var fetchResponse = new NodeWrapper(service.Post(service.MerchantPath() + "/payment_methods/all/expired", query));

                var creditCards = new List<CreditCard>();
                foreach (var node in fetchResponse.GetList("credit-card"))
                {
                    creditCards.Add(new CreditCard(node, gateway));
                }
                return creditCards;
            });
        }

        public virtual ResourceCollection<CreditCard> ExpiringBetween(DateTime start, DateTime end)
        {
            string queryString = string.Format("start={0:MMyyyy}&end={1:MMyyyy}", start, end);

            var response = new NodeWrapper(service.Post(service.MerchantPath() + "/payment_methods/all/expiring_ids?" + queryString));

            return new ResourceCollection<CreditCard>(response, delegate(string[] ids) {
                var query = new IdsSearchRequest().
                    Ids.IncludedIn(ids);

                var fetchResponse = new NodeWrapper(service.Post(service.MerchantPath() + "/payment_methods/all/expiring?" + queryString, query));

                var creditCards = new List<CreditCard>();
                foreach (var node in fetchResponse.GetList("credit-card"))
                {
                    creditCards.Add(new CreditCard(node, gateway));
                }
                return creditCards;
            });
        }

        public virtual async Task<ResourceCollection<CreditCard>> ExpiringBetweenAsync(DateTime start, DateTime end)
        {
            string queryString = string.Format("start={0:MMyyyy}&end={1:MMyyyy}", start, end);

            var response = new NodeWrapper(await service.PostAsync(service.MerchantPath() + "/payment_methods/all/expiring_ids?" + queryString).ConfigureAwait(false));

            return new ResourceCollection<CreditCard>(response, delegate(string[] ids) {
                var query = new IdsSearchRequest().
                    Ids.IncludedIn(ids);

                var fetchResponse = new NodeWrapper(service.Post(service.MerchantPath() + "/payment_methods/all/expiring?" + queryString, query));

                var creditCards = new List<CreditCard>();
                foreach (var node in fetchResponse.GetList("credit-card"))
                {
                    creditCards.Add(new CreditCard(node, gateway));
                }
                return creditCards;
            });
        }

        public virtual CreditCard Find(string token)
        {
            if(token == null || token.Trim().Equals(""))
                throw new NotFoundException();

            XmlNode creditCardXML = service.Get(service.MerchantPath() + "/payment_methods/credit_card/" + token);

            return new CreditCard(new NodeWrapper(creditCardXML), gateway);
        }

        public virtual async Task<CreditCard> FindAsync(string token)
        {
            if(token == null || token.Trim().Equals(""))
                throw new NotFoundException();

            XmlNode creditCardXML = await service.GetAsync(service.MerchantPath() + "/payment_methods/credit_card/" + token).ConfigureAwait(false);

            return new CreditCard(new NodeWrapper(creditCardXML), gateway);
        }

        public virtual CreditCard FromNonce(string nonce)
        {
            if(nonce == null || nonce.Trim().Equals(""))
                throw new NotFoundException();

            try {
                XmlNode creditCardXML = service.Get(service.MerchantPath() + "/payment_methods/from_nonce/" + nonce);
                return new CreditCard(new NodeWrapper(creditCardXML), gateway);
            } catch (NotFoundException) {
                throw new NotFoundException("Payment method with nonce " + nonce + " locked, consumed or not found");
            }
        }

        public virtual void Delete(string token)
        {
            service.Delete(service.MerchantPath() + "/payment_methods/credit_card/" + token);
        }

        public virtual Task DeleteAsync(string token)
        {
            return service.DeleteAsync(service.MerchantPath() + "/payment_methods/credit_card/" + token);
        }

        public virtual Result<CreditCard> Update(string token, CreditCardRequest request)
        {
            XmlNode creditCardXML = service.Put(service.MerchantPath() + "/payment_methods/credit_card/" + token, request);

            return new ResultImpl<CreditCard>(new NodeWrapper(creditCardXML), gateway);
        }

        public virtual async Task<Result<CreditCard>> UpdateAsync(string token, CreditCardRequest request)
        {
            XmlNode creditCardXML = await service.PutAsync(service.MerchantPath() + "/payment_methods/credit_card/" + token, request);

            return new ResultImpl<CreditCard>(new NodeWrapper(creditCardXML), gateway);
        }
    }
}
