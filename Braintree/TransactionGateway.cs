#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Xml;
using Braintree.Exceptions;

namespace Braintree
{
    /// <summary>
    /// Provides operations for sales, credits, refunds, voids, submitting for settlement, and searching for transactions in the vault
    /// </summary>
    public class TransactionGateway
    {
        private BraintreeService Service;
        private BraintreeGateway Gateway;

        protected internal TransactionGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            Gateway = gateway;
            Service = new BraintreeService(gateway.Configuration);
        }

        [Obsolete("Use gateway.TransparentRedirect.Url")]
        public virtual string TransparentRedirectURLForCreate()
        {
            return Service.BaseMerchantURL() + "/transactions/all/create_via_transparent_redirect_request";
        }

        public virtual Result<Transaction> CancelRelease(string id)
        {
            var request = new TransactionRequest();

            XmlNode response = Service.Put(Service.MerchantPath() + "/transactions/" + id + "/cancel_release", request);

            return new ResultImpl<Transaction>(new NodeWrapper(response), Gateway);
        }

        [Obsolete("Use gateway.TransparentRedirect.Confirm()")]
        public virtual Result<Transaction> ConfirmTransparentRedirect(string queryString)
        {
            var trRequest = new TransparentRedirectRequest(queryString, Service);
            XmlNode node = Service.Post(Service.MerchantPath() + "/transactions/all/confirm_transparent_redirect_request", trRequest);

            return new ResultImpl<Transaction>(new NodeWrapper(node), Gateway);
        }

        public virtual Result<Transaction> HoldInEscrow(string id)
        {
            var request = new TransactionRequest();

            XmlNode response = Service.Put(Service.MerchantPath() + "/transactions/" + id + "/hold_in_escrow", request);

            return new ResultImpl<Transaction>(new NodeWrapper(response), Gateway);
        }

        public virtual string SaleTrData(TransactionRequest trData, string redirectURL)
        {
            trData.Type = TransactionType.SALE;

            return TrUtil.BuildTrData(trData, redirectURL, Service);
        }

        public virtual string CreditTrData(TransactionRequest trData, string redirectURL)
        {
            trData.Type = TransactionType.CREDIT;

            return TrUtil.BuildTrData(trData, redirectURL, Service);
        }

        public virtual Result<Transaction> Credit(TransactionRequest request)
        {
            request.Type = TransactionType.CREDIT;
            XmlNode response = Service.Post(Service.MerchantPath() + "/transactions", request);

            return new ResultImpl<Transaction>(new NodeWrapper(response), Gateway);
        }

        public virtual Transaction Find(string id)
        {
            if(id == null || id.Trim().Equals(""))
                throw new NotFoundException();

            XmlNode response = Service.Get(Service.MerchantPath() + "/transactions/" + id);

            return new Transaction(new NodeWrapper(response), Gateway);
        }

        public virtual Result<Transaction> Refund(string id)
        {
            XmlNode response = Service.Post(Service.MerchantPath() + "/transactions/" + id + "/refund");
            return new ResultImpl<Transaction>(new NodeWrapper(response), Gateway);
        }

        public virtual Result<Transaction> Refund(string id, decimal amount)
        {
            var request = new TransactionRequest
            {
                Amount = amount
            };
            XmlNode response = Service.Post(Service.MerchantPath() + "/transactions/" + id + "/refund", request);
            return new ResultImpl<Transaction>(new NodeWrapper(response), Gateway);
        }

        public virtual Result<Transaction> Sale(TransactionRequest request)
        {
            request.Type = TransactionType.SALE;
            XmlNode response = Service.Post(Service.MerchantPath() + "/transactions", request);

            return new ResultImpl<Transaction>(new NodeWrapper(response), Gateway);
        }

        public virtual Result<Transaction> ReleaseFromEscrow(string id)
        {
            var request = new TransactionRequest();

            XmlNode response = Service.Put(Service.MerchantPath() + "/transactions/" + id + "/release_from_escrow", request);

            return new ResultImpl<Transaction>(new NodeWrapper(response), Gateway);
        }

        public virtual Result<Transaction> SubmitForSettlement(string id)
        {
            return SubmitForSettlement(id, 0);
        }

        public virtual Result<Transaction> SubmitForSettlement(string id, decimal amount)
        {
            var request = new TransactionRequest();
            request.Amount = amount;

            XmlNode response = Service.Put(Service.MerchantPath() + "/transactions/" + id + "/submit_for_settlement", request);

            return new ResultImpl<Transaction>(new NodeWrapper(response), Gateway);
        }

        public virtual Result<Transaction> Void(string id)
        {
            XmlNode response = Service.Put(Service.MerchantPath() + "/transactions/" + id + "/void");

            return new ResultImpl<Transaction>(new NodeWrapper(response), Gateway);
        }

        public virtual ResourceCollection<Transaction> Search(TransactionSearchRequest query)
        {
            var response = new NodeWrapper(Service.Post(Service.MerchantPath() + "/transactions/advanced_search_ids", query));

            if (response.GetName() == "search-results") {
                return new ResourceCollection<Transaction>(response, delegate(string[] ids) {
                    return FetchTransactions(query, ids);
                });
            } else {
                throw new DownForMaintenanceException();
            }
        }

        public virtual Result<Transaction> CloneTransaction(string id, TransactionCloneRequest cloneRequest)
        {
            XmlNode response = Service.Post(Service.MerchantPath() + "/transactions/" + id + "/clone", cloneRequest);

            return new ResultImpl<Transaction>(new NodeWrapper(response), Gateway);
        }

        private List<Transaction> FetchTransactions(TransactionSearchRequest query, string[] ids)
        {
            query.Ids.IncludedIn(ids);

            var response = new NodeWrapper(Service.Post(Service.MerchantPath() + "/transactions/advanced_search", query));

            var transactions = new List<Transaction>();
            foreach (var node in response.GetList("transaction"))
            {
                transactions.Add(new Transaction(node, Gateway));
            }
            return transactions;
        }
    }
}
