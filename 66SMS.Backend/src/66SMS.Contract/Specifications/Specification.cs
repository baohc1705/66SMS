namespace _66SMS.Contracts.Specifications
{
    /// <summary>
    /// Specification with optional manual Skip/Take.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Specification<TEntity> : SpecificationBase<TEntity> where TEntity : class
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}
