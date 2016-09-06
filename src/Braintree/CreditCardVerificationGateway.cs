#pragma warning disable 1591

using Braintree.Exceptions;
using System.Collections.Generic;
using System.Xml;

namespace Braintree
{
    /// <summary>
    /// Provides operations for finding verifications
    /// </summary>
    public class CreditCardVerificationGateway : ICreditCardVerificationGateway
    {
        private readonly BraintreeService service;
        private readonly BraintreeGateway gateway;

        protected internal CreditCardVerificationGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            this.gateway = gateway;
            service = new BraintreeService(gateway.Configuration);
        }

        public virtual Result<CreditCardVerification> Create(CreditCardVerificationRequest request)
        {
            XmlNode response = service.Post(service.MerchantPath() + "/verifications", request);

            return new ResultImpl<CreditCardVerification>(new NodeWrapper(response), gateway);
        }

        public virtual CreditCardVerification Find(string Id)
        {
            if (Id == null || Id.Trim().Equals(""))
            {
                throw new NotFoundException();
            }

            XmlNode creditCardVerificationXML = service.Get(service.MerchantPath() + "/verifications/" + Id);

            return new CreditCardVerification(new NodeWrapper(creditCardVerificationXML), gateway);
        }

        public virtual ResourceCollection<CreditCardVerification> Search(CreditCardVerificationSearchRequest query)
        {
            var response = new NodeWrapper(service.Post(service.MerchantPath() + "/verifications/advanced_search_ids", query));

            return new ResourceCollection<CreditCardVerification>(response, delegate(string[] ids) {
                return FetchCreditCardVerifications(query, ids);
            });
        }

        private List<CreditCardVerification> FetchCreditCardVerifications(CreditCardVerificationSearchRequest query, string[] ids)
        {
            query.Ids.IncludedIn(ids);

            var response = new NodeWrapper(service.Post(service.MerchantPath() + "/verifications/advanced_search", query));

            var verifications = new List<CreditCardVerification>();
            foreach (var node in response.GetList("verification"))
            {
                verifications.Add(new CreditCardVerification(node, gateway));
            }
            return verifications;
        }
    }
}
