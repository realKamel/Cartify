using System.Numerics;
using System.Runtime.InteropServices;
using Cartify.Domain.Entities;
using Cartify.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cartify.Persistence;

public static class QueryBuilder
{
	public static IQueryable<TEntity> CreateSpecificationQuery<TEntity, TKey>(IQueryable<TEntity> initQuery,
		ISpecification<TEntity, TKey> specification) where TEntity : BaseEntity<TKey> where TKey : INumber<TKey>
	{
		var query = initQuery;

		if (specification.Criteria is not null)
		{
			query = query.Where(specification.Criteria);
		}

		//TODO this must be update
		if (specification.RelatedDataIncludes is not null && specification.RelatedDataIncludes.Any())

		{
			query = specification.RelatedDataIncludes
				.Aggregate(query, (currentQuery, item) =>
					currentQuery.Include(item));
		}
		else
		{
			query = specification
				.RelatedDataIncludesStringBased?
				.Aggregate(query, (currentQuery, item) => currentQuery.Include(item));
		}

		if (specification.OrderByExpression is not null)
		{
			query = query?.OrderBy(specification.OrderByExpression);
		}

		if (specification.OrderByDescExpression is not null)
		{
			query = query?.OrderByDescending(specification.OrderByDescExpression);
		}

		if (specification.IsPaginated)
		{
			query = query?.Skip(specification.Skip);
			query = query?.Take(specification.Take);
		}

		return query;
	}
}