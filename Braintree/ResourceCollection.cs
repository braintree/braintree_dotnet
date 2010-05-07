#pragma warning disable 1591

using System;
using System.Collections.Generic;

namespace Braintree
{
    public class ResourceCollection<T> : System.Collections.IEnumerable where T : class
    {
        public delegate List<T> PagingDelegate(String[] ids);

        public Int32 MaximumCount {
            get
            {
                return Ids.Count;
            }
        }
        private Int32 PageSize;
        private List<String> Ids;
        private PagingDelegate NextPage;
        public T FirstItem {
            get {
                return NextPage(new String[] { Ids[0] })[0];
            }
        }

        public ResourceCollection(NodeWrapper response, PagingDelegate nextPage)
        {
            NextPage = nextPage;
            Ids = response.GetStrings("ids/*");
            PageSize = Int32.Parse(response.GetString("page-size"));
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            foreach(List<String> batchIds in BatchIds(Ids, PageSize))
            {
                List<T> items = NextPage(batchIds.ToArray());
                foreach(T item in items)
                {
                    yield return item;
                }
            }
        }

        private List<List<String>> BatchIds(List<String> ids, Int32 size)
        {
            List<List<String>> batches = new List<List<String>>();

            for (Int32 index = 0; index < ids.Count; index += size) {
                Int32 count = size;
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
