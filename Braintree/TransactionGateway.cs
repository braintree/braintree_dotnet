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

        protected internal TransactionGateway(BraintreeService service)
        {
            Service = service;
        }

        [Obsolete("Use gateway.TransparentRedirect.Url")]
        public virtual String TransparentRedirectURLForCreate()
        {
            return Service.BaseMerchantURL() + "/transactions/all/create_via_transparent_redirect_request";
        }

        public virtual Result<Transaction> CancelRelease(String id)
        {
            TransactionRequest request = new TransactionRequest();

            XmlNode response = Service.Put(Service.MerchantPath() + "/transactions/" + id + "/cancel_release", request);

            return new ResultImpl<Transaction>(new NodeWrapper(response), Service);
        }

        [Obsolete("Use gateway.TransparentRedirect.Confirm()")]
        public virtual Result<Transaction> ConfirmTransparentRedirect(String queryString)
        {
            TransparentRedirectRequest trRequest = new TransparentRedirectRequest(queryString, Service);
            XmlNode node = Service.Post(Service.MerchantPath() + "/transactions/all/confirm_transparent_redirect_request", trRequest);

            return new ResultImpl<Transaction>(new NodeWrapper(node), Service);
        }

        public virtual Result<Transaction> HoldInEscrow(String id)
        {
            TransactionRequest request = new TransactionRequest();

            XmlNode response = Service.Put(Service.MerchantPath() + "/transactions/" + id + "/hold_in_escrow", request);

            return new ResultImpl<Transaction>(new NodeWrapper(response), Service);
        }

        public virtual String SaleTrData(TransactionRequest trData, String redirectURL)
        {
            trData.Type = TransactionType.SALE;

            return TrUtil.BuildTrData(trData, redirectURL, Service);
        }

        public virtual String CreditTrData(TransactionRequest trData, String redirectURL)
        {
            trData.Type = TransactionType.CREDIT;

            return TrUtil.BuildTrData(trData, redirectURL, Service);
        }

        public virtual Result<Transaction> Credit(TransactionRequest request)
        {
            request.Type = TransactionType.CREDIT;
            XmlNode response = Service.Post(Service.MerchantPath() + "/transactions", request);

            return new ResultImpl<Transaction>(new NodeWrapper(response), Service);
        }

        public virtual Transaction Find(String id)
        {
            if(id == null || id.Trim().Equals(""))
                throw new NotFoundException();

            XmlNode response = Service.Get(Service.MerchantPath() + "/transactions/" + id);

            return new Transaction(new NodeWrapper(response), Service);
        }

        public virtual Result<Transaction> Refund(String id)
        {
            XmlNode response = Service.Post(Service.MerchantPath() + "/transactions/" + id + "/refund");
            return new ResultImpl<Transaction>(new NodeWrapper(response), Service);
        }

        public virtual Result<Transaction> Refund(String id, Decimal amount)
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = amount
            };
            XmlNode response = Service.Post(Service.MerchantPath() + "/transactions/" + id + "/refund", request);
            return new ResultImpl<Transaction>(new NodeWrapper(response), Service);
        }

        public virtual Result<Transaction> Sale(TransactionRequest request)
        {
            request.Type = TransactionType.SALE;
            XmlNode response = Service.Post(Service.MerchantPath() + "/transactions", request);

            return new ResultImpl<Transaction>(new NodeWrapper(response), Service);
        }

        public virtual Result<Transaction> ReleaseFromEscrow(String id)
        {
            TransactionRequest request = new TransactionRequest();

            XmlNode response = Service.Put(Service.MerchantPath() + "/transactions/" + id + "/release_from_escrow", request);

            return new ResultImpl<Transaction>(new NodeWrapper(response), Service);
        }

        public virtual Result<Transaction> SubmitForSettlement(String id)
        {
            return SubmitForSettlement(id, 0);
        }

        public virtual Result<Transaction> SubmitForSettlement(String id, Decimal amount)
        {
            TransactionRequest request = new TransactionRequest();
            request.Amount = amount;

            XmlNode response = Service.Put(Service.MerchantPath() + "/transactions/" + id + "/submit_for_settlement", request);

            return new ResultImpl<Transaction>(new NodeWrapper(response), Service);
        }

        public virtual Result<Transaction> Void(String id)
        {
            XmlNode response = Service.Put(Service.MerchantPath() + "/transactions/" + id + "/void");

            return new ResultImpl<Transaction>(new NodeWrapper(response), Service);
        }

        public virtual ResourceCollection<Transaction> Search(TransactionSearchRequest query)
        {
            NodeWrapper response = new NodeWrapper(Service.Post(Service.MerchantPath() + "/transactions/advanced_search_ids", query));

            if (response.GetName() == "search-results") {
                return new ResourceCollection<Transaction>(response, delegate(String[] ids) {
                    return FetchTransactions(query, ids);
                });
            } else {
                throw new DownForMaintenanceException();
            }
        }

        public virtual Result<Transaction> CloneTransaction(String id, TransactionCloneRequest cloneRequest)
        {
            XmlNode response = Service.Post(Service.MerchantPath() + "/transactions/" + id + "/clone", cloneRequest);

            return new ResultImpl<Transaction>(new NodeWrapper(response), Service);
        }

        private List<Transaction> FetchTransactions(TransactionSearchRequest query, String[] ids)
        {
            query.Ids.IncludedIn(ids);

            NodeWrapper response = new NodeWrapper(Service.Post(Service.MerchantPath() + "/transactions/advanced_search", query));

            List<Transaction> transactions = new List<Transaction>();
            foreach (NodeWrapper node in response.GetList("transaction"))
            {
                transactions.Add(new Transaction(node, Service));
            }
            return transactions;
        }
    }
}
