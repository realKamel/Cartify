using System.Numerics;
using Cartify.Domain.Entities;

namespace Cartify.Domain.Interfaces;

public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey> where TKey : INumber<TKey>
{
	void Add(TEntity entity);
	void Update(TEntity entity);
	void Remove(TEntity entity);
	Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

	Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> specifications,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<TEntity>>
		GetAllAsync(ISpecification<TEntity, TKey>? specifications, bool noTracking = true,
			CancellationToken cancellationToken = default);

	Task<int> CountAsync(ISpecification<TEntity, TKey> specifications,
		CancellationToken cancellationToken = default);

	Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default);

	Task<TEntity?> GetSingleAsync(ISpecification<TEntity, TKey> specifications, CancellationToken cancellationToken = default);
}