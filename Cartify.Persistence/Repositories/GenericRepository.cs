using System.Numerics;
using Cartify.Domain.Entities;
using Cartify.Domain.Interfaces;
using Cartify.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Cartify.Persistence.Repositories;

public class GenericRepository<TEntity, TKey>(AppDbContext dbContext)
    : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey> where TKey : INumber<TKey>
{
    public async Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecification<TEntity, TKey>? specifications,
        bool noTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> result = dbContext.Set<TEntity>();

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

    public async Task<int> CountAsync(ISpecification<TEntity, TKey> specifications)
    {
        return await QueryBuilder
            .CreateSpecificationQuery(dbContext.Set<TEntity>(), specifications).CountAsync();
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

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<TEntity>().FindAsync([id], cancellationToken);
    }
}