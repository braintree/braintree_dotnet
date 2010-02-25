using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Braintree
{
    public class TransactionGateway
    {
        public String TransparentRedirectURLForCreate()
        {
            return Configuration.BaseMerchantURL() + "/transactions/all/create_via_transparent_redirect_request";
        }

        public Result<Transaction> ConfirmTransparentRedirect(String queryString)
        {
            TransparentRedirectRequest trRequest = new TransparentRedirectRequest(queryString);
            XmlNode node = WebServiceGateway.Post("/transactions/all/confirm_transparent_redirect_request", trRequest);

            return new Result<Transaction>(new NodeWrapper(node));
        }

        public String SaleTrData(TransactionRequest trData, String redirectURL)
        {
            trData.Type = TransactionType.SALE;

            return TrUtil.BuildTrData(trData, redirectURL);
        }

        public String CreditTrData(TransactionRequest trData, String redirectURL)
        {
            trData.Type = TransactionType.CREDIT;

            return TrUtil.BuildTrData(trData, redirectURL);
        }

        public Result<Transaction> Credit(TransactionRequest request)
        {
            request.Type = TransactionType.CREDIT;
            XmlNode response = WebServiceGateway.Post("/transactions", request);

            return new Result<Transaction>(new NodeWrapper(response));
        }

        public Transaction Find(String id)
        {
            XmlNode response = WebServiceGateway.Get("/transactions/" + id);

            return new Transaction(new NodeWrapper(response));
        }

        public Result<Transaction> Refund(String id)
        {
            XmlNode response = WebServiceGateway.Post("/transactions/" + id + "/refund");
            return new Result<Transaction>(new NodeWrapper(response));
        }

        public Result<Transaction> Sale(TransactionRequest request)
        {
            request.Type = TransactionType.SALE;
            XmlNode response = WebServiceGateway.Post("/transactions", request);

            return new Result<Transaction>(new NodeWrapper(response));
        }

        public Result<Transaction> SubmitForSettlement(String id)
        {
            return SubmitForSettlement(id, 0);
        }

        public Result<Transaction> SubmitForSettlement(String id, Decimal amount)
        {
            TransactionRequest request = new TransactionRequest { Amount = amount };
            XmlNode response = WebServiceGateway.Put("/transactions/" + id + "/submit_for_settlement", request);

            return new Result<Transaction>(new NodeWrapper(response));
        }

        public Result<Transaction> Void(String id)
        {
            XmlNode response = WebServiceGateway.Put("/transactions/" + id + "/void");

            return new Result<Transaction>(new NodeWrapper(response));
        }

        public PagedCollection Search(String query)
        {
            return Search(query, 1);
        }

        public PagedCollection Search(String query, int pageNumber)
        {
            String queryString = new QueryString().Append("q", query).Append("page", pageNumber).ToString();
            NodeWrapper response = new NodeWrapper(WebServiceGateway.Get("/transactions/all/search?" + queryString));
            int currentPageNumber = (Int32)response.GetInteger("current-page-number");
            int pageSize = (Int32)response.GetInteger("page-size");
            int totalItems = (Int32)response.GetInteger("total-items");

            List<Transaction> transactions = new List<Transaction>();
            foreach (NodeWrapper transactionNode in response.GetList("transaction"))
            {
                transactions.Add(new Transaction(transactionNode));
            }

            return new PagedCollection(query, transactions, currentPageNumber, totalItems, pageSize);
        }
    }
}
