using Cartify.Shared.DataTransferObjects.Product;

// using Cartify.Shared.DataTransferObjects.JoinDtos;

namespace Cartify.Shared.DataTransferObjects.Brand;

public record BrandResponseDto
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string Image { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }

    public ICollection<ProductResponseDto>? Products { get; set; }
    // public ICollection<BrandCategoryResponseDto>? Categories { get; set; }
}