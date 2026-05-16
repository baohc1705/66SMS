using System.Linq.Expressions;

namespace _66SMS.Contracts.Specifications
{
    /// <summary>
    /// Base query specification. Holds: conditions, eager-loads, ordering, and split-query hint.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class SpecificationBase<TEntity> where TEntity : class
    {
        /// <summary>
        /// Hỗ trợ nhiều điều kiện
        /// </summary>
        public List<Expression<Func<TEntity, bool>>> Conditions { get; set; } = [];

        /// <summary>
        /// Hỗ trợ include qua Expression
        /// </summary>
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = [];

        /// <summary>
        /// Hỗ trợ include với "Customer.Address"
        /// </summary>
        public List<string> IncludeString { get; set; } = [];

        /// <summary>
        /// Strongly-typed ordering.
        /// </summary>
        public Expression<Func<TEntity, object>>? OrderBy { get; set; }

        /// <summary>
        /// Strongly-typed ordering (descending).
        /// </summary>
        public Expression<Func<TEntity, object>>? OrderByDescending { get; set; }

        /// <summary>
        /// AsNoTracking override
        /// </summary>
        public bool? AsNoTracking { get; set; }
    }
}
