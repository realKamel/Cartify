using Cartify.Domain.Entities;
using Cartify.Domain.Interfaces;
using Cartify.Persistence.DbContexts;
using Cartify.Persistence.Repositories;
using System.Numerics;

namespace Cartify.Persistence;

public class UnitOfWork(AppDbContext dbContext)
    : IUnitOfWork
{
    private readonly Dictionary<string, object> _repositories = [];

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public IGenericRepository<TEntity, TKey> GetOrCreateRepository<TEntity, TKey>()
        where TEntity : BaseEntity<TKey> where TKey : INumber<TKey>
    {
        var typeEntityName = typeof(TEntity).Name;

        if (_repositories.TryGetValue(typeEntityName, out var repo))
        {
            return (IGenericRepository<TEntity, TKey>)repo;
        }
        else
        {
            _repositories[typeEntityName] = new GenericRepository<TEntity, TKey>(dbContext);

            return (IGenericRepository<TEntity, TKey>)_repositories[typeEntityName];
        }
    }
}