namespace Cartify.Shared.DataTransferObjects.Product;

public class ProductBrandResponseDto
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string Image { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}