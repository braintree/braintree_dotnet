#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Xml;
using Braintree.Exceptions;

namespace Braintree
{
    /// <summary>
    /// Provides operations for finding verifications
    /// </summary>
    public class CreditCardVerificationGateway
    {
        private BraintreeService Service;
        private BraintreeGateway Gateway;

        protected internal CreditCardVerificationGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            Gateway = gateway;
            Service = new BraintreeService(gateway.Configuration);
        }

        public virtual CreditCardVerification Find(String Id)
        {
            if(Id == null || Id.Trim().Equals(""))
                throw new NotFoundException();

            XmlNode creditCardVerificationXML = Service.Get(Service.MerchantPath() + "/verifications/" + Id);

            return new CreditCardVerification(new NodeWrapper(creditCardVerificationXML), Gateway);
        }

        public virtual ResourceCollection<CreditCardVerification> Search(CreditCardVerificationSearchRequest query)
        {
            NodeWrapper response = new NodeWrapper(Service.Post(Service.MerchantPath() + "/verifications/advanced_search_ids", query));

            return new ResourceCollection<CreditCardVerification>(response, delegate(String[] ids) {
                return FetchCreditCardVerifications(query, ids);
            });
        }

        private List<CreditCardVerification> FetchCreditCardVerifications(CreditCardVerificationSearchRequest query, String[] ids)
        {
            query.Ids.IncludedIn(ids);

            NodeWrapper response = new NodeWrapper(Service.Post(Service.MerchantPath() + "/verifications/advanced_search", query));

            List<CreditCardVerification> verifications = new List<CreditCardVerification>();
            foreach (NodeWrapper node in response.GetList("verification"))
            {
                verifications.Add(new CreditCardVerification(node, Gateway));
            }
            return verifications;
        }
    }
}
