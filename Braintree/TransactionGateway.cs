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

        public virtual PagedCollection<Transaction> Search(String query)
        {
            return Search(query, 1);
        }

        public virtual PagedCollection<Transaction> Search(String query, int pageNumber)
        {
            String queryString = new QueryString().Append("q", query).Append("page", pageNumber).ToString();
            NodeWrapper response = new NodeWrapper(WebServiceGateway.Get("/transactions/all/search?" + queryString));

            int totalItems = response.GetInteger("total-items").Value;

            List<Transaction> transactions = new List<Transaction>();
            foreach (NodeWrapper transactionNode in response.GetList("transaction"))
            {
                transactions.Add(new Transaction(transactionNode));
            }

            return new PagedCollection<Transaction>(transactions, totalItems, delegate() { return Search(query, pageNumber + 1); });
        }
    }
}
