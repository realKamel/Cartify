using System.Numerics;
using Cartify.Domain.Entities;
using Cartify.Domain.Interfaces;
using Cartify.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Cartify.Persistence.Repositories;

public class GenericRepository<TEntity,TKey>(AppDbContext dbContext)
    : IGenericRepository<TEntity,TKey> where TEntity : BaseEntity<TKey> where  TKey : INumber<TKey>
{
    public IEnumerable<TEntity> GetAll(bool noTracking = true)
    {
        return noTracking == true ? dbContext.Set<TEntity>().AsNoTracking() : dbContext.Set<TEntity>();
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

    public async Task<TEntity?> GetByIdAsync(long id)
    {
        return await dbContext.FindAsync<TEntity>(id);
    }
}