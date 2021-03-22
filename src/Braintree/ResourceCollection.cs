#pragma warning disable 1591

using System.Collections.Generic;

namespace Braintree
{
    public class ResourceCollection<T> : IEnumerable<T> where T : class
    {
        public delegate List<T> PagingDelegate(string[] ids);

        public int MaximumCount => Ids.Count;
        private int PageSize;
        public List<string> Ids { get; private set; }
        private PagingDelegate NextPage;
        public T FirstItem {
            get {
                return NextPage(new string[] { Ids[0] })[0];
            }
        }

        public ResourceCollection(NodeWrapper response, PagingDelegate nextPage)
        {
            NextPage = nextPage;
            Ids = response.GetStrings("ids/*");
            PageSize = int.Parse(response.GetString("page-size"));
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return (this as IEnumerable<T>).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            foreach (var batchIds in BatchIds(Ids, PageSize))
            {
                List<T> items = NextPage(batchIds.ToArray());
                foreach (T item in items)
                {
                    yield return item;
                }
            }
        }

        private List<List<string>> BatchIds(List<string> ids, int size)
        {
            var batches = new List<List<string>>();

            for (int index = 0; index < ids.Count; index += size) {
                int count = size;
                if (index + count > ids.Count)
                {
                    count = ids.Count - index;
                }
                batches.Add(ids.GetRange(index, count));
            }

            return batches;
        }
    }
}
