using _66SMS.Contracts.Specifications;
using Microsoft.EntityFrameworkCore;

namespace _66SMS.Persistence.Specifications
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> query, Specification<TEntity> spec) where TEntity : class
        {
            query = ApplyBase(query, spec);
            if (spec.Skip > 0) query = query.Skip(spec.Skip.Value);
            if (spec.Take > 0) query = query.Take(spec.Take.Value);

            return query;
        }

        public static IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> query, PaginationSpecification<TEntity> spec) where TEntity : class
        {
            query = ApplyBase(query, spec);

            int skip = (spec.PageIndex - 1) * spec.PageSize;
            query = query.Skip(skip).Take(spec.PageSize);

            return query;
        }

        private static IQueryable<TEntity> ApplyBase<TEntity>(IQueryable<TEntity> query, SpecificationBase<TEntity> spec) where TEntity : class
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            if (spec == null) throw new ArgumentNullException(nameof(spec));

            // Tracking
            if (spec.AsNoTracking == true)
                query = query.AsNoTracking();

            // Filter
            foreach (var condition in spec.Conditions)
            {
                query = query.Where(condition);
            }

            // Include (Expression)
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            // String-based include
            foreach (var include in spec.IncludeString)
                query = query.Include(include);

            // Order By
            if (spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy);
            else if (spec.OrderByDescending != null)
                query = query.OrderByDescending(spec.OrderByDescending);

            return query;
        }
    }
}
