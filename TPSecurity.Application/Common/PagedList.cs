namespace TPSecurity.Application.Common
{
    public class PagedList<T> : List<T>
    {
        public int TotalCount { get; private set; }

        public PagedList(List<T> items, int count)
        {
            TotalCount = count;
            AddRange(items);
        }
        public static PagedList<T> ToPagedList(IEnumerable<T> items, int totalCount)
        {            
            return new PagedList<T>(items.ToList(),
                                    count: totalCount);
        }

        public static void ApplyPagination(ref IQueryable<T> source, int offSet, int limit, out int totalCount)
        {
            totalCount = source.Count();
            source = source.Skip(offSet);
            if (limit > 0)
                source = source.Take(limit);            
        }
    }
}
