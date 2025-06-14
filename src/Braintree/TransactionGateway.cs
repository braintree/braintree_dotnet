#pragma warning disable 1591

using Braintree.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace Braintree
{
    /// <summary>
    /// Provides operations for sales, credits, refunds, voids, submitting for settlement, and searching for transactions in the vault
    /// </summary>
    public class TransactionGateway : ITransactionGateway
    {
        private readonly BraintreeService service;
        private readonly IBraintreeGateway gateway;

        protected internal TransactionGateway(IBraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            this.gateway = gateway;
            service = gateway.Service;
        }

        public virtual Result<Transaction> AdjustAuthorization(string id, decimal amount)
        {
            var request = new TransactionRequest
            {
                Amount = amount
            };
            XmlNode response = service.Put(service.MerchantPath() + "/transactions/" + id + "/adjust_authorization", request);

            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual async Task<Result<Transaction>> AdjustAuthorizationAsync(string id, decimal amount)
        {
            var request = new TransactionRequest
            {
                Amount = amount
            };
            XmlNode response = await service.PutAsync(service.MerchantPath() + "/transactions/" + id + "/adjust_authorization", request).ConfigureAwait(false);

            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual Result<Transaction> CancelRelease(string id)
        {
            var request = new TransactionRequest();

            XmlNode response = service.Put(service.MerchantPath() + "/transactions/" + id + "/cancel_release", request);

            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual async Task<Result<Transaction>> CancelReleaseAsync(string id)
        {
            var request = new TransactionRequest();

            XmlNode response = await service.PutAsync(service.MerchantPath() + "/transactions/" + id + "/cancel_release", request).ConfigureAwait(false);

            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual Result<Transaction> Credit(TransactionRequest request)
        {
            request.Type = TransactionType.CREDIT;
            XmlNode response = service.Post(service.MerchantPath() + "/transactions", request);

            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual async Task<Result<Transaction>> CreditAsync(TransactionRequest request)
        {
            request.Type = TransactionType.CREDIT;
            XmlNode response = await service.PostAsync(service.MerchantPath() + "/transactions", request).ConfigureAwait(false);

            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual Transaction Find(string id)
        {
            if(id == null || id.Trim().Equals(""))
                throw new NotFoundException();

            XmlNode response = service.Get(service.MerchantPath() + "/transactions/" + id);

            return new Transaction(new NodeWrapper(response), gateway);
        }

        public virtual async Task<Transaction> FindAsync(string id)
        {
            if(id == null || id.Trim().Equals(""))
                throw new NotFoundException();

            XmlNode response = await service.GetAsync(service.MerchantPath() + "/transactions/" + id).ConfigureAwait(false);

            return new Transaction(new NodeWrapper(response), gateway);
        }

        public virtual Result<Transaction> Refund(string id)
        {
            XmlNode response = service.Post(service.MerchantPath() + "/transactions/" + id + "/refund");
            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual async Task<Result<Transaction>> RefundAsync(string id)
        {
            XmlNode response = await service.PostAsync(service.MerchantPath() + "/transactions/" + id + "/refund").ConfigureAwait(false);
            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual Result<Transaction> Refund(string id, decimal amount)
        {
            var request = new TransactionRefundRequest
            {
                Amount = amount
            };

            return Refund(id, request);
        }

        public virtual Task<Result<Transaction>> RefundAsync(string id, decimal amount)
        {
            var request = new TransactionRefundRequest
            {
                Amount = amount
            };

            return RefundAsync(id, request);
        }

        public virtual Result<Transaction> Refund(string id, TransactionRefundRequest refundRequest)
        {
            XmlNode response = service.Post(service.MerchantPath() + "/transactions/" + id + "/refund", refundRequest);
            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual async Task<Result<Transaction>> RefundAsync(string id, TransactionRefundRequest refundRequest)
        {
            XmlNode response = await service.PostAsync(service.MerchantPath() + "/transactions/" + id + "/refund", refundRequest).ConfigureAwait(false);
            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual Result<Transaction> Sale(TransactionRequest request)
        {
            request.Type = TransactionType.SALE;
            XmlNode response = service.Post(service.MerchantPath() + "/transactions", request);

            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual async Task<Result<Transaction>> SaleAsync(TransactionRequest request)
        {
            request.Type = TransactionType.SALE;
            XmlNode response = await service.PostAsync(service.MerchantPath() + "/transactions", request).ConfigureAwait(false);

            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual Result<Transaction> SubmitForSettlement(string id)
        {
            return SubmitForSettlement(id, 0);
        }

        public virtual Task<Result<Transaction>> SubmitForSettlementAsync(string id)
        {
            return SubmitForSettlementAsync(id, 0);
        }

        public virtual Result<Transaction> SubmitForSettlement(string id, decimal amount)
        {
            var request = new TransactionRequest
            {
                Amount = amount
            };

            return SubmitForSettlement(id, request);
        }

        public virtual async Task<Result<Transaction>> SubmitForSettlementAsync(string id, decimal amount)
        {
            var request = new TransactionRequest
            {
                Amount = amount
            };

            return await SubmitForSettlementAsync(id, request);
        }

        public virtual Result<Transaction> SubmitForSettlement(string id, TransactionRequest request)
        {
            XmlNode response = service.Put(service.MerchantPath() + "/transactions/" + id + "/submit_for_settlement", request);

            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual async Task<Result<Transaction>> SubmitForSettlementAsync(string id, TransactionRequest request)
        {
            XmlNode response = await service.PutAsync(service.MerchantPath() + "/transactions/" + id + "/submit_for_settlement", request).ConfigureAwait(false);
            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual Result<Transaction> UpdateDetails(string id, TransactionRequest request)
        {
            XmlNode response = service.Put(service.MerchantPath() + "/transactions/" + id + "/update_details", request);

            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual Result<Transaction> PackageTracking(string id, PackageTrackingRequest request)
        {
            XmlNode response = service.Post(service.MerchantPath() + "/transactions/" + id + "/shipments", request);

            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual async Task<Result<Transaction>> PackageTrackingAsync(string id, PackageTrackingRequest request)
        {
            XmlNode response = await service.PostAsync(service.MerchantPath() + "/transactions/" + id + "/shipments", request).ConfigureAwait(false);;

            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual Result<Transaction> SubmitForPartialSettlement(string id, decimal amount)
        {
            var request = new TransactionRequest();
            request.Amount = amount;

            return SubmitForPartialSettlement(id, request);
        }

        public virtual Result<Transaction> SubmitForPartialSettlement(string id, TransactionRequest request)
        {
            XmlNode response = service.Post(service.MerchantPath() + "/transactions/" + id + "/submit_for_partial_settlement", request);

            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual Result<Transaction> Void(string id)
        {
            XmlNode response = service.Put(service.MerchantPath() + "/transactions/" + id + "/void");

            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual async Task<Result<Transaction>> VoidAsync(string id)
        {
            XmlNode response = await service.PutAsync(service.MerchantPath() + "/transactions/" + id + "/void").ConfigureAwait(false);

            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual ResourceCollection<Transaction> Search(TransactionSearchRequest query)
        {
            var response = new NodeWrapper(service.Post(service.MerchantPath() + "/transactions/advanced_search_ids", query));

            if (response.GetName() == "search-results")
            {
                return new ResourceCollection<Transaction>(response, ids => FetchTransactions(query, ids));
            }
            else
            {
                throw new UnexpectedException();
            }
        }

        public virtual async Task<ResourceCollection<Transaction>> SearchAsync(TransactionSearchRequest query)
        {
            var response = new NodeWrapper(await service.PostAsync(service.MerchantPath() + "/transactions/advanced_search_ids", query).ConfigureAwait(false));

            if (response.GetName() == "search-results")
            {
                return new ResourceCollection<Transaction>(response, ids => FetchTransactions(query, ids));
            }
            else
            {
                throw new UnexpectedException();
            }
        }

        public virtual Result<Transaction> CloneTransaction(string id, TransactionCloneRequest cloneRequest)
        {
            XmlNode response = service.Post(service.MerchantPath() + "/transactions/" + id + "/clone", cloneRequest);

            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        public virtual async Task<Result<Transaction>> CloneTransactionAsync(string id, TransactionCloneRequest cloneRequest)
        {
            XmlNode response = await service.PostAsync(service.MerchantPath() + "/transactions/" + id + "/clone", cloneRequest).ConfigureAwait(false);

            return new ResultImpl<Transaction>(new NodeWrapper(response), gateway);
        }

        private List<Transaction> FetchTransactions(TransactionSearchRequest query, string[] ids)
        {
            query.Ids.IncludedIn(ids);

            var response = new NodeWrapper(service.Post(service.MerchantPath() + "/transactions/advanced_search", query));

            if (response.GetName() == "credit-card-transactions")
            {
                var transactions = new List<Transaction>();
                foreach (var node in response.GetList("transaction"))
                {
                    transactions.Add(new Transaction(node, gateway));
                }
                return transactions;
            }
            else
            {
                throw new UnexpectedException();
            }
        }
    }
}
