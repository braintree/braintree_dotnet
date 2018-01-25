#pragma warning disable 1591

using Braintree.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace Braintree
{
    /// <summary>
    /// Provides operations for interacting with transaction line items
    /// </summary>
    public class TransactionLineItemGateway : ITransactionLineItemGateway
    {
        private readonly BraintreeService service;
        private readonly IBraintreeGateway gateway;

        protected internal TransactionLineItemGateway(IBraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            this.gateway = gateway;
            service = new BraintreeService(gateway.Configuration);
        }

        public virtual List<TransactionLineItem> FindAll(string id)
        {
            if (id == null || id.Trim().Equals(""))
                throw new NotFoundException();

            var response = new NodeWrapper(service.Get(service.MerchantPath() + "/transactions/" + id + "/line_items"));

            if (response.GetName() == "line-items")
            {
                var transactionLineItems = new List<TransactionLineItem>();
                foreach (var node in response.GetList("line-item"))
                {
                    transactionLineItems.Add(new TransactionLineItem(node));
                }
                return transactionLineItems;
            }
            else
            {
                throw new DownForMaintenanceException();
            }
        }
    }
}
