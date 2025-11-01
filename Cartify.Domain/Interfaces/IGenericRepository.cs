using System.Numerics;
using Cartify.Domain.Entities;

namespace Cartify.Domain.Interfaces;

public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey> where TKey : INumber<TKey>
{
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task<IReadOnlyList<TEntity>>
        GetAllAsync(ISpecification<TEntity, TKey>? specifications, bool noTracking = true,
            CancellationToken cancellationToken = default);
}