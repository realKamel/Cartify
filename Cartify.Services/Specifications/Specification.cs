using System.Linq.Expressions;
using System.Numerics;
using Cartify.Domain.Entities;
using Cartify.Domain.Interfaces;

namespace Cartify.Services.Specifications;

public abstract class Specification<TEntity, TKey> : ISpecification<TEntity, TKey>
    where TEntity : BaseEntity<TKey> where TKey : INumber<TKey>
{
    public Expression<Func<TEntity, bool>>? Criteria { get; private set; }
    public ICollection<Expression<Func<TEntity, object>>>? RelatedDataIncludes { get; private set; }
    public Expression<Func<TEntity, object>>? OrderByExpression { get; private set; }
    public Expression<Func<TEntity, object>>? OrderByDescExpression { get; private set; }


    protected Specification(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }

    public void AddRelatedDataInclude(Expression<Func<TEntity, object>> expression)
    {
        RelatedDataIncludes ??= new List<Expression<Func<TEntity, object>>>();
        RelatedDataIncludes.Add(expression);
    }

    public void AddOrderBy(Expression<Func<TEntity, object>> expression) => OrderByExpression ??= expression;


    public void AddOrderByDesc(Expression<Func<TEntity, object>> expression) => OrderByDescExpression ??= expression;

    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool IsPaginated { get; set; }

    protected void ApplyPagination(int pageIndex, int pageSize)
    {
        IsPaginated = true;
        Take = pageSize;
        Skip = (pageIndex - 1) * pageSize;
    }
}