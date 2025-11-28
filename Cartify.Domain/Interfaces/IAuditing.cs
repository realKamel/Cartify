namespace Cartify.Domain.Interfaces;

public interface IAuditing<TKey>
{
	DateTimeOffset CreatedAtUtc { get; set; }
	TKey CreatedBy { get; set; }

	DateTimeOffset? UpdatedAtUtc { get; set; }
	TKey? UpdatedBy { get; set; }

	DateTimeOffset? DeletedAtUtc { get; set; }
	TKey? DeletedBy { get; set; }

	bool IsDeleted => DeletedAtUtc.HasValue;
}