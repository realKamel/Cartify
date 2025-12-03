using System.Linq.Expressions;
using System.Numerics;
using Cartify.Domain.Entities;

namespace Cartify.Domain.Interfaces;

public interface ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey> where TKey : INumber<TKey>
{
	Expression<Func<TEntity, bool>>? Criteria { get; }

	IList<Expression<Func<TEntity, object>>>? RelatedDataIncludes { get; }
	IList<string>? RelatedDataIncludesStringBased { get; }
	Expression<Func<TEntity, object>>? OrderByExpression { get; }
	Expression<Func<TEntity, object>>? OrderByDescExpression { get; }
	protected void AddRelatedDataInclude(Expression<Func<TEntity, object>> expression);
	protected void AddRelatedDataInclude(string property);
	protected void AddOrderBy(Expression<Func<TEntity, object>> expression);
	protected void AddOrderByDesc(Expression<Func<TEntity, object>> expression);
	int Skip { get; }
	int Take { get; }
	bool IsPaginated { get; }

}