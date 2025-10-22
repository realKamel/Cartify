
namespace Cartify.Domain.Entities;

public class Product : BaseEntity<long>
{
    public required string Title { get; set; }
    public required string Slug { get; set; }
    public string? Description { get; set; }
    public ICollection<string>? Images { get; set; }
    public required string ImageCover { get; set; }
    public int Sold { get; set; }
    public double RatingsAverage { get; set; }
    public int RatingsCount { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    
    //Product,Brand relation
    public long BrandId { get; set; }
    public Brand Brand { get; set; }
    
    //product,Category relation
    public long CategoryId { get; set; }
    public Category Category { get; set; }
}