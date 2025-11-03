namespace Cartify.Shared.DataTransferObjects.Product;

public class ProductCategoryResponseDto
{
    public int Id { get; init; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
    public required string Image { get; set; }
}