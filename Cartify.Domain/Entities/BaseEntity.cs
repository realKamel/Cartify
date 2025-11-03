using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Cartify.Domain.Interfaces;

namespace Cartify.Domain.Entities;

public class BaseEntity<TKey> : IAuditing where TKey : INumber<TKey>
{
    public TKey Id { get; init; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
    public Guid? DeletedBy { get; set; }
}