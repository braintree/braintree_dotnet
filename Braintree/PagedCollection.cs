#pragma warning disable 1591

using System;
using System.Collections.Generic;

namespace Braintree
{
    public class PagedCollection<T> : System.Collections.IEnumerable
    {
        public delegate PagedCollection<T> PagingDelegate();

        public Int32 CurrentPageNumber { get; protected set; }
        public Int32 PageSize { get; protected set; }
        public Int32 TotalItems { get; protected set; }
        public List<T> Items { get; protected set; }
        private PagingDelegate NextPage;

        public PagedCollection(List<T> items, int currentPageNumber, int totalItems, int pageSize, PagingDelegate nextPage)
        {
            Items = items;
            CurrentPageNumber = currentPageNumber;
            TotalItems = totalItems;
            PageSize = pageSize;
            NextPage = nextPage;
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            PagedCollection<T> page = this;
            while (page.Items.Count > 0)
            {
                foreach(T item in page.Items)
                {
                    yield return item;
                }
                page = page.GetNextPage();
            }
        }

        public virtual PagedCollection<T> GetNextPage()
        {
            return NextPage();
        }

        public virtual Boolean IsLastPage()
        {
            return TotalPages() == CurrentPageNumber;
        }

        public virtual Int32 TotalPages()
        {
            if (TotalItems == 0)
            {
                return 1;
            }
            
            var totalPages = TotalItems / PageSize;
            if (TotalItems % PageSize != 0)
            {
                totalPages += 1;
            }

            return totalPages;
        }
    }
}
