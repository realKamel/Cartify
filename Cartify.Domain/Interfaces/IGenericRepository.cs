using System.Numerics;
using Cartify.Domain.Entities;

namespace Cartify.Domain.Interfaces;

public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey> where TKey : INumber<TKey>
{
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    Task<TEntity?> GetByIdAsync(long id);
    IEnumerable<TEntity> GetAll(bool noTracking = true);
    Task<TEntity?> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync(bool noTracking = true);
}