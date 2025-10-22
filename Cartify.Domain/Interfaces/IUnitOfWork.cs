using System.Numerics;
using Cartify.Domain.Entities;

namespace Cartify.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();

    IGenericRepository<TEntity, TKey> GetOrCreateRepository<TEntity, TKey>()
        where TEntity : BaseEntity<TKey> where TKey : INumber<TKey>;
}