#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Xml;

namespace Braintree
{
    /// <summary>
    /// Provides operations for sales, credits, refunds, voids, submitting for settlement, and searching for transactions in the vault
    /// </summary>
    public class TransactionGateway
    {
        public virtual String TransparentRedirectURLForCreate()
        {
            return Configuration.BaseMerchantURL() + "/transactions/all/create_via_transparent_redirect_request";
        }

        public virtual Result<Transaction> ConfirmTransparentRedirect(String queryString)
        {
            TransparentRedirectRequest trRequest = new TransparentRedirectRequest(queryString);
            XmlNode node = WebServiceGateway.Post("/transactions/all/confirm_transparent_redirect_request", trRequest);

            return new Result<Transaction>(new NodeWrapper(node));
        }

        public virtual String SaleTrData(TransactionRequest trData, String redirectURL)
        {
            trData.Type = TransactionType.SALE;

            return TrUtil.BuildTrData(trData, redirectURL);
        }

        public virtual String CreditTrData(TransactionRequest trData, String redirectURL)
        {
            trData.Type = TransactionType.CREDIT;

            return TrUtil.BuildTrData(trData, redirectURL);
        }

        public virtual Result<Transaction> Credit(TransactionRequest request)
        {
            request.Type = TransactionType.CREDIT;
            XmlNode response = WebServiceGateway.Post("/transactions", request);

            return new Result<Transaction>(new NodeWrapper(response));
        }

        public virtual Transaction Find(String id)
        {
            XmlNode response = WebServiceGateway.Get("/transactions/" + id);

            return new Transaction(new NodeWrapper(response));
        }

        public virtual Result<Transaction> Refund(String id)
        {
            XmlNode response = WebServiceGateway.Post("/transactions/" + id + "/refund");
            return new Result<Transaction>(new NodeWrapper(response));
        }

        public virtual Result<Transaction> Refund(String id, Decimal amount)
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = amount
            };
            XmlNode response = WebServiceGateway.Post("/transactions/" + id + "/refund", request);
            return new Result<Transaction>(new NodeWrapper(response));
        }

        public virtual Result<Transaction> Sale(TransactionRequest request)
        {
            request.Type = TransactionType.SALE;
            XmlNode response = WebServiceGateway.Post("/transactions", request);

            return new Result<Transaction>(new NodeWrapper(response));
        }

        public virtual Result<Transaction> SubmitForSettlement(String id)
        {
            return SubmitForSettlement(id, 0);
        }

        public virtual Result<Transaction> SubmitForSettlement(String id, Decimal amount)
        {
            TransactionRequest request = new TransactionRequest();
			request.Amount = amount;
			
            XmlNode response = WebServiceGateway.Put("/transactions/" + id + "/submit_for_settlement", request);

            return new Result<Transaction>(new NodeWrapper(response));
        }

        public virtual Result<Transaction> Void(String id)
        {
            XmlNode response = WebServiceGateway.Put("/transactions/" + id + "/void");

            return new Result<Transaction>(new NodeWrapper(response));
        }

        public virtual ResourceCollection<Transaction> Search(TransactionSearchRequest query)
        {
            NodeWrapper response = new NodeWrapper(WebServiceGateway.Post("/transactions/advanced_search_ids", query));

            return new ResourceCollection<Transaction>(response, delegate(String[] ids) {
                return FetchTransactions(query, ids);
            });
        }

        private List<Transaction> FetchTransactions(TransactionSearchRequest query, String[] ids)
        {
            query.Ids.IncludedIn(ids);

            NodeWrapper response = new NodeWrapper(WebServiceGateway.Post("/transactions/advanced_search", query));

            List<Transaction> transactions = new List<Transaction>();
            foreach (NodeWrapper node in response.GetList("transaction"))
            {
                transactions.Add(new Transaction(node));
            }
            return transactions;
        }
    }
}
