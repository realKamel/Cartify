using Cartify.Domain.Entities.JoinEntities;

namespace Cartify.Domain.Entities;

public class Category : BaseEntity<int>
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string? Image { get; set; }

    //Product Navigation Property
    public ICollection<Product>? Products { get; set; }

    
    //Brands Navigation Property
    public ICollection<BrandCategory> Brand { get; set; }
}