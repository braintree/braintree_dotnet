using System.Collections.Generic;

namespace Braintree
{
    public class PaginatedCollection<T> : IEnumerable<T> where T : class
    {
        public int Size { get { return TotalItems; } }

        private int PageSize = 0;
        private int CurrentPage = 0;
        private int Index = 0;
        private int TotalItems = 0;
        private List<T> Items = new List<T>();
        private System.Func<int, PaginatedResult<T>> Pager;

        public PaginatedCollection(System.Func<int, PaginatedResult<T>> pager)
        {
            Pager = pager;
        }

        public IEnumerator<T> GetEnumerator()
        {
            do
            {
                if (CurrentPage == 0 || Index % PageSize == 0)
                {
                    CurrentPage++;
                    var results = Pager(CurrentPage);
                    TotalItems = results.TotalItems;
                    PageSize = results.PageSize;
                    Items = results.Items;
                }

                foreach (T item in Items)
                {
                    Index++;
                    yield return item;
                }
            } while(Index < TotalItems);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
