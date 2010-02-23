using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class PagedCollection
    {
        public Int32 CurrentPageNumber { get; protected set; }
        public Int32 PageSize { get; protected set; }
        public String Query { get; protected set; }
        public Int32 TotalItems { get; protected set; }
        public List<Transaction> Transactions { get; protected set; }

        public PagedCollection(String query, List<Transaction> transactions, int currentPageNumber, int totalItems, int pageSize)
        {
            Query = query;
            Transactions = transactions;
            CurrentPageNumber = currentPageNumber;
            TotalItems = totalItems;
            PageSize = pageSize;
        }

        public PagedCollection GetNextPage()
        {
            return new TransactionGateway().Search(Query, CurrentPageNumber + 1);
        }
    }
}
