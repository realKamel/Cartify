using Cartify.Domain.Interfaces;

namespace Cartify.Domain.Entities.JoinEntities;

public class BrandCategory : IAuditing
{
    public long BrandId { get; set; }
    public Brand Brand { get; set; }

    public long CategoryId { get; set; }
    public Category Category { get; set; }
    
    //for Auditing
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
    public Guid? DeletedBy { get; set; }
}