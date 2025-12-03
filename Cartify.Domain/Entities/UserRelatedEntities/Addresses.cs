
namespace Cartify.Domain.Entities.UserRelatedEntities;

public class Address : BaseEntity<int>

{
    public required string UserId { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public required string City { get; set; }
    public required string Details { get; set; }

}