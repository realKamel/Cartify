using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Cartify.Domain.Interfaces;

namespace Cartify.Domain.Entities;

public class BaseEntity<TKey> : IAuditing<string> where TKey : notnull, INumber<TKey>
{
	public TKey Id { get; init; }
	public DateTimeOffset CreatedAtUtc { get; set; }
	public string CreatedBy { get; set; }
	public DateTimeOffset? UpdatedAtUtc { get; set; }
	public string? UpdatedBy { get; set; }
	public DateTimeOffset? DeletedAtUtc { get; set; }
	public string? DeletedBy { get; set; }
	public bool IsDeleted => DeletedAtUtc.HasValue;

}