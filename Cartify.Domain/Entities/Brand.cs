using Cartify.Domain.Entities.JoinEntities;
namespace Cartify.Domain.Entities;
public class Brand : BaseEntity<int>
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string Image { get; set; }
    
    public ICollection<Product> Products { get; set; }
    
    
    public ICollection<BrandCategory> BrandCategories { get; set; }
}