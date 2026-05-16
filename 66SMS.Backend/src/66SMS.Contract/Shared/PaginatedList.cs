namespace _66SMS.Contracts.Shared
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class PaginatedList<TEntity> where TEntity : class
    {
        public IReadOnlyList<TEntity> Items { get; set; } = [];
        public long TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static PaginatedList<TEntity> Empty(int pageIndex, int pageSize) => new PaginatedList<TEntity>()
        {
            Items = [],
            TotalCount = 0,
            PageIndex = pageIndex,
            PageSize = pageSize
        };

    }
}
