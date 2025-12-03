using System.Numerics;
using Cartify.Domain.Entities;
using Cartify.Domain.Interfaces;
using Cartify.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Cartify.Persistence.Repositories;

public class GenericRepository<TEntity, TKey>(AppDbContext dbContext)
	: IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey> where TKey : INumber<TKey>
{
	public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> specifications,
		CancellationToken cancellationToken = default)
	{
		var result = dbContext.Set<TEntity>().AsQueryable();
		var query = QueryBuilder.CreateSpecificationQuery(result, specifications).AsNoTracking();
		return await query.FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecification<TEntity, TKey>? specifications,
		bool noTracking = true, CancellationToken cancellationToken = default)
	{
		var result = dbContext.Set<TEntity>().AsQueryable();

		if (specifications is not null)
		{
			result = QueryBuilder.CreateSpecificationQuery(result, specifications);
		}

		if (noTracking)
		{
			result = result.AsNoTracking();
		}

		return await result.ToListAsync(cancellationToken);
	}

	public async Task<int> CountAsync(ISpecification<TEntity, TKey> specifications,
		CancellationToken cancellationToken = default)
	{
		return await QueryBuilder
			.CreateSpecificationQuery(dbContext.Set<TEntity>(), specifications)
			.AsNoTracking()
			.CountAsync(cancellationToken);
	}

	public void Add(TEntity entity)
	{
		dbContext.Set<TEntity>().Add(entity);
	}

	public void Update(TEntity entity)
	{
		dbContext.Set<TEntity>().Update(entity);
	}

	public void Remove(TEntity entity)
	{
		dbContext.Set<TEntity>().Remove(entity);
	}

	public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
	{
		return await dbContext.Set<TEntity>().FindAsync([id], cancellationToken);
	}
	public async Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
	{
		return await dbContext.Set<TEntity>().AnyAsync(e => e.Id.Equals(id), cancellationToken);
	}

	public async Task<TEntity?> GetSingleAsync(ISpecification<TEntity, TKey> specifications, CancellationToken cancellationToken = default)
	{
		var set = dbContext.Set<TEntity>().AsQueryable<TEntity>();
		var query = QueryBuilder.CreateSpecificationQuery(set, specifications);
		return await query.FirstOrDefaultAsync(cancellationToken);
	}
}