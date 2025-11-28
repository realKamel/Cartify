using Cartify.Domain.Interfaces;

namespace Cartify.Domain.Entities.JoinEntities;

public class BrandCategory : IAuditing<string>
{
    public int BrandId { get; set; }
    public Brand Brand { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    //for Auditing
    public DateTimeOffset CreatedAtUtc { get; set; }
    public required string CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedAtUtc { get; set; }
    public string? DeletedBy { get; set; }
}