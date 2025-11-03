
namespace Cartify.Shared.DataTransferObjects.Product;

public record ProductResponseDto
{
    public int Id { get; init; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
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
    public ProductBrandResponseDto? Brand { get; set; }
    public ProductCategoryResponseDto? Category { get; set; }
}