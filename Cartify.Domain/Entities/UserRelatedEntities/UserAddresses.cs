namespace Cartify.Domain.Entities.UserRelatedEntities;

public class UserAddresses : BaseEntity<int>
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public required string Name { get; set; }
    public required string Phone { get; set; }
    public required string City { get; set; }
    public required string Details { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
    public Guid? DeletedBy { get; set; }
}