#pragma warning disable 1591

using Braintree.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace Braintree
{
    /// <summary>
    /// Provides operations for finding verifications
    /// </summary>
    public class UsBankAccountVerificationGateway : IUsBankAccountVerificationGateway
    {
        private readonly BraintreeService service;
        private readonly BraintreeGateway gateway;

        protected internal UsBankAccountVerificationGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            this.gateway = gateway;
            service = gateway.Service;
        }

        public virtual Result<UsBankAccountVerification> ConfirmMicroTransferAmounts(string Id, UsBankAccountVerificationConfirmRequest request)
        {
            var response = new NodeWrapper(service.Put(service.MerchantPath() + "/us_bank_account_verifications/" + Id + "/confirm_micro_transfer_amounts", request));
            return new ResultImpl<UsBankAccountVerification>(response, gateway);
        }

        public virtual UsBankAccountVerification Find(string Id)
        {
            if (Id == null || Id.Trim().Equals(""))
            {
                throw new NotFoundException();
            }

            XmlNode verificationXML = service.Get(service.MerchantPath() + "/us_bank_account_verifications/" + Id);

            return new UsBankAccountVerification(new NodeWrapper(verificationXML));
        }

        public virtual ResourceCollection<UsBankAccountVerification> Search(UsBankAccountVerificationSearchRequest query)
        {
            var response = new NodeWrapper(service.Post(service.MerchantPath() + "/us_bank_account_verifications/advanced_search_ids", query));

            return new ResourceCollection<UsBankAccountVerification>(response,
                ids => FetchUsBankAccountVerifications(query, ids));
        }

        public virtual async Task<ResourceCollection<UsBankAccountVerification>> SearchAsync(UsBankAccountVerificationSearchRequest query)
        {
            var response = new NodeWrapper(await service.PostAsync(service.MerchantPath() + "/us_bank_account_verifications/advanced_search_ids", query).ConfigureAwait(false));

            return new ResourceCollection<UsBankAccountVerification>(response,
                ids => FetchUsBankAccountVerifications(query, ids));
        }

        private List<UsBankAccountVerification> FetchUsBankAccountVerifications(UsBankAccountVerificationSearchRequest query, string[] ids)
        {
            query.Ids.IncludedIn(ids);

            var response = new NodeWrapper(service.Post(service.MerchantPath() + "/us_bank_account_verifications/advanced_search", query));

            var verifications = new List<UsBankAccountVerification>();
            foreach (var node in response.GetList("us-bank-account-verification"))
            {
                verifications.Add(new UsBankAccountVerification(node));
            }
            return verifications;
        }
    }
}
