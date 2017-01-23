using System.Collections.Generic;

namespace Braintree
{
    public class PaginatedResult<T> where T : class
    {
        public int TotalItems { get; private set; }
        public int PageSize { get; private set; }
        public List<T> Items { get; private set; }

        public PaginatedResult(int totalItems, int pageSize, List<T> items)
        {
            TotalItems = totalItems;
            PageSize = pageSize;
            Items = items;
        }
    }
}
