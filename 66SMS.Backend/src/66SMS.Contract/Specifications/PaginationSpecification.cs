namespace _66SMS.Contracts.Specifications
{
    /// <summary>
    /// Specification for 1-based page-number pagination.
    /// </summary>
    public class PaginationSpecification<TEntity> : SpecificationBase<TEntity> where TEntity : class
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
