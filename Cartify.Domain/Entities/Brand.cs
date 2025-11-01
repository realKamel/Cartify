using Cartify.Domain.Entities.JoinEntities;

namespace Cartify.Domain.Entities;

public class Brand : BaseEntity<int>
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string? Image { get; set; }

    //Product Navigation Property
    public ICollection<Product>? Products { get; set; }

    //Categories Navigation Property
    public ICollection<BrandCategory> Categories { get; set; }
}