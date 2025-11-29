// using Cartify.Shared.DataTransferObjects.JoinDtos;
using Cartify.Shared.DataTransferObjects.Product;

namespace Cartify.Shared.DataTransferObjects.Category;

public record CategoryResponseDto
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string Image { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }

    public ICollection<ProductResponseDto>? Products { get; set; }
    // public ICollection<BrandCategoryResponseDto>? Brands { get; set; }
}